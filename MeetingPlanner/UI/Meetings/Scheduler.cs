using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Scheduler : ContentPage
    {
        public Scheduler()
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

