using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace SmartSchool
{
    /// <summary>
    /// Summary description for gmapdashWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class gmapdashWS : System.Web.Services.WebService
    {
        [WebMethod]
        public string[] GetHeadQuarterInfoWS(string Hduid)
        {
            string[] HQInfo = new string[3];

            string Lat = "0";
            string Lng = "0";
            string CompanyArabicName = "HQ";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            CMD = new SqlCommand("SELECT CompanyArabicName, Latitude, Longitude "
                                 + "FROM Headquarters "
                                 + "WHERE CompanyID = ( "
                                 + "SELECT CompanyID "
                                 + "FROM Users "
                                 + "WHERE UserID = " + Hduid + ")", con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                Lat = DR["Latitude"].ToString();
                Lng = DR["Longitude"].ToString();
                CompanyArabicName = DR["CompanyArabicName"].ToString().Trim();
            }

            if (!DR.IsClosed) DR.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            HQInfo[0] = Lat;
            HQInfo[1] = Lng;
            HQInfo[2] = CompanyArabicName;

            return HQInfo;
        }

        [WebMethod]
        public string[] GetSchoolsInfoWS(string Hduid)
        {
            string[] SchInfo = new string[5000];

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            CMD = new SqlCommand("SELECT SchoolArabicName, Latitude, Longitude "
                                 + "FROM SchoolBranches "
                                 + "WHERE CompanyID = ( "
                                 + "SELECT CompanyID "
                                 + "FROM Users "
                                 + "WHERE UserID = " + Hduid + ")", con);
            DR = CMD.ExecuteReader();

            int indx = 0;
            while (DR.Read())
            {
                SchInfo[indx] = DR["Latitude"].ToString() + "," + DR["Longitude"].ToString() + "|" + DR["SchoolArabicName"].ToString();
                indx++;
            }

            if (!DR.IsClosed) DR.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            return SchInfo;
        }
    }
}
