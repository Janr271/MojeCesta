using System;
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
           await Task.Run(Services.Database.Inicializovat);
           await Task.Run(Services.AktualizaceDat.ZkontrolovatAktualizace);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
