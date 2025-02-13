using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace SmartSchool.ManagerDashaspx
{
    public partial class SchoolDetails : System.Web.UI.Page
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

                    Divs_Tbls_Visiblity(false);

                    GetSchoolTotals();
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

        private void GetSchoolTotals()
        {
            LblSchoolName.Text = "";
            LblTotalEmployees.Text = "0";
            LblTotalStudents.Text = "0";
            LblTotalBuses.Text = "0";
            string InitUrlID = "s_0";
            string[] UrlID = InitUrlID.Split('_');
            int SchoolID = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != string.Empty)
            {
                UrlID = Request.QueryString["id"].Split('_');
                SchoolID = int.Parse(UrlID[1].ToString());
            }

            hdschid.Value = SchoolID.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            //Get School Name
            CMD = new SqlCommand("SELECT SchoolArabicName "
                               + "FROM SchoolBranches "
                               + "WHERE SchoolID = " + SchoolID.ToString(), con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                LblSchoolName.Text = DR["SchoolArabicName"].ToString();
            }
            if (!DR.IsClosed) DR.Close();

            //Staff
            CMD = new SqlCommand("SELECT COUNT(*) "
                               + "FROM Staff Staff "
                               + "INNER JOIN StaffJobDetails StaffJobDetails ON "
                               + "(Staff.StaffID = StaffJobDetails.StaffID) "
                               + "WHERE StaffJobDetails.SchoolID = " + SchoolID.ToString(), con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                LblTotalEmployees.Text = DR[0].ToString();
            }
            if (!DR.IsClosed) DR.Close();

            //Students
            CMD = new SqlCommand("SELECT COUNT(*) "
                               + "FROM Student Student "
                               + "INNER JOIN StudentSchoolDetails StudentSchoolDetails ON "
                               + "(StudentSchoolDetails.StudentID = Student.StudentID) "
                               + "INNER JOIN Guardians Guardians ON "
                               + "(Guardians.GuardianID = Student.GuardianID) "
                               + "INNER JOIN StudentStatus StudentStatus ON "
                               + "(StudentStatus.StatusID  = StudentSchoolDetails.Status + 1) "
                               + "INNER JOIN SchoolClasses SchoolClasses ON "
                               + "(SchoolClasses.SchoolClassID = StudentSchoolDetails.ClassID) "
                               + "INNER JOIN Class Class ON "
                               + "(Class.ClassID = SchoolClasses.ClassID) "
                               + "WHERE StudentSchoolDetails.SchoolID = " + SchoolID.ToString(), con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                LblTotalStudents.Text = DR[0].ToString();
            }
            if (!DR.IsClosed) DR.Close();

            //Buses
            CMD = new SqlCommand("SELECT COUNT(*) "
                               + "FROM BusInfo "
                               + "WHERE SchoolID = " + SchoolID.ToString(), con);
            DR = CMD.ExecuteReader();

            if (DR.Read())
            {
                LblTotalBuses.Text = DR[0].ToString();
            }
            if (!DR.IsClosed) DR.Close();


            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        protected void BtnTotalEmployees_Click(object sender, EventArgs e)
        {
            Divs_Tbls_Visiblity(false);
            LblTblStaffData.Text = "الموظفين";
            DivTblStaffData.Visible = true;

            string CmdStr;
            CmdStr = "SELECT Staff.StaffID, StaffArabicName = REPLACE(Staff.StaffArabicName, '-', ' '), StaffContactDetails.MobileNo, "
                   + "       Departments.DepartmentArabicName, Designations.DesignationArabicText, StaffJobDetails.DateOfJoining, Countries.ArabicNationality "
                   + "FROM Staff Staff "
                   + "INNER JOIN StaffJobDetails StaffJobDetails ON "
                   + "(StaffJobDetails.StaffID = Staff.StaffID) "
                   + "INNER JOIN StaffContactDetails StaffContactDetails ON "
                   + "(StaffContactDetails.StaffID = Staff.StaffID) "
                   + "INNER JOIN Designations Designations ON "
                   + "(Designations.DesignationID = StaffJobDetails.Designation) "
                   + "INNER JOIN Departments Departments ON "
                   + "(Departments.DepartmentID = StaffJobDetails.Department) "
                   + "INNER JOIN Countries Countries ON "
                   + "(Countries.ID = Staff.Nationality) "
                   + "WHERE StaffJobDetails.SchoolID = " + hdschid.Value;
            ReptTblStaffData.DataSource = GetDataTablesFor(CmdStr);
            ReptTblStaffData.DataBind();
        }

        protected void BtnTotalStudents_Click(object sender, EventArgs e)
        {
            Divs_Tbls_Visiblity(false);
            LblTblStudentsData.Text = "الطلاب";
            DivTblStudentsData.Visible = true;

            string CmdStr;
            CmdStr = "SELECT StudentArabicName = REPLACE(Student.StudentArabicName, '-', ' '),  "
                   + "       GuardianArabicName = REPLACE(Guardians.GuardianArabicName, '-', ' '), "
                   + "       CLASS.ClassArabicName, StudentStatus.StatusArabicText "
                   + "FROM Student Student "
                   + "INNER JOIN StudentSchoolDetails StudentSchoolDetails ON "
                   + "(StudentSchoolDetails.StudentID = Student.StudentID) "
                   + "INNER JOIN Guardians Guardians ON "
                   + "(Guardians.GuardianID = Student.GuardianID) "
                   + "INNER JOIN StudentStatus StudentStatus ON "
                   + "(StudentStatus.StatusID  = StudentSchoolDetails.Status + 1) "
                   + "INNER JOIN SchoolClasses SchoolClasses ON "
                   + "(SchoolClasses.SchoolClassID = StudentSchoolDetails.ClassID) "
                   + "INNER JOIN Class Class ON "
                   + "(Class.ClassID = SchoolClasses.ClassID) "
                   + "WHERE StudentSchoolDetails.SchoolID = " + hdschid.Value;
            ReptTblStudentsData.DataSource = GetDataTablesFor(CmdStr);
            ReptTblStudentsData.DataBind();
        }

        protected void BtnTotalBuses_Click(object sender, EventArgs e)
        {
            Divs_Tbls_Visiblity(false);
            LblTblBusesData.Text = "الحافلات";
            DivTblBusesData.Visible = true;

            string CmdStr;
            CmdStr = "SELECT BusInfo.BusNo, BusInfo.PlateNumber, BusInfo.BusOwner,  "
                   + "       BusInfo.NumberofSeats, StaffArabicName = REPLACE(Staff.StaffArabicName, '-', ' ') "
                   + "FROM BusInfo BusInfo "
                   + "INNER JOIN Staff Staff ON "
                   + "(Staff.StaffID = BusInfo.DriverID) "
                   + "WHERE BusInfo.SchoolID = " + hdschid.Value;
            ReptTblBusesData.DataSource = GetDataTablesFor(CmdStr);
            ReptTblBusesData.DataBind();
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

        private void Divs_Tbls_Visiblity(bool v)
        {
            DivTblStaffData.Visible = v;
            DivTblStudentsData.Visible = v;
            DivTblBusesData.Visible = v;
        }
    }
}