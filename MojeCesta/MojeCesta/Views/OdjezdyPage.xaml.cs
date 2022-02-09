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
    public partial class OdjezdyPage : ContentPage
    {
        public OdjezdyPage()
        {
            InitializeComponent();

            Datum.MinimumDate = DateTime.Now;
        }

        private void Hledat_Clicked(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(ZeZastavky.Text))
            {
                Services.Odjezdy.NajitOdjezdy(Services.Database.NajitZastavku(ZeZastavky.Text).Result, Cas.Time);
            }

            Shell.Current.GoToAsync(nameof(VysledkyOdjezduPage));
        }

        private void Nastaveni_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Historie_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}