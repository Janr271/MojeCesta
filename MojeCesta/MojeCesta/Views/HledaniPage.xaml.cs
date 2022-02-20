using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MojeCesta.Services;
using MojeCesta.Models;

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

        private void Hledani_Changed(object sender, TextChangedEventArgs e)
        {
            SeznamStanic.ItemsSource = Database.NajitStanici(Hledani.Text).Result;
        }

        private async void SeznamStanic_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (Rodic is OdjezdyPage)
            {
                ((OdjezdyPage)Rodic).odjezdyViewModel.ZeZastavky = e.Item.ToString();
            }
            else if (Rodic is SpojeniPage)
            {
                if (NaZastavku)
                {
                    ((SpojeniPage)Rodic).spojeniViewModel.NaZastavku = e.Item.ToString();
                }
                else
                {
                    ((SpojeniPage)Rodic).spojeniViewModel.ZeZastavky = e.Item.ToString();
                }
            }

            await Navigation.PopModalAsync(false);
        }

        public void VybratEntry()
        {
            Hledani.Focus();
        }
    }
}