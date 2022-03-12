using Xamarin.Forms;
using MojeCesta.Services;

namespace MojeCesta
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Promenne.AplikovatStyl();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            Promenne.AplikovatStyl();
        }
    }
}
