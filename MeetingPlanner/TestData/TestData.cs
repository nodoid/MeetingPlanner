using System;
using System.Collections.Generic;
using System.Threading;
namespace MeetingPlanner
{
#if DEBUG
    public class TestData
    {
        List<AppointmentList> appts = new List<AppointmentList>();
        List<Attendees> attds = new List<Attendees>();
        List<Meeting> mtgs = new List<Meeting>();
        List<PollCast> pc = new List<PollCast>();
        List<Times> tms = new List<Times>();
        List<Polling> pollList = new List<Polling>();
        List<PollingData> pollInfo = new List<PollingData>();

        public TestData()
        {
            attds.AddRange(new List<Attendees>()
            {
                new Attendees { id = 20, UserId="JohnsonPa", MeetingId = 1, Name = "Paul Johnnson", Email = "paul.johnson@nelft.nhs.uk", Attending = false, IsOrganiser = true },
                new Attendees { id = 0, UserId="BloggsFr", MeetingId = 1, Name = "Fred Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
            new Attendees { id = 1,UserId="BloggsJo",MeetingId = 1, Name = "John Bloggs", Email = "a@bc.com", Attending = true, IsOrganiser = false },
                new Attendees { id = 2,UserId="BloggsTi",MeetingId = 1, Name = "Tim Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 3,UserId="BloggsTo",MeetingId = 1, Name = "Tom Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 4,UserId="BloggsSi",MeetingId = 1, Name = "Sid Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },

            new Attendees {id = 5, UserId="BloggsFr",MeetingId = 2, Name = "Fred Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
            new Attendees { id = 6,UserId="BloggsJo",MeetingId = 2, Name = "John Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = true },
                new Attendees { id = 21,UserId="JohnsonPa",MeetingId = 2, Name = "Paul Johnnson", Email = "paul.johnson@nelft.nhs.uk", Attending = false, IsOrganiser = false },

                new Attendees { id = 7,UserId="BloggsTi",MeetingId = 3, Name = "Tim Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 8,UserId="BloggsTo",MeetingId = 3, Name = "Tom Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 9,UserId="BloggsSi",MeetingId = 3, Name = "Sid Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = true },
                new Attendees { id = 10,UserId="BloggsFr",MeetingId = 3, Name = "Fred Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },

            new Attendees { id = 11,UserId="BloggsJo",MeetingId = 4, Name = "John Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 12,UserId="BloggsTi",MeetingId = 4, Name = "Tim Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = true },
                new Attendees { id = 13,UserId="BloggsTo",MeetingId = 4, Name = "Tom Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 14,UserId="BloggsSi",MeetingId = 4, Name = "Sid Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },

                new Attendees { id = 15,UserId="BloggsFr",MeetingId = 5, Name = "Fred Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
            new Attendees { id = 16,UserId="BloggsJo",MeetingId = 5, Name = "John Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 17,UserId="BloggsTi",MeetingId = 5, Name = "Tim Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 18,UserId="BloggsTo",MeetingId = 5, Name = "Tom Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false },
                new Attendees { id = 19,UserId="BloggsSi",MeetingId = 5, Name = "Sid Bloggs", Email = "a@bc.com", Attending = false, IsOrganiser = false }});

            appts.AddRange(new List<AppointmentList>
            {
                new AppointmentList{id = 1,MeetingName="Test Meeting 1", DateTimeFrom="1st Sept 2016", MeetingId=1, Length=90,Room="B2C", Venue="The old pub" },
                new AppointmentList{id = 2,MeetingName="Test Meeting 2", DateTimeFrom="11th Sept 2016", MeetingId=2,Length=90,Room="B2C", Venue="The old pub" },
                new AppointmentList{id = 3,MeetingName="Test Meeting 3", DateTimeFrom="21st Sept 2016", MeetingId=3,Length=90, Room="B2C", Venue="The old pub" },
                new AppointmentList{id = 4,MeetingName="Test Meeting 4", DateTimeFrom="1st Oct 2016", MeetingId=4,Length=90, Room="B2C", Venue="The old pub" },
                new AppointmentList{id = 5,MeetingName="Test Meeting 5", DateTimeFrom="14th Oct 2016", MeetingId=5,Length=90, Room="B2C", Venue="The old pub" },
            });

            mtgs.AddRange(new List<Meeting>
            {
                new Meeting{id = 1, IsMyMeeting = true, IveResponded =true, TimeId = 1, MeetingSchedule = new DateTime(2016,9,1)},
                new Meeting{id=2, IsMyMeeting=false, IveResponded=false, TimeId = 2, MeetingSchedule = new DateTime(2016,9,11)},
                new Meeting{id = 3, IsMyMeeting = true, IveResponded =true, TimeId = 3, MeetingSchedule = new DateTime(2016,9,21)},
                new Meeting{id=4, IsMyMeeting=false, IveResponded=false, TimeId = 4, MeetingSchedule = new DateTime(2016,10,1)},
                new Meeting{id = 5, IsMyMeeting = true, IveResponded =true, TimeId =5, MeetingSchedule = new DateTime(2016,10,14)}
            });

            pollList.AddRange(new List<Polling>
            {
                new Polling { id = 1, PollId = 1, MeetingId = 1, SuggestedDate = new DateTime(2016, 9, 27)},
                new Polling { id = 2, PollId = 2, MeetingId = 2, SuggestedDate = new DateTime(2016, 9, 28)},
                new Polling { id = 3, PollId = 3, MeetingId = 3, SuggestedDate = new DateTime(2016, 9, 29)},
                new Polling { id = 4, PollId = 4, MeetingId = 4, SuggestedDate = new DateTime(2016, 10, 1)},
                new Polling { id = 5, PollId = 5, MeetingId = 5, SuggestedDate = new DateTime(2016, 10, 14)},
            });

            pollInfo.AddRange(new List<PollingData>
            {
                new PollingData{id = 1, PollId = 1, TotalVotesReceived = 4, Votes = 4},
                new PollingData{id = 2, PollId = 2, TotalVotesReceived = 4, Votes = 4},
                new PollingData{id = 3, PollId = 3, TotalVotesReceived = 4, Votes = 4},
                new PollingData{id = 4, PollId = 4, TotalVotesReceived = 4, Votes = 4},
                new PollingData{id = 5, PollId = 5, TotalVotesReceived = 4, Votes = 4}
            });

            pc.AddRange(new List<PollCast>
            {
                new PollCast{id = 0, PollDataId = 1, AttendeeId = 1, Accept = false},
                new PollCast{id = 1, PollDataId = 1, AttendeeId = 1, Accept = true},
                new PollCast{id = 2, PollDataId = 1, AttendeeId = 1, Accept = false},
                new PollCast{id = 3, PollDataId = 1, AttendeeId = 1, Accept = false},
                new PollCast{id = 4, PollDataId = 1, AttendeeId = 1, Accept = true},

                new PollCast{id = 5, PollDataId = 1, AttendeeId = 2, Accept = true},
                new PollCast{id = 6, PollDataId = 1, AttendeeId = 2, Accept = false},
                new PollCast{id = 7, PollDataId = 1, AttendeeId = 2, Accept = true},
                new PollCast{id = 8, PollDataId = 1, AttendeeId = 2, Accept = false},
                new PollCast{id = 9, PollDataId = 1, AttendeeId = 2, Accept = true},

                new PollCast{id = 10, PollDataId = 1, AttendeeId = 3, Accept = false},
                new PollCast{id = 11, PollDataId = 1, AttendeeId = 3, Accept = true},
                new PollCast{id = 12, PollDataId = 1, AttendeeId = 3, Accept = true},
                new PollCast{id = 13, PollDataId = 1, AttendeeId = 3, Accept = false},
                new PollCast{id = 14, PollDataId = 1, AttendeeId = 3, Accept = false},

                new PollCast{id = 15, PollDataId = 1, AttendeeId = 4, Accept = true},
                new PollCast{id = 16, PollDataId = 1, AttendeeId = 4, Accept = false},
                new PollCast{id = 17, PollDataId = 1, AttendeeId = 4, Accept = false},
                new PollCast{id = 18, PollDataId = 1, AttendeeId = 4, Accept = false},
                new PollCast{id = 19, PollDataId = 1, AttendeeId = 4, Accept = false},

                new PollCast{id = 20, PollDataId = 1, AttendeeId = 5, Accept = true},
                new PollCast{id = 21, PollDataId = 1, AttendeeId = 5, Accept = true},
                new PollCast{id = 22, PollDataId = 1, AttendeeId = 5, Accept = false},
                new PollCast{id = 23, PollDataId = 1, AttendeeId = 5, Accept = true},
                new PollCast{id = 24, PollDataId = 1, AttendeeId = 5, Accept = false},

                new PollCast{id = 25, PollDataId = 1, AttendeeId = "JohnsonPa", Accept = true},
                new PollCast{id = 26, PollDataId = 1, AttendeeId = "JohnsonPa", Accept = true},
                new PollCast{id = 27, PollDataId = 1, AttendeeId = "JohnsonPa", Accept = false},
                new PollCast{id = 28, PollDataId = 1, AttendeeId = "JohnsonPa", Accept = true},
                new PollCast{id = 29, PollDataId = 1, AttendeeId = "JohnsonPa", Accept = false},

                new PollCast{id = 30, PollDataId = 2, AttendeeId = 1, Accept = false},
                new PollCast{id = 31, PollDataId = 2, AttendeeId = 1, Accept = true},
                new PollCast{id = 32, PollDataId = 2, AttendeeId = 1, Accept = false},

                new PollCast{id = 33, PollDataId = 2, AttendeeId = 2, Accept = true},
                new PollCast{id = 34, PollDataId = 2, AttendeeId = 2, Accept = false},
                new PollCast{id = 35, PollDataId = 2, AttendeeId = 2, Accept = true},

                new PollCast{id = 36, PollDataId = 2, AttendeeId = "JohnsonPa", Accept = false},
                new PollCast{id = 37, PollDataId = 2, AttendeeId = "JohnsonPa", Accept = true},
                new PollCast{id = 38, PollDataId = 2, AttendeeId = "JohnsonPa", Accept = true},

                new PollCast{id = 39, PollDataId = 3, AttendeeId = 1, Accept = false},
                new PollCast{id = 40, PollDataId = 3, AttendeeId = 1, Accept = true},
                new PollCast{id = 41, PollDataId = 3, AttendeeId = 1, Accept = false},
                new PollCast{id = 42, PollDataId = 3, AttendeeId = 1, Accept = false},

                new PollCast{id = 43, PollDataId = 3, AttendeeId = 2, Accept = true},
                new PollCast{id = 44, PollDataId = 3, AttendeeId = 2, Accept = false},
                new PollCast{id = 45, PollDataId = 3, AttendeeId = 2, Accept = true},
                new PollCast{id = 46, PollDataId = 3, AttendeeId = 2, Accept = false},

                new PollCast{id = 47, PollDataId = 3, AttendeeId = 3, Accept = false},
                new PollCast{id = 48, PollDataId = 3, AttendeeId = 3, Accept = true},
                new PollCast{id = 49, PollDataId = 3, AttendeeId = 3, Accept = true},
                new PollCast{id = 50, PollDataId = 3, AttendeeId = 3, Accept = false},

                new PollCast{id = 51, PollDataId = 3, AttendeeId = 4, Accept = true},
                new PollCast{id = 52, PollDataId = 3, AttendeeId = 4, Accept = false},
                new PollCast{id = 53, PollDataId = 3, AttendeeId = 4, Accept = false},
                new PollCast{id = 54, PollDataId = 3, AttendeeId = 4, Accept = false},

                new PollCast{id = 55, PollDataId = 4, AttendeeId = 1, Accept = false},
                new PollCast{id = 56, PollDataId = 4, AttendeeId = 1, Accept = true},
                new PollCast{id = 57, PollDataId = 4, AttendeeId = 1, Accept = false},
                new PollCast{id = 58, PollDataId = 4, AttendeeId = 1, Accept = false},

                new PollCast{id = 59, PollDataId = 4, AttendeeId = 2, Accept = true},
                new PollCast{id = 60, PollDataId = 4, AttendeeId = 2, Accept = false},
                new PollCast{id = 61, PollDataId = 4, AttendeeId = 2, Accept = true},
                new PollCast{id = 62, PollDataId = 4, AttendeeId = 2, Accept = false},

                new PollCast{id = 63, PollDataId = 4, AttendeeId = 3, Accept = false},
                new PollCast{id = 64, PollDataId = 4, AttendeeId = 3, Accept = true},
                new PollCast{id = 65, PollDataId = 4, AttendeeId = 3, Accept = true},
                new PollCast{id = 66, PollDataId = 4, AttendeeId = 3, Accept = false},

                new PollCast{id = 67, PollDataId = 4, AttendeeId = 4, Accept = true},
                new PollCast{id = 68, PollDataId = 4, AttendeeId = 4, Accept = false},
                new PollCast{id = 69, PollDataId = 4, AttendeeId = 4, Accept = false},
                new PollCast{id = 70, PollDataId = 4, AttendeeId = 4, Accept = false},

                new PollCast{id = 71, PollDataId = 5, AttendeeId = 1, Accept = false},
                new PollCast{id = 72, PollDataId = 5, AttendeeId = 1, Accept = true},
                new PollCast{id = 73, PollDataId = 5, AttendeeId = 1, Accept = false},
                new PollCast{id = 74, PollDataId = 5, AttendeeId = 1, Accept = false},
                new PollCast{id = 75, PollDataId = 5, AttendeeId = 1, Accept = true},

                new PollCast{id = 76, PollDataId = 5, AttendeeId = 2, Accept = true},
                new PollCast{id = 77, PollDataId = 5, AttendeeId = 2, Accept = false},
                new PollCast{id = 78, PollDataId = 5, AttendeeId = 2, Accept = true},
                new PollCast{id = 79, PollDataId = 5, AttendeeId = 2, Accept = false},
                new PollCast{id = 80, PollDataId = 5, AttendeeId = 2, Accept = true},

                new PollCast{id = 81, PollDataId = 5, AttendeeId = 3, Accept = false},
                new PollCast{id = 82, PollDataId = 5, AttendeeId = 3, Accept = true},
                new PollCast{id = 83, PollDataId = 5, AttendeeId = 3, Accept = true},
                new PollCast{id = 84, PollDataId = 5, AttendeeId = 3, Accept = false},
                new PollCast{id = 85, PollDataId = 5, AttendeeId = 3, Accept = false},

                new PollCast{id = 86, PollDataId = 5, AttendeeId = 4, Accept = true},
                new PollCast{id = 87, PollDataId = 5, AttendeeId = 4, Accept = false},
                new PollCast{id = 88, PollDataId = 5, AttendeeId = 4, Accept = false},
                new PollCast{id = 89, PollDataId = 5, AttendeeId = 4, Accept = false},
                new PollCast{id = 90, PollDataId = 5, AttendeeId = 4, Accept = false},

                new PollCast{id = 91, PollDataId = 5, AttendeeId = 5, Accept = true},
                new PollCast{id = 92, PollDataId = 5, AttendeeId = 5, Accept = true},
                new PollCast{id = 93, PollDataId = 5, AttendeeId = 5, Accept = false},
                new PollCast{id = 94, PollDataId = 5, AttendeeId = 5, Accept = true},
                new PollCast{id = 95, PollDataId = 5, AttendeeId = 5, Accept = false},

                new PollCast{id = 96, PollDataId = 5, AttendeeId = 1, Accept = false},
                new PollCast{id = 97, PollDataId = 5, AttendeeId = 2, Accept = true},
                new PollCast{id = 98, PollDataId = 5, AttendeeId = 3, Accept = false},
                new PollCast{id = 99, PollDataId = 5, AttendeeId = 4, Accept = false},
                new PollCast{id = 100, PollDataId = 5, AttendeeId = 5, Accept = true},
            });

            tms.AddRange(new List<Times>
            {
                new Times{id = 0, MeetingId = 1, Time = "09:00-10:00"},
                new Times{id = 1, MeetingId = 1, Time = "10:00-11:00"},
                new Times{id = 2, MeetingId = 1, Time = "12:00-13:00"},
                new Times{id = 3, MeetingId = 1, Time = "14:00-15:00"},
                new Times{id = 4, MeetingId = 1, Time = "15:00-16:00"},

                new Times{id = 5, MeetingId = 2, Time = "09:00-10:00"},
                new Times{id = 6, MeetingId = 2, Time = "10:00-11:00"},
                new Times{id = 7, MeetingId = 2, Time = "12:00-13:00"},

                new Times{id = 8, MeetingId = 3, Time = "10:00-11:00"},
                new Times{id = 9, MeetingId = 3, Time = "12:00-13:00"},
                new Times{id = 10, MeetingId = 3, Time = "15:00-16:00"},

                new Times{id = 11, MeetingId = 4, Time = "09:00-10:00"},
                new Times{id = 12, MeetingId = 4, Time = "14:00-15:00"},
                new Times{id = 13, MeetingId = 4, Time = "15:00-16:00"},

                new Times{id = 14, MeetingId = 5, Time = "09:00-10:00"},
                new Times{id = 15, MeetingId = 5, Time = "10:00-11:00"},
                new Times{id = 16, MeetingId = 5, Time = "12:00-13:00"},
                new Times{id = 17, MeetingId = 5, Time = "14:00-15:00"},
                new Times{id = 18, MeetingId = 5, Time = "15:00-16:00"},
                new Times{id = 19, MeetingId = 5, Time = "16:00-17:00"},
            });

            App.Self.DBManager.AddOrUpdateAppointments(appts);
            App.Self.DBManager.AddOrUpdateAttendees(attds);
            App.Self.DBManager.AddOrUpdateMeeting(mtgs);
            App.Self.DBManager.AddOrUpdatePolling(pollList);
            App.Self.DBManager.AddOrUpdatePollCast(pc);
            App.Self.DBManager.AddOrUpdatePollingData(pollInfo);
            App.Self.DBManager.AddOrUpdateTimes(tms);
        }
    }
#endif
}
