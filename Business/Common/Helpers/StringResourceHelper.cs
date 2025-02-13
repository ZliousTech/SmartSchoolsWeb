using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Common.Helpers
{
    public class R
    {
        public static string GetResource(string key)
        {
            var item = HttpContext.GetGlobalResourceObject("Resource", key);
            if (item != null)
            {
                return item.ToString();
            }
            else
            {
                ErrorLog.LogData(string.Format("Resource {0} not found", key));
            }
            return key;
        }
    }
}