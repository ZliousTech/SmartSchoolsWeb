using Common.Helpers;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;
using System.Web.Security;
using System.Web;

namespace Common.Base
{
    public class SystemBase
    {
        public SystemBase()
        {

        }

        public static DataTable GetDataTble(string cmdStr)
        {
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (Conn.State == ConnectionState.Closed) Conn.Open();
            SqlCommand cmd = new SqlCommand(cmdStr, Conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            da.Fill(ds);
            dt = ds.Tables[0];
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                cmd.Cancel();
                da.Dispose();
                ds.Dispose();
            }
            return dt;
        }

        public static string EncryptF(string Password)
        {
            string sMessage = String.Empty;
            Byte[] encode = new Byte[Password.Length - 1];
            encode = Encoding.UTF8.GetBytes(Password);
            sMessage = Convert.ToBase64String(encode);
            return sMessage;

        }

        public static string DecryptF(string EncryptPassword)
        {
            string DecryptPassword = string.Empty;
            UTF8Encoding EncodePassword = new UTF8Encoding();
            Decoder Decode = EncodePassword.GetDecoder();
            byte[] ToDecode_Byte = Convert.FromBase64String(EncryptPassword);
            int CharCount = Decode.GetCharCount(ToDecode_Byte, 0, ToDecode_Byte.Length);
            char[] Decoded_Char = new char[CharCount - 1 + 1];
            Decode.GetChars(ToDecode_Byte, 0, ToDecode_Byte.Length, Decoded_Char, 0);
            DecryptPassword = new String(Decoded_Char);
            return (DecryptPassword);
        }

        public static bool RegIsMatch(string sentText, string TextType)
        {
            Regex RX;
            bool MatchRes = false;

            switch (TextType)
            {
                case "REGULAR_TEXT":
                    RX = new Regex(@"[|<>+]"); //RX = new Regex(@"[|<>,;+]");
                    MatchRes = RX.IsMatch(sentText) == true ? false : true;
                    break;

                case "LANG_EN":
                    RX = new Regex(@"[A-Za-z0-9?.{}[\]\-_=!@#$%\^&*()]+");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;

                case "EMAIL":
                    RX = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;

                case "NUMBER":
                    RX = new Regex(@"^[0-9]+$");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;

                case "NUMBER-OR-DECIMAL":
                    RX = new Regex(@"^\d*\.?\d+$");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;

                case "PASS_COMPLX":
                    //Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
                    RX = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;

                case "TIME24":
                    RX = new Regex(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$");
                    MatchRes = RX.IsMatch(sentText) == true ? true : false;
                    break;
            }

            return MatchRes;
        }

        public static string GetCookie(int value)
        {
            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            string[] UserData = identity.Ticket.UserData.ToString().Split('$');

            switch (value)
            {
                case (int)clsenumration.UserData.UserID:
                    return UserData[0];
                case (int)clsenumration.UserData.Username:
                    return UserData[1];
                case (int)clsenumration.UserData.SchoolID:
                    return UserData[2];
                case (int)clsenumration.UserData.UserType:
                    return UserData[3];
                case (int)clsenumration.UserData.GuardianID:
                    return UserData[4];
                case (int)clsenumration.UserData.StaffID:
                    return UserData[5];
                case (int)clsenumration.UserData.CompanyID:
                    return UserData[6];
                case (int)clsenumration.UserData.PreviousSchoolID:
                    return UserData[7];
                case (int)clsenumration.UserData.StudentID:
                    return UserData[8];
                default:
                    return "";
            }
        }

        public static string GetCurrentCultureName(string coockieName)
        {
            string cultureName = "en";

            // Attempt to read the culture cookie from Request
            if (CookieHelper.Exists(coockieName))
            {
                cultureName = CookieHelper.Get(coockieName);
            }
            else
            {
                //Create language coockie
                CookieHelper.Set(coockieName, cultureName);
            }

            return cultureName;
        }

        public static void SetCulture(string coockieName, string cultureName)
        {
            if (cultureName == "en")
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

        public static string GetUserName(string UserID)
        {
            string UserName = "";

            DataTable UserNameDB =
                GetDataTble($"SELECT UserName " +
                            $"FROM Users " +
                            $"WHERE UserID = {UserID}");
            if (UserNameDB != null)
            {
                if (UserNameDB.Rows.Count > 0)
                {
                    UserName = UserNameDB.Rows[0]["UserName"].ToString().Trim();
                }
            }

            return UserName;
        }

        public static string GenerateGuidID(string FieldName, string TableName)
        {
            string GuidID = "";
            bool Done = false;

            Guid G = Guid.NewGuid();
            GuidID = G.ToString();
            GuidID = GuidID.Replace("-", "");

            while (!Done)
            {
                if (int.Parse(GetDataTble("SELECT COUNT(" + FieldName + ") FROM " + TableName + " WHERE " + FieldName + " = '" + GuidID + "'").Rows[0][0].ToString()) == 0)
                {
                    Done = !Done;
                }
                else
                {
                    G = Guid.NewGuid();
                    GuidID = G.ToString();
                    GuidID = GuidID.Replace("-", "");
                }
            }

            return GuidID;
        }

        public static string GetIDByGuidIDFor(string FieldNameID, string FieldNameGuidID, string GuidIDValue, string TableName)
        {
            string ID = "0";

            SqlConnection DBConnection;
            SqlCommand CMD;
            SqlDataReader DR;

            DBConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (DBConnection.State == ConnectionState.Closed) DBConnection.Open();

            CMD = new SqlCommand("SELECT " + FieldNameID + " " +
                                 "FROM " + TableName + " " +
                                 "WHERE " + FieldNameGuidID + " = @" + FieldNameGuidID + "", DBConnection);
            CMD.Parameters.AddWithValue("@" + FieldNameGuidID + "", GuidIDValue);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                if (DR.HasRows)
                {
                    ID = DR[FieldNameID].ToString();
                }
            }

            if (!DR.IsClosed) DR.Close();
            if (DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
                DBConnection.Dispose();
            }

            return ID;
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Cookies.Clear();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            HttpContext.Current.Response.Cookies.Add(cookie1);
            HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
