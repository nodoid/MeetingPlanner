using SQLite.Net.Interop;

namespace WODTasticMobile
{
    public interface ISQLite
    {
        string GetConnectionString();

        ISQLitePlatform GetPlatform();
    }
}

