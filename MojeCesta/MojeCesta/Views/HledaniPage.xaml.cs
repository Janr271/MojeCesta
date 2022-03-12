using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MojeCesta.Services;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HledaniPage : ContentPage
    {
        private Page Rodic { get; set; }
        private bool NaZastavku { get; set; }

        public HledaniPage(Page rodic, string text, bool naZastavku = false)
        {
            InitializeComponent();

            Rodic = rodic;
            NaZastavku = naZastavku;
            Hledani.Text = text;
        }

        private async void Hledani_Changed(object sender, TextChangedEventArgs e)
        {
            SeznamStanic.ItemsSource = await Database.HledaniStanicePodleJmena(Hledani.Text);
        }

        private async void Hledani_Completed(object sender, System.EventArgs e)
        {
            // Vrátit hodnotu zpět volajícímu
            if (Rodic is OdjezdyPage odjezdy)
            {
                odjezdy.odjezdyViewModel.ZeZastavky = Hledani.Text;
            }
            else if (Rodic is SpojeniPage spojeni)
            {
                if (NaZastavku)
                {
                    spojeni.spojeniViewModel.NaZastavku = Hledani.Text;
                }
                else
                {
                    spojeni.spojeniViewModel.ZeZastavky = Hledani.Text;
                }
            }

            // Zavřít okno
            await Navigation.PopModalAsync(false);
        }

        private async void SeznamStanic_Tapped(object sender, ItemTappedEventArgs e)
        {
            // Vrátit hodnotu zpět volajícímu
            if (Rodic is OdjezdyPage odjezdy)
            {
                odjezdy.odjezdyViewModel.ZeZastavky = e.Item.ToString();
            }
            else if (Rodic is SpojeniPage spojeni)
            {
                if (NaZastavku)
                {
                    spojeni.spojeniViewModel.NaZastavku = e.Item.ToString();
                }
                else
                {
                    spojeni.spojeniViewModel.ZeZastavky = e.Item.ToString();
                }
            }

            // Zavřít okno
            await Navigation.PopModalAsync(false);
        }

        public void VybratEntry()
        {
            Hledani.Focus();
            Hledani.CursorPosition = Hledani.Text.Length;
        }
    }
}