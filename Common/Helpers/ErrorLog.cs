using System;
using System.Web;

namespace Common.Helpers
{
    public class ErrorLog
    {

        public static void LogException(Exception exc)
        {
            try
            {

                string lines = exc.ToString();
                lines += "--------------------";
                lines += exc.Message;
                lines += "--------------------";
                lines += exc.StackTrace;
                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/errorlog.txt"), true);
                file.WriteLine("-----New Exception Occered at " + DateTime.Now.ToString());
                file.WriteLine(lines);
                file.WriteLine("---------------------------------------------");
                file.Close();
            }
            catch (Exception)
            {
            }
        }


        public static void LogData(string data)
        {

            string lines = ""; ;
            lines += "--------------------";
            lines += data;
            lines += "--------------------";
            // Write the string to a file.
            System.IO.StreamWriter file = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/datalog.txt"), true);
            file.WriteLine("-----New data log at " + DateTime.Now.ToString());
            file.WriteLine(lines);
            file.WriteLine("---------------------------------------------");
            file.Close();
        }
    }
}