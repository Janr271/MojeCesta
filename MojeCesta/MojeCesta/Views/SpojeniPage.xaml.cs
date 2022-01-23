using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpojeniPage : ContentPage
    {
        public SpojeniPage()
        {
            InitializeComponent();

            Datum.MinimumDate = DateTime.Now;
            Odjezd.IsChecked = true;
        }

        public void Historie_Tapped(object sender, ItemTappedEventArgs e)
        {

        }

        private async void Nastaveni_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Zmena_Clicked(object sender, EventArgs e)
        {
            string tmp = ZeZastavky.Text;
            ZeZastavky.Text = NaZastavku.Text;
            NaZastavku.Text = tmp;
        }

        private void Upresnit_Clicked(object sender, EventArgs e)
        {

        }

        private void Hledat_Clicked(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(ZeZastavky.Text) && !String.IsNullOrEmpty(NaZastavku.Text))
            {
                DateTime cas = new DateTime(2022, 1, 1);
                cas += Cas.Time;
                Services.Spojeni.NajitSpojeni(Services.Database.NajitZastavku(ZeZastavky.Text).Result, Services.Database.NajitZastavku(NaZastavku.Text).Result, cas, Odjezd.IsChecked);
            }

            Shell.Current.GoToAsync(nameof(VysledkySpojeniPage));
        }
    }
}