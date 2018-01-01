using System.Collections.Generic;

namespace MeetingPlanner
{
    public class Userdetails
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string UserId { get; set; }
    }

    public class LoginDetails
    {
        public List<Userdetails> Login { get; set; }
    }
}
