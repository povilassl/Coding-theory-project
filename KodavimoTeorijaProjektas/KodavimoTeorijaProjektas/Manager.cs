using KodavimoTeorijaProjektas.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace KodavimoTeorijaProjektas
{
    public static class Manager
    {
        //Mūsų pradiniai parametrai: K, N ir Pe
        public static int InputK { get; set; }
        public static int InputN { get; set; }
        public static decimal InputPe { get; set; }

        //Mūsų matricos: generuojanti (G) ir kontrolinė (H)
        public static int[,] MatrixG { get; set; }
        public static int[,] MatrixH { get; set; }

        //Mūsų pasirinkimas, ką norime koduoti: vektorius, pranešimas arba paveiksliukas
        public static int Choice { get; set; }

        //Pradinis (vartotojo įrašytas) vektorius, užkoduotas vektorius bei iš kanalo gautas vektorius
        public static string Vector { get; set; }
        public static string EncodedVector { get; set; }
        public static string ReceivedVector { get; set; }

        //Standartinė lentelė bei sindromai
        public static string[,] StandardTable { get; set; }
        public static string[,] Syndromes { get; set; }
        public static string[,] Code { get; set; }

        //Pradinis (vartotojo įrašytas) pranešimas, užkoduotas pranešimas
        public static string Message { get; set; }
        public static string EncodedMessage { get; set; }

        //Iš kanalo gautas užkoduotas pranešimas ir gautas nekoduotas pranešimas (naudojamas palyginimui)
        public static string ReceivedMessage { get; set; }
        public static string ReceivedMessageWithoutEncoding { get; set; }

        //Viskas, kas susiję su paveiksliukais: baitai, bitai, failo pavadinimas, užkoduotas pranešimas bei gauti pranešimai
        public static byte[] OrgImageBytes { get; set; }
        public static string OrgImageBits { get; set; }
        public static string ImageFilename { get; set; }
        public static string EncodedImage { get; set; }
        public static string ReceivedImage { get; set; }
        public static string ReceivedImageWithoutEncoding { get; set; }

        //Skirtumas - rekia žinoti, kiek mes papildomų bitų pridėjom pranešimui ar paveiksliukui
        //(kad galėtume užkoduoti)
        public static int Difference { get; set; }


        //Funkcija, naudojama uzkoduoti vektoriu sekai
        //Paduodama: bitu seka (neriboto ilgio, svarbu, kad būtų K kartotinis) ir matrica, su kuria koduosime
        //Grąžina: užkoduotą bitų seką
        public static string EncodeVector(int[,] matrix, string orgVector)
        {
            string finalVector = "";
            for (int i = 0; i < orgVector.Length; i += Manager.InputK)
            {
                //Gauname individualų vektorių iš bitų sekos
                string individualVector = orgVector.Substring(i, Manager.InputK);

                //Pridedame prie galutinio vektoriaus užkoduotą individualų vektorių
                finalVector += EncodeIndividual(matrix, individualVector);
            }

            //Grąžiname užkuotų vektorių seką
            return finalVector;
        }

        //Funckija, naudojama užkoduoti individualų vektorių, dažniausiai naudojama kartu su kit funkcija (EncodeVector)
        //Parametrai: matrica, kuria koduosime, ir vektorius (ilgio K), kurį koduosime
        //Grąžina: užkoduotą vektorių (ilgio N)
        public static string EncodeIndividual(int[,] matrix, string individualVector)
        {
            //Pasidarome ilgio N vektorių iš 0-io elementų
            string result = new('0', Manager.InputN);

            //Iteruojame kiekvieną vektoriaus bitą, jeigu jis yra '1', tai mum reikia prie 
            //galutinio vektoriaus pridėti visą matricos eilutę (eilutės indeksas = i)
            for (int i = 0; i < individualVector.Length; i++)
            {
                if (individualVector[i] != '0')
                {
                    //Prie galutinio pridedame tam tikrą matricos eilutę
                    result = AddVector(matrix, i, result);
                }
            }

            return result;
        }

        //Funkcija, kuri prie vektoriaus prideda tam tikrą matricos eilutę
        //Parametrai: matrica, indeksas (kad nurodyti matricos eilutę), vektorius (prie kurio pridedinėsime)
        //Grąžina: prie paduoto vektoriaus pridėtą matricos eilutę
        public static string AddVector(int[,] matrix, int i, string orgV)
        {
            string matrixV = "";
            string newV = "";

            //Pirma gauname matricos i eilutę (pasidarome vektorių, kurį pridedinėsime)
            for (int j = 0; j < orgV.Length; j++)
            {
                matrixV += matrix[i, j];
            }

            //Grąžiname sudėtus vektorius
            return AddVector(orgV, matrixV);
        }

        //Funkcija, kuri sudeda 2 vektorius
        //Parametrai: 2 vektoriai, kuriuo sudedinėsime
        //Grąžina: sudėtus vektorius
        public static string AddVector(string v1, string v2)
        {
            var newVector = "";

            //sudedinėjame kiekvieną elementą atskirai
            for (int i = 0; i < v1.Length; i++)
            {
                //Jeigu elementai lygūs, tai jų sudėtis visada bus 0 (1+1 = 0, 0+0 = 0)
                //jei elementai skirtingi, jų sudėtis visada bus 1 (1+0 = 1, 0+1 = 1)
                if (v1[i] == v2[i])
                    newVector += "0";
                else
                    newVector += "1";
            }

            return newVector;
        }

        //Funkcija, skaičiuojanti vektoriaus sindromą
        //Parametras: vektorius, kurio sindromą skaičiuosime
        //Grąžina paduoto vektoriaus sindromą
        public static string CalculateSyndrome(string orgV)
        {
            //Sindromo ilgis visada bus lygus matricos H stulpelių kiekiui
            //Tai užpildome jį nuliais
            var newV = new string('0', Manager.MatrixH.GetLength(0));

            for (int i = 0; i < orgV.Length; i++)
            {
                var addingV = "";

                //Pridedinėsime tik tada, kai vektoriaus elementas == 1
                if (orgV[i] == '1')
                {
                    //Surenkame vektorių, kurį pridedinėsime iš matricos H
                    for (int j = 0; j < Manager.MatrixH.GetLength(0); j++)
                    {
                        addingV += (Manager.MatrixH[j, i]).ToString();
                    }

                    //sudedame
                    newV = Manager.AddVector(newV, addingV);
                }
            }
            return newV;
        }

        //Funkcija, naudojama gauti tam tikro sindromo lyderiui
        //Parametras: sindromas
        //Grąžina: klasės lyderį
        public static string SyndromeToLeader(string synd)
        {
            var height = Manager.Syndromes.GetLength(0);
            var matrix = Manager.Syndromes;

            //Iteruojame visus sindromus, kadangi mūsų sindromų lentelė atitinkamose eilutėse laiko ir lyderius
            //tai mum užtenka iteruoti ir ieškoti teisingo
            for (int i = 0; i < height; i++)
            {
                //kai randame teisinga, gražiname jo lyderį
                if (matrix[i, 1] == synd)
                {
                    return matrix[i, 0];
                }
            }

            return "";
        }

        //Funkcija, naudojama dekoduoti bitų sekai, kuri suskirstyti į ilgio N vektorius
        //Parametras: vektorių masyvas
        //Grąžina: viena dekoduotą bitų seką
        public static string DecodeStringOfVectors(string[] subs)
        {
            var decoded = new List<string>();
            var code = Manager.Code;

            //iteruojame visus vektorius
            foreach (var el in subs)
            {
                //iteruojame kodą
                for (int i = 0; i < code.GetLength(0); i++)
                {
                    //jei kodo vektorius atitinka mūsų vektorių, tai pridedame to kodo vektoriaus klasės lyderį
                    if (code[i, 1] == el.ToString())
                    {
                        decoded.Add(code[i, 0]); //pridėjimas
                    }
                }
            }

            //sujungiame visus vektorius į vieną bitų seką ir grąžiname
            return String.Join("", decoded.ToArray());
        }

        //Funkcija, apsukanti tam tikrą eilutę
        //Parametras: eilutė
        //Grąžina: apsuktą eilutę
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        //Funkcija, bitų seką paverčianti į pranešimą (UTF8 characters)
        //Parametrai: masyvas vektorių, kuriuos dekoduosime; boolean reikšmė, pasakanti, ar tas masyvas yra užkoduotas kodo
        //Grąžiną: pranešimą
        public static string DecodeArrayOfVectorsToString(string[] newArr, bool decode)
        {
            string bits;

            //Jeigu pranešimas yra užkoduotas, jį reikia dekoduoti
            if (decode)
            {
                var decodedArr = new string[newArr.Length];

                //decoding fully
                int index = 0;
                foreach (var el in newArr)
                {
                    var code = Manager.Code;
                    for (int i = 0; i < code.GetLength(0); i++)
                    {
                        //Ieškome, kurį kodo žodį atitinka ir sudedame su lyderiu
                        if (code[i, 1] == el)
                        {
                            decodedArr[index] += code[i, 0];
                            index++;
                        }
                    }
                }

                //Iš masyvo elementų padarome vieną bitų seką, taip pat nuimame papildomai pridėtus bitus
                bits = String.Join("", decodedArr).Substring(0, decodedArr.Length * Manager.InputK - Manager.Difference);
            }
            else
            {
                //jei nedekoduojame, tai tiesiog padarome vieną bitų seką
                bits = String.Join("", newArr);
            }

            //Kad sėkmingai iš bitų padarytume baitus, mum kiekvieną 8-ių bitų seką reikia apsukti
            var temp = "";
            for (int i = 0; i < bits.Length; i += 8)
            {
                //gauname tam tikrą bitų dalį
                var chunk = bits.Substring(i, 8);

                //apsukame
                temp += Reverse(chunk);
            }
            bits = temp;

            //šioje vietoje, mes savo bitų seką padarome iš string į BitArray
            BitArray bitArr = new(bits.Length);
            for (int i = 0; i < bits.Length; i++)
            {
                bitArr[i] = bits[i] == '1' ? true : false;
            }

            //iš bitų sekos padarome baitų seką
            var bytesArr = new byte[bits.Length / 8];
            bitArr.CopyTo(bytesArr, 0);

            //Grąžiname dekoduotą baitų seką (character'ius)
            return Encoding.UTF8.GetString(bytesArr);
        }

        //Naudojama tekstui paversti į bitų seką (string)
        //Parametrai: kodavimas, kurį naudosime (mes naudojame UTF-8), bei originalus tekstas
        //Grąžina bitų seką
        public static string ToBinaryString(Encoding encoding, string text)
        {
            return string.Join("", encoding.GetBytes(text).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
        }

        //Funkcija i6 baitų masyvo padaro bitų seką String tipu
        //parametras: baitų masyvas
        //Grąžina: vieną eilutę, kurioje yra mūsų bitų seka
        public static string ByteArrayToBitString(byte[] arr)
        {
            //iš baitų pasidarome bitus
            var bits = new BitArray(arr);

            var str = "";

            //iteruoajame bitus, ir juos keičiame į atitinkamus string tipo elementus
            for (int i = 0; i < bits.Length; i++)
            {
                str += bits[i] ? "1" : "0";
            }

            return str;
        }

        //Funkcija naudojama bitų seką (string tipo) paversti į baitų seką
        //parametras: bitų eilutė
        //Grąžiną: baitų seką
        public static byte[] StringToByteArray(string str)
        {
            //Verčiame į bitus (char -> bit)
            BitArray bitArr = new(str.Length);
            for (int i = 0; i < str.Length; i++)
            {
                bitArr[i] = str[i] == '1' ? true : false;
            }

            //iš bitų sekos darome baitų seką (bit * 8 -> byte)
            var bytesArr = new byte[str.Length / 8];
            bitArr.CopyTo(bytesArr, 0);

            return bytesArr;
        }

        //Funkcija, iš baitų sekos padaranti paveiksliuką
        //Parametras: baitų seka
        //grąžina: paveiksliuką
        public static BitmapImage BytesToImage(byte[] imageData)
        {
            //Jeigu baitų seka yra null arba tuščia, tai grąžinamą null
            if (imageData == null || imageData.Length == 0) return null;

            //Byte[] -> BitmapImage
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            //Grąžiname paveiksliuką
            return image;
        }
    }
}
