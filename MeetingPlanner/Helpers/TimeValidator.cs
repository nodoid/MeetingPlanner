using System.Text.RegularExpressions;
namespace MeetingPlanner
{
    public static class TimeValidator
    {
        static string nums = "^[0-9:-]";

        public static bool TimeCheck(this string check)
        {
            return Regex.IsMatch(check, nums);
        }
    }
}
