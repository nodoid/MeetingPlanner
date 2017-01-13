using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MeetingPlanner.Languages;
using System.Linq;

namespace MeetingPlanner
{
    public class CancelAppt : ContentView
    {
        ObservableCollection<AppointmentList> apptsList = new ObservableCollection<AppointmentList>();
        public CancelAppt(List<int> aptId)
        {
            foreach (var apt in aptId)
                apptsList.Add(App.Self.DBManager.GetSingleObject<AppointmentList>("id", apt.ToString()));

            var listView = new ListView
            {
                ItemsSource = apptsList,
                ItemTemplate = new DataTemplate(typeof(DeleteViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                MinimumWidthRequest = App.ScreenSize.Width * .95,
                WidthRequest = App.ScreenSize.Width * .95,
                HasUnevenRows = true
            };

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(12, 12),
                Children = { listView }
            };

            App.Self.MessageEvents.Change += (s, ea) =>
            {
                if (ea.ModuleName == "DeleteRefresh")
                {
                    listView.ItemsSource = null;
                    apptsList.Remove(apptsList.FirstOrDefault(t => t.id == Convert.ToInt32(ea.Message)));
                    if (apptsList.Count > 0)
                        listView.ItemsSource = apptsList;
                }
            };
        }
    }

    public class DeleteViewCell : ViewCell
    {
        public DeleteViewCell()
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

            /*var swtchOrganiser = new Switch
            {
                IsEnabled = true,
            };

            swtchOrganiser.Toggled += async (object sender, ToggledEventArgs e) =>
            {
                var swtch = sender as Switch;
                if (swtch.IsToggled)
                {
                    if (await App.Self.MainPage.DisplayAlert(Langs.Cancel_Title, Langs.Cancel_Message, Langs.General_OK, Langs.General_Cancel))
                    {
                        App.Self.DBManager.DeleteObject<AppointmentList>(Convert.ToInt32(swtch.ClassId));
                        App.Self.MessageEvents.BroadcastIt("DeleteRefresh", swtch.ClassId);
                    }
                    else
                        swtch.IsToggled = false;
                }
            };

            swtchOrganiser.SetBinding(Switch.IsToggledProperty, new Binding("IsOrganiser", converter: new BoolToString()));
            swtchOrganiser.SetBinding(Switch.ClassIdProperty, new Binding("id", converter: new IntToString()));*/

            var chkOrganiser = new Checkbox
            {
                IsEnabled = true,
            };

            chkOrganiser.SetBinding(Checkbox.CheckedProperty, new Binding("IsOrganiser", converter: new BoolToString()));
            chkOrganiser.SetBinding(Checkbox.ClassIdProperty, new Binding("id", converter: new IntToString()));

            chkOrganiser.CheckedChanged += async (object sender, CheckedChangedEventArgs e) =>
            {
                var swtch = sender as Checkbox;
                if (swtch.Checked)
                {
                    if (await App.Self.MainPage.DisplayAlert(Langs.Cancel_Title, Langs.Cancel_Message, Langs.General_OK, Langs.General_Cancel))
                    {
                        App.Self.DBManager.DeleteObject<AppointmentList>(Convert.ToInt32(swtch.ClassId));
                        App.Self.MessageEvents.BroadcastIt("DeleteRefresh", swtch.ClassId);
                    }
                    else
                        swtch.Checked = false;
                }
            };

            lblTitle.SetBinding(Label.TextProperty, new Binding("MeetingName"));
            lblTitle.SetBinding(Label.TextColorProperty, new Binding("IsMine", converter: new BoolToColor()));

            lblDate.SetBinding(Label.TextProperty, new Binding("DateTimeFrom"));
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
                                Children =
                                {
                                    new Label
                                    {
                                        Text = Langs.List_Delete,
                                        FontSize = 14,
                                        TextColor = Constants.NELFTBlue
                                    },
                                    chkOrganiser
                                }
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

