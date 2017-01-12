using System;

using Xamarin.Forms;

namespace MeetingPlanner
{
    public class NewMeeting : BasePage
    {
        public NewMeeting()
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

