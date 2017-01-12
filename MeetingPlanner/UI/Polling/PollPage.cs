using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class PollPage : ContentPage
    {
        public PollPage()
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

