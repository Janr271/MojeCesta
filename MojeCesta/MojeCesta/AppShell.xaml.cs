using MojeCesta.Views;
using Xamarin.Forms;

namespace MojeCesta
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Inicializace cest pro správce oken
            Routing.RegisterRoute(nameof(SpojeniPage), typeof(SpojeniPage));
            Routing.RegisterRoute(nameof(OdjezdyPage), typeof(OdjezdyPage));
            Routing.RegisterRoute(nameof(NastaveniPage), typeof(NastaveniPage));
            Routing.RegisterRoute(nameof(VysledkySpojeniPage), typeof(VysledkySpojeniPage));
            Routing.RegisterRoute(nameof(VysledkyOdjezduPage), typeof(VysledkyOdjezduPage));
        }

    }
}
