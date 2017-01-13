
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using HockeyApp.Android;
using Xamarin.Forms;

namespace MeetingPlanner.Droid
{
    [Activity(Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Activity Active { get; private set; }

        public static ISharedPreferences Prefs { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            Prefs = GetSharedPreferences("MeetingPlanner", FileCreationMode.Private);

            Active = this;

            base.OnCreate(bundle);

            CrashManager.Register(this, "5a553b0759b843aeb2e9cd49a41d0269");
            UpdateManager.Register(this, "5a553b0759b843aeb2e9cd49a41d0269"); global::Xamarin.Forms.Forms.Init(this, bundle);

            App.ScreenSize = new Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density,
                Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();

            //Start Tracking usage in this activity
            Tracking.StartUsage(this);
        }

        protected override void OnPause()
        {
            //Stop Tracking usage in this activity
            Tracking.StopUsage(this);

            base.OnPause();
        }
    }
}
