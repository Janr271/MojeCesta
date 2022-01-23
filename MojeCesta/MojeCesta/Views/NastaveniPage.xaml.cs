using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            AktualizaceDat.Text = DateTime.Now.ToString("d");
            PlatnostDat.Text = DateTime.Now.AddDays(7).ToString("d");
            Verze.Text = VersionTracking.CurrentVersion;
        }

        private void Rezim_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Aktualizace_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Wifi_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Aktualizovat_Clicked(object sender, EventArgs e)
        {

        }

        public void OnAktualizovat()
        {

        }
    }
}