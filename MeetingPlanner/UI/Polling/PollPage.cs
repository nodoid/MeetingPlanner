using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using System.Linq;
using Rg.Plugins.Popup.Extensions;

namespace MeetingPlanner
{
    public class PollPage : PopupPage
    {
        public PollPage(AppointmentList item)
        {
            var attendees = MeetingHelper.Invited(item.MeetingId).Count;

            var voteStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = App.ScreenSize.Height * .7,
            };

            foreach (var date in AppointmentListHelpers.PollList(item.MeetingId))
                voteStack.Children.Add(GenerateStack(date, attendees));

            Content = voteStack;
        }

        async void OnClose(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        StackLayout GenerateStack(Polling info, int attendees)
        {
            var attend = MeetingHelper.Invited(info.MeetingId).Count(t => t.Attending == 1);
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout
                    {
                        WidthRequest = App.ScreenSize.Width * .4,
                        Children =
                        {
                            new Label
                            {
                                Text = info.SuggestedDate.ToString("g"),
                                LineBreakMode = LineBreakMode.WordWrap,
                                TextColor = Constants.NELFTBlue,
                                FontSize = Constants.GeneralFontSize
                            },
                            new ProgressBar
                            {
                                WidthRequest = App.ScreenSize.Width * .6,
                                IsEnabled = false,
                                Progress = (double)(attend/attendees)
                            }
                        }
                    }
                }
            };

            return stack;
        }
    }
}

