using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Common.Helpers
{
    public class ConnectionString
    {
        public static string ConnStr() => ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;

    }
}