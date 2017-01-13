using SQLite.Net.Interop;

namespace MeetingPlanner
{
    public interface ISQLite
    {
        string GetConnectionString();

        ISQLitePlatform GetPlatform();
    }
}

