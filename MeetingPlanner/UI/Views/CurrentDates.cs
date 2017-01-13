using Xamarin.Forms;

namespace MeetingPlanner
{
    public class CurrentDates : ViewCell
    {
        public CurrentDates()
        {
            var chkSelected = new Checkbox
            {
                VerticalOptions = LayoutOptions.Center
            };

            chkSelected.SetBinding(Checkbox.CheckedProperty, new Binding("Selected", converter: new BoolToString()));

            var lblDate = new Label
            {
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
                Text = "fish"
            };

            var lblTime = new Label
            {
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
                Text = "fish"
            };
            var lblDur = new Label
            {
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue,
                Text = "fish"
            };

            lblDate.SetBinding(Label.TextProperty, new Binding("Date"));
            lblTime.SetBinding(Label.TextProperty, new Binding("Time"));
            lblDur.SetBinding(Label.TextProperty, new Binding("Duration"));

            chkSelected.CheckedChanged += (sender, e) =>
            {
                App.Self.MessageEvents.BroadcastIt("RemoveDate", string.Format("{0}|{1}", lblDate.Text, lblTime.Text));
            };

            var grid = new Grid
            {
                RowDefinitions =
                        {
                            new RowDefinition {Height= GridLength.Auto}
                        },
                ColumnDefinitions =
                        {
                            new ColumnDefinition {Width = App.ScreenSize.Width * .1},
                            new ColumnDefinition { Width= GridLength.Star}
                        }
            };

            var stkLabel = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    lblDate,
                    new Label {Text = " (", TextColor = Constants.NELFTBlue, FontSize = Constants.GeneralFontSize},
                    lblTime, lblDur,
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

