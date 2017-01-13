using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MeetingPlanner.Languages;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class Login : BasePage
    {
        readonly IEncryptionManager encryptionManager;

        public Login()
        {
            encryptionManager = new EncryptionManager();

            CreateUI();
        }

        void CreateUI()
        {
            var imgCompany = new Image
            {
                IsOpaque = true,
                Source = "companylogo",
                HeightRequest = App.ScreenSize.Height * .05,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var lblLoginTitle = new Label
            {
                Text = Langs.Login_Title,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Constants.NELFTBlue,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            var gridMain = new Grid
            {
                RowSpacing = 10,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = 40 },
                    new RowDefinition { Height = 70 },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = App.ScreenSize.Width * .1 },
                    new ColumnDefinition { Width = App.ScreenSize.Width * .8 },
                }
            };

            var imgName = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Source = "iconname",
                HeightRequest = 30,
                WidthRequest = 30
            };

            var entryUsername = new CustomEntry
            {
                Placeholder = Langs.Login_Username,
                PlaceholderColor = Constants.NELFTOrange,
                FontSize = 15,
                TextColor = Constants.NELFTBlue,
                Keyboard = Keyboard.Email,
            };

            var imgPassword = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Source = "iconpassword",
                HeightRequest = 30,
                WidthRequest = 30
            };

            var entryPassword = new CustomEntry
            {
                Placeholder = Langs.Login_Password,
                PlaceholderColor = Constants.NELFTOrange,
                FontSize = 15,
                TextColor = Constants.NELFTBlue,
                IsPassword = true,
                Keyboard = Keyboard.Default
            };

            var btnLogin = new Button
            {
                HeightRequest = 64,
                WidthRequest = App.ScreenSize.Width * .75,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Text = Langs.Login_Button,
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = Constants.NELFTGreen,
                Command = new Command(async (t) =>
                {
                    if (!string.IsNullOrEmpty(entryUsername.Text) && !string.IsNullOrEmpty(entryPassword.Text))
                    {
                        var user = await LoginUser(entryUsername.Text, entryPassword.Text);
                        if (user != null)
                        {
                            IsEnabled = false;
                            var us = entryUsername.Text;
                            us = char.ToUpperInvariant(us[0]) + us.Substring(1);
                            us = us.Substring(0, us.Length - 2) + char.ToUpperInvariant(us[us.Length - 2]) + us.Substring(us.Length - 1);
                            App.Self.UserSettings.SaveSetting<string>("Username", us, SettingType.String);
                            App.Self.UserSettings.SaveSetting<string>("Password", entryPassword.Text, SettingType.String);

                            await DataGatherer.GetInitialGather();
                            if (DataGatherer.ThisWeek != 0)
                            {
                                if (await DisplayAlert(Langs.MeetingWarn_Title, Langs.MeetingWarn_Message, Langs.MeetingWarn_View, Langs.General_Cancel))
                                    Application.Current.MainPage = new NavigationPage(new MainPage(true));
                                else
                                    Application.Current.MainPage = new NavigationPage(new MainPage());
                            }
                            else
                                Application.Current.MainPage = new NavigationPage(new MainPage());
                        }
                        else
                        {
                            IsEnabled = true;
                            await DisplayAlert(Langs.Error_Message_Login_Fail, Langs.Error_Message_Login_No_User, Langs.General_OK);
                        }
                    }
                })
            };

            gridMain.Children.Add(imgName, 0, 0);
            gridMain.Children.Add(entryUsername, 1, 0);
            gridMain.Children.Add(imgPassword, 0, 1);
            gridMain.Children.Add(entryPassword, 1, 1);
            gridMain.Children.Add(btnLogin, 0, 2);

            Grid.SetColumnSpan(btnLogin, 2);

            var contentStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10, 10),
                Children =
                {
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .2,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Children =
                        {
                            imgCompany,
                    new Label {HeightRequest = 10},
                    lblLoginTitle,
                        }
                    },
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .5,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Children = {gridMain}
                    },
                    new StackLayout
                    {
                        HeightRequest = App.ScreenSize.Height * .3,
                        VerticalOptions = LayoutOptions.Start,
                        Margin = new Thickness(0,8),
                        Children =
                        {
                            new Image
                    {
                        Source = "icon_large",
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        IsOpaque = true,
                                HeightRequest = App.ScreenSize.Height *.2,
                    }
                        }
                    }

                }
            };

            Content = CreateContent(contentStack);

            var username = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String);
            var password = App.Self.UserSettings.LoadSetting<string>("Password", SettingType.String);

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                entryUsername.Text = username;
                entryPassword.Text = password;
                LoginUser(entryUsername.Text, entryPassword.Text).ContinueWith((t) =>
                {
                    if (t.IsCompleted)
                    {
                        if (t != null)
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                btnLogin.IsEnabled = false;
                                var us = entryUsername.Text;
                                us = char.ToUpperInvariant(us[0]) + us.Substring(1);
                                us = us.Substring(0, us.Length - 2) + char.ToUpperInvariant(us[us.Length - 2]) + us.Substring(us.Length - 1);
                                App.Self.UserSettings.SaveSetting<string>("Username", us, SettingType.String);
                                App.Self.UserSettings.SaveSetting<string>("Password", entryPassword.Text, SettingType.String);

                                await DataGatherer.GetDataGatherForDates();
                                if (DataGatherer.ThisWeek != 0)
                                {
                                    if (await DisplayAlert(Langs.MeetingWarn_Title, Langs.MeetingWarn_Message, Langs.MeetingWarn_View, Langs.General_Cancel))
                                        Application.Current.MainPage = new NavigationPage(new MainPage(true));
                                    else
                                        Application.Current.MainPage = new NavigationPage(new MainPage());
                                }
                                else
                                    Application.Current.MainPage = new NavigationPage(new MainPage());
                            });
                        }
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                    {
                        btnLogin.IsEnabled = true;
                        await DisplayAlert(Langs.Error_Message_Login_Fail, Langs.Error_Message_Login_No_User, Langs.General_OK);
                    });
                    }
                });
            }
        }

        async Task<ActiveDirectoryUser> LoginUser(string username, string password)
        {
            if (App.Self.IsConnected)
            {
                App.Self.NetSpinner.Spinner(true, Langs.Spinner_LoggingIn, Langs.Spinner_Wait);
                var passwordEn = System.Net.WebUtility.UrlEncode(password);
                var uri = string.Format(@"https://apps.nelft.nhs.uk/ADAuthentication/api/UserSecurity/AuthenticateEncryptedUser?login={0}&password={1}", username, passwordEn);

                var client = new HttpClient();
                var response = await client.GetAsync(uri);
                var userEncryptionString = response.Content.ReadAsStringAsync().Result;
                var encryptionObjects = JsonConvert.DeserializeObject<IEnumerable<Encryption>>(userEncryptionString);

                var users = encryptionManager.DecryptUsers(encryptionObjects);

                var user = users.FirstOrDefault();
                App.Self.NetSpinner.Spinner(false, string.Empty, string.Empty);
                return user;
            }
            else
            {
                await DisplayAlert(Langs.Error_NetworkTitle, Langs.Error_NetworkMessage, Langs.General_OK);
                App.Self.NetSpinner.Spinner(false, string.Empty, string.Empty);
            }
            return null;
        }
    }
}

