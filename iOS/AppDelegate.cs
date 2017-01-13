
using Foundation;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using ObjCRuntime;
using PushNotification.Plugin;
using UIKit;
using Xamarin.Forms;

namespace MeetingPlanner.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static AppDelegate Self { get; private set; }

        public bool IsRetina { get; private set; }

        public bool IsIPhone { get; private set; }

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            MobileCenter.Start("2adc4118-bf63-424f-b486-a194841e58dd",
                    typeof(Analytics), typeof(Crashes));

            AppDelegate.Self = this;
            CheckboxRenderer.Init();
            CrossPushNotification.Initialize<CrossPushNotificationListener>();
            global::Xamarin.Forms.Forms.Init();
            IsIPhone = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

            IsRetina = UIScreen.MainScreen.RespondsToSelector(new Selector("scale")) ? true : false;
            App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
