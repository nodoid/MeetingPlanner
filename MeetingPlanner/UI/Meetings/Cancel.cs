using System.Linq;
using MeetingPlanner.Languages;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class CancelMeeting : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;

        public CancelMeeting()
        {
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

            var topbar = new TopBar(Langs.Cancel_Title, this, "icoback", "icomenu", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var data = App.Self.DBManager.GetListOfObjects<AppointmentList>().OrderByDescending(t => t.DateDue.Date).ToList();
            var appts = (from d in data
                         where AppointmentListHelpers.GetMeeting(d.MeetingId).IsMyMeeting == 1
                         select d.id).ToList();

            stack.Children.Add(
 new StackLayout
 {
     Orientation = StackOrientation.Vertical,
     Children = { new CancelAppt(appts) }
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

