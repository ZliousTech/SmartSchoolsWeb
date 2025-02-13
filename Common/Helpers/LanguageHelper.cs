using System.Threading;

namespace Common.Helpers
{
    public class LanguageHelper
    {
        private static string coockieName = "_culture";
        public static void SwitchLanguage()
        {


            string cultureName = null;

            // Attempt to read the culture cookie from Request
            if (CookieHelper.Exists(coockieName))
            {
                cultureName = CookieHelper.Get(coockieName);
            }
            else
            {
                //Create language coockie
                CookieHelper.Set(coockieName, "en");
            }

            if (cultureName == "ar")
            {
                CookieHelper.Set(coockieName, "en");

                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


            }
            else
            {
                CookieHelper.Set(coockieName, "ar");

                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-JO");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;



            }

        }
    }
}