
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class StaffAttendeeList : ViewCell
    {
        public StaffAttendeeList()
        {
            var chkSelected = new Checkbox
            {
                VerticalOptions = LayoutOptions.Center
            };

            chkSelected.SetBinding(Checkbox.CheckedProperty, new Binding("Selected", converter: new BoolToString()));

            var lblName = new Label
            {
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
                Text = "fish"
            };

            var lblEmail = new Label
            {
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
                Text = "fish"
            };

            lblName.SetBinding(Label.TextProperty, new Binding("Name"));
            lblEmail.SetBinding(Label.TextProperty, new Binding("Email"));

            chkSelected.CheckedChanged += (sender, e) =>
            {
                var chk = sender as Checkbox;
                var msg = chk.Checked ? "AddStaff" : "RemoveStaff";
                App.Self.MessageEvents.BroadcastIt(msg, string.Format("{0}|{1}", lblName.Text, lblEmail.Text));
            };

            var grid = new Grid
            {
                RowDefinitions =
                        {
                            new RowDefinition {Height= GridLength.Auto}
                        },
                ColumnDefinitions =
                        {
                            new ColumnDefinition {Width = App.ScreenSize.Width * .05},
                            new ColumnDefinition { Width= GridLength.Star}
                        }
            };

            var stkLabel = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    lblName,
                    new Label {Text = " (", TextColor = Constants.NELFTBlue, FontSize = Constants.GeneralFontSize},
                    lblEmail,
                    new Label {Text = ")", TextColor = Constants.NELFTBlue, FontSize = Constants.GeneralFontSize},
                }
            };

            grid.Children.Add(chkSelected, 0, 0);
            grid.Children.Add(stkLabel, 1, 0);

            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(4),
                Children =
                {
                    grid
                }
            };
        }
    }
}

