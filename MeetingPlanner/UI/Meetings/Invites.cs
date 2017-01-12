using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Invites : ContentPage
    {
        public Invites()
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

