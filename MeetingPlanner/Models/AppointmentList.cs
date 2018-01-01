using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace MeetingPlanner
{
    public class BaseAppointmentList
    {
        public List<AppointmentList> AppointmentList { get; set; }
    }

    public class BaseAppointment
    {
        public AppointmentList AppointmentList { get; set; }
    }

    public class AppointmentList : IInterface
    {
        public int id { get; set; }

        public int MeetingId { get; set; }

        public string MeetingName { get; set; }

        public string DateTimeFrom { get; set; }

        public int Length { get; set; }

        public string Room { get; set; }

        public string Venue { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime DateDue { get; set; }

        public string UserId { get; set; }
    }

    public class AppointmentListHelpers
    {
        public static Meeting GetMeeting(int MeetingId)
        {
            var _ = App.Self.DBManager.GetSingleObject<Meeting>("id", MeetingId.ToString());
            return _ ?? new Meeting();
        }

        public static Polling GetPolls(int MeetingId)
        {
            return App.Self.DBManager.GetSingleObject<Polling>("MeetingId", MeetingId.ToString());
        }

        public static List<Polling> PollList(int MeetingId)
        {
            return App.Self.DBManager.GetListOfObjects<Polling>("MeetingId", MeetingId.ToString());
        }

        public static int InvitedTotal(int MeetingId)
        {
            return App.Self.DBManager.GetListOfObjects<Attendees>("MeetingId", MeetingId.ToString()).Count;
        }

        public static int IsMine(int MeetingId)
        {
            return App.Self.DBManager.GetSingleObject<Meeting>("id", MeetingId.ToString()).IsMyMeeting;
        }
    }
}


