using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MojeCesta.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AktualizacePage : ContentPage
    {
        public AktualizacePage(bool vynucena)
        {
            InitializeComponent();

            if (vynucena)
            {
                Shell.SetNavBarIsVisible(this, false);
            }
            else
            {
                Shell.SetNavBarIsVisible(this, true);
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Services.AktualizaceDat.Aktualizovat(this);
        }

        public void Zavrit()
        {
            Aktivita.IsRunning = false;
            Navigation.PopModalAsync();
        }
    }
}