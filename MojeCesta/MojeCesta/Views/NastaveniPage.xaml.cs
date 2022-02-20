using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NastaveniPage : ContentPage
    {
        public NastaveniPage()
        {
            InitializeComponent();

            Aktualizace.IsToggled = Services.AktualizaceDat.AutomatickaAktualizace;
            Wifi.IsToggled = Services.AktualizaceDat.PouzeWifi;
            AktualizaceDat.Text = Services.AktualizaceDat.PosledniAktualizace.ToString();
            PlatnostDat.Text = Services.Database.InformaceODatabazi().Result.Feed_end_date.ToShortDateString();
            Verze.Text = VersionTracking.CurrentVersion;
        }

        private void Rezim_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Aktualizace_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Services.AktualizaceDat.AutomatickaAktualizace = Aktualizace.IsToggled;
        }

        private void Wifi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Services.AktualizaceDat.PouzeWifi = Wifi.IsToggled;
        }

        private async void Aktualizovat_Clicked(object sender, EventArgs e)
        {
            await Task.Run(() => Services.AktualizaceDat.Aktualizovat(true));
        }
    }
}