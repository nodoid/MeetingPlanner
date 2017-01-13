using MeetingPlanner;
using MeetingPlanner.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace MeetingPlanner.iOS
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var btn = Control as UIButton;
                btn.TitleLabel.LineBreakMode = UILineBreakMode.WordWrap;
            }
        }
    }
}
