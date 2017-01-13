using System.Text.RegularExpressions;
namespace MeetingPlanner
{
    public static class EmailVerify
    {
        static string theEmailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        public static bool CheckPattern(this string emailAddress)
        {
            var t = emailAddress.Contains("(") && emailAddress.Contains(")");
            return t && Regex.IsMatch(emailAddress, theEmailPattern);
        }
    }
}
