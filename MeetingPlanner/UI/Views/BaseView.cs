using System;
using Xamarin.Forms;
namespace MeetingPlanner
{
    public class BaseView : ContentView
    {
        Image bgImage { get; set; } = new Image { Source = Constants.BackgroundFilename, Aspect = Aspect.Fill };

        public RelativeLayout CreateContent(StackLayout masterStack)
        {
            var relativeLayout = new RelativeLayout();

            relativeLayout.Children.Add(bgImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Height));

            relativeLayout.Children.Add(masterStack,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Width),
                Constraint.RelativeToParent((parent) => App.ScreenSize.Height));

            return relativeLayout;
        }
    }
}
