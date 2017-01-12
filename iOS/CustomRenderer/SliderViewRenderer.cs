using System;

using Xamarin.Forms;

namespace MeetingPlanner.iOS
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

