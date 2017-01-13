using MeetingPlanner.Languages;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using System.Linq;
using System;

namespace MeetingPlanner
{
    public class UpComingMeeting : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;
        bool thisWeek;

        ObservableCollection<AppointmentList> appts = new ObservableCollection<AppointmentList>();

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (innerStack != null)
                if (innerStack.Children.Count != 0)
                    innerStack.Children.Clear();

            CreateUI();
        }

        public UpComingMeeting(bool thisWeekOnly = false)
        {
            BackgroundColor = Color.White;
            thisWeek = thisWeekOnly;
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

            var topbar = new TopBar(Langs.Menu_Upcoming, this, "icoback", "icomenu", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            PropogateAppts();

            var listView = new ListView
            {
                ItemsSource = appts,
                HasUnevenRows = true,
                WidthRequest = App.ScreenSize.Width * .95,
                MinimumWidthRequest = App.ScreenSize.Width * .95,
                HeightRequest = stack.HeightRequest,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(DataListView)),
                IsPullToRefreshEnabled = true,
            };
            listView.ItemTapped += ListView_ItemTapped;
            listView.RefreshCommand = new Command(async () =>
{
    listView.IsRefreshing = true;
    await Webservices.GetListData<BaseAppointmentList>("getAllAppointments.php", "userId", App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String)).ContinueWith((t) =>
    {
        if (t.IsCompleted)
        {
            if (t.Result.AppointmentList.Count != appts.Count)
            {
                App.Self.DBManager.AddOrUpdateAppointments(t.Result.AppointmentList);
                appts.Clear();
                PropogateAppts();
                Device.BeginInvokeOnMainThread(() => { listView.ItemsSource = null; listView.ItemsSource = appts; listView.IsRefreshing = false; });
            }
        }
    });

});

            stack.Children.Add(
                new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { listView }
                });
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

        void PropogateAppts()
        {
            var appt = App.Self.DBManager.GetListOfObjects<AppointmentList>().Where(t => t.DateDue.Month == DateTime.Now.Month).OrderByDescending(t => t.DateDue.Date).ToList();
            if (thisWeek)
            {
                var startOfWeek = DateTime.Now.StartOfWeek();
                var newappt = appt.Where(t => t.DateDue >= startOfWeek).ToList();
                newappt = newappt.Where(t => t.DateDue <= startOfWeek.AddDays(4)).ToList();
                appt = newappt;
            }

            foreach (var id in appt)
                appts.Add(id);
        }

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            list.SelectedItem = null;
            var tap = e.Item as AppointmentList;
            var listAttend = MeetingHelper.Invited(tap.MeetingId);
            if (MeetingHelper.MeetingTimes(tap.MeetingId).Count != 0)
                await Navigation.PushPopupAsync(new Attending(tap.MeetingId));
            else
                await DisplayAlert(Langs.Error_Message_PollTitle, Langs.Error_Message_Poll_NoPoll, Langs.General_OK);
        }
    }
}

