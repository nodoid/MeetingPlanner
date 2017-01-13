using System;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace MeetingPlanner
{
    public class BaseTimes
    {
        public List<Times> Times { get; set; }
    }

    public class BaseTime
    {
        public Times Times { get; set; }
    }

    public class Times : IInterface
    {
        public int id { get; set; }

        public int MeetingId { get; set; }

        public string Time { get; set; }
    }
}
