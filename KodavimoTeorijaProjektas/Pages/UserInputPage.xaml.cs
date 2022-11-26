using KodavimoTeorijaProjektas.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KodavimoTeorijaProjektas
{
    /// <summary>
    /// Interaction logic for UserInputPage.xaml
    /// </summary>
    public partial class UserInputPage : Page
    {
        public bool Validated { get; set; }

        public UserInputPage()
        {
            InitializeComponent();
            Validated = false;

            //Nustatome tai, ką vartotojas mato ant ekrano, kolkas dar nieko (Viskas Hidden)
            matrixGgeneratedPanel.Visibility = Visibility.Hidden;
            matrixHPanel.Visibility = Visibility.Hidden;
            matrixGinputPanel.Visibility = Visibility.Hidden;
        }

        //Vartotojas, įvedes parametrus, paspaudžia mygtuką
        //Mes jo įvedimus validuojame
        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            //gaunami visi vartotojo ivedimai: k, n ir pe
            var k = inputK.Text;
            var n = inputN.Text;
            var pe = inputPe.Text;

            int resultK;
            int resultN;
            decimal resultPe;


            //Toliau validuojami visi ivedimai, jei kas blogai - rodomas pranešimas
            if (String.IsNullOrEmpty(k))
            {
                MessageBox.Show($"\"k\" input is empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (String.IsNullOrEmpty(n))
            {
                MessageBox.Show($"\"n\" input is empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (String.IsNullOrEmpty(pe))
            {
                MessageBox.Show($"\"pe\" input is empty", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                resultK = Int32.Parse(k);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Couldnt parse {k} - not an integer", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                resultN = Int32.Parse(n);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Couldnt parse {n} - not an integer", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            try
            {
                pe = pe.Replace(',', '.');

                var dec = decimal.Parse(pe);

                if (dec < (decimal)0.0001 || dec >= 1)
                {
                    MessageBox.Show($"Input Pe cannot be less that 0.0001 and more than 1", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                resultPe = dec;
            }
            catch (FormatException)
            {
                MessageBox.Show($"Couldnt parse {pe} - not a decimal", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (resultK >= resultN)
            {
                MessageBox.Show("\"k\" can not be larger than or equal to \"n\"", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            if (resultK > 20 || resultN > 20)
            {
                MessageBox.Show("Values \'n\' and \'k\' can can not exceed 20", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (resultPe <= 0 || resultPe >= 1)
            {
                MessageBox.Show("Value \'pe\' must be from interval (0,1)", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //Teisingi (jau validuoti) įvedimai išsaugomi tolimesniam naudojimui
            Manager.InputK = resultK;
            Manager.InputN = resultN;
            Manager.InputPe = resultPe;

            Validated = true;
        }

        //Vartoojas pasirenka pats įvesti matricą G
        //Patikriname, ar jo pradiniai parametrai validuoti, jei taip - parodome vienetinę matricos dalį ant ekrano
        private void RadioButtonInput_Checked(object sender, RoutedEventArgs e)
        {
            if (!Validated)
            {
                MessageBox.Show("Input not yet validated", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                radio1.IsChecked = false;
                return;
            }

            GenerateMatrixButton.Visibility = Visibility.Hidden;
            matrixGgeneratedPanel.Visibility = Visibility.Hidden;
            matrixHPanel.Visibility = Visibility.Hidden;

            matrixGinputPanel.Visibility = Visibility.Visible;

            //Generuojama Matrica G (vienetinio pavidalo)
            GenerateUnitaryMatrix();
        }

        //Funkcija pasiima issaugotus atmintyje ivedimus (K ir N), sugeneruoja vienetinio pavidalo matrica,
        //kuri bus naudojama, kai vartotojas pats nores ivesti Matrica G
        //T.y. pusė matricos rodoma ant ekrano (vienetinė dalis), o kitą pusę leidžiama įvesti vartojui
        private void GenerateUnitaryMatrix()
        {
            string matrix = "";

            var k = Manager.InputK;
            var n = Manager.InputN;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i == j)
                    {
                        matrix += "1 ";
                    }
                    else
                    {
                        matrix += "0 ";
                    }
                }
                if (i != k - 1)
                    matrix += "\n";
            }

            unitaryMatrixForInput.Content = matrix;
        }

        //Pasirenkame, kad patys įvedinėsime matricą
        private void RadioButtonGenerate_Checked(object sender, RoutedEventArgs e)
        {
            if (!Validated)
            {
                MessageBox.Show("Input not yet validated", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                radio2.IsChecked = false;
                return;
            }

            matrixGinputPanel.Visibility = Visibility.Hidden;
            GenerateMatrixButton.Visibility = Visibility.Visible;
        }

        //Automatiškai sukuriama Matrica G pagal įvestus parametrus (K ir N), taip pat funkcija priima parametra 
        // "show", jei jis yra "True", matricą parodome an ekrano
        private void CreateGeneratingMatrix(bool show)
        {
            //Mes sukuriame vienetinio pavidalo matricą G
            //Kairė jos pusė - vienetinė
            //Dešinė - turi pilnai atsitiktines reikšmes
            Manager.MatrixG = new int[Manager.InputK, Manager.InputN];
            var rand = new Random();

            for (int i = 0; i < Manager.InputK; i++)
            {
                for (int j = 0; j < Manager.InputN; j++)
                {
                    if (j < Manager.InputK)
                    {
                        if (j == i)
                        {
                            Manager.MatrixG[i, j] = 1;
                        }
                        else
                        {
                            Manager.MatrixG[i, j] = 0;
                        }
                    }
                    else
                    {
                        //Įvedama atsitiktinė reikšmė [0,1]
                        Manager.MatrixG[i, j] = rand.Next(0, 2);
                    }
                }
            }
            if (show)
            {
                //create matrix G for showing on screen
                string matrixBinding = "";
                for (int i = 0; i < Manager.InputK; i++)
                {
                    matrixBinding += "| ";

                    for (int j = 0; j < Manager.InputN; j++)
                    {
                        if (j == Manager.InputK)
                        {
                            matrixBinding += " | ";
                        }

                        matrixBinding += (Manager.MatrixG[i, j]);
                        matrixBinding += " ";
                    }

                    matrixBinding += " |";
                    if (i != Manager.InputK - 1)
                        matrixBinding += "\n";
                }

                matrixG.Content = matrixBinding;
            }
        }

        //Automatiškai sukuriama Kontrolinė matrica H
        //Parametras "Show", kaip ir ankščiau, nusako, ar ją rodysime ekrane
        //matrica kuriama tada, kai mes jau turime matricą G
        public void CreateControlMatrix(bool show)
        {
            if (Manager.MatrixG == null || Manager.MatrixG.Length == 0)
            {
                MessageBox.Show("Matrix G does not exist", "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //Creating Control matrix (H)
            int k = Manager.InputN - Manager.InputK; //height
            int n = Manager.InputN;                  //width
            Manager.MatrixH = new int[k, n];

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j < Manager.InputK)
                    {
                        //transponuota dalis
                        Manager.MatrixH[i, j] = Manager.MatrixG[j, i + Manager.InputK];
                    }
                    else
                    {
                        //vienetine dalis
                        if (i == j - Manager.InputK)
                        {
                            Manager.MatrixH[i, j] = 1;
                        }
                        else
                        {
                            Manager.MatrixH[i, j] = 0;
                        }
                    }
                }
            }


            if (show)
            {
                //create matrix G for binding
                string matrixBinding = "";
                for (int i = 0; i < k; i++)
                {
                    matrixBinding += "| ";

                    for (int j = 0; j < n; j++)
                    {
                        if (j == Manager.InputK)
                        {
                            matrixBinding += " | ";
                        }

                        matrixBinding += (Manager.MatrixH[i, j]);
                        matrixBinding += " ";
                    }

                    matrixBinding += " |";
                    if (i != k - 1)
                        matrixBinding += "\n";

                }

                matrixH.Content = matrixBinding;
                matrixHPanel.Visibility = Visibility.Visible;
            }
        }

        //Kai vartotojas įveda matricą G, ją reikia validuoti, ar tokia matrica egzistuoja
        //T.y. ar joje teisingi elementai (0 ir 1), ar ji teisingo ilgio ir t.t.
        //Jeigu ji validuojama, ji išsaugoma atmintyje tolimesniam naudojimui
        private void ValidateMatrixGInput_Click(object sender, RoutedEventArgs e)
        {
            string org = matrixGInput.Text;

            //removes newline chars
            org = org.Replace('\n', ' ');
            org = org.Replace('\r', ' ');

            //removes all whitespace
            org = Regex.Replace(org, @"\s+", "");

            var k = Manager.InputK;
            var n = Manager.InputN;

            //checking if number of elements is correct
            //TODO: this would pass both n x k and k x n
            if (org.Length != k * (n - k))
            {
                MessageBox.Show("Number of elements in matrix doesn't correspond to your input",
                    "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;

            }

            //checking 
            for (int i = 0; i < org.Length; i++)
            {
                if (org[i] != '0' && org[i] != '1')
                {
                    MessageBox.Show("There are elements that are not '0' or '1'.",
                        "Attention", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            Manager.MatrixG = new int[k, n];
            int index = 0;

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j < k)
                    {
                        if (i == j)
                        {
                            Manager.MatrixG[i, j] = 1;
                        }
                        else
                        {
                            Manager.MatrixG[i, j] = 0;
                        }
                    }
                    else
                    {
                        Manager.MatrixG[i, j] = Int32.Parse(org[index].ToString());
                        index++;
                    }
                }
            }

            //Po teisingo matricos G sukūrimo, kuriame matricą H
            CreateControlMatrix(true);

            //Taip pat kuriame standartinę lentelę ir sidromus
            GenerateStandardTable();
        }

        //Vartotojas gali paspausti mygtuką, kad matrica G susikurtų pati
        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            CreateGeneratingMatrix(true); //create and show generating matrix
            CreateControlMatrix(true); //create and show control matrix

            GenerateStandardTable(); //Kuriame standartinę lentelę ir sindromus

            //Pakeičiame Atitinkamai tai, ką vartotojas mato ant ekrano
            matrixGgeneratedPanel.Visibility = Visibility.Visible;
            matrixHPanel.Visibility = Visibility.Visible;
            matrixGinputPanel.Visibility = Visibility.Hidden;
        }

        //Programa sukuria standartinę lentelę ir sindromus
        //Jos veikimo principas: turime visus įmanomus kodo žodžius (vadiname - pool)
        //Kai mes įdedame kažką į lentelę, mes išimame tą žodį iš pool
        //To reikia tam, kad galėtume teisingai apskaičiuoti klasės lyderį
        private void GenerateStandardTable()
        {
            //Gauname visus įmanomus ilgio N pranešimus
            var messagesPool = GenerateMessages(Manager.InputN);

            //gauname visus užkoduotus pranešimus
            var code = GenerateOrgCodeMessages();

            //nustatome Stand. lentelės dimensijas
            var tableHeight = (int)Math.Pow(2, Manager.InputN - Manager.InputK);
            var tableWidth = (int)(Math.Pow(2, Manager.InputN) / tableHeight);
            var standardTable = new string[tableHeight, tableWidth];

            //Į pirmą eilute dedame visus užkoduotus pranešimus
            for (int i = 0; i < tableWidth; i++)
            {
                //Įstatome
                standardTable[0, i] = code[i];

                //Triname kątik įstatytą užkoduotą vektorių iš pool
                messagesPool = messagesPool.Where(el => el != code[i]).ToArray();
            }

            //Toliau einame nuo i = 1, nes su pirma eilute jau susitvarkėme
            for (int i = 1; i < tableHeight; i++)
            {
                //Gauname mažiausio svorio vektorių ir jį nustatome kaip klasės lyderį
                var minV = GetMinimalWeightVector(messagesPool);
                standardTable[i, 0] = minV;
                messagesPool = messagesPool.Where(el => el != minV).ToArray(); //triname iš pool

                //Einame per visas tos elutės reikšmes (stulpelis)
                for (int j = 1; j < tableWidth; j++)
                {
                    //Turime klasės lyderį ir kodo vektorių
                    //Jų suma = mūsų ieškomas vektorius
                    var classLeader = standardTable[i, 0];
                    var codeVector = standardTable[0, j];
                    var newV = Manager.AddVector(classLeader, codeVector); //Gauname vektorių sumą

                    //Įstatome
                    standardTable[i, j] = newV;

                    //Triname
                    messagesPool = messagesPool.Where(el => el != newV).ToArray();
                }
            }

            //Nustatome STD lentelę tolimesniam naudojimui
            Manager.StandardTable = standardTable;

            //Skaičiuojame sindromus
            //Ši lentelė susideda iš trijų stulpelių: klasės lyderis, sindromas, lyderio svoris
            var syndromes = new string[tableHeight, 3];

            for (int i = 0; i < tableHeight; i++)
            {
                //skaičiuojame sindromą
                var synd = Manager.CalculateSyndrome(standardTable[i, 0]);

                //Įstatome reikšmes į visus 3 tam tikros eilutės stulpelius
                syndromes[i, 0] = standardTable[i, 0]; //lyderis
                syndromes[i, 1] = synd; //apskaičiuotas sindromas
                syndromes[i, 2] = CalculateVectorWeight(standardTable[i, 0]).ToString(); //lyderio svoris
            }

            //set syndromes
            Manager.Syndromes = syndromes;
        }


        //Funkcija gražina visus galimus užkoduotus žodžius pagal parametrą N (q = 2, tai jau žinome)
        private string[] GenerateOrgCodeMessages()
        {
            //Gauname visas įmanomas žinutes, kurias galime koduoti
            var messages = GenerateMessages(Manager.InputK);
            var newMessages = new string[messages.Length];

            //Visas žinutes užkoduojame
            for (int i = 0; i < messages.Length; i++)
            {
                //Kodavimo funckijai paduodame matricą, kuria koduojame ir žinutę, kurią norime užkoduoti
                newMessages[i] = Manager.EncodeVector(Manager.MatrixG, messages[i]);
            }

            //Dabar galime nustatyti patį kodą, kurį naudosime vėliau
            //Jis susidaro iš dviejų stulpelių:
            //Pirmame - pradinis pranešimas
            //Antrame - užkoduotas pranešimas

            Manager.Code = new string[messages.Length, 2]; //Čia viską inicializuojame

            //Čia užpildome
            for (int i = 0; i < messages.Length; i++)
            {
                Manager.Code[i, 0] = messages[i];
                Manager.Code[i, 1] = newMessages[i];
            }

            //Grąžiname visus įmanomus užkoduotus žodžius
            return newMessages;
        }

        //Funkcija naudojama iš žinučių masyvo gauti pirmą pasitaikiusį mažiausio svorio pranešimą
        //Funkcijai paduodame visas žinutes (iš kurių mes norime gauti min svorio vekt.)
        //Grąžina - min. svorio vektorių
        public string GetMinimalWeightVector(string[] pool)
        {
            //Visą tai atliekame paprastu būdu - pirmą vektorių pradžioje laikome mažiausio svorio
            //O tada iteruojame masyvą ir jeigu randame mažesnio svorio - pakeičiame

            //Nustatome pradinį vektorių kaip mažiausio svorio
            string minVector = pool[0];
            int minWeight = CalculateVectorWeight(pool[0]);

            //Iteruojame ir ieškome mažesnių
            for (int i = 0; i < pool.Length; i++)
            {
                var weight = CalculateVectorWeight(pool[i]);
                if (weight < minWeight)
                {
                    //Radome mažesnį, tai nustatome naujas reikšmes
                    minWeight = weight;
                    minVector = pool[i];
                }
            }

            //Grąžiname mažiausio svorio vektorių
            return minVector;
        }


        //Funkcija apskaičiuoja vektoriaus svorį
        //Paduodame vektorių, gauname - jo svorį
        public int CalculateVectorWeight(string vector)
        {
            //pradinis svoris yra 0
            int weight = 0;

            //iteruojame vektorių
            for (int i = 0; i < vector.Length; i++)
            {
                //jei randame vienetą - padidiname svorį per vienetą
                if (vector[i] == '1')
                    weight++;
            }

            return weight;
        }

        //Generuojame visas tam tikro ilgio žinutes
        //Paduodame - ilgį, gauname - visų žinučių masyvą
        private string[] GenerateMessages(int length)
        {
            //masyvo dydis bus 2 pakelta ilgiu (2^x, kur x = paduotas ilgis) 
            string[] messages = new string[(int)Math.Pow(2, length)];

            //visus pranešimus galime gauti tiesiog koduodami dešimtainius skaičius dvejetainiais
            for (int i = 0; i < Math.Pow(2, length); i++)
            {
                //koduojame: dešimtainis -> dvejetainis
                messages[i] = Convert.ToString(i, 2);
                if (messages[i].Length < length)
                {
                    //Jeigu mūsų vektorius yra mažesnio ilgio už x (kur x = paduotas ilgis)
                    //Tuomet mes jo pradžioje pridedame papildomai 0
                    //Taip gaunasi nes funkcija Convert.ToString(x, 2) grąžina skiringo ilgio pranešimus
                    //Pvz.: '3' -> '11', jei ilgis nėra 2, vektoriui reikia pridėti nulių
                    messages[i] = (new string('0', length - messages[i].Length)) + messages[i];
                }
            }

            return messages;
        }

        //Jei vartotojas paspaudžia syndromų mygtuką - atidaromas langas su visa informacija
        private void SyndromesButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new SyndromesWindow();
            win.ShowDialog();
        }

        //Jei vartotojas paspaudžia standartinės lentelės mygtuką - atidaromas langas su visa informacija
        private void STDTableButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new StandardTableWindow();
            win.ShowDialog();
        }
    }
}
