using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;

namespace SmartSchool.ManagerDashaspx
{
    public partial class DashFinance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    DivTblOfDetailData.Visible = false;

                    FormsIdentity identity = (FormsIdentity)Context.User.Identity;
                    string userData = identity.Ticket.UserData;
                    string[] InfoData = userData.Split('$');
                    int UserID = int.Parse(InfoData[0].ToString());

                    hduid.Value = UserID.ToString();

                    LblUName.Text = GetHeadQuarterUerName(UserID);

                    GetRequstedDataForCalculations(UserID);
                    GetTotalCompanyContracts();
                    GetAcceptedCompanyContracts();
                    GetRejectedCompanyContracts();
                    GetNoActionCompanyContracts();
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

        private void GetRequstedDataForCalculations(int UserID)
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
            hdSchoolsIDs.Value = SchoolsIDs;
            SchoolsIDsArr = SchoolsIDs.Split('|');

            //Fetch the registered student in external mode.
            string StudentsIDs_Ext = "";
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
            hdStudentsIDs_Ext.Value = StudentsIDs_Ext;

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void GetTotalCompanyContracts()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            string[] StudentsIDs_ExtArr;
            StudentsIDs_ExtArr = hdStudentsIDs_Ext.Value.Split('|');

            //Get the summation of all fees by student id.
            double TotalAmount = 0;
            int i;
            for (i = 0; i < (StudentsIDs_ExtArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT SUM(FeeAmount) "
                                   + "FROM GuardianContractInvoice GuardianContractInvoice "
                                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID)"
                                   + "WHERE GuardianContractInvoice.StudentID = " + StudentsIDs_ExtArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    if (DR[0].ToString() != "")
                        TotalAmount = TotalAmount + Convert.ToDouble(int.Parse(DR[0].ToString()));
                }
                if (!DR.IsClosed) DR.Close();
            }
            //LblTotalAll.Text = TotalAmount.ToString() + "  $";
            LblTotalAll.Text = TotalAmount.ToString("C2");

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void GetAcceptedCompanyContracts()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            string[] StudentsIDs_ExtArr;
            StudentsIDs_ExtArr = hdStudentsIDs_Ext.Value.Split('|');

            //Get the summation of all fees by student id.
            double TotalAmount = 0;
            int i;
            for (i = 0; i < (StudentsIDs_ExtArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT SUM(FeeAmount) "
                                   + "FROM GuardianContractInvoice GuardianContractInvoice "
                                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID)"
                                   + "WHERE ExternalGuardStudentsRequests.RequestStatus = 2 AND GuardianContractInvoice.StudentID = " + StudentsIDs_ExtArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    if (DR[0].ToString() != "")
                        TotalAmount = TotalAmount + Convert.ToDouble(int.Parse(DR[0].ToString()));
                }
                if (!DR.IsClosed) DR.Close();
            }
            LblTotalAccepted.Text = TotalAmount.ToString("C2");

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void GetRejectedCompanyContracts()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            string[] StudentsIDs_ExtArr;
            StudentsIDs_ExtArr = hdStudentsIDs_Ext.Value.Split('|');

            //Get the summation of all fees by student id.
            double TotalAmount = 0;
            int i;
            for (i = 0; i < (StudentsIDs_ExtArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT SUM(FeeAmount) "
                                   + "FROM GuardianContractInvoice GuardianContractInvoice "
                                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID)"
                                   + "WHERE ExternalGuardStudentsRequests.RequestStatus = 1 AND GuardianContractInvoice.StudentID = " + StudentsIDs_ExtArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    if (DR[0].ToString() != "")
                        TotalAmount = TotalAmount + Convert.ToDouble(int.Parse(DR[0].ToString()));
                }
                if (!DR.IsClosed) DR.Close();
            }
            LblTotalRejected.Text = TotalAmount.ToString("C2");

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void GetNoActionCompanyContracts()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;
            SqlDataReader DR;

            string[] StudentsIDs_ExtArr;
            StudentsIDs_ExtArr = hdStudentsIDs_Ext.Value.Split('|');

            //Get the summation of all fees by student id.
            double TotalAmount = 0;
            int i;
            for (i = 0; i < (StudentsIDs_ExtArr.Length) - 1; i++)
            {
                CMD = new SqlCommand("SELECT SUM(FeeAmount) "
                                   + "FROM GuardianContractInvoice GuardianContractInvoice "
                                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID)"
                                   + "WHERE ExternalGuardStudentsRequests.RequestStatus = 0 AND GuardianContractInvoice.StudentID = " + StudentsIDs_ExtArr[i], con);
                DR = CMD.ExecuteReader();
                if (DR.Read())
                {
                    if (DR[0].ToString() != "")
                        TotalAmount = TotalAmount + Convert.ToDouble(int.Parse(DR[0].ToString()));
                }
                if (!DR.IsClosed) DR.Close();
            }
            LblTotalNoAction.Text = TotalAmount.ToString("C2");

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        protected void BtnTotalAll_Click(object sender, EventArgs e)
        {
            LblTableOfDataTitle.Text = "طلبات التسجيل";
            DivTblOfDetailData.Visible = true;

            string[] StudentsIDs = hdStudentsIDs_Ext.Value.Split('|');
            string SQL_Str_ForStudentsIDs = "";
            for (int i = 0; i < (StudentsIDs.Length) - 1; i++)
            {
                if (StudentsIDs[i] != "")
                {
                    SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + "ExternalStudent.StudentID = " + StudentsIDs[i];
                    if (i < (StudentsIDs.Length) - 2) SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + " OR ";
                }
            }

            string CmdStr;
            CmdStr = "SELECT ExternalStudent.StudentID, "
                   + "       SchoolBranches.SchoolArabicName, "
                   + "       FeeAmount = SUM(GuardianContractInvoice.FeeAmount), "
                   + "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), "
                   + "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), "
                   + "       RequestStatus = CASE "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 0 THEN N'لم يتم عليه اجراء' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 1 THEN N'مرفوض' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 2 THEN N'مقبول'  "
                   + "                       END "
                   + "FROM ExternalStudent ExternalStudent "
                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                   + "(ExternalStudent.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN GuardianContractInvoice GuardianContractInvoice ON "
                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID) "
                   + "INNER JOIN ExternalGuardians ExternalGuardians ON "
                   + "(ExternalGuardians.UserID = ExternalStudent.GuardianID) "
                   + "INNER JOIN SchoolBranches SchoolBranches ON "
                   + "(ExternalStudentSchoolDetails.SchoolID = SchoolBranches.SchoolID) "
                   + "WHERE " + SQL_Str_ForStudentsIDs + " "
                   + "GROUP BY ExternalStudent.StudentID, ExternalStudent.StudentArabicName, SchoolBranches.SchoolArabicName, "
                   + "         ExternalGuardStudentsRequests.RequestStatus, ExternalGuardians.ArabicName ";
            ReptTableOfData.DataSource = GetDataTablesFor(CmdStr);
            ReptTableOfData.DataBind();
        }

        protected void BtnTotalAccepted_Click(object sender, EventArgs e)
        {
            LblTableOfDataTitle.Text = "طلبات التسجيل المقبولة";
            DivTblOfDetailData.Visible = true;

            string[] StudentsIDs = hdStudentsIDs_Ext.Value.Split('|');
            string SQL_Str_ForStudentsIDs = "";
            for (int i = 0; i < (StudentsIDs.Length) - 1; i++)
            {
                if (StudentsIDs[i] != "")
                {
                    SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + "ExternalStudent.StudentID = " + StudentsIDs[i];
                    if (i < (StudentsIDs.Length) - 2) SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + " OR ";
                }
            }

            string CmdStr;
            CmdStr = "SELECT ExternalStudent.StudentID, "
                   + "       SchoolBranches.SchoolArabicName, "
                   + "       FeeAmount = SUM(GuardianContractInvoice.FeeAmount), "
                   + "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), "
                   + "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), "
                   + "       RequestStatus = CASE "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 0 THEN N'لم يتم عليه اجراء' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 1 THEN N'مرفوض' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 2 THEN N'مقبول'  "
                   + "                       END "
                   + "FROM ExternalStudent ExternalStudent "
                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                   + "(ExternalStudent.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN GuardianContractInvoice GuardianContractInvoice ON "
                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID) "
                   + "INNER JOIN ExternalGuardians ExternalGuardians ON "
                   + "(ExternalGuardians.UserID = ExternalStudent.GuardianID) "
                   + "INNER JOIN SchoolBranches SchoolBranches ON "
                   + "(ExternalStudentSchoolDetails.SchoolID = SchoolBranches.SchoolID) "
                   + "WHERE (" + SQL_Str_ForStudentsIDs + ") AND ExternalGuardStudentsRequests.RequestStatus = 2 "
                   + "GROUP BY ExternalStudent.StudentID, ExternalStudent.StudentArabicName, SchoolBranches.SchoolArabicName, "
                   + "         ExternalGuardStudentsRequests.RequestStatus, ExternalGuardians.ArabicName ";
            ReptTableOfData.DataSource = GetDataTablesFor(CmdStr);
            ReptTableOfData.DataBind();
        }

        protected void BtnTotalRejected_Click(object sender, EventArgs e)
        {
            LblTableOfDataTitle.Text = "طلبات التسجيل المرفوضة";
            DivTblOfDetailData.Visible = true;

            string[] StudentsIDs = hdStudentsIDs_Ext.Value.Split('|');
            string SQL_Str_ForStudentsIDs = "";
            for (int i = 0; i < (StudentsIDs.Length) - 1; i++)
            {
                if (StudentsIDs[i] != "")
                {
                    SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + "ExternalStudent.StudentID = " + StudentsIDs[i];
                    if (i < (StudentsIDs.Length) - 2) SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + " OR ";
                }
            }

            string CmdStr;
            CmdStr = "SELECT ExternalStudent.StudentID, "
                   + "       SchoolBranches.SchoolArabicName, "
                   + "       FeeAmount = SUM(GuardianContractInvoice.FeeAmount), "
                   + "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), "
                   + "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), "
                   + "       RequestStatus = CASE "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 0 THEN N'لم يتم عليه اجراء' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 1 THEN N'مرفوض' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 2 THEN N'مقبول'  "
                   + "                       END "
                   + "FROM ExternalStudent ExternalStudent "
                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                   + "(ExternalStudent.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN GuardianContractInvoice GuardianContractInvoice ON "
                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID) "
                   + "INNER JOIN ExternalGuardians ExternalGuardians ON "
                   + "(ExternalGuardians.UserID = ExternalStudent.GuardianID) "
                   + "INNER JOIN SchoolBranches SchoolBranches ON "
                   + "(ExternalStudentSchoolDetails.SchoolID = SchoolBranches.SchoolID) "
                   + "WHERE (" + SQL_Str_ForStudentsIDs + ") AND ExternalGuardStudentsRequests.RequestStatus = 1 "
                   + "GROUP BY ExternalStudent.StudentID, ExternalStudent.StudentArabicName, SchoolBranches.SchoolArabicName, "
                   + "         ExternalGuardStudentsRequests.RequestStatus, ExternalGuardians.ArabicName ";
            ReptTableOfData.DataSource = GetDataTablesFor(CmdStr);
            ReptTableOfData.DataBind();
        }

        protected void BtnTotalNoAction_Click(object sender, EventArgs e)
        {
            LblTableOfDataTitle.Text = "طلبات تسجيل لم يتم عليها إجراء";
            DivTblOfDetailData.Visible = true;

            string[] StudentsIDs = hdStudentsIDs_Ext.Value.Split('|');
            string SQL_Str_ForStudentsIDs = "";
            for (int i = 0; i < (StudentsIDs.Length) - 1; i++)
            {
                if (StudentsIDs[i] != "")
                {
                    SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + "ExternalStudent.StudentID = " + StudentsIDs[i];
                    if (i < (StudentsIDs.Length) - 2) SQL_Str_ForStudentsIDs = SQL_Str_ForStudentsIDs + " OR ";
                }
            }

            string CmdStr;
            CmdStr = "SELECT ExternalStudent.StudentID, "
                   + "       SchoolBranches.SchoolArabicName, "
                   + "       FeeAmount = SUM(GuardianContractInvoice.FeeAmount), "
                   + "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), "
                   + "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), "
                   + "       RequestStatus = CASE "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 0 THEN N'لم يتم عليه اجراء' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 1 THEN N'مرفوض' "
                   + "                           WHEN ExternalGuardStudentsRequests.RequestStatus = 2 THEN N'مقبول'  "
                   + "                       END "
                   + "FROM ExternalStudent ExternalStudent "
                   + "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON "
                   + "(ExternalStudent.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON "
                   + "(ExternalGuardStudentsRequests.StudentID = ExternalStudentSchoolDetails.StudentID) "
                   + "INNER JOIN GuardianContractInvoice GuardianContractInvoice ON "
                   + "(GuardianContractInvoice.StudentID = ExternalGuardStudentsRequests.StudentID) "
                   + "INNER JOIN ExternalGuardians ExternalGuardians ON "
                   + "(ExternalGuardians.UserID = ExternalStudent.GuardianID) "
                   + "INNER JOIN SchoolBranches SchoolBranches ON "
                   + "(ExternalStudentSchoolDetails.SchoolID = SchoolBranches.SchoolID) "
                   + "WHERE (" + SQL_Str_ForStudentsIDs + ") AND ExternalGuardStudentsRequests.RequestStatus = 0 "
                   + "GROUP BY ExternalStudent.StudentID, ExternalStudent.StudentArabicName, SchoolBranches.SchoolArabicName, "
                   + "         ExternalGuardStudentsRequests.RequestStatus, ExternalGuardians.ArabicName ";
            ReptTableOfData.DataSource = GetDataTablesFor(CmdStr);
            ReptTableOfData.DataBind();
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