using MojeCesta.Services;
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
            Cas.Time = DateTime.Now.TimeOfDay;
            PocetPrestupu.Value = 3;
            Odjezd.IsChecked = true;
            spojeniViewModel = BindingContext as ViewModels.SpojeniViewModel;
        }

        public ViewModels.SpojeniViewModel spojeniViewModel;

        public void Historie_Tapped(object sender, ItemTappedEventArgs e)
        {
            Tuple<string, string> hodnota = (Tuple<string, string>)e.Item;
            spojeniViewModel.ZeZastavky = hodnota.Item1;
            spojeniViewModel.NaZastavku = hodnota.Item2;

            Hledat_Clicked(sender, EventArgs.Empty);
        }

        private async void Nastaveni_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Zmena_Clicked(object sender, EventArgs e)
        {
            string tmp = spojeniViewModel.ZeZastavky;
            spojeniViewModel.ZeZastavky = spojeniViewModel.NaZastavku;
            spojeniViewModel.NaZastavku = tmp;
        }

        private async void Hledat_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(spojeniViewModel.ZeZastavky))
            {
                await DisplayAlert("Chyba", "Nebyla zadána výchozí stanice!", "OK");
                return;
            }

            if (String.IsNullOrEmpty(spojeniViewModel.NaZastavku))
            {
                await DisplayAlert("Chyba", "Nebyla zadána cílová stanice!", "OK");
                return;
            }

            if (await Database.NajitZastavku(spojeniViewModel.ZeZastavky) == null)
            {
                await DisplayAlert("Chyba", "Výchozí stanice nebyla nalezena!", "OK");

                return;
            }

            if (await Database.NajitZastavku(spojeniViewModel.NaZastavku) == null)
            {
                await DisplayAlert("Chyba", "Cílová stanice nebyla nalezena!", "OK");

                return;
            }

            if (Cas.Time == null || Datum.Date == null)
            {
                await DisplayAlert("Chyba", "Nebyl zadán čas nebo datum!", "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(VysledkyOdjezduPage));

            await Task.Run(() => spojeniViewModel.NajitSpojeni());

        }

        private async void ZeZastavkyS_Focused(object sender, FocusEventArgs e)
        {
            ZeZastavkyS.Unfocus();
            HledaniPage hledani = new HledaniPage(this, spojeniViewModel.ZeZastavky);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }

        private async void NaZastavkuS_Focused(object sender, FocusEventArgs e)
        {
            NaZastavkuS.Unfocus();
            HledaniPage hledani = new HledaniPage(this, spojeniViewModel.NaZastavku, true);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }
    }
}