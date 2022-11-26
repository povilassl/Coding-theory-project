using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KodavimoTeorijaProjektas
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //Visus exception'us gaudome globaliai
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            base.OnStartup(e);
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Jei gavome exception, kviečiame funkciją, kuri jį parodo
            RunException(e.Exception);
            e.Handled = true;
        }

        //Funkcija, kuri parodo exceptionus per MessageBox (t.y. alert)
        //Ji gauna patį Exception kaip parametrą
        void RunException(Exception ex)
        {
            //Rodome exception ekrane
            MessageBox.Show("A handled exception occured, message: " + ex.Message,
                "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);

            //jei Exception turi vidinių Exception, jiems rekursiškai kviečiamę šią funkciją
            if (ex.InnerException != null)
                RunException(ex.InnerException);
        }
    }
}
