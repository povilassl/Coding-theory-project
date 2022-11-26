using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KodavimoTeorijaProjektas
{
    /// <summary>
    /// Interaction logic for EncodingPage.xaml
    /// </summary>
    public partial class EncodingPage : Page
    {
        public EncodingPage()
        {
            InitializeComponent();
        }

        //Kadangi turime 3 pasirinkimus siuntimui: vektorius, pranešimas ir paveiksliukas,
        //tai turime nustatyti, ką vartotojas mato
        //Šiuo atveju - vektorius
        private void VectorSelected_Checked(object sender, RoutedEventArgs e)
        {
            //jei nėra sukurta matrica - grįžtame
            if (!CheckIfMatrixExists()) return;

            //Nustatome mūsų pasirinkimą tolimesiam naudojimui
            Manager.Choice = 1;

            //Atitinkamai nustatome, ką vartotojas mato
            VectorPanel.Visibility = Visibility.Visible;
            MessagePanel.Visibility = Visibility.Hidden;
            ImagePanel.Visibility = Visibility.Hidden;
            UploadedImagePanel.Visibility = Visibility.Hidden;

            EncodedVectorPanel.Visibility = Visibility.Visible;
        }

        //vartotojo pasirinkimas - pranešimas
        private void MessageSelected_Checked(object sender, RoutedEventArgs e)
        {
            //jei nėra sukurta matrica - grįžtame
            if (!CheckIfMatrixExists()) return;

            //Nustatome mūsų pasirinkimą tolimesiam naudojimui
            Manager.Choice = 2;

            //Atitinkamai nustatome, ką vartotojas mato
            VectorPanel.Visibility = Visibility.Hidden;
            MessagePanel.Visibility = Visibility.Visible;
            ImagePanel.Visibility = Visibility.Hidden;
            UploadedImagePanel.Visibility = Visibility.Hidden;

            EncodedVectorPanel.Visibility = Visibility.Hidden;
        }

        //vartotojo pasirinkimas - paveiksliukas
        private void ImageSelected_Checked(object sender, RoutedEventArgs e)
        {
            if (!CheckIfMatrixExists()) return;

            //Nustatome mūsų pasirinkimą tolimesiam naudojimui
            Manager.Choice = 3;

            //Atitinkamai nustatome, ką vartotojas mato
            VectorPanel.Visibility = Visibility.Hidden;
            MessagePanel.Visibility = Visibility.Hidden;
            ImagePanel.Visibility = Visibility.Visible;
            UploadedImagePanel.Visibility = Visibility.Visible;

            EncodedVectorPanel.Visibility = Visibility.Hidden;
        }

        //Turime patikrinti, ar sukurta matrica, jei ne - parodome pranšimą vartotojui
        //Matrica sukurta - nėra praešimo, grąžiname True
        //Matrica nesukurta - praešimas vartotojui, grąžiname False
        private bool CheckIfMatrixExists()
        {
            if (Manager.MatrixG == null || Manager.MatrixG.Length == 0 ||
                Manager.MatrixH == null || Manager.MatrixH.Length == 0)
            {
                MessageBox.Show("Matrix not yet generated", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        //Turime validuoti vektorių ir pranešimą, (paveiksliukui validacijos nereikia)
        //jei validuojame - užkoduojame
        private void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            switch (Manager.Choice)
            {
                case 1:
                    EncodeVector();
                    break;
                case 2:
                    EncodeMessage();
                    break;
                case 3:
                    break;
                default:
                    break;

            }
        }


        //Naudojama užkoduoti vartotojo patiktam veiktoriui
        //jei įvedimas teisingas ir viskas gerai (užkoduota sėkmingai) - vektorius išsaugomas atmintyje
        private void EncodeVector()
        {
            //Pirma mes turime jį išvalyti - patikrinti, ar viskas gerai, įvedimas teisingas, be to - ištrinti 
            //visą "whitespace"
            Manager.Vector = CleanVector(VectorInput.Text);

            //jei validacijos įvestas vektorius nepraėjo, funckija CleanVector() grąžina tuščią vektorių - grįžtame
            if (String.IsNullOrEmpty(Manager.Vector)) return;

            //mūsų galutins vektorius bus be jokių tarpų - vienas ilgas nulių ir vientų string
            string finalVector = "";
            string orgVector = Manager.Vector;

            for (int i = 0; i < orgVector.Length; i += Manager.InputK)
            {
                //Mes iteruojame įvestą vektorių kas K elementų ir imame viso vektoriaus dalį
                //Dalis - [i : i+K]
                var individualVector = orgVector.Substring(i, Manager.InputK);

                //koduojame individualiai pagal matricą G ir sujungiame su jau turimu galutiniu vektoriumi
                finalVector += Manager.EncodeIndividual(Manager.MatrixG, individualVector);
            }
            //išsaugome
            Manager.EncodedVector = finalVector;

            //Čia sukuriame vektorius, kurios rodysime ekrane - jie turi tarpus kas N elementų
            string vectorToBind = "";
            for (int i = 0; i < finalVector.Length; i++)
            {
                if (i != 0 && i % Manager.InputN == 0)
                {
                    vectorToBind += " ";
                }
                vectorToBind += finalVector[i];
            }

            //Rodome ekrane
            EncodedVector.Text = vectorToBind;
        }

        //prieš užkoduojant vektorių reikia jį patikrinti bei išvalyti visą whitespace
        //ši funkcija tai ir padaro
        //jei kas blogai - parodo pranešimą su atitinkama informacija
        private string CleanVector(string vector)
        {
            //vektorius privalo egzistuoti
            if (string.IsNullOrEmpty(vector))
            {
                MessageBox.Show("Vector can not be empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }

            //Išvalome visas naujas eilutes
            vector = vector.Replace('\n', ' ');
            vector = vector.Replace('\r', ' ');

            //removes all whitespace
            vector = Regex.Replace(vector, @"\s+", "");

            //Vektorius turi būti skaičiaus K kartotinis
            if (vector.Length % Manager.InputK != 0)
            {
                MessageBox.Show("Number of elements is not a multiple of your input \'k\'",
                    "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }

            //vektorius privalo būti tik iš 1 ir 0 elementų (nes q = 2)
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] != '0' && vector[i] != '1')
                {
                    MessageBox.Show("There are elements that are not '0' or '1'.",
                        "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return "";
                }
            }

            //Grąžiname patikrintą ir išvalytą vektorių
            return vector;
        }

        //Ši funkcija patikrina ir užkoduoja pranešimą
        //Jei viskas gerai - išsaugo atmintyje
        private void EncodeMessage()
        {
            var mess = MessageInput.Text;

            //Vienintelis reikalavimas - pranešimas turi egzistuoti
            if (String.IsNullOrEmpty(mess))
            {
                MessageBox.Show("Message cannot be empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            //Išsaugome
            Manager.Message = mess;

            //paverčiame pranešimą į bitų seką
            var binaryMess = ToBinaryString(Encoding.UTF8, mess);

            //Tikriname, kiek trūksta bitų, kad galėtume užkoduoti (turi būti kartotinis K)
            var diff = Manager.InputK - binaryMess.Length % Manager.InputK;

            //Pridedame papildomus bitus pačiam gale
            binaryMess = binaryMess + new string('0', diff);

            //Išsaugome skirtumą
            Manager.Difference = diff;

            //Iteruojame ir koduojame - principas toks pats kaip ir anksčiau su paprastu vektoriumi
            var encoded = "";
            for (int i = 0; i < binaryMess.Length; i += Manager.InputK)
            {
                encoded += Manager.EncodeIndividual(Manager.MatrixG, binaryMess.Substring(i, Manager.InputK));
            }

            //Išsaugome
            Manager.EncodedMessage = encoded;
        }

        //Ši funkcija gauna parametrus encoding (mes naudojame UTF-8) ir patį tekstą
        //Ji paverčia bet koki string į vektorių (bitų seką)
        //gražina visą bitų seką string tipu
        static string ToBinaryString(Encoding encoding, string text)
        {
            return string.Join("", encoding.GetBytes(text).Select(n => Convert.ToString(n, 2).PadLeft(8, '0')));
        }

        //Paspaudėme mygtuką, kuris leidžia įkelti paveiksliuką
        //Funkcija atidaro langą, kuriame galime pasirinkti failą
        //jei viskas gerai - užkoduojame
        private void SelectImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".bmp"; // Default file extension
            dialog.Filter = "Images (.bmp)|*.bmp"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Failas buvo įkeltas
            if (result == true)
            {
                //Turime PATH iki failo
                string filename = dialog.FileName;

                //Įkeltą failą atodarome ir parodome ant ekrano
                Uri fileUri = new Uri(filename);
                var bitmap = new BitmapImage(fileUri);
                UploadedImage.Source = bitmap;

                //Nuskaitome visą failą į baitų seką
                byte[] data;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }

                //Išsaugome baitus ir pavadinimą
                Manager.OrgImageBytes = data;
                Manager.ImageFilename = filename;

                //Koduojame paveiksliuką
                EncodeImage();
            }
        }

        //Naudojama užkoduoti paveiksliukui po sėkmingo įkėlimo
        private void EncodeImage()
        {
            var data = Manager.OrgImageBytes;

            //koduojame visus baitus į vieną bitų eilutę
            var str = Manager.ByteArrayToBitString(data);

            var test = str.Length;

            //Išsaugome
            Manager.OrgImageBits = str; //TODO: cia be difference!!!!!

            //jei tai nėra K kartotinis - pridedame papildomų bitų gale
            var diff = Manager.InputK - str.Length % Manager.InputK;

            //Išsaugome skirtumą, jei reikia - papildome
            if (diff != Manager.InputK)
            {
                str += new string('0', diff);
                Manager.Difference = diff;
            }
            else
            {
                Manager.Difference = 0;
            }


            var test2 = str.Length;


            //Visą paveiksliuką užkoduojame
            //Kodavimo principas toks pats, kaip ir anksčiau
            var encoded = new string[str.Length / Manager.InputK + 1];
            for (int i = 0; i < str.Length; i += Manager.InputK)
            {
                var temp = str.Substring(i, Manager.InputK);
                encoded[i / Manager.InputK] = Manager.EncodeVector(Manager.MatrixG, temp);
            }

            //Sujungiame į vieną ilgą bitų seką
            var final = String.Join("", encoded);

            //išsaugome
            Manager.EncodedImage = final;
        }
    }
}
