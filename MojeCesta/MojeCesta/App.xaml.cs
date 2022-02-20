using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MojeCesta
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
           await Services.Database.Inicializovat();
           await Task.Run(() => Services.AktualizaceDat.Aktualizovat(false));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
