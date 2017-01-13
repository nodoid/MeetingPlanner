using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MeetingPlanner.Languages;
using System.Collections.Generic;

namespace MeetingPlanner
{
    public class DateSelector : BaseView
    {
        int year, month;

        public DateSelector()
        {
            month = DateTime.Now.Month;
            year = DateTime.Now.Year;
            CreateUI(DateTime.Now.Month, DateTime.Now.Year);
        }

        void CreateUI(int month, int year)
        {
            var dtn = new DateTime(year, month, DateTime.Now.Day);

            var months = new List<string>
            {
                 Langs.Month_Jan, Langs.Month_Feb, Langs.Month_Mar, Langs.Month_Apr,
                 Langs.Month_May, Langs.Month_Jun, Langs.Month_Jul, Langs.Month_Aug,
                 Langs.Month_Sep, Langs.Month_Oct, Langs.Month_Nov, Langs.Month_Dec
            };

            var lblDate = new Label
            {
                Text = string.Format("{0} {1}", months[month - 1], year.ToString()),
                BackgroundColor = Constants.NELFTOrange,
                WidthRequest = App.ScreenSize.Width * .8,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var currentDay = dtn.Day;

            var btnBack = new Button
            {
                Text = "<",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                WidthRequest = App.ScreenSize.Width * .1,
            };

            var btnNext = new Button
            {
                Text = ">",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                WidthRequest = App.ScreenSize.Width * .1
            };


            var stackTop = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { btnBack, lblDate, btnNext }
            };


            var width = (App.ScreenSize.Width * .6) / 7;
            var height = (App.ScreenSize.Height * .4) / 7;

            var grid = new Grid
            {
                WidthRequest = App.ScreenSize.Width * .7 + 8,
                HeightRequest = App.ScreenSize.Height * .5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(8, 0),
                RowDefinitions =
                {
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height },
                    new RowDefinition { Height = height }
                },
                ColumnSpacing = 4,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                    new ColumnDefinition { Width = width },
                }
            };

            var dayLabels = CreateDayLabels();
            var dateLabels = CreateDateLabels(dtn);

            var lblSelectedDate = new Label
            {
                FontSize = Constants.SubHeadingFontSize,
                TextColor = Constants.NELFTBlue
            };

            var lblHeading = new Label
            {
                FontSize = Constants.HeadlineFontSize,
                Text = Langs.NewAppt_DateTitle,
                TextColor = Constants.NELFTGreen,
                HorizontalTextAlignment = TextAlignment.Center
            };

            int left = 0, top = 0;
            foreach (var dl in dayLabels)
                grid.Children.Add(dl, left++, top);

            left = (int)dtn.FirstDayOfMonth().DayOfWeek;
            top++;

            if (left < 0)
                left = 0;

            foreach (var dl in dateLabels)
            {
                var day = Convert.ToInt32(dl.Text);
                if (month == dtn.Month && year == dtn.Year)
                {
                    if (day >= dtn.Day)
                    {
                        var tgr = new TapGestureRecognizer();
                        tgr.Tapped += (sender, e) =>
                        {
                            var lbl = sender as Label;
                            var ending = lbl.StyleId == "1" || lbl.StyleId == "21" || lbl.StyleId == "31" ? "st" : lbl.StyleId == "2" || lbl.StyleId == "22" ? "nd" : lbl.StyleId == "3" || lbl.StyleId == "23" ? "rd" : "th";
                            lblSelectedDate.Text = string.Format("{0}{1} {2} {3}", lbl.StyleId, ending, months[month - 1], year);
                            App.Self.MessageEvents.BroadcastIt("DateSet", lblSelectedDate.Text);
                        };
                        dl.GestureRecognizers.Add(tgr);
                    }
                }
                else
                {
                    if (month >= dtn.Month && year >= dtn.Year)
                    {
                        var tgr = new TapGestureRecognizer();
                        tgr.Tapped += (sender, e) =>
                        {
                            var lbl = sender as Label;
                            var ending = lbl.StyleId == "1" || lbl.StyleId == "21" || lbl.StyleId == "31" ? "st" : lbl.StyleId == "2" || lbl.StyleId == "22" ? "nd" : lbl.StyleId == "3" || lbl.StyleId == "23" ? "rd" : "th";
                            lblSelectedDate.Text = string.Format("{0}{1} {2} {3}", lbl.StyleId, ending, months[month - 1], year);
                            App.Self.MessageEvents.BroadcastIt("DateSet", lblSelectedDate.Text);
                        };
                        dl.GestureRecognizers.Add(tgr);
                    }
                }
                grid.Children.Add(dl, left, top);

                left++;
                if (left == 7)
                {
                    left = 0;
                    top++;
                }
            }

            var cont = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HeightRequest = App.ScreenSize.Height * .8,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new StackLayout
                    {
                        Children =
                        {
                            lblHeading,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                WidthRequest = App.ScreenSize.Width * .8,
                        Children =
                        {
                            new Label
            {
                                Text = Langs.NewAppt_Selected,
                                        FontSize = Constants.SubHeadingFontSize,
                TextColor = Constants.NELFTBlue
            },
                            lblSelectedDate
                        }
                            }
                        }
                    },
                    new StackLayout
                    {
                        BackgroundColor = Color.Gray,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = App.ScreenSize.Width * .70 + 8,
                        Children = {
                        stackTop,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children = {grid}
                    }
                        }
                    }
                }
            };

            Content = CreateContent(cont);

            btnBack.Clicked += delegate
            {
                month--;
                if (month == 0)
                {
                    month = 12;
                    year -= 1;
                }

                CreateUI(month, year);
            };

            btnNext.Clicked += delegate
            {
                month++;
                if (month == 13)
                {
                    month = 1;
                    year += 1;
                }

                CreateUI(month, year);
            };
        }

        ObservableCollection<Label> CreateDayLabels()
        {
            return new ObservableCollection<Label>
            {
                new Label {Text = Langs.Day_Sunday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Monday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Tuesday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Wednesday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Thursday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Friday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
                new Label {Text = Langs.Day_Saturday, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center},
            };
        }

        ObservableCollection<Label> CreateDateLabels(DateTime today)
        {
            var labelList = new ObservableCollection<Label>();
            var color = Color.White;

            for (var n = 0; n < DateTime.DaysInMonth(today.Year, today.Month); ++n)
            {
                if (today.Month == DateTime.Now.Month && today.Year == DateTime.Now.Year)
                {
                    if (n + 1 < today.Day)
                        color = Color.Gray;
                    else
                        color = Color.White;
                }
                else
                {
                    if (today.Month < DateTime.Now.Month)
                        color = Color.Gray;
                    else
                        color = Color.White;
                }

                labelList.Add
                (
                    new Label
                    {
                        BackgroundColor = color,
                        Text = (n + 1).ToString(),
                        StyleId = (n + 1).ToString(),
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                );
            }
            return labelList;
        }
    }
}

