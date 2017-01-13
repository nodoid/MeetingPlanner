using System;
namespace MeetingPlanner
{
    public static class DateTimeDayOfMonthExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }

        public static DateTime StartOfWeek(this DateTime value)
        {
            var today = (int)DateTime.Now.DayOfWeek;

            var month = value.Month;
            var year = value.Year;
            var day = value.Day;

            if (month != 12)
            {
                if (value.Day + 1 > DaysInMonth(value) || value.Day + 2 > DaysInMonth(value))
                    month++;
            }
            else
            {
                if (value.Day + 1 > DaysInMonth(value) || value.Day + 2 > DaysInMonth(value))
                {
                    month = 0;
                    year++;
                }
            }

            switch (value.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day++;
                    break;
                case DayOfWeek.Saturday:
                    day += 2;
                    break;
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    day--;
                    break;
                case DayOfWeek.Wednesday:
                    day -= 2;
                    break;
                case DayOfWeek.Thursday:
                    day -= 3;
                    break;
                case DayOfWeek.Friday:
                    day -= 4;
                    break;
            }

            return new DateTime(year, month, day);
        }
    }
}
