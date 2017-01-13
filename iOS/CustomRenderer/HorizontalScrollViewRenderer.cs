using MeetingPlanner;
using MeetingPlanner.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HorizontalScrollView), typeof(HorizontalScrollViewRenderer))]
namespace MeetingPlanner.iOS
{
    public class HorizontalScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (this.NativeView == null)
            {
                return;
            }

            var scrollView = (UIScrollView)this.NativeView;

            scrollView.Bounces = false;
            scrollView.ShowsHorizontalScrollIndicator = false;
        }
    }
}
