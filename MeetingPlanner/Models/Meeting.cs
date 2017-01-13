using System;
using SQLite.Net.Attributes;
using System.Collections.Generic;
namespace MeetingPlanner
{
    public class BaseMeeting
    {
        public List<Meeting> Meeting { get; set; }
    }

    public class BaseMeet
    {
        public Meeting Meeting { get; set; }
    }

    public class Meeting : IInterface
    {
        public int id { get; set; }

        public int TimeId { get; set; }

        public DateTime MeetingSchedule { get; set; }

        public int IsMyMeeting { get; set; }

        public int IveResponded { get; set; }


    }

    public class MeetingHelper
    {
        public static List<Attendees> Invited(int id)
        {
            return App.Self.DBManager.GetListOfObjects<Attendees>("MeetingId", id.ToString());
        }

        public static List<Times> MeetingTimes(int id)
        {
            return App.Self.DBManager.GetListOfObjects<Times>("MeetingId", id.ToString());
        }
    }
}
