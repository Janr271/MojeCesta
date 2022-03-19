using MojeCesta.Models;
using MojeCesta.Services;
using System;
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
            if(e.Item is HistorieSpojeni spojeni)
            {
                spojeniViewModel.ZeZastavky = new Stop(spojeni.ZeZastavkyId, spojeni.ZeZastavky);
                spojeniViewModel.NaZastavku = new Stop(spojeni.NaZastavkuId, spojeni.NaZastavku);

                Hledat_Clicked(sender, EventArgs.Empty);
            }
            else
            {
                throw new ArgumentException("Vybraný prvek musí být typu HistorieSpojení");
            }
        }

        private async void Nastaveni_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NastaveniPage));
        }

        private void Zmena_Clicked(object sender, EventArgs e)
        {
            var tmp = spojeniViewModel.ZeZastavky;
            spojeniViewModel.ZeZastavky = spojeniViewModel.NaZastavku;
            spojeniViewModel.NaZastavku = tmp;
        }

        private async void Hledat_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(spojeniViewModel.ZeZastavky.Stop_name))
            {
                await DisplayAlert("Chyba", "Nebyla zadána výchozí stanice!", "OK");
                return;
            }

            if (String.IsNullOrEmpty(spojeniViewModel.NaZastavku.Stop_name))
            {
                await DisplayAlert("Chyba", "Nebyla zadána cílová stanice!", "OK");
                return;
            }

            if (await Database.NajitZastavku(spojeniViewModel.ZeZastavky.Stop_id) == null)
            {
                await DisplayAlert("Chyba", "Výchozí stanice nebyla nalezena!", "OK");

                return;
            }

            if (await Database.NajitZastavku(spojeniViewModel.NaZastavku.Stop_id) == null)
            {
                await DisplayAlert("Chyba", "Cílová stanice nebyla nalezena!", "OK");

                return;
            }

            if (Cas.Time == null || Datum.Date == null)
            {
                await DisplayAlert("Chyba", "Nebyl zadán čas nebo datum!", "OK");
                return;
            }

            await Task.Run(() => spojeniViewModel.NajitSpojeni());

        }

        private async void ZeZastavkyS_Focused(object sender, FocusEventArgs e)
        {
            ZeZastavkyS.Unfocus();
            HledaniPage hledani = new HledaniPage(this, spojeniViewModel.ZeZastavky.Stop_name);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }

        private async void NaZastavkuS_Focused(object sender, FocusEventArgs e)
        {
            NaZastavkuS.Unfocus();
            HledaniPage hledani = new HledaniPage(this, spojeniViewModel.NaZastavku.Stop_name, true);
            await Navigation.PushModalAsync(hledani, false);
            hledani.VybratEntry();
        }
    }
}