using Xamarin.Forms;
using MeetingPlanner.Languages;

namespace MeetingPlanner
{
    public class DataListView : ViewCell
    {
        public DataListView()
        {
            var lblTitle = new Label
            {
                Text = "title",
                TextColor = Constants.NELFTBlue,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };

            var lblDate = new Label
            {
                Text = "title",
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            var lblLength = new Label
            {
                Text = Langs.List_Length,
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            var lblLengthMins = new Label
            {
                Text = "title",
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            var lblRoom = new Label
            {
                Text = Langs.List_Room,
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            var lblRoomNumber = new Label
            {
                Text = "title",
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            var lblVenue = new Label
            {
                Text = "title",
                TextColor = Constants.NELFTBlue,
                FontSize = 14
            };

            lblTitle.SetBinding(Label.TextProperty, new Binding("MeetingName"));
            lblTitle.SetBinding(Label.TextColorProperty, new Binding("IsMine", converter: new BoolToColor()));

            lblDate.SetBinding(Label.TextProperty, new Binding("DateTimeFrom"));
            lblLengthMins.SetBinding(Label.TextProperty, new Binding("Length", stringFormat: "{0} mins"));
            lblRoomNumber.SetBinding(Label.TextProperty, new Binding("Room"));
            lblVenue.SetBinding(Label.TextProperty, new Binding("Venue"));

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(8, 0, 8, 8),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = Langs.NewAppt_Title,
                                TextColor = Constants.NELFTBlue,
                                FontSize = 14
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
                                WidthRequest = App.ScreenSize.Width * .5,
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label
                            {
                                Text = Langs.List_Date,
                                FontSize = 14,
                                TextColor = Constants.NELFTBlue
                            },
                            lblDate
                                }
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = {lblLength, lblLengthMins}
                            }
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout
                            {
                                WidthRequest = App.ScreenSize.Width * .5,
                                Children = {lblVenue}
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = {lblRoom, lblRoomNumber}
                            }
                        }
                    }
                }
            };
        }
    }
}

