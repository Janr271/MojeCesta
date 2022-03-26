using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MojeCesta.Services;
using MojeCesta.Models;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OdjezdyPage : ContentPage
    {
        public OdjezdyPage()
        {
            InitializeComponent();

            Datum.MinimumDate = DateTime.Now;
            Cas.Time = DateTime.Now.TimeOfDay;
            odjezdyViewModel = BindingContext as ViewModels.OdjezdyViewModel;
        }

        public ViewModels.OdjezdyViewModel odjezdyViewModel;

        private async void Hledat_Clicked(object sender, EventArgs e)
        {
            if (Promenne.SeznamZastavek == null || Promenne.SeznamLinek == null)
            {
                await DisplayAlert("Chyba", "Cache ještě nebyla načtena!", "OK");
                return;
            }

            if (String.IsNullOrEmpty(odjezdyViewModel.ZeZastavky.Stop_name))
            {
                await DisplayAlert("Chyba", "Nebyla zadána výchozí stanice!", "OK");
                return;
            }

            if (await Database.NajitZastavku(odjezdyViewModel.ZeZastavky.Stop_id) == null)
            {
                await DisplayAlert("Chyba", "Výchozí stanice nebyla nalezena!", "OK");
               
                return;
            }

            if (Cas.Time == null || Datum.Date == null)
            {
                await DisplayAlert("Chyba", "Nebyl zadán čas nebo datum!", "OK");
                return;
            }

            await Task.Run(() => odjezdyViewModel.NajitOdjezdy());

        }

        private void Nastaveni_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Historie_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is HistorieOdjezdu odjezd)
            {
                odjezdyViewModel.ZeZastavky = new Stop(odjezd.ZeZastavkyId, odjezd.ZeZastavky);

                Hledat_Clicked(sender, EventArgs.Empty);
            }
            else
            {
                throw new ArgumentException("Vybraný prvek musí být typu HistorieSpojení");
            }
        }

        private async void ZeZastavky_Focused(object sender, FocusEventArgs e)
        {
            ZeZastavkyO.Unfocus();
            HledaniPage hledani = new HledaniPage(this, odjezdyViewModel.ZeZastavky.Stop_name);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }
    }
}