using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace SmartSchool.ManagerDashaspx
{
    public partial class ManagerDash : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    FormsIdentity identity = (FormsIdentity)Context.User.Identity;
                    string userData = identity.Ticket.UserData;
                    string[] InfoData = userData.Split('$');
                    int UserID = int.Parse(InfoData[0].ToString());

                    hduid.Value = UserID.ToString();

                    LblUName.Text = GetHeadQuarterUerName(UserID);

                    //Finance Dash Item.
                    GetCompanyFinance(UserID);

                    //SchoolBranches.
                    string CmdStr;
                    CmdStr = "SELECT * "
                           + "FROM SchoolBranches "
                           + "WHERE CompanyID = ("
                           + "  SELECT CompanyID "
                           + "  FROM Users "
                           + "  WHERE UserID = " + UserID.ToString() + ")";

                    ReptCompSchls.DataSource = GetDataTablesFor(CmdStr);
                    ReptCompSchls.DataBind();
                }
                else
                {
                    FormsAuthentication.SignOut();
                    Response.Cookies.Clear();
                    HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                    cookie1.Expires = DateTime.Now.AddYears(-1);
                    Response.Cookies.Add(cookie1);
                    Response.Redirect(FormsAuthentication.LoginUrl);
                }
            }
        }

        private void GetCompanyFinance(int UserID)
        {
            string SchoolsIDs = "";
            string[] SchoolsIDsArr;
            int i;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            //Fetch the schools.
            CMD = new SqlCommand("SELECT SchoolID "
                                 + "FROM SchoolBranches "
                                 + "WHERE CompanyID = ( "
                                 + "    SELECT CompanyID "
                                 + "    FROM Users "
                                 + "    WHERE UserID = " + UserID + ")", con);
            DR = CMD.ExecuteReader();

            while (DR.Read())
            {
                SchoolsIDs = SchoolsIDs + DR["SchoolID"].ToString() + "|";
            }
            if (!DR.IsClosed) DR.Close();
            SchoolsIDsArr = SchoolsIDs.Split('|');

            //Fetch the registered student in external mode.
            string StudentsIDs_Ext = "";
            string[] StudentsIDs_ExtArr;
            for (i = 0; i < (SchoolsIDsArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT StudentID "
                                     + "FROM ExternalStudentSchoolDetails "
                                     + "WHERE SchoolID = " + SchoolsIDsArr[i], con);
                DR = CMD.ExecuteReader();
                while (DR.Read())
                {
                    StudentsIDs_Ext = StudentsIDs_Ext + DR["StudentID"] + "|";
                }
                if (!DR.IsClosed) DR.Close();
            }
            if (!DR.IsClosed) DR.Close();
            StudentsIDs_ExtArr = StudentsIDs_Ext.Split('|');
            LblTotalCompanyContracts.Text = ((StudentsIDs_ExtArr.Length) - 1).ToString();

            //Fetch the all requests.
            int AllSchoolsRequestsCount = 0;
            for (i = 0; i < (SchoolsIDsArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT COUNT(*) "
                                   + "FROM ExternalGuardStudentsRequests ExternalGuardStudentsRequests "
                                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                                   + "WHERE ExternalStudentSchoolDetails.SchoolID = " + SchoolsIDsArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    AllSchoolsRequestsCount = AllSchoolsRequestsCount + int.Parse(DR[0].ToString());
                }
                if (!DR.IsClosed) DR.Close();
            }
            if (!DR.IsClosed) DR.Close();

            //Fetch the accepted requests.
            int AllSchoolsAcceptedRequestsCount = 0;
            for (i = 0; i < (SchoolsIDsArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT COUNT(*) "
                                   + "FROM ExternalGuardStudentsRequests ExternalGuardStudentsRequests "
                                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                                   + "WHERE ExternalGuardStudentsRequests.RequestStatus = 2 AND ExternalStudentSchoolDetails.SchoolID = " + SchoolsIDsArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    AllSchoolsAcceptedRequestsCount = AllSchoolsAcceptedRequestsCount + int.Parse(DR[0].ToString());
                }
                if (!DR.IsClosed) DR.Close();
            }
            if (!DR.IsClosed) DR.Close();

            LblAcceptedRequestsPercent.Text = Convert.ToDouble(Convert.ToDouble(AllSchoolsAcceptedRequestsCount) / Convert.ToDouble(AllSchoolsRequestsCount) * 100).ToString();
            ProgAcceptedRequestsPercent.Style.Add("width", LblAcceptedRequestsPercent.Text + "%");

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private string GetHeadQuarterUerName(int UserID)
        {
            string UserName = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            CMD = new SqlCommand("SELECT UserName  "
                                 + "FROM Users "
                                 + "WHERE SchoolID = -1 "
                                 + "AND "
                                 + "CompanyID = ( "
                                 + "    SELECT CompanyID "
                                 + "    FROM Users "
                                 + "    WHERE UserID = " + UserID + ")", con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                UserName = DR["UserName"].ToString();
            }

            if (!DR.IsClosed) DR.Close();
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            return UserName;
        }

        private DataTable GetDataTablesFor(string cmdStr)
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
    }
}