using Xamarin.Forms;
using System.Collections.Generic;
using System;

namespace turtlewax
{
    public class MenuListClass
    {
        public string image { get; set; }

        public string text { get; set; }
    }

    public class MenuView : ContentView
    {
        List<MenuListClass> menuList;

        public MenuView()
        {
            menuList = new List<MenuListClass>
            {
                new MenuListClass { text = LangResources.MainThreeScan, image = "SCAN_A_PRODUCT.png" },
                new MenuListClass { text = LangResources.MainThreeExplorer, image = "PRODUCT_EXPLORER.png" },
                new MenuListClass { text = LangResources.MainWhereToBuy, image = "WHERE_TO_BUY.png" },
                new MenuListClass { text = LangResources.RegionalSettings, image = "regional.png" },
                new MenuListClass { image = "favourites_settings.png", text = LangResources.Favourites },
            };

            /*listView.ItemSelected += async (object sender, SelectedItemChangedEventArgs e) =>
            {
                var selected = e.SelectedItem as MenuListClass;
                Page page = null;
                if (selected != null)
                {
                    if (selected.text == LangResources.MainThreeScan)
                        page = new HowToScan();
                    else if (selected.text == LangResources.MainThreeExplorer)
                        page = new ProductMenu();
                    else if (selected.text == LangResources.MainWhereToBuy)
                        page = new buyFromPage();
                    else if (selected.text == LangResources.RegionalSettings)
                        page = new RegionalSettings();
                    else
                        page = new FavouriteUI();

                    if (page != null)
                    {
                        MessagingCenter.Send(this, "Menu", "Close");
                        await Navigation.PushAsync(page).ContinueWith((t) =>
                            {
                                if (t.IsCompleted)
                                    Device.BeginInvokeOnMainThread(() => listView.SelectedItem = null);  
                            });
                    }
                }
            };*/

            var masterStack = new StackLayout
            {
                BackgroundColor = Color.White,
                Orientation = StackOrientation.Vertical,
                MinimumWidthRequest = App.ScreenSize.Width * .85,
                WidthRequest = App.ScreenSize.Width * .85,
                HeightRequest = App.ScreenSize.Height /*- 52 - 36*/,
            };
            for (var i = 0; i < menuList.Count; ++i)
                masterStack.Children.Add(MenuListView(i));


            Content = masterStack;
        }

        StackLayout MenuListView(int i)
        {
            var imgIcon = new Image
            {
                WidthRequest = 42,
                HeightRequest = 42,
                Source = menuList[i].image
            };

            var lblText = new CustomLabel
            {
                FontFamily = Constants.TradeGothic,
                FontSize = 18,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Constants.MainCopy,
                Text = menuList[i].text
            };

            var tap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(async(t) =>
                    {
                        MessagingCenter.Send(this, "Menu", "Close");
                        Page page = null;
                        switch (i)
                        {
                            case 0:
                                page = new HowToScan();
                                break;
                            case 1:
                                page = new ProductMenu();
                                break;
                            case 2:
                                page = new BuyFromPage();
                                break;
                            case 3:
                                page = new RegionalSettings();
                                break;
                            case 4:
                                page = new FavouriteUI();
                                break;
                        }

                        await Navigation.PushAsync(page);
                    }
                )
            };

            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(0, 4, 0, 0),
                Children =
                { 
                    new StackLayout
                    {
                        Padding = new Thickness(8),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        { imgIcon, 
                            new StackLayout
                            {
                                Padding = new Thickness(8, 0, 0, 0),
                                VerticalOptions = LayoutOptions.Center,
                                Children = { lblText }
                            }
                        }
                    }, new BoxView
                    {
                        WidthRequest = App.ScreenSize.Width,
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Color = Constants.LightGrey
                    }
                },
            };
            stack.GestureRecognizers.Add(tap);

            return stack;
        }
    }
}


