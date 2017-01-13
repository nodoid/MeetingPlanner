using System;

using Xamarin.Forms;
using MeetingPlanner.Languages;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MeetingPlanner
{
    public class NewMeeting : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;
        ObservableCollection<MeetingSched> meetings = new ObservableCollection<MeetingSched>();
        ObservableCollection<AllAttendees> attendees = new ObservableCollection<AllAttendees>();

        public NewMeeting()
        {
            CreateUI();
        }

        void CreateUI()
        {
            int selDate = 0, selTime = 0, selDur = 0;

            stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            innerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
            };

            var topbar = new TopBar(Langs.NewAppt_Title, this, "icoback", "icomenu", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var imgStaff = new Image
            {
                Source = "staff",
                HeightRequest = 32,
                WidthRequest = 32,
            };
            var imgEmail = new Image
            {
                Source = "email",
                HeightRequest = 32,
                WidthRequest = 32,
            };
            var imgPlus = new Image
            {
                Source = "plus",
                HeightRequest = 32,
                WidthRequest = 32,
            };

            var lblInternalEmail = new Label
            {
                Text = Langs.NewAppt_Internal,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
            };
            var lblExternalEmail = new Label
            {
                Text = Langs.NewAppt_External,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
            };
            var lblProposedDates = new Label
            {
                Text = Langs.NewAppt_ProposedDates,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
            };
            var btnSend = new Button
            {
                Text = Langs.NewAppt_Send,
                BorderRadius = 5,
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
            };

            var entExternalNames = new CustomEntry
            {
                HeightRequest = App.ScreenSize.Height * .23,
                MinimumHeightRequest = App.ScreenSize.Height * .23,
                WidthRequest = App.ScreenSize.Width * .9,
                MinimumWidthRequest = App.ScreenSize.Width * .9,
                Placeholder = Langs.NewAppt_ExternalEmail,
                Keyboard = Keyboard.Email
            };

            var lstDate = new ListView
            {
                ItemsSource = meetings,
                HeightRequest = App.ScreenSize.Height * .23,
                MinimumHeightRequest = App.ScreenSize.Height * .23,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(CurrentDates))
            };

            var lstStaff = new ListView
            {
                ItemsSource = attendees,
                HeightRequest = App.ScreenSize.Height * .23,
                MinimumHeightRequest = App.ScreenSize.Height * .23,
                WidthRequest = App.ScreenSize.Width,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(StaffAttendeeList))
            };

            var times = new List<string> { "8.00", "8.30", "9.00", "9.30", "10.00", "10.30", "11.00", "11.30", "12.00", "12.30", "1.00", "1.30", "2.00", "2.30", "3.00", "3.30", "4.00", "4.30", "5.00", "5.30", };
            var pickerTime = new Picker
            {
                WidthRequest = App.ScreenSize.Width * .2
            };
            pickerTime.Items.Add(Langs.NewAppt_Time);
            foreach (var t in times)
                pickerTime.Items.Add(t);

            var pickerDate = new Picker
            {
                WidthRequest = App.ScreenSize.Width * .4
            };
            pickerDate.Items.Add(Langs.NewAppt_Date);
            var dtNow = DateTime.Now.Date;
            var dtTM = dtNow.Date.AddMonths(2);
            var dates = new List<string>();
            var c = 0;

            for (var i = dtNow.Date.DayOfYear; i < dtTM.Date.DayOfYear; i++)
            {
                if (dtNow.Date.AddDays(c).DayOfWeek >= DayOfWeek.Monday && dtNow.Date.AddDays(c).DayOfWeek <= DayOfWeek.Friday)
                    dates.Add(dtNow.Date.AddDays(c).ToString("d"));
                c++;
            }
            foreach (var d in dates)
                pickerDate.Items.Add(d);

            var duration = new List<string> { "30m", "1hr", "1hr 30", "2hr", "2hr 30", "3hr", "3hr 30", "4hr" };
            var pickerDur = new Picker
            {
                WidthRequest = App.ScreenSize.Width * .2
            };

            pickerDur.Items.Add(Langs.NewAppt_Len);
            foreach (var d in duration)
                pickerDur.Items.Add(d);

            pickerDate.SelectedIndexChanged += (sender, e) =>
            {
                selDate = ((Picker)sender).SelectedIndex;
            };
            pickerTime.SelectedIndexChanged += (sender, e) =>
            {
                selTime = ((Picker)sender).SelectedIndex;
            };
            pickerDur.SelectedIndexChanged += (sender, e) =>
            {
                selDur = ((Picker)sender).SelectedIndex;
            };

            pickerTime.SelectedIndex = 0;
            pickerDate.SelectedIndex = 0;
            pickerDur.SelectedIndex = 0;

            var imgPlusTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() =>
                {
                    if (selTime != 0 && selDate != 0 && selDur != 0)
                    {
                        meetings.Add(new MeetingSched { Selected = false, Date = dates[selDate - 1], Time = times[selTime - 1], Duration = duration[selDur - 1] });
                        dates.RemoveAt(selDate);
                        times.RemoveAt(selTime);
                        selTime = selDate = selDur = 0;
                        pickerDate.SelectedIndex = 0;
                        pickerTime.SelectedIndex = 0;
                        pickerDur.SelectedIndex = 0;
                    }
                })
            };
            imgPlus.GestureRecognizers.Add(imgPlusTap);

            App.Self.MessageEvents.Change += async (s, ea) =>
            {
                if (ea.ModuleName == "RemoveDate")
                {
                    if (await DisplayAlert(Langs.NewAppt_RemoveDates, Langs.NewAppt_RemoveMessage, Langs.General_OK, Langs.General_Cancel))
                    {
                        var parts = ea.Message.Split('|');
                        var p = 0;
                        foreach (var m in meetings)
                        {
                            if (m.Date == parts[0] && m.Time == parts[1])
                                break;
                            else
                                p++;
                        }
                        lstDate.ItemsSource = null;
                        dates.Add(parts[0]);
                        times.Add(parts[1]);
                        dates = dates.OrderBy(t => t).ToList();
                        times = times.OrderBy(t => t).ToList();
                        meetings.RemoveAt(p);
                        lstDate.ItemsSource = meetings;
                    }
                }
            };

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width * .95,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .25,
                        MinimumHeightRequest = App.ScreenSize.Height * .25,
                        Children =
                        {
                            lblInternalEmail,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children ={lstStaff}
                            }
                        }
                    },
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .25,
                        MinimumHeightRequest = App.ScreenSize.Height * .25,
                        Children =
                        {
                            lblExternalEmail,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children ={entExternalNames}
                            }
                        },
                    },
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .25,
                        MinimumHeightRequest = App.ScreenSize.Height * .25,
                        Children =
                        {
                            lblProposedDates,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children = {pickerDate, pickerTime, pickerDur}
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                Children =
                                {
                                    new StackLayout
                                    {
                                        WidthRequest = App.ScreenSize.Width * .8,
                                        Children = {lstDate}
                                    },
                                    new StackLayout
                                    {
                                        Orientation = StackOrientation.Vertical,
                                        VerticalOptions = LayoutOptions.Center,
                                        Children = {imgPlus}
                                    }
                                }
                            }
                        },
                    },
                    btnSend
                }
            };

            innerStack.Children.Add(layout);

            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        WidthRequest = App.ScreenSize.Width,
                        Children = { topbar }
                    },
                    innerStack
                }
            };
        }
    }
}

