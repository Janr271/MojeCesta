using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MojeCesta.Services;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OdjezdyPage : ContentPage
    {
        public OdjezdyPage()
        {
            InitializeComponent();

            ZeZastavkyO.Unfocus();
            Datum.MinimumDate = DateTime.Now;
            Cas.Time = DateTime.Now.TimeOfDay;
            odjezdyViewModel = BindingContext as ViewModels.OdjezdyViewModel;
        }

        public ViewModels.OdjezdyViewModel odjezdyViewModel;

        private async void Hledat_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(odjezdyViewModel.ZeZastavky))
            {
                await DisplayAlert("Chyba", "Nebyla zadána výchozí stanice!", "OK");
                return;
            }

            if (await Database.NajitZastavku(odjezdyViewModel.ZeZastavky) == null)
            {
                await DisplayAlert("Chyba", "Výchozí stanice nebyla nalezena!", "OK");
               
                return;
            }

            if (Cas.Time == null || Datum.Date == null)
            {
                await DisplayAlert("Chyba", "Nebyl zadán čas nebo datum!", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(VysledkyOdjezduPage));

            await Task.Run(() => odjezdyViewModel.NajitOdjezdy());
        }

        private void Nastaveni_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Historie_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            odjezdyViewModel.ZeZastavky = e.Item.ToString();
            Hledat_Clicked(sender, EventArgs.Empty);
        }

        private async void ZeZastavky_Focused(object sender, FocusEventArgs e)
        {
            ZeZastavkyO.Unfocus();
            HledaniPage hledani = new HledaniPage(this, odjezdyViewModel.ZeZastavky);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }
    }
}