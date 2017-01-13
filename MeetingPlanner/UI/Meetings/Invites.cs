
using Xamarin.Forms;
using MeetingPlanner.Languages;

namespace MeetingPlanner
{
    public class Invites : BaseView
    {
        public Invites()
        {
            CreateUI();
        }

        void CreateUI()
        {
            var lblHeading = new Label
            {
                Text = Langs.NewAppt_InviteTitle,
                FontSize = Constants.HeadlineFontSize,
                TextColor = Constants.NELFTGreen,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var lblEmail = new Label
            {
                Text = Langs.NewAppt_InviteSub,
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            var entryEmail = new CustomEntry
            {
                HeightRequest = App.ScreenSize.Height * .25,
                Keyboard = Keyboard.Email,
                Placeholder = string.Format("{0}\n{1}\n{2}", Langs.Helper_EmailsOne, Langs.Helper_EmailsTwo, Langs.Helper_EmailsThree),
                PlaceholderColor = Constants.NELFTOrange,
                TextColor = Constants.NELFTBlue,
            };

            var lblPersonal = new Label
            {
                Text = Langs.NewAppt_InvitePersonal,
                TextColor = Constants.NELFTBlue,
                FontSize = Constants.GeneralFontSize
            };

            var entryPersonal = new CustomEntry
            {
                HeightRequest = App.ScreenSize.Height * .20,
                Keyboard = Keyboard.Default,
                TextColor = Constants.NELFTBlue,
                Placeholder = string.Format("{0}\n{1}", Langs.Helper_PersonalMessageOne, Langs.Helper_PersonalMessageTwo),
                PlaceholderColor = Constants.NELFTOrange
            };
            entryEmail.TextChanged += delegate
            {
                var total = string.Empty;
                if (!string.IsNullOrEmpty(entryEmail.Text))
                {
                    var split = entryEmail.Text.Split(',');
                    for (var i = 0; i < split.Length; ++i)
                    {
                        if (!string.IsNullOrEmpty(split[i]))
                        {
                            if (split[i].CheckPattern())
                                total += string.Format("{0}|", split[i]);
                        }
                    }
                }
                App.Self.MessageEvents.BroadcastIt("InvitesSet", total);
            };

            entryPersonal.TextChanged += delegate
            {
                App.Self.MessageEvents.BroadcastIt("MessageSet", entryPersonal.Text);
            };

            Content = CreateContent(new StackLayout
            {
                WidthRequest = App.ScreenSize.Width * .95,
                HeightRequest = App.ScreenSize.Height * .8,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    lblHeading,
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Children =
                        {
                            lblEmail,entryEmail,
                            lblPersonal, entryPersonal
                        }

                        }
                }
            });
        }
    }
}

