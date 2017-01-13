using MeetingPlanner.Languages;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;

namespace MeetingPlanner
{
    public class CheckPolls : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;

        ObservableCollection<AppointmentList> appts = new ObservableCollection<AppointmentList>();

        public CheckPolls()
        {
            var apps = App.Self.DBManager.GetListOfObjects<AppointmentList>();
            foreach (var a in apps)
                appts.Add(a);
            CreateUI();
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

            var topbar = new TopBar(Langs.Menu_Check, this, "icoback", "icomenu", innerStack).CreateTopBar();

            var lstView = new ListView
            {
                ItemsSource = appts,
                HasUnevenRows = true,
                SeparatorVisibility = SeparatorVisibility.None,
                ItemTemplate = new DataTemplate(typeof(PollDataView))
            };

            lstView.ItemSelected += async (sender, e) =>
            {
                var appt = e.SelectedItem as AppointmentList;
                if (appt.PollList.Count == 0)
                {
                    await DisplayAlert(Langs.Error_Message_PollTitle, Langs.Error_Message_Poll_NoPoll, Langs.General_OK);
                }
                else
                {
                    await Navigation.PushPopupAsync(new Attending(appt.MeetingId));
                }
            };

            innerStack.Children.Add(lstView);

            Content = CreateContent(new StackLayout
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
            });
        }
    }
}

