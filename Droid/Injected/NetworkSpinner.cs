using WODTasticMobile.Droid;
using Xamarin.Forms;
using Android.App;

[assembly: Dependency(typeof(NetworkSpinner))]
namespace WODTasticMobile.Droid
{
    public class NetworkSpinner : INetworkSpinner
    {
        static ProgressDialog progress;

        public void ChangeMessage(string newMessage)
        {
            if (progress != null)
                MainActivity.Active.RunOnUiThread(delegate { progress.SetMessage(newMessage); });
        }

        public void Spinner(bool on, string title, string message)
        {
            if (on)
            {
                MainActivity.Active.RunOnUiThread(delegate
                {
                    progress = ProgressDialog.Show(Forms.Context, title, message);
                });
            }
            else
            {
                MainActivity.Active.RunOnUiThread(delegate
                {
                    progress.Dismiss();
                });
            }
        }
    }
}

