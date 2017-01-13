using Xamarin.Forms;
using MeetingPlanner;
using MeetingPlanner.Droid;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MeetingPlanner.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Background = Resources.GetDrawable(Resource.Drawable.EntryBorder);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical | Android.Views.GravityFlags.Left;
                Control.SetPadding(12, 0, 12, 0);
            }
        }
    }
}
