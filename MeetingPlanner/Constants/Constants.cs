using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Constants : ContentPage
    {
        public Constants()
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

