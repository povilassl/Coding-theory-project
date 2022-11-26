using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using KodavimoTeorijaProjektas.Pages;

namespace KodavimoTeorijaProjektas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Page Page1 { get; set; }
        public Page Page2 { get; set; }
        public Page Page3 { get; set; }

        public MainWindow()
        {
            Page1 = new UserInputPage();
            Page2 = new EncodingPage();
            Page3 = new DecodingPage();

            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageFrame.Content = Page1;
            PreviousButton.Visibility = Visibility.Hidden;
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainPageFrame.Content == Page2)
            {
                MainPageFrame.Content = Page1;
                PreviousButton.Visibility = Visibility.Hidden;
                NextButton.Visibility = Visibility.Visible;
            }
            else if (MainPageFrame.Content == Page3)
            {
                MainPageFrame.Content = Page2;
                PreviousButton.Visibility = Visibility.Visible;
                NextButton.Visibility = Visibility.Visible;
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainPageFrame.Content == Page1)
            {
                MainPageFrame.Content = Page2;
                PreviousButton.Visibility = Visibility.Visible;
                NextButton.Visibility = Visibility.Visible;
            }
            else if (MainPageFrame.Content == Page2)
            {
                MainPageFrame.Content = Page3;
                PreviousButton.Visibility = Visibility.Visible;
                NextButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
