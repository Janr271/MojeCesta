using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;

namespace MojeCesta.Droid
{
    [Activity(
        Label = "MojeCesta",
        Icon = "@mipmap/ikona",
        Theme = "@style/MainTheme.Launcher",
        MainLauncher = true,
        NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LauncherActivity : Activity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Spustit hlavní aplikaci
        protected async override void OnResume()
        {
            base.OnResume();
            await Services.Database.Inicializovat();
            await Task.Run(() => StartActivity(new Intent(Application.Context, typeof(MainActivity))));
        }

        // Ochrana proti stisknutí tlačítka zpět
        public override void OnBackPressed() { }
    }
}