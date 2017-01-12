using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class AppointmentList : ContentPage
    {
        public AppointmentList()
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

