using System.Diagnostics;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;

namespace MeetingPlanner
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        //Here you will receive all push notification messages
        //Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
        public void OnMessage(JObject parameters, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
        }
        //Gets the registration token after push registration
        public void OnRegistered(string Token, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", Token));
        }
        //Fires when device is unregistered
        public void OnUnregistered(PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");

        }

        //Fires when error
        public void OnError(string message, PushNotification.Plugin.Abstractions.DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        //Enable/Disable Showing the notification
        public bool ShouldShowNotification()
        {
            return true;
        }
    }
}
