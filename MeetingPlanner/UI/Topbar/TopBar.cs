﻿using System.Linq;
using System.Collections.Generic;

namespace turtlewax
{
    using Xamarin.Forms;

    public class TopBar : View
    {
        private string Title, LeftImage, RightImage;
        readonly Page currentPage;
        StackLayout panel;
        Image rightCell;
        Grid grid;
        MenuView menu;
        bool FromMain;

        public TopBar(string text = "", Page current = null, string leftImage = "", string rightImage = "", StackLayout stack = null, bool fromMain = true)
        {
            Title = text;
            LeftImage = leftImage;
            RightImage = rightImage;
            currentPage = current;
            panel = stack;
            FromMain = fromMain;
        }

        public Grid CreateTopBar()
        {
            grid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Constants.Green,
                HeightRequest = 52,
                MinimumHeightRequest = 52,
                WidthRequest = App.ScreenSize.Width,
            };

            var padView = new ContentView
            {
                WidthRequest = 8
            };

            var leftView = new ContentView
            {
                HeightRequest = 16,
                WidthRequest = 16
            };
            var rightView = new ContentView
            {
                HeightRequest = 16,
                WidthRequest = 16
            };

            var leftCell = new Image();
            if (!string.IsNullOrEmpty(LeftImage))
            {
                leftCell.Source = LeftImage;
                leftCell.HeightRequest = leftCell.WidthRequest = 16;
                leftCell.HorizontalOptions = LayoutOptions.StartAndExpand;

                var gestLeft = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command(async() => await currentPage.Navigation.PopAsync())
                };
                leftCell.GestureRecognizers.Add(gestLeft);
                leftView.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { padView, leftCell }
                };
            }

            dynamic title;
            if (!string.IsNullOrEmpty(Title))
            {
                title = new Label
                {
                    FontSize = 22,
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    MinimumWidthRequest = App.ScreenSize.Width - 64,
                    Text = Title
                };
            }
            else
            {
                title = new Image
                {
                    Source = "headerlogo.png",
                    HeightRequest = 42,
                    WidthRequest = 42,
                    VerticalOptions = LayoutOptions.Center
                };
                var imgTap = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    //Command = new Command(async () => await Navigation.PopToRootAsync(true))
                };
                title.GestureRecognizers.Add(imgTap);
            }

            rightCell = new Image();
            Rectangle origBounds;

            if (!string.IsNullOrEmpty(RightImage))
            {
                rightCell.Source = RightImage;
                rightCell.HeightRequest = rightCell.WidthRequest = 32;
                rightCell.HorizontalOptions = LayoutOptions.EndAndExpand;

                rightView.Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { rightCell, padView }
                };

                menu = new MenuView();
                menu.HeightRequest = App.ScreenSize.Height - grid.HeightRequest;

                var gestRight = new TapGestureRecognizer
                {
                    NumberOfTapsRequired = 1,
                    Command = new Command((o) =>
                        {
                            var bounds = panel.Children[0].Bounds;
                            bounds.X = App.ScreenSize.Width * .18;
                            if (!App.Self.PanelShowing)
                            {
                                Device.BeginInvokeOnMainThread(async() =>
                                    {
                                        panel.WidthRequest = panel.Width + menu.Content.WidthRequest;
                                            
                                        panel.Children.Add(new StackLayout
                                            {
                                                Padding = new Thickness(0, FromMain ? -8 : 0),
                                                Children = { menu }
                                            }
                                        );

                                        origBounds = panel.Children[1].Bounds;
                                        if (origBounds.X < App.ScreenSize.Width)
                                            origBounds.X = App.ScreenSize.Width + 6;

                                        await panel.Children[1].LayoutTo(bounds, 250, Easing.CubicIn);
                                        panel.Children[0].Opacity = .5;
                                        App.Self.PanelShowing = true;
                                    });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(async() =>
                                        await panel.Children[1].LayoutTo(origBounds, 250, Easing.CubicOut)); 
                                panel.Children.Remove(menu);
                                //panel.WidthRequest = panel.Width - menu.Content.WidthRequest;
                                panel.Children[0].Opacity = 1;
                                App.Self.PanelShowing = false;
                            }
                        })
                };
                rightCell.GestureRecognizers.Add(gestRight);
            }

            MessagingCenter.Subscribe<MenuView, string>(this, "Menu", (sender, args) =>
                {
                    if (args == "Close")
                    {
                        if (App.Self.PanelShowing)
                        {
                            Device.BeginInvokeOnMainThread(async() =>
                                {
                                    if (panel.Children.Count > 1)
                                    {
                                        await panel.Children[1].LayoutTo(origBounds, 250, Easing.CubicOut); 
                                        panel.Children.Remove(menu);
                                        panel.Children[0].Opacity = 1;
                                        App.Self.PanelShowing = false;
                                    }
                                });
                        }
                    }
                });

            grid.Children.Add(leftView, 0, 0);
            grid.Children.Add(title, 1, 0);
            grid.Children.Add(rightView, 2, 0);
            return grid;
        }
    }
}