using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MeetingPlanner.Languages;

namespace MeetingPlanner
{
    public class Appointments : ContentView
    {
        ObservableCollection<AppointmentList> appts;

        public Appointments(List<int> apptIds)
        {
            appts = new ObservableCollection<AppointmentList>();

            foreach (var id in apptIds)
                appts.Add(App.Self.DBManager.GetSingleObject<AppointmentList>("id", id.ToString()));

            var listView = new ListView
            {
                ItemsSource = appts,
                HasUnevenRows = true,
                WidthRequest = App.ScreenSize.Width * .95,
                HeightRequest = App.ScreenSize.Height * .7,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(DataListView)),
            };
            listView.ItemTapped += ListView_ItemTapped;

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { listView }
            };
        }

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tap = e.Item as AppointmentList;
            var listAttend = tap.Attendees;
        }
    }

    public class DataListView : ViewCell
    {
        public DataListView()
        {
            var lblTitle = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold
            };

            var lblDate = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblLength = new Label
            {
                Text = Langs.List_Length,
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblLengthMins = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblRoom = new Label
            {
                Text = Langs.List_Room,
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblRoomNumber = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblVenue = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
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
                    lblTitle,
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
                                TextColor = Color.Blue
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

