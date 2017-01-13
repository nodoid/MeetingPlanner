
using Xamarin.Forms;
using MeetingPlanner.Languages;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Text.RegularExpressions;

namespace MeetingPlanner
{
    public class TimeSelector : BaseView
    {
        List<CustomEntry> entryBoxes;
        List<Label> labelBoxes;
        List<string> times;
        int totalEntries = 2;
        int currentRow = 0;
        int testLen = 0;

        public TimeSelector()
        {
            entryBoxes = new List<CustomEntry>();
            labelBoxes = new List<Label>();
            times = new List<string> { string.Empty,string.Empty,string.Empty,
            string.Empty,string.Empty,string.Empty,
            string.Empty,string.Empty,string.Empty,
            string.Empty,string.Empty,string.Empty};
            CreateUI();
        }

        bool AcceptedCharacter(string c)
        {
            var rg = Regex.IsMatch(c, "[0-9\\:\\-]");
            return rg;
        }

        void CreateUI()
        {
            var lblTimeHeading = new Label
            {
                Text = Langs.NewAppt_TimeTitle,
                FontSize = Constants.HeadlineFontSize,
                TextColor = Constants.NELFTGreen,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var lblMessage = new Label
            {
                Text = Langs.NewAppt_TimeSubtitle,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };

            var lblAddMore = new Label
            {
                Text = Langs.NewAppt_TimeAddMore,
                FontSize = Constants.GeneralFontSize,
                TextColor = Constants.NELFTBlue
            };

            var btnSet = new Button
            {
                Text = Langs.General_Set,
                BorderRadius = 5,
                BackgroundColor = Constants.NELFTGreen,
                TextColor = Color.Black,
                Command = new Command(() =>
                {
                    var total = string.Empty;
                    foreach (var t in times)
                    {
                        if (!string.IsNullOrEmpty(t))
                            total += string.Format("{0}|", t);
                    }
                    App.Self.MessageEvents.BroadcastIt("TimeSet", total);
                })
            };

            var btnClear = new Button
            {
                Text = Langs.General_Clear,
                BorderRadius = 5,
                BackgroundColor = Constants.NELFTGreen,
                TextColor = Color.Black,
                Command = new Command(() =>
                {
                    for (var i = 0; i < totalEntries; ++i)
                        entryBoxes[i].Text = string.Empty;
                    times.Clear();
                    var total = string.Empty;
                    App.Self.MessageEvents.BroadcastIt("TimeSet", total);
                })
            };

            var width = (App.ScreenSize.Width * .8) / 2;

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .8,
                RowDefinitions =
                {
                    new RowDefinition{Height = GridLength.Auto},
                    new RowDefinition{Height = GridLength.Auto},
                    new RowDefinition{Height = GridLength.Auto},
                },
                ColumnSpacing = 4,
                ColumnDefinitions =
                {
                    new ColumnDefinition{Width = width},
                    new ColumnDefinition{Width = width},
                }
            };

            for (int i = 0; i < totalEntries; ++i)
            {
                labelBoxes.Add(new Label
                {
                    Text = string.Format("{0} {1}", Langs.NewAppt_Time, i + 1),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Constants.NELFTBlue,
                    FontSize = Constants.SmallEntryFontSize
                });
                entryBoxes.Add(new CustomEntry
                {
                    TextColor = Constants.NELFTBlue,
                    FontSize = Constants.GeneralFontSize,
                    StyleId = i.ToString(),
                    PlaceholderColor = Constants.NELFTOrange,
                    Placeholder = Langs.Helper_Time
                });
                entryBoxes[i].TextChanged += (sender, e) =>
                             {
                                 var ent = sender as CustomEntry;
                                 if (!string.IsNullOrEmpty(ent.Text))
                                 {
                                     if (AcceptedCharacter(ent.Text[ent.Text.Length - 1].ToString()))
                                         times[Convert.ToInt32(ent.StyleId)] = ent.Text;
                                     else
                                     {
                                         if (ent.Text.Length >= 2)
                                             ent.Text = ent.Text.Substring(0, ent.Text.Length - 1);
                                         else
                                             ent.Text = string.Empty;
                                     }
                                 }
                             };
                var intStack = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { labelBoxes[i], entryBoxes[i] }
                };
                grid.Children.Add(intStack, i, currentRow);
            }

            var lblMoreTap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 1,
                Command = new Command(() =>
                {
                    if (currentRow < 4)
                    {
                        currentRow++;
                        totalEntries += 2;
                        for (int i = currentRow * 2; i < totalEntries; ++i)
                        {
                            labelBoxes.Add(new Label
                            {
                                Text = string.Format("{0} {1}", Langs.NewAppt_Time, i + 1),
                                FontAttributes = FontAttributes.Bold,
                                TextColor = Constants.NELFTBlue,
                                FontSize = Constants.SmallEntryFontSize
                            });
                            entryBoxes.Add(new CustomEntry
                            {
                                TextColor = Constants.NELFTBlue,
                                FontSize = Constants.GeneralFontSize,
                                StyleId = i.ToString()
                            });
                            entryBoxes[i].TextChanged += (sender, e) =>
                             {
                                 var ent = sender as CustomEntry;
                                 times[Convert.ToInt32(ent.StyleId)] = ent.Text;
                             };
                            var intStack = new StackLayout
                            {
                                Orientation = StackOrientation.Vertical,
                                Children = { labelBoxes[i], entryBoxes[i] }
                            };
                            grid.Children.Add(intStack, i - (currentRow * 2), currentRow);
                        }
                    }
                })
            };
            lblAddMore.GestureRecognizers.Add(lblMoreTap);

            Content = CreateContent(new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width * .95,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = App.ScreenSize.Height * .8,
                Children =
                {
                    lblTimeHeading,
                    lblMessage,
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Children =
                        {
                            grid,
                            lblAddMore,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.End,
                                Children =
                                {
                                    btnSet, btnClear
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}

