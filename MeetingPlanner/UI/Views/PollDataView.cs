using Xamarin.Forms;
using MeetingPlanner.Languages;

namespace MeetingPlanner
{
    public class PollDataView : ViewCell
    {
        public PollDataView()
        {
            var lblTitle = new Label
            {
                Text = "fish",
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            var lblVenue = new Label
            {
                Text = "fish",
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            var lblRoom = new Label
            {
                Text = "fish",
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            var lblInvited = new Label
            {
                Text = "fish",
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            lblTitle.SetBinding(Label.TextProperty, new Binding("MeetingName"));
            lblVenue.SetBinding(Label.TextProperty, new Binding("Venue"));
            lblRoom.SetBinding(Label.TextProperty, new Binding("Room"));
            lblInvited.SetBinding(Label.TextProperty, new Binding("InvitedTotal", converter: new IntToString()));

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Poll_Title,
                                FontSize = Constants.GeneralFontSize,
                                TextColor = Constants.NELFTBlue,
                            },
                            lblTitle,
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .6,
                                Children = {lblVenue}
                            },
                            new StackLayout
                            {
                                Children = {lblRoom}
                            }
                        }
                        },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.Poll_Invited,
                                FontSize = Constants.GeneralFontSize,
                                TextColor = Constants.NELFTBlue,
                            },
                            lblInvited
                        }
                    }
                }
            };
        }
    }
}

