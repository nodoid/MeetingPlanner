using System;

using Xamarin.Forms;
using System.Collections.Generic;
using MeetingPlanner.Languages;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using Messier16.Forms.Controls;

namespace MeetingPlanner
{
    public class AttendeesView : PopupPage
    {
        ObservableCollection<Attendees> attendees = new ObservableCollection<Attendees>();

        public AttendeesView(List<Attendees> atds)
        {
            foreach (var a in atds)
                attendees.Add(a);

            var listView = new ListView
            {
                ItemsSource = attendees,
                ItemTemplate = new DataTemplate(typeof(AttendeeViewCell)),
                HasUnevenRows = true,
                SeparatorVisibility = SeparatorVisibility.None,
                WidthRequest = App.ScreenSize.Width * .75,
                HeightRequest = App.ScreenSize.Height * .55
            };

            var btnClose = new Button
            {
                Text = Langs.General_OK,
                BorderRadius = 5,
                BackgroundColor = Color.Blue,
                TextColor = Color.White,
            };
            btnClose.Clicked += OnClose;

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width * .6,
                HeightRequest = App.ScreenSize.Height * .6,
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(20, 20),
                Children =
                {
                    new Label
                    {
                        Text = Langs.List_Attending,
                        FontSize = 18,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    listView,
                    btnClose
                }
            };
        }

        async void OnClose(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }

    public class AttendeeViewCell : ViewCell
    {
        public AttendeeViewCell()
        {
            var lblName = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblEmail = new Label
            {
                Text = "title",
                TextColor = Color.Blue,
                FontSize = 14
            };

            var lblAttending = new Label
            {
                Text = Langs.List_Attending,
                TextColor = Color.Blue,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 14
            };

            var lblOrganisers = new Label
            {
                Text = Langs.List_Organiser,
                TextColor = Color.Blue,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 14
            };

            /*var swtchAttending = new Switch
            {
                IsEnabled = false
            };*/

            /*var swtchOrganiser = new Switch
            {
                IsEnabled = false
            };*/
            var chkOrganiser = new Checkbox
            {
                IsEnabled = false
            };
            var chkAttending = new Checkbox
            {
                IsEnabled = false
            };

            lblName.SetBinding(Label.TextProperty, new Binding("Name"));
            lblEmail.SetBinding(Label.TextProperty, new Binding("Email"));

            chkOrganiser.SetBinding(Checkbox.CheckedProperty, new Binding("IsOrganiser", converter: new BoolToString()));
            chkAttending.SetBinding(Checkbox.CheckedProperty, new Binding("Attending", converter: new BoolToString()));

            View = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(20, 20),
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                           new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        WidthRequest = App.ScreenSize.Width * .5,
                        Children = {lblName}
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        WidthRequest = App.ScreenSize.Width * .5,
                        Children = {lblEmail}
                    }
                        }
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                           new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        WidthRequest = App.ScreenSize.Width * .5,
                                Children = {lblAttending, chkAttending}
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        WidthRequest = App.ScreenSize.Width * .5,
                                Children = {lblOrganisers, chkOrganiser}
                    }
                        }
                    }
                }
            };
        }
    }
}

