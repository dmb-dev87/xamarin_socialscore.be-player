using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;

namespace SocialScore.Droid
{
    [Activity(Theme = "@style/AppTheme.Logo", MainLauncher = true, NoHistory = true)]
    public class LogoActivity : Activity
    {
        static readonly string TAG = "X:" + typeof(LogoActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await Task.Delay(1000);
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}