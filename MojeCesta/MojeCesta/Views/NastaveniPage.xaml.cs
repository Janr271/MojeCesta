using System;
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

            Verze.Text = VersionTracking.CurrentVersion;
        }
    }
}