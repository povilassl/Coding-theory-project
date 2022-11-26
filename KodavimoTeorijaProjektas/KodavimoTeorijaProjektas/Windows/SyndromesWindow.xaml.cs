using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KodavimoTeorijaProjektas.Windows
{
    /// <summary>
    /// Interaction logic for SyndromesWindow.xaml
    /// </summary>
    public partial class SyndromesWindow : Window
    {
        public SyndromesWindow()
        {
            InitializeComponent();

            var synd = Manager.Syndromes;

            if (synd.Length == 0) return;

            var tableHeight = synd.GetLength(0);
            var tableWidth = 3;

            List<List<string>> lsts = new();

            for (int i = 0; i < tableHeight; i++)
            {
                lsts.Add(new List<string>());

                for (int j = 0; j < tableWidth; j++)
                {
                    lsts[i].Add(synd[i, j]);
                }
            }

            DataGrid.ItemsSource = lsts;
        }
    }
}
