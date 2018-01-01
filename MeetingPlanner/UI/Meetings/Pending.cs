using System;
using System.Collections.ObjectModel;
using System.Linq;
using MeetingPlanner.Languages;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using System.Collections.Generic;

namespace MeetingPlanner
{
    public class PendingMeeting : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;
        ObservableCollection<AppointmentList> apptsList = new ObservableCollection<AppointmentList>();

        public PendingMeeting()
        {
            CreateUI();
        }

        void PropogateAppts()
        {
            var appts = App.Self.DBManager.GetListOfObjects<AppointmentList>().DistinctBy(t => t.id).Where(t => t.DateDue.Month >= DateTime.Now.Month).OrderByDescending(t => t.DateDue.Date).ToList();
            var id = new List<int>();

            var ids = (from a in appts
                       where AppointmentListHelpers.GetMeeting(a.MeetingId)?.IveResponded == 1
                       select a.id).ToList();

            foreach (var apt in ids)
                apptsList.Add(App.Self.DBManager.GetSingleObject<AppointmentList>("id", apt.ToString()));
        }

        void CreateUI()
        {
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

            var topbar = new TopBar(Langs.Menu_Pending, this, "icoback", "icomenu", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            PropogateAppts();

            var listView = new ListView
            {
                ItemsSource = apptsList,
                ItemTemplate = new DataTemplate(typeof(PendingViewCell)),
                SeparatorVisibility = SeparatorVisibility.None,
                HeightRequest = stack.HeightRequest,
                MinimumWidthRequest = App.ScreenSize.Width * .95,
                WidthRequest = App.ScreenSize.Width * .95,
                HasUnevenRows = true,
                IsPullToRefreshEnabled = true
            };

            listView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var tap = e.SelectedItem as AppointmentList;
                var listAttend = MeetingHelper.Invited(tap.MeetingId);
                if (MeetingHelper.MeetingTimes(tap.MeetingId).Count != 0)
                    await Navigation.PushPopupAsync(new Attending(tap.MeetingId, true));
                else
                    await DisplayAlert(Langs.Error_Message_PollTitle, Langs.Error_Message_Poll_NoPoll, Langs.General_OK);
            };
            listView.RefreshCommand = new Command(async () =>
            {
                await Webservices.GetListData<BaseAppointmentList>("getAllAppointments.php", "userId", App.Self.UserSettings.LoadSetting<string>("userId", SettingType.String)).ContinueWith((t) =>
    {
        if (t.IsCompleted)
        {
            if (t.Result.AppointmentList.Count != apptsList.Count)
            {
                App.Self.DBManager.AddOrUpdateAppointments(t.Result.AppointmentList);
                apptsList.Clear();
                PropogateAppts();
                Device.BeginInvokeOnMainThread(() => { listView.ItemsSource = null; listView.ItemsSource = apptsList; listView.IsRefreshing = false; });
            }
        }
    });
            });

            Content = CreateContent(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(12, 12),
                Children = { listView }
            });

            App.Self.MessageEvents.Change += (s, ea) =>
            {
                if (ea.ModuleName == "UpdateAccept")
                {
                    var msg = ea.Message.Split('|');
                    var appt = apptsList.FirstOrDefault(t => t.id == Convert.ToInt32(msg[0]));
                    AppointmentListHelpers.GetMeeting(appt.MeetingId).IveResponded = Convert.ToInt32(msg[1]);
                    App.Self.DBManager.AddOrUpdateMeeting(AppointmentListHelpers.GetMeeting(appt.MeetingId));
                }
            };

            stack.Children.Add(
                new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { listView }
                }
            );

            innerStack.Children.Add(stack);

            Content = CreateContent(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
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
            });
        }
    }
}

