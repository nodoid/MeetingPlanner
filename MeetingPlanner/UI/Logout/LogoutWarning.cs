using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class LogoutWarning : ContentPage
    {
        public LogoutWarning()
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

