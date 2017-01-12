using System;

using Xamarin.Forms;

namespace MeetingPlanner.Droid
{
    public class SliderViewRenderer : ContentPage
    {
        public SliderViewRenderer()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

