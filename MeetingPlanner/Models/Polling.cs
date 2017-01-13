using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;

namespace MeetingPlanner
{
    public class BasePolling
    {
        public List<Polling> Polling { get; set; }
    }

    public class BasePoll
    {
        public Polling Polling { get; set; }
    }

    public class Polling : IInterface
    {
        public int id { get; set; }

        public int MeetingId { get; set; }

        public int PollId { get; set; }

        public DateTime SuggestedDate { get; set; }


    }

    public class PollingHelpers
    {
        public static PollingData Poll(int pollId)
        {
            return App.Self.DBManager.GetSingleObject<PollingData>("id", pollId.ToString());
        }

        public static Meeting Meeting(int MeetingId)
        {
            return App.Self.DBManager.GetSingleObject<Meeting>("id", MeetingId.ToString());
        }
    }


    public class BasePollingData
    {
        public List<PollingData> PollingData { get; set; }
    }

    public class BasePD
    {
        public PollingData PollingData { get; set; }
    }

    public class PollingData : IInterface
    {
        public int id { get; set; }

        public int PollId { get; set; }

        public int TotalVotesReceived { get; set; }

        public int Votes { get; set; }
    }

    public class PollingDataHelpers
    {
        public static List<PollCast> Polling(int PollId)
        {
            return App.Self.DBManager.GetListOfObjects<PollCast>("PollDataId", PollId.ToString());
        }
    }


    public class BasePollCast
    {
        public List<PollCast> PollCast { get; set; }
    }

    public class BasePollCast2
    {
        public PollCast PollCast { get; set; }
    }

    public class PollCast : IInterface
    {
        public int id { get; set; }

        public int PollDataId { get; set; }

        public string AttendeeId { get; set; }

        public int Accept { get; set; }
    }
}
