using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class DateTimeSelector : ContentPage
    {
        public DateTimeSelector()
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

