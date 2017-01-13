using System;
using MeetingPlanner.Languages;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Attending : PopupPage
    {
        int id;
        bool pending;
        ScrollView scrollGrid;

        public Attending(int meetingId, bool fromPending = false)
        {
            id = meetingId;
            pending = fromPending;
            CreateUI();
        }

        void CreateUI()
        {
            var appts = App.Self.DBManager.GetSingleObject<AppointmentList>("MeetingId", id.ToString());

            var lblMeetingDate = new Label
            {
                WidthRequest = App.ScreenSize.Width * .8,
                TextColor = Constants.NELFTBlue,
                Text = AppointmentListHelpers.GetPolls(id)?.SuggestedDate.ToString("D"),
            };

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .95,
            };

            GenerateGrid(grid, appts);

            var lblTitle = new Label
            {
                Text = Langs.Poll_Attending,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.HeadlineFontSize,
            };

            var btnClose = new Button
            {
                Text = Langs.General_OK,
                BorderRadius = 5,
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
            };
            btnClose.Clicked += OnClose;

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = App.ScreenSize.Height * .6,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(20, 20),
                Children =
                {
                    lblTitle,

                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Padding = new Thickness(0,12),
                        Children = {
new StackLayout
                            {
                                BackgroundColor = Constants.NELFTOrange,
                                Padding = new Thickness(8,0,0,0),
                                Children = {lblMeetingDate}
                            },
                            scrollGrid
                        }
                    },
                    btnClose
                }
            };

            Content = stack;
        }

        async void OnClose(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        void GenerateGrid(Grid grid, AppointmentList appts)
        {
            var noTimes = MeetingHelper.MeetingTimes(appts.MeetingId).Count < 3 ? 3 : MeetingHelper.MeetingTimes(appts.MeetingId).Count;
            var gridWidth = App.ScreenSize.Width * .65 / noTimes;

            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = App.ScreenSize.Width * .2
            });

            scrollGrid = new ScrollView
            {
                Orientation = ScrollOrientation.Both
            };

            for (var i = 0; i < noTimes; ++i)
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = gridWidth
                });
            for (var i = 0; i < AppointmentListHelpers.InvitedTotal(appts.MeetingId) + 1; ++i)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = 32
                });
            }
            var xpos = 1;
            var ypos = 0;

            var fontSize = Constants.SmallEntryFontSize;
            fontSize -= noTimes > 4 ? noTimes + 1 : 0;

            foreach (var time in MeetingHelper.MeetingTimes(appts.MeetingId))
            {
                var lblTime = new Label
                {
                    TextColor = Constants.NELFTBlue,
                    FontSize = fontSize,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = time.Time,
                    VerticalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.CharacterWrap
                };
                grid.Children.Add(lblTime, xpos, ypos);
                xpos++;
            }

            xpos = 1;
            ypos++;

            var attend = 0;
            var start = 0;
            var end = MeetingHelper.MeetingTimes(appts.MeetingId).Count;
            for (var i = 0; i < AppointmentListHelpers.InvitedTotal(appts.MeetingId); ++i)
            {
                var name = string.Empty;
                if (MeetingHelper.Invited(appts.MeetingId)[attend].Name.Contains(" "))
                    name = MeetingHelper.Invited(appts.MeetingId)[attend].Name.Split(' ')[0];
                else
                    name = MeetingHelper.Invited(appts.MeetingId)[attend].Name.Split('@')[0];

                var lblAttend = new Label
                {
                    BackgroundColor = MeetingHelper.Invited(appts.MeetingId)[attend].UserId == App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String) ? Constants.NELFTYellow : Constants.LightGrey,
                    Text = name,
                    FontSize = Constants.SmallEntryFontSize,
                    VerticalTextAlignment = TextAlignment.Center
                };

                grid.Children.Add(lblAttend, 0, ypos);
                for (var n = start; n < end; ++n)
                {
                    var check = PollingDataHelpers.Polling(appts.MeetingId)[n].Accept;
                    var chkAttend = new Checkbox
                    {
                        StyleId = string.Format("{0}|{1}|{2}", MeetingHelper.Invited(appts.MeetingId)[attend].UserId, AppointmentListHelpers.GetMeeting(appts.MeetingId).id, n),
                        WidthRequest = gridWidth,
                        BackgroundColor = check == 1 ? Constants.NELFTGreen : Color.Red,
                        Checked = check == 1,
                        HeightRequest = 28,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        IsEnabled = MeetingHelper.Invited(appts.MeetingId)[attend].UserId == App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String) & !pending
                    };
                    chkAttend.CheckedChanged += async (sender, e) =>
                    {
                        var chk = sender as Checkbox;
                        chk.BackgroundColor = e.IsChecked ? Constants.NELFTGreen : Color.Red;
                        var splt = chk.StyleId.Split('|');
                        var val = Convert.ToInt32(splt[2]);
                        var attendee = App.Self.DBManager.GetSingleObject<Attendees>("UserId", splt[0]);
                        attendee.Attending = e.IsChecked ? 1 : 0;
                        App.Self.DBManager.AddOrUpdateAttendees(attendee);
                        var usr = PollingDataHelpers.Polling(appts.MeetingId)[val];
                        PollingDataHelpers.Polling(appts.MeetingId)[val].Accept = e.IsChecked ? 1 : 0;

                        if (App.Self.IsConnected)
                            await Webservices.SendData("setPollCast.php", JsonConvert.SerializeObject(PollingDataHelpers.Polling(appts.MeetingId)[val]));

                        App.Self.DBManager.AddOrUpdatePollCast(PollingDataHelpers.Polling(appts.MeetingId)[val]);
                    };
                    grid.Children.Add(chkAttend, xpos, ypos);
                    xpos++;
                }
                //}
                start += MeetingHelper.MeetingTimes(appts.MeetingId).Count;
                end += MeetingHelper.MeetingTimes(appts.MeetingId).Count;
                ypos++;
                attend++;
                xpos = 1;
            }

            scrollGrid.Content = grid;
        }
    }
}

