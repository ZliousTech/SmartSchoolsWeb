using System.Configuration;

namespace Common.Helpers
{
    public class ConnectionString
    {
        public static string ConnStr() => ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;
    }
}