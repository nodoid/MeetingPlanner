using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace MeetingPlanner
{
    public class BaseAttendees
    {
        public List<Attendees> Attendees { get; set; }
    }

    public class BaseAttend
    {
        public Attendees Attendees { get; set; }
    }

    public class Attendees : IInterface
    {
        public int id { get; set; }

        public int MeetingId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Attending { get; set; }

        public int IsOrganiser { get; set; }
    }
}
