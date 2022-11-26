using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
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
    /// Interaction logic for StandardTableWindow.xaml
    /// </summary>
    public partial class StandardTableWindow : Window
    {
        public StandardTableWindow()
        {
            InitializeComponent();

            var table = Manager.StandardTable;

            if (table.Length == 0) return;

            var tableHeight = (int)Math.Pow(2, Manager.InputN - Manager.InputK);
            var tableWidth = (int)(Math.Pow(2, Manager.InputN) / tableHeight);

            List<List<string>> lsts = new();

            for (int i = 0; i < tableHeight; i++)
            {
                lsts.Add(new List<string>());

                for (int j = 0; j < tableWidth; j++)
                {
                    lsts[i].Add(table[i, j]);
                }
            }

            DataGrid.ItemsSource = lsts;


        }
    }
}
