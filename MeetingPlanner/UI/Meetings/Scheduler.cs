using System.Collections.Generic;
using Xamarin.Forms;
using MeetingPlanner.Languages;

namespace MeetingPlanner
{
    public class Scheduler : BaseView
    {
        List<Location> locations = new List<Location>();
        List<string> subLocations = new List<string>();
        int locPick, subPick;

        public Scheduler()
        {
            CreateUI();
        }

        void CreateUI()
        {
            var lblHeading = new Label
            {
                FontSize = Constants.HeadlineFontSize,
                Text = Langs.NewAppt_General,
                TextColor = Constants.NELFTGreen,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var lblTitle = new Label
            {
                Text = Langs.NewAppt_Title,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };
            var lblLocation = new Label
            {
                Text = Langs.NewAppt_Location,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };

            var lblDescription = new Label
            {
                Text = Langs.NewAppt_Description,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };

            var lblName = new Label
            {
                Text = Langs.NewAppt_YourName,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };

            var entryTitle = new CustomEntry
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Keyboard = Keyboard.Default,
                TextColor = Constants.NELFTBlue,
                Placeholder = Langs.Helper_MeetingTitle,
                PlaceholderColor = Constants.NELFTOrange
            };

            var entryDesc = new CustomEntry
            {
                WidthRequest = App.ScreenSize.Width * .9,
                HeightRequest = App.ScreenSize.Height * .2,
                Keyboard = Keyboard.Default,
                TextColor = Constants.NELFTBlue,
                Placeholder = Langs.Helper_MeetingDescription,
                PlaceholderColor = Constants.NELFTOrange
            };

            var entryYourName = new CustomEntry
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Keyboard = Keyboard.Default,
                TextColor = Constants.NELFTBlue,
                PlaceholderColor = Constants.NELFTOrange,
                Placeholder = Langs.Helper_MeetingYourName
            };

            var entryLocation = new CustomEntry
            {
                WidthRequest = App.ScreenSize.Width * .9,
                Keyboard = Keyboard.Default,
                TextColor = Constants.NELFTBlue,
                Placeholder = Langs.Helper_MeetingLocation,
                PlaceholderColor = Constants.NELFTOrange
            };

            entryTitle.TextChanged += delegate
            {
                App.Self.MessageEvents.BroadcastIt("TitleSet", entryTitle.Text);
            };

            entryDesc.TextChanged += delegate
            {
                App.Self.MessageEvents.BroadcastIt("DescSet", entryDesc.Text);
            };

            entryYourName.TextChanged += delegate
            {
                App.Self.MessageEvents.BroadcastIt("NameSet", entryYourName.Text);
            };

            entryLocation.TextChanged += delegate
            {
                App.Self.MessageEvents.BroadcastIt("LocSet", entryLocation.Text);
            };

            Content = CreateContent(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width * .95,
                HeightRequest = App.ScreenSize.Height * .8,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    lblHeading,
                    new StackLayout
                    {
                        Padding = new Thickness(0,8),
                        Children =
                        {
                            lblTitle, entryTitle
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,8),
                        Children =
                        {
                            lblName, entryYourName
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,8),
                        Children =
                        {
                            lblLocation, entryLocation
                        }
                    },
                    new StackLayout
                    {
                        Padding = new Thickness(0,8),
                        Children =
                        {
                            lblDescription, entryDesc
                        }
                    },
                }
            });
        }
    }
}

