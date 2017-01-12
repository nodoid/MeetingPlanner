using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Attending : ContentPage
    {
        public Attending()
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

