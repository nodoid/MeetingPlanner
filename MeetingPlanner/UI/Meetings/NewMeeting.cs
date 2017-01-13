using System.Collections.Generic;
using MeetingPlanner.Languages;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq.Expressions;

namespace MeetingPlanner
{
    public class NewMeeting : BasePage
    {
        public StackLayout stack;
        StackLayout innerStack;
        bool titleSet, nameSet, dateSet, timeSet, invitesSet, allDone, locSet, meetSet;
        List<string> emailAddresses, meetingTimes, locations, locationsSub;
        string calendarDate, title, myName, loc, message, description;
        readonly string email;
        SliderView slider;
        int meetingLength;

        bool AllDone
        {
            get { return allDone; }
            set
            {
                allDone = value;
                OnPropertyChanged("AllDone");
            }
        }

        public NewMeeting()
        {
            titleSet = nameSet = dateSet = timeSet = invitesSet = allDone = false;
            emailAddresses = new List<string>();
            meetingTimes = new List<string>();
            calendarDate = title = myName = loc = /*subloc =*/ message = description = string.Empty;
            email = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String);
            CreateEvents();
            CreateUI();
        }

        void CreateUI()
        {
            stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            innerStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Horizontal,
                MinimumWidthRequest = App.ScreenSize.Width,
                WidthRequest = App.ScreenSize.Width,
                HeightRequest = App.ScreenSize.Height - 52,
            };

            var topbar = new TopBar(Langs.Menu_NewMeeting, this, "icoback", "icomenu", innerStack).CreateTopBar();
            stack.HeightRequest = App.ScreenSize.Height - topbar.HeightRequest;

            var schedule = new Scheduler();

            slider = new SliderView(schedule, App.ScreenSize.Height * 0.8, App.ScreenSize.Width)
            {
                TransitionLength = 200,
                StyleId = "SliderView",
                MinimumSwipeDistance = 50
            };

            slider.Children.Add(new DateSelector());
            slider.Children.Add(new TimeSelector());
            slider.Children.Add(new Invites());

            var btnGo = new Button
            {
                Text = Langs.NewAppt_Done,
                BackgroundColor = Constants.NELFTGreen,
                TextColor = Color.White,
                WidthRequest = App.ScreenSize.Width * .6,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Constants.ButtonFontSize,
                IsVisible = false,
                HeightRequest = 36,
                BorderRadius = 4,
                Command = new Command(async (t) =>
                    {
                        var results = await CheckData();
                        if (results != DataResults.OK)
                        {
                            switch (results)
                            {
                                case DataResults.TimesFail:
                                    await DisplayAlert(Langs.Error_Message_Times_Fail, Langs.Error_Message_Times_Bad, Langs.General_OK);
                                    break;
                                case DataResults.TimesInvalid:
                                    await DisplayAlert(Langs.Error_Message_Times_Fail, Langs.Error_Message_Times_Invalid, Langs.General_OK);
                                    break;
                                case DataResults.EmailsExternal:
                                    await DisplayAlert(Langs.Error_Message_Email_Fail, Langs.Error_Message_Email_Bad, Langs.General_OK);
                                    break;
                                case DataResults.EmailsInvalid:
                                    await DisplayAlert(Langs.Error_Message_Email_Fail, Langs.Error_Message_Email_Invalid, Langs.General_OK);
                                    break;
                            }
                            return;
                        }

                        var meetingId = App.Self.DBManager.GetLastId<Meeting>();
                        var pollId = App.Self.DBManager.GetLastId<Polling>();
                        var attendeeId = App.Self.DBManager.GetLastId<Attendees>();
                        var timeId = App.Self.DBManager.GetLastId<Times>();
                        var apptId = App.Self.DBManager.GetLastId<AppointmentList>();

                        var appt = new AppointmentList
                        {
                            id = apptId,
                            Venue = loc,
                            DateTimeFrom = calendarDate,
                            MeetingId = meetingId,
                            MeetingName = title,
                            Length = meetingLength,
                            Room = loc,
                            DateDue = Convert.ToDateTime(calendarDate.Replace("th", "").Replace("st", "").Replace("nd", "").Replace("rd", "")),
                            UserId = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String)
                        };
                        App.Self.DBManager.AddOrUpdateAppointments(appt);

                        var calDate = calendarDate.Replace("th", "").Replace("st", "").Replace("nd", "").Replace("rd", "");

                        var meeting = new Meeting
                        {
                            id = meetingId,
                            IsMyMeeting = 1,
                            IveResponded = 0,
                            TimeId = timeId,
                            MeetingSchedule = Convert.ToDateTime(calDate),
                        };
                        App.Self.DBManager.AddOrUpdateMeeting(meeting);

                        foreach (var w in meetingTimes)
                        {
                            if (!string.IsNullOrEmpty(w))
                            {
                                var time = new Times
                                {
                                    id = timeId,
                                    MeetingId = meetingId,
                                    Time = w,
                                };
                                App.Self.DBManager.AddOrUpdateTimes(time);

                                timeId++;
                            }
                        }

                        //TODO: 
                        //UserId and AttendeeId are returned from the service

                        foreach (var a in emailAddresses)
                        {
                            if (!string.IsNullOrEmpty(a))
                            {
                                var nelft = a;
                                if (nelft.Contains("nelft"))
                                {
                                    var n = a.Split('@')[0];
                                    var names = n.Split(' ');
                                    var first = names[0];
                                    var last = names[names.Length - 1];
                                    first = Char.ToUpper(first[0]).ToString();
                                    last = Char.ToUpper(last[0]).ToString();
                                    nelft = last + first.Substring(0, 2);
                                }
                                var emailAddr = new Attendees
                                {
                                    id = attendeeId,
                                    MeetingId = meetingId,
                                    Attending = 0,
                                    Email = a,
                                    IsOrganiser = 0,
                                    Name = a,
                                    UserId = nelft
                                };
                                var currentAttId = attendeeId;

                                for (var i = 0; i < meetingTimes.Count; ++i)
                                {
                                    var pollCast = new PollCast
                                    {
                                        PollDataId = pollId,
                                        AttendeeId = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String),
                                        Accept = 0,
                                        id = App.Self.DBManager.GetLastId<PollCast>(),
                                    };
                                    currentAttId++;
                                    App.Self.DBManager.AddOrUpdatePollCast(pollCast);
                                }

                                attendeeId++;
                                App.Self.DBManager.AddOrUpdateAttendees(emailAddr);
                            }
                        }

                        var pid = App.Self.DBManager.GetLastId<Polling>();
                        var poll = new Polling
                        {
                            id = pid,
                            PollId = pollId,
                            SuggestedDate = Convert.ToDateTime(calDate),
                            MeetingId = meetingId,
                        };

                        var pdd = App.Self.DBManager.GetLastId<PollingData>();
                        var pollData = new PollingData
                        {
                            PollId = pollId,
                            id = pdd,
                            TotalVotesReceived = 0,
                            Votes = 0
                        };

                        App.Self.DBManager.AddOrUpdatePolling(poll);
                        App.Self.DBManager.AddOrUpdatePollingData(pollData);

#if DEBUG
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetSingleObject<AppointmentList>("id", apptId.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetSingleObject<Meeting>("id", meetingId.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetListOfObjects<Times>("MeetingId", meetingId.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetListOfObjects<Attendees>("MeetingId", meetingId.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetListOfObjects<PollCast>("PollDataId", pollId.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetSingleObject<Polling>("id", pid.ToString())));
                        Debug.WriteLine(JsonConvert.SerializeObject(App.Self.DBManager.GetSingleObject<PollingData>("id", pdd.ToString())));
#endif
                        dynamic appty = new { AppointmentList = App.Self.DBManager.GetSingleObject<AppointmentList>("id", apptId.ToString()) };
                        await Webservices.SendData("setAppointment.php", JsonConvert.SerializeObject(appty));
                        dynamic meety = new { Meeting = App.Self.DBManager.GetSingleObject<Meeting>("id", meetingId.ToString()) };
                        await Webservices.SendData("setMeeting.php", JsonConvert.SerializeObject(meety));
                        dynamic timey = new { Times = App.Self.DBManager.GetListOfObjects<Times>("MeetingId", meetingId.ToString()) };
                        await Webservices.SendData("setTimes.php", JsonConvert.SerializeObject(timey));
                        dynamic att = new { Attendees = App.Self.DBManager.GetListOfObjects<Attendees>("MeetingId", meetingId.ToString()) };
                        await Webservices.SendData("setAttendees.php", JsonConvert.SerializeObject(att));
                        dynamic pollcasty = new { PollCast = App.Self.DBManager.GetListOfObjects<PollCast>("PollDataId", pollId.ToString()) };
                        await Webservices.SendData("setPollCast.php", JsonConvert.SerializeObject(pollcasty));
                        dynamic polly = new { Polling = App.Self.DBManager.GetSingleObject<Polling>("id", pid.ToString()) };
                        await Webservices.SendData("setPolling.php", JsonConvert.SerializeObject(polly));
                        dynamic pollydata = new { PollingData = App.Self.DBManager.GetSingleObject<PollingData>("id", pdd.ToString()) };
                        await Webservices.SendData("setPollingData.php", JsonConvert.SerializeObject(pollydata));

                        await DisplayAlert(Langs.NewAppt_InviteDoneTitle, Langs.NewAppt_InviteDoneMessage, Langs.General_OK);
                        await Navigation.PopAsync();
                    })
            };

            PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "AllDone")
                    btnGo.IsVisible = AllDone;
            };

            stack.Children.Add(slider);
            stack.Children.Add(btnGo);

            innerStack.Children.Add(stack);

            var contentStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.Start,
                        WidthRequest = App.ScreenSize.Width,
                        Children = { topbar }
                    },
                    innerStack
                }
            };

            var relView = new RelativeLayout
            {
                VerticalOptions = LayoutOptions.Start/*CenterAndExpand*/,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            Func<RelativeLayout, double> btnGoWidth = (parent) => btnGo.Measure(relView.Width, relView.Height).Request.Width;


            relView.Children.Add(contentStack, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(App.ScreenSize.Width), Constraint.RelativeToParent((parent) => App.ScreenSize.Height * .75));
            relView.Children.Add(btnGo, Constraint.RelativeToParent((parent) =>
                ((parent.Width / 2) - (btnGoWidth(parent) / 2))), Constraint.Constant(App.ScreenSize.Height * .87));

            Content = relView;
        }

        async Task<DataResults> CheckData()
        {
            foreach (var em in emailAddresses)
                if (!string.IsNullOrEmpty(em))
                {
                    if (!em.CheckPattern())
                        return DataResults.EmailsInvalid;
                }

            foreach (var t in meetingTimes)
            {
                if (t.TimeCheck())
                {
                    if (!t.Contains("-") && !t.Contains(":"))
                        return DataResults.TimesInvalid;
                }
            }
            return DataResults.OK;
        }

        void CreateEvents()
        {
            App.Self.MessageEvents.Change += (s, ea) =>
            {
                switch (ea.ModuleName)
                {
                    case "DateSet":
                        calendarDate = ea.Message;
                        dateSet = !string.IsNullOrEmpty(ea.Message);
                        AllDone = TestSetters();
                        break;
                    case "TitleSet":
                        title = ea.Message;
                        titleSet = !string.IsNullOrEmpty(title);
                        AllDone = TestSetters();
                        break;
                    case "NameSet":
                        myName = ea.Message;
                        nameSet = !string.IsNullOrEmpty(myName);
                        AllDone = TestSetters();
                        break;
                    case "TimeSet":
                        meetingTimes.Clear();
                        meetingTimes.AddRange(ea.Message.Split('|').ToList());
                        timeSet = meetingTimes.Count != 0;
                        allDone = TestSetters();
                        var times = meetingTimes[0].Split('-');
                        var timeEnd = Convert.ToDateTime(times[1]);
                        var timeStart = Convert.ToDateTime(times[0]);
                        meetingLength = Convert.ToInt32(timeEnd.Subtract(timeStart).TotalMinutes);
                        break;
                    case "InvitesSet":
                        emailAddresses.Clear();
                        emailAddresses.AddRange(ea.Message.Split('|').ToList());
                        invitesSet = emailAddresses.Count != 0;
                        AllDone = TestSetters();
                        break;
                    case "LocSet":
                        loc = ea.Message;
                        locSet = !string.IsNullOrEmpty(loc);
                        AllDone = TestSetters();
                        break;
                    /*case "SublocSet":
                        subloc = ea.Message;
                        sublocSet = !string.IsNullOrEmpty(subloc);
                        AllDone = TestSetters();
                        break;*/
                    case "MessageSet":
                        message = ea.Message;
                        break;
                    case "DescSet":
                        description = ea.Message;
                        meetSet = !string.IsNullOrEmpty(description);
                        AllDone = TestSetters();
                        break;
                }
            };
        }

        bool TestSetters()
        {
            return titleSet && nameSet && dateSet && timeSet && invitesSet && locSet && /*sublocSet &&*/ meetSet;
        }
    }
}
