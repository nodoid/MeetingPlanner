using System;
using System.Linq;
using System.Threading.Tasks;
using MeetingPlanner.Languages;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace MeetingPlanner
{
    public class DataGatherer
    {
        public static int ThisWeek
        {
            get
            {
                var startOfWeek = DateTime.Now.StartOfWeek();
                var endOfWeek = startOfWeek.AddDays(4);

                var appts = App.Self.DBManager.GetListOfObjects<AppointmentList>().Where(t => t.DateDue >= startOfWeek).Where(t => t.DateDue <= endOfWeek).ToList();

                return appts.Count;
            }
        }

        public static async Task GetInitialGather()
        {
            if (App.Self.IsConnected)
            {
                var username = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String);

                App.Self.NetSpinner.Spinner(true, Langs.Spinner_GettingData, Langs.Spinner_Wait);
                var appts = await Webservices.GetListData<BaseAppointmentList>("getAllAppointments.php", "userId", username);
                var apptList = (from apl in appts.AppointmentList
                                select apl).ToList();

                App.Self.DBManager.AddOrUpdateAppointments(apptList);
                var meetings = await Webservices.GetListData<BaseMeeting>("getMeetingsForUserId.php", "userId", username);
                var meets = (from mtg in meetings.Meeting
                             select mtg).ToList();

                App.Self.DBManager.AddOrUpdateMeeting(meets);
                foreach (var m in meets)
                {
                    await GetMeetingData(m.id.ToString());
                }

                App.Self.UserSettings.SaveSetting("LastUpdated", DateTime.Now.ToString("d"), SettingType.String);
                App.Self.NetSpinner.Spinner(false, Langs.Spinner_GettingData, Langs.Spinner_Wait);
                Notify(appts.AppointmentList.Count);
            }
            else
                await Application.Current.MainPage.DisplayAlert(Langs.Error_Message_Login_Fail, Langs.Error_Message_Login_No_User, Langs.General_OK);
        }

        public static async Task GetDataGatherForDates()
        {
            if (App.Self.IsConnected)
            {
                var lastDate = App.Self.UserSettings.LoadSetting<string>("LastUpdated", SettingType.String)?.Replace('/', '_');
                if (string.IsNullOrEmpty(lastDate))
                {
                    await GetInitialGather();
                    return;
                }

                var username = App.Self.UserSettings.LoadSetting<string>("Username", SettingType.String);

                App.Self.NetSpinner.Spinner(true, Langs.Spinner_GettingData, Langs.Spinner_Wait);
                var appts = await Webservices.GetListData<BaseAppointmentList>("getAppointmentsForDateRange.php", "userId=", username, "&startDate=", lastDate, "&endDate=", DateTime.Now.Date.ToString().Replace('/', '_'));
                if (appts != null)
                {
                    if (appts.AppointmentList.Count != 0)
                    {
                        foreach (var appt in appts.AppointmentList)
                        {
                            var meetingId = appt.MeetingId.ToString();

                            var meeting = await Webservices.GetData<BaseMeeting>("getMeetingsForMeetingId.php", "meetingId", meetingId);
                            var meets = (from mtg in meeting.Meeting
                                         select mtg).ToList();

                            App.Self.DBManager.AddOrUpdateMeeting(meets);
                            foreach (var m in meets)
                            {
                                await GetMeetingData(meetingId);
                            }
                        }
                    }
                    Notify(appts.AppointmentList.Count);
                }
                else
                    Notify(0);
                App.Self.UserSettings.SaveSetting("LastUpdated", DateTime.Now.ToString("d"), SettingType.String);
                App.Self.NetSpinner.Spinner(false, string.Empty, string.Empty);

            }
            else
                await Application.Current.MainPage.DisplayAlert(Langs.Error_Message_Login_Fail, Langs.Error_Message_Login_No_User, Langs.General_OK);
        }

        static void Notify(int n)
        {
            if (n != 0)
            {
                CrossLocalNotifications.Current.Show(Langs.Notifier_MeetingsTitle, string.Format("{0}{1} {2}", Langs.Notifier_MeetingsOne, n, n == 1 ? Langs.Notifier_MeetingsNew : Langs.Notifier_MeetingsNewPl));
            }
            else
            {
                CrossLocalNotifications.Current.Show(Langs.Notifier_MeetingsTitle, Langs.Notifier_MeetingsNone);
            }
        }

        static async Task GetMeetingData(string meetingId)
        {
            var times = await Webservices.GetListData<BaseTimes>("getTimes.php", "meetingId", meetingId);
            var t = (from ti in times.Times
                     select ti).ToList();
            App.Self.DBManager.AddOrUpdateTimes(t);

            var polls = await Webservices.GetListData<BasePolling>("getPollForMeeting.php", "meetingId", meetingId);
            var pl = (from p in polls.Polling
                      select p).ToList();

            App.Self.DBManager.AddOrUpdatePolling(pl);

            var attend = await Webservices.GetListData<BaseAttendees>("getAttendeesForMeeting.php", "meetingId", meetingId);
            var att = (from ate in attend.Attendees
                       select ate).ToList();
            App.Self.DBManager.AddOrUpdateAttendees(att);

            foreach (var p in pl)
            {
                var pcast = await Webservices.GetListData<BasePollCast>("getPollCastForPollId.php", "pollId", p.PollId.ToString());
                var cast = (from pc in pcast.PollCast
                            select pc).ToList();

                App.Self.DBManager.AddOrUpdatePollCast(cast);

                var pdata = await Webservices.GetListData<BasePollingData>("getPollDataForPoll.php", "pollId", p.PollId.ToString());
                var pd = (from pr in pdata.PollingData
                          select pr).ToList();

                App.Self.DBManager.AddOrUpdatePollingData(pd);
            }
        }
    }
}
