using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
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
using Color = System.Drawing.Color;

namespace KodavimoTeorijaProjektas.Pages
{
    /// <summary>
    /// Interaction logic for DecodingPage.xaml
    /// </summary>
    public partial class DecodingPage : Page
    {
        public DecodingPage()
        {
            InitializeComponent();

            //Kiekvieną kartą, kai ekranas užsikrauna (yra parodomas vartotojui), mes pakeičiame
            //Tai, ką vartotojas mato ant ekrano
            Loaded += MyWindow_Loaded;
        }

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Pagal tai, koks yra vartotojo pasirinkimas (vektorius, žinutė ar paveiksliukas),
            //Rodome skirtingus variantus

            switch (Manager.Choice)
            {
                case 1:
                    //vectors
                    SentVectorPanel.Visibility = Visibility.Visible;
                    DecodedVectorPanel.Visibility = Visibility.Visible;
                    AddedLeadersVectorPanel.Visibility = Visibility.Visible;
                    OriginalVectorPanel.Visibility = Visibility.Visible;
                    SentVectorInputPanel.Visibility = Visibility.Hidden;
                    Checkbox.Visibility = Visibility.Visible;

                    //messages
                    OriginalMessagePanel.Visibility = Visibility.Hidden;
                    MessageWithSyndromesPanel.Visibility = Visibility.Hidden;
                    MessageWithoutSyndromesPanel.Visibility = Visibility.Hidden;

                    //pictures
                    OriginalImagePanel.Visibility = Visibility.Hidden;
                    ImageWithoutEncodingPanel.Visibility = Visibility.Hidden;
                    ImageWithEncodingPanel.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    //vectors
                    SentVectorPanel.Visibility = Visibility.Hidden;
                    DecodedVectorPanel.Visibility = Visibility.Hidden;
                    AddedLeadersVectorPanel.Visibility = Visibility.Hidden;
                    OriginalVectorPanel.Visibility = Visibility.Hidden;
                    Checkbox.Visibility = Visibility.Hidden;
                    SentVectorInputPanel.Visibility = Visibility.Hidden;

                    //messages
                    OriginalMessagePanel.Visibility = Visibility.Visible;
                    MessageWithSyndromesPanel.Visibility = Visibility.Visible;
                    MessageWithoutSyndromesPanel.Visibility = Visibility.Visible;

                    //images
                    OriginalImagePanel.Visibility = Visibility.Hidden;
                    ImageWithoutEncodingPanel.Visibility = Visibility.Hidden;
                    ImageWithEncodingPanel.Visibility = Visibility.Hidden;
                    break;
                case 3:
                    //vectors
                    SentVectorPanel.Visibility = Visibility.Hidden;
                    DecodedVectorPanel.Visibility = Visibility.Hidden;
                    AddedLeadersVectorPanel.Visibility = Visibility.Hidden;
                    Checkbox.Visibility = Visibility.Hidden;
                    OriginalVectorPanel.Visibility = Visibility.Hidden;
                    SentVectorInputPanel.Visibility = Visibility.Hidden;

                    //messages
                    OriginalMessagePanel.Visibility = Visibility.Hidden;
                    MessageWithSyndromesPanel.Visibility = Visibility.Hidden;
                    MessageWithoutSyndromesPanel.Visibility = Visibility.Hidden;

                    //images
                    OriginalImagePanel.Visibility = Visibility.Visible;
                    ImageWithoutEncodingPanel.Visibility = Visibility.Visible;
                    ImageWithEncodingPanel.Visibility = Visibility.Visible;
                    break;
                default:
                    //no choice - making everything invisible
                    SentVectorPanel.Visibility = Visibility.Hidden;
                    DecodedVectorPanel.Visibility = Visibility.Hidden;
                    AddedLeadersVectorPanel.Visibility = Visibility.Hidden;
                    Checkbox.Visibility = Visibility.Hidden;
                    OriginalVectorPanel.Visibility = Visibility.Hidden;
                    SentVectorInputPanel.Visibility = Visibility.Hidden;

                    //messages
                    OriginalMessagePanel.Visibility = Visibility.Hidden;
                    MessageWithSyndromesPanel.Visibility = Visibility.Hidden;
                    MessageWithoutSyndromesPanel.Visibility = Visibility.Hidden;

                    //images
                    OriginalImagePanel.Visibility = Visibility.Hidden;
                    ImageWithoutEncodingPanel.Visibility = Visibility.Hidden;
                    ImageWithEncodingPanel.Visibility = Visibility.Hidden;
                    break;
            }
        }

        //Vienas mygtukas aptarnauja visus variantus - čia mes atskiriame, kokias funkcijas kviečiame
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            switch (Manager.Choice)
            {
                case 1:
                    if (SendVector())
                        DecodeVector();
                    break;
                case 2:
                    SendMessage();
                    DecodeMessage();
                    break;
                case 3:
                    SendImage();
                    DecodeImage();
                    break;
                default:
                    break;

            }
        }

        //Šis metodas naudojamas patenkinti abu veiktoriaus siuntimo kanalu variantus:
        //  - kai vartotojas nori gautą vektorių įrašyti pats
        //  - kai vektorius yra atsitiktinai sugadinamas
        //Funkcija grąžina True, jei visą validaciją praėjo arba bendrai viskas gerai
        private bool SendVector()
        {
            //Patikrina, ar vartotojas nori pats įvedinėti
            //jei taip - programa validuoja, ar įvedimas yra tinkamas
            if (Checkbox.IsChecked!.Value)
            {
                var recV = SentVectorInput.Text;

                //Vektorius negali būti tuščias
                if (String.IsNullOrEmpty(recV))
                {
                    MessageBox.Show($"Input cannot be empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                //Ištrinam visą whitespace
                recV = Regex.Replace(recV.ToString(), @"\s+", "");

                //Vektorius turi būti N kartotinis
                if (recV.Length % Manager.InputN != 0)
                {
                    MessageBox.Show($"Input is of bad length", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                //Vektorius turi susidaryti iš 0 ir 1 sekos
                for (int i = 0; i < recV.Length; i++)
                {
                    if (recV[i] != '0' && recV[i] != '1')
                    {
                        MessageBox.Show($"Input cannot contain element '" + recV[i] + "'", "Attention",
                            MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return false;
                    }
                }

                //Vektorius turi turėti tokį patį skaičių žodžių, kaip ir prieš tia įvestas pranešimas
                if ((recV.Length / Manager.InputN) != (Manager.Vector.Length / Manager.InputK))
                {
                    MessageBox.Show($"Encoded vector has to contain an equal amount of messages to original vector",
                        "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return false;
                }

                //jei viskas gerai - išsaugome ir grąžiname True
                Manager.ReceivedVector = recV;
                return true;
            }

            //Šita tolimesnė dalis yra tada, kai vartotojas nenori pats nieko keisti
            //T.y. pranešimas siunčiamas kanalu automatiškai

            //Ištriname dabartinį ekrane rodomą pranešimą
            SentVector.Inlines.Clear();

            var vector = Manager.EncodedVector;
            var newVector = "";

            //jei neturime užkoduoto vektoriaus - grąžiname false
            if (String.IsNullOrEmpty(vector)) return false;

            var p = Manager.InputPe;
            var rnd = new Random();

            //Siunčiame kanalu
            for (int i = 0; i < vector.Length; i++)
            {
                decimal r = new(rnd.NextDouble());

                //jeigu mūsų random double yra mažesnis už klaidos tikimybę, bitą apverčiame
                if (r <= p)
                {
                    newVector += (vector[i] == '0') ? '1' : '0';
                }
                else
                {
                    //jei r > p, įstatome originalų (teisingą) elementą
                    newVector += vector[i];
                }
            }

            //Išsaugome
            Manager.ReceivedVector = newVector;


            //Čia mes naują (gautą iš kanalo) vektorių parodome ant ekrano
            var arr1 = vector.ToString();
            var arr2 = newVector.ToString();

            for (int i = 0; i < arr1.Length; i++)
            {
                //Vektorių suskirstome N ilgio dalimis (atskiriame tarpais)
                if (i != 0 && i % Manager.InputN == 0)
                {
                    Run runSpace = new Run(" ");
                    SentVector.Inlines.Add(runSpace);
                }

                Run run = new Run(arr2[i].ToString());

                //jei klaidos nebuvo, elementas yra juodos spalvos
                //jei klaida pasitaikė - raudonos
                if (arr1[i] == arr2[i])
                {
                    run.Foreground = Brushes.Black;
                }
                else
                {
                    run.Foreground = Brushes.Red;
                }

                //pridedame elementą į ekraną
                SentVector.Inlines.Add(run);
            }

            //Viskas pavyko, grąžiname true
            return true;
        }

        //Funkcija naudojama vektoriaus dekodavimui
        //Ji iš atminties pasiima Vektorių, gautą iš kanalo ir dekoduoja
        //Dekodavus, vektorius yra parodomas ant ekrano
        private void DecodeVector()
        {
            var vector = Manager.ReceivedVector;

            //Jeigu dar neturime vektoriaus - grįžtame
            if (String.IsNullOrEmpty(vector)) return;

            var size = vector.Length / Manager.InputN;

            //dekoduotas
            var newArr = new string[size];

            //Visą vektorių dekoduojame imdami jo iš eilės einančias N ilgio dalis
            //ir sudėdami su atitinkamais klasės lyderiais
            //Taip gauname masyvą su kodo žodžiais, kuriais dekodavome gautą iš kanalo vektorių
            for (int i = 0; i < vector.Length; i += Manager.InputN)
            {
                //pasiimame vektoriaus dalį [i: i+N]
                var substr = vector.Substring(i, Manager.InputN);

                var synd = Manager.CalculateSyndrome(substr);   //Gauname sindromą
                var leader = Manager.SyndromeToLeader(synd);    //iš vektoriaus gauname klasės lyderį
                var decoded = Manager.AddVector(leader, substr);//Sudedame vektorių su sindromu

                //išsaugome
                newArr[i / Manager.InputN] = decoded;
            }

            //Sudedame visus masyvo elementus (kuriame yra dekoduoti vektoriai) ir rodome ekrane
            var showV = "";
            foreach (var el in newArr)
            {
                showV += el + " ";
            }

            AddedLeadersVector.Text = showV;

            //Pilnai dekoduojame visus gautus vektorius
            //jau turime vektorius su pridėtai klasiu lyderiais
            //Lieka iš ju gauti pirminius pranešimus
            var decV = new string[newArr.Length];
            var code = Manager.Code;

            for (int i = 0; i < newArr.Length; i++)
            {
                for (int j = 0; j < code.GetLength(0); j++)
                {
                    if (code[j, 1] == newArr[i])
                    {
                        decV[i] += code[j, 0];
                    }
                }
            }

            //Parodome dekoduotą vektorių ant ekrano
            //Juodi simboliai - teisingi
            //Raudoni - klaidingi (lyginant su pirminiu įvestu vektoriumi)

            DecodedVector.Inlines.Clear();
            var arr1 = String.Join("", decV).ToCharArray(); //gautas vektorius
            var arr2 = Manager.Vector.ToCharArray();        //originalus vekt

            for (int i = 0; i < arr1.Length; i++)
            {
                if (i != 0 && i % Manager.InputK == 0)
                {
                    Run runSpace = new Run(" ");
                    DecodedVector.Inlines.Add(runSpace);
                }

                Run run = new Run(arr1[i].ToString());

                if (arr1[i] == arr2[i])
                {
                    run.Foreground = Brushes.Black;
                }
                else
                {
                    run.Foreground = Brushes.Red;
                }
                DecodedVector.Inlines.Add(run);
            }

            //Taip pat ekrane rodome originalų vektorių
            //Jį suskaidome K ilgio elementais ir atskiriame tarpais
            var messPrint = "";
            for (int i = 0; i < Manager.Vector.Length; i += Manager.InputK)
            {
                messPrint += Manager.Vector.Substring(i, Manager.InputK) + " ";
            }

            OriginalVector.Text = messPrint;
        }

        //Jeigu vartotojas pasirenka, kad nori pats redaguoti iš kanalo gautą vektorių
        //mes pakeičiame tam tikrų elementų matomumą atitinkamai 
        //bei užpildome redagavimo dalį užkoduotu vektoriumi
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Manager.EncodedVector)) return;

            SentVectorInputPanel.Visibility = Visibility.Visible;
            SentVectorPanel.Visibility = Visibility.Hidden;

            var fillV = "";

            for (int i = 0; i < Manager.EncodedVector.Length; i += Manager.InputN)
            {
                fillV += Manager.EncodedVector.Substring(i, Manager.InputN) + " ";
            }

            SentVectorInput.Text = fillV;
        }

        //Jeigu vartotojas pasirenka, kad nori pats nenori redaguoti,
        //tai mums reikia tik pakeisti elementų matomumą
        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            SentVectorInputPanel.Visibility = Visibility.Hidden;
            SentVectorPanel.Visibility = Visibility.Visible;
        }


        //Metodas naudojamas siųsti kanalu pranešimą
        //Jis jį verčia bitų seka ir naudoja lygiai tokį patį principą, koks panaudotas siunčiant vektorių
        private void SendMessage()
        {
            //užkoduota žinutė
            var mess = Manager.EncodedMessage;
            var newMess = "";

            //nekoduota žinutė (naudojama rodant ant ekrano, kad vartotojas galėtų palyginti)
            var messageOrg = Manager.ToBinaryString(Encoding.UTF8, Manager.Message);
            var messageWithoutEncoding = "";

            //klaidos tikimybė ir Random() inicializavimas
            var p = Manager.InputPe;
            var rnd = new Random();

            //difference reik atimt, nes saugiai siunciam tuos paskutinius nulius
            //TODO: dabar pilnai nuimmam paskutini
            for (int i = 0; i < mess.Length - Manager.Difference; i++)
            {
                //Kanalu siunčiama užkoduota žinutė
                decimal r = new(rnd.NextDouble());
                if (r <= p)
                {
                    newMess += (mess[i] == '0') ? '1' : '0';
                }
                else
                {
                    newMess += mess[i];
                }

                //Jeigu dar neviršijome pradinės (neužkoduotos) žinutės ilgio, tai darome klaidą toje pačioje vietoje
                if (i < messageOrg.Length)
                {
                    if (r <= p)
                    {
                        messageWithoutEncoding += (messageOrg[i] == '0') ? '1' : '0';
                    }
                    else
                    {
                        messageWithoutEncoding += messageOrg[i];
                    }
                }
            }

            //Perkeliame papildomai pridetus bitus
            newMess += mess.Substring(mess.Length - Manager.Difference, Manager.Difference);

            //Išsaugom
            Manager.ReceivedMessage = newMess;
            Manager.ReceivedMessageWithoutEncoding = messageWithoutEncoding;
        }

        //Metodas naudojamas dekoduoti pranešimą, gautą iš kanalo
        //Nieko negrąžina, bet parodo reikšmes ekrane
        private void DecodeMessage()
        {
            //gautas pranešimas
            var mess = Manager.ReceivedMessage;

            var arr = new string[mess.Length / Manager.InputN];
            var newArr = new string[arr.Length];

            //suskaidome vektorių į dalis, kad galėtume dekoduoti
            for (int i = 0; i < mess.Length; i += Manager.InputN)
            {
                arr[i / Manager.InputN] = mess.Substring(i, Manager.InputN);
            }

            //Gauname naują masyvą, kuriame yra gauti pranešimai su pridėtais klasių lyderiais
            for (int i = 0; i < arr.Length; i++)
            {
                var synd = Manager.CalculateSyndrome(arr[i]);
                var leader = Manager.SyndromeToLeader(synd);
                var decoded = Manager.AddVector(leader, arr[i]);
                newArr[i] = decoded;
            }

            //Dekoduojame visą masyvą iš bitų į žinutę
            var str = Manager.DecodeArrayOfVectorsToString(newArr, true);

            MessageWithSyndromes.Text = str;

            OriginalMessage.Text = Manager.Message;

            //Reikia gauti ir neužkoduotą pranešimą
            var str1 = Manager.DecodeArrayOfVectorsToString(new string[] { Manager.ReceivedMessageWithoutEncoding }, false);

            MessageWithoutSyndromes.Text = str1;
        }

        //Funkcija naudojama paveiksliukui siųsti kanalu
        //Ji iš atminties pasiima pradinį paveiksliuką ir užkoduotą paveiksliuką,
        //juos abu siunčia kanalu su tikimybe 'pe' ir gautas reikšmes vėl išsaugo atmintyje
        private void SendImage()
        {
            //jei nėra paveiksliuko, tai tiesiog grįžtame
            if (String.IsNullOrEmpty(Manager.ImageFilename)) return;

            //Parodome pradinį paveiksliuką
            Uri fileUri = new Uri(Manager.ImageFilename);
            var bitmap = new BitmapImage(fileUri);
            OriginalImage.Source = bitmap;


            //Jei nėra užkoduoto paveiksliuko - grįžtame
            if (String.IsNullOrEmpty(Manager.EncodedImage)) return;


            //Apsirašome paveiksliukų paraščių ilgius
            var start1 = 10000;
            var start2 = 5000;

            //Gauname užkuoduotą pranešimą
            var mess = Manager.EncodedImage;

            //Nukopijuojame paraštę į naują žinutę
            var newMess = mess.Substring(0, (int)Math.Ceiling((double)start1));

            //Gauname nekoduotą pranešimą
            var messageOrg = Manager.OrgImageBits;

            //Nukopijuojame paraštę į naują žinutę
            var messageWithoutEncoding = messageOrg.Substring(0, start2);

            //tikimybė ir Random init
            var p = Manager.InputPe;
            var rnd = new Random();

            //Randame pradžią ir pabaigą, kurios tinka abejoms žinutėms
            var start = (int)Math.Min(start1, start2);
            var end = Math.Max(mess.Length, messageOrg.Length);

            //Siunčiame kanalu
            for (int i = start; i < end; i++)
            {
                decimal r = new(rnd.NextDouble());

                //Jei mūsų indeksas patenka į užkoduotos žinutės ilgio rėžius
                if (i < mess.Length && i >= start1)
                {
                    if (r <= p)
                        newMess += mess[i] == '0' ? '1' : '0';
                    else
                        newMess += mess[i];
                }

                //Jei mūsų indeksas patenka į nekoduotos žinutės ilgio rėžius
                if (i < messageOrg.Length && i >= start2)
                {
                    if (r <= p)
                        messageWithoutEncoding += messageOrg[i] == '0' ? '1' : '0';
                    else
                        messageWithoutEncoding += messageOrg[i];
                }
            }

            //Išsaugome atmintyje
            Manager.ReceivedImage = newMess;
            Manager.ReceivedImageWithoutEncoding = messageWithoutEncoding;
        }

        //Funkcija naudojama paveiksliukų dekodavimui
        //Ji iš atminties pasiima siųstus kanalu paveiksliukus (koduotą ir nekoduotą)
        //Užkoduotą paveiksliuką dekoduoja ir parodo ekrane
        //Neužkoduotą - tiesiog parodo
        private void DecodeImage()
        {
            //Mūsų paveiksliukai
            var rec1 = Manager.ReceivedImage;
            var rec2 = Manager.ReceivedImageWithoutEncoding;

            //Dekoduojame paveiksliuką
            var arr = new string[rec1.Length / Manager.InputN];
            for (int i = 0; i < rec1.Length; i += Manager.InputN)
            {
                //pasiimame vektoriaus dalį [i: i+N]
                var substr = rec1.Substring(i, Manager.InputN);

                var synd = Manager.CalculateSyndrome(substr);   //Gauname sindromą
                var leader = Manager.SyndromeToLeader(synd);    //iš vektoriaus gauname klasės lyderį
                var decoded = Manager.AddVector(leader, substr);//Sudedame vektorių su sindromu

                arr[i / Manager.InputN] = decoded;
            }

            //Nustatome dekoduotus paveiksliukus
            var decoded1 = Manager.DecodeStringOfVectors(arr);
            var decoded2 = rec2;

            //Taip pat iš užkoduoto paveiksliuko reikia atimti papildomai pridėtus bitus
            decoded1 = decoded1.Substring(0, decoded1.Length - Manager.Difference);

            //Abu paveiksliukus rodome ekrane
            DisplayImage(decoded1, ImageWithEncoding);
            DisplayImage(decoded2, ImageWithoutEncoding);
        }


        //Funkcija, naudojama paveiksliukui dekoduoti atgal į baitų seką ir parodyti ekrane
        //Ji gauna - visų paveiksliuko bitų seką; lokaciją, kurioje reikia paveiksliuką parodyti
        //Nieko negrąžina, bet parodo ekrane paveiksliuką
        private void DisplayImage(string str, Image display)
        {
            //verčiame bitus į baitų masyvą
            var bytes = Manager.StringToByteArray(str);

            //verčiame baitų masyvą į paveiksliuką
            var src = Manager.BytesToImage(bytes);

            //parodome paveiksliuką ekrane
            display.Source = src;
        }
    }
}
