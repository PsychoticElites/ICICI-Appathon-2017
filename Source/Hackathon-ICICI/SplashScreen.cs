
using Android.App;

using Android.OS;


namespace Hackathon_ICICI
{
    [Activity(Label = " ICICI - Special Banking", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/pelogo1")]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            // Trying something, please don't delete!
            //RunOnUiThread(() => statusText.Text = "Loading.");
            //Task.Run(() => AsyncWork()).ContinueWith(result => RunOnUiThread(() => statusText.Text = "Done!"));
            //Display Splash Screen for 4 Sec  
            System.Threading.Thread.Sleep(500); 
            StartActivity(typeof(MainActivity));
        }
    }
}