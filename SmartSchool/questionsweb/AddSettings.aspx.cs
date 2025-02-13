using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SmartSchool.questionsweb
{
    public partial class AddSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);

                    try
                    {
                        if (String.IsNullOrEmpty(Request.QueryString["FGID"]))
                        {
                            SystemBase.Logout();
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(Convert.ToString(exp));
                        SystemBase.Logout();
                    }
                    string FormGuidID = Request.QueryString["FGID"];
                    hdfgid.Value = FormGuidID;
                    string[] QuestionsFormInfo = GetQuestionFormInfo(FormGuidID).Split('|');
                    LblQuestionsFormTitle.Text = QuestionsFormInfo[1];

                    SetCulture();
                    SetLookups();
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
            SetASPElementsTxt();

            CreateFormSettings();
            if (!IsPostBack) RetriveFormSettings();
        }

        private void SetCulture()
        {
            string CultureName = SystemBase.GetCurrentCultureName("_culture");
            if (String.IsNullOrEmpty(CultureName)) CultureName = "ar";
            SystemBase.SetCulture("_culture", CultureName);
            hdclutrueName.Value = CultureName;
        }

        private void SetLookups()
        {
            //DropDownList1.Items.Insert(0, new ListItem("Add New", ""));
            DrpStudentsSelections.Items.Clear();
            DrpStudentsSelections.Items.Insert(0, new ListItem(Resources.Resource.AllStudents, "1"));
            DrpStudentsSelections.Items.Insert(1, new ListItem(Resources.Resource.SpecificStudents, "2"));
            DrpStudentsSelections.SelectedValue = "1";
        }

        private void SetASPElementsTxt()
        {
            BtnSave.Text = Resources.Resource.Save;
            BtnCancel.Text = Resources.Resource.Cancel;
        }

        private void ShowMsg(string Msg, string MsgType)
        {
            switch (MsgType)
            {
                case "success":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.WellDone + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "error":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.SorrySomethingWentWrong + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "info":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.MsgInfoType + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "warning":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.MsgWarningType + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;
            }
        }

        private string GetQuestionFormInfo(string FormGuidID)
        {
            try
            {
                DataTable QuestionsFormInfoDB =
                    SystemBase.GetDataTble($"SELECT QuestionsFormStyleID, QuestionsFormTitle, QuestionsFormDurationMinutes " +
                                           $"FROM QuestionsBank " +
                                           $"WHERE FormGuidID = '{FormGuidID}'");
                return String.Concat(QuestionsFormInfoDB.Rows[0]["QuestionsFormStyleID"].ToString(), "|",
                                     QuestionsFormInfoDB.Rows[0]["QuestionsFormTitle"].ToString(), "|",
                                     QuestionsFormInfoDB.Rows[0]["QuestionsFormDurationMinutes"].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SystemBase.Logout();
            }
            return "0|0|0";
        }

        protected void DrpStudentsSelections_IndxChange(object sender, EventArgs e)
        {
            string[] Students = hdallstds.Value.Split('|');
            foreach (string Student_Check in Students)
            {
                if (!String.IsNullOrEmpty(Student_Check))
                {
                    CheckBox chkBox;
                    chkBox = (CheckBox)PnlChks.FindControl(Student_Check) as CheckBox;
                    if (chkBox != null)
                    {
                        if (DrpStudentsSelections.SelectedValue == "1") chkBox.Checked = true;
                        else chkBox.Checked = false;
                    }
                }
            }
        }

        private void CreateFormSettings()
        {
            hdallstds.Value = "";
            string[] FormInfo = GetFormInfo();
            string CurriculumID = FormInfo[0];
            string SchoolClassID = FormInfo[1];
            if (!GetStudentsList(CurriculumID, SchoolClassID))
            {
                ShowMsg(hdclutrueName.Value.Contains("en")
                    ?
                    "There is no students found"
                    :
                    "لم يتم العثور على أي طلاب", "warning");
            }
        }

        private string[] GetFormInfo()
        {
            string CurriculumID = "0";
            string SchoolClassID = "0";
            DataTable FormInfoDB =
                SystemBase.GetDataTble($"SELECT CurriculumID, SchoolClassID " +
                                       $"FROM QuestionsBank " +
                                       $"WHERE FormGuidID = '{hdfgid.Value}'");
            if (FormInfoDB != null)
            {
                if (FormInfoDB.Rows.Count > 0)
                {
                    CurriculumID = FormInfoDB.Rows[0]["CurriculumID"].ToString();
                    SchoolClassID = FormInfoDB.Rows[0]["SchoolClassID"].ToString();
                }
                else SystemBase.Logout();
            }
            else SystemBase.Logout();

            string[] FormInfo = new string[2];
            FormInfo[0] = CurriculumID;
            FormInfo[1] = SchoolClassID;

            return FormInfo;
        }

        private bool GetStudentsList(string CurriculumID, string SchoolClassID)
        {
            bool IsThereStudents = false;
            string StudentName = hdclutrueName.Value.Contains("en") ? "StudentEnglishName" : "StudentArabicName";

            DataTable StudentsListDB =
                SystemBase.GetDataTble($"SELECT S.StudentID, StudentName = S.{StudentName}, SC.CurriculumID, sc.SchoolClassID " +
                                       $"FROM Student S " +
                                       $"INNER JOIN StudentSchoolDetails SSD ON " +
                                       $"(SSD.StudentID = S.StudentID) " +
                                       $"INNER JOIN SchoolClasses SC ON " +
                                       $"(SC.SchoolClassID = SSD.ClassID) " +
                                       $"WHERE SSD.ClassID = {SchoolClassID} AND SC.CurriculumID = {CurriculumID}");
            if (StudentsListDB != null)
            {
                if (StudentsListDB.Rows.Count > 0)
                {
                    foreach (DataRow Student in StudentsListDB.Rows)
                    {
                        TableRow TRow = new TableRow();
                        TableCell CellSelect = new TableCell();
                        TableCell CellStdNo = new TableCell();
                        TableCell CellStdName = new TableCell();

                        CheckBox ChkSelect = new CheckBox();
                        ChkSelect.Text = Resources.Resource.Select;
                        ChkSelect.ID = "ChkSel_" + Student["StudentID"] + "_" + Student["CurriculumID"] + "_" + Student["SchoolClassID"];
                        ChkSelect.Attributes["runat"] = "server";
                        //ChkSelect.AutoPostBack = true;
                        //ChkSelect.Checked = DrpStudentsSelections.SelectedValue == "1" ? true : false;

                        CellSelect.Controls.Add(ChkSelect);
                        CellStdNo.Controls.Add(new LiteralControl(Student["StudentID"].ToString()));
                        CellStdName.Controls.Add(new LiteralControl(Student["StudentName"].ToString()));

                        TRow.Cells.Add(CellSelect);
                        TRow.Cells.Add(CellStdNo);
                        TRow.Cells.Add(CellStdName);

                        tBodyStds.Controls.Add(TRow);

                        hdallstds.Value += "ChkSel_" + Student["StudentID"] + "_" + Student["CurriculumID"] + "_" + Student["SchoolClassID"] + "|";
                    }

                    IsThereStudents = true;
                }
            }

            DrpStudentsSelections_IndxChange(null, null);
            return IsThereStudents;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckInputsRes = CheckInputs();
            if (CheckInputsRes != "success")
            {
                ShowMsg(CheckInputsRes, "error");
                return;
            }

            bool IsThereCheckedItems = false;
            string[] Students = hdallstds.Value.Split('|');
            try
            {
                string FormGuidID = hdfgid.Value.ToString().Trim();

                //Delete data for QuestionsForm
                DeleteQuestionFormStudents(FormGuidID);

                foreach (string Student_Check in Students)
                {
                    if (!String.IsNullOrEmpty(Student_Check))
                    {
                        CheckBox chkBox;
                        chkBox = (CheckBox)PnlChks.FindControl(Student_Check) as CheckBox;
                        if (chkBox != null)
                        {
                            string[] StudentInfo = chkBox.ID.ToString().Split('_');
                            string StudentID = StudentInfo[1];
                            string CurriculumID = StudentInfo[2];
                            string ClassID = StudentInfo[3];
                            string StartDateTime = String.Concat(TxtDate.Text, " ", TxtTime.Text);
                            string EndDateTime = String.Concat(TxtDate.Text, " ", TxtEndTime.Text);
                            bool IsActive = ChkIsActive.Checked;

                            //Insert Data to QuestionsFormSettings
                            if (chkBox.Checked)
                            {
                                IsThereCheckedItems = true;
                                AssignTheFormToStudents(FormGuidID, StudentID, CurriculumID, ClassID, StartDateTime, EndDateTime, IsActive);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }

            if (IsThereCheckedItems) Response.Redirect("FormsSettings.aspx", true);
            else ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "You have not selected students."
                :
                "لم تقم باختيار الطلاب", "info");
        }

        private string CheckInputs()
        {
            string DateTimeStr = String.Concat(TxtDate.Text, " ", TxtTime.Text);
            string EndDateTimeStr = String.Concat(TxtDate.Text, " ", TxtTime.Text);
            try
            {
                DateTime DT = DateTime.Parse(DateTimeStr);
                DT = DateTime.Parse(EndDateTimeStr);
            }
            catch (Exception ex)
            {
                return hdclutrueName.Value.Contains("en") ? "There is no date or time assigned to questions form" : "لم يتم تحديد تاريخ أو وقت لنموذج الأسئلة";
            }

            return "success";
        }

        private void DeleteQuestionFormStudents(string FormGuidID)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            //Delete data for QuestionsForm
            CMD = new SqlCommand($"DELETE FROM QuestionsFormSettings WHERE FormGuidID = '{FormGuidID}'", con);
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void AssignTheFormToStudents(string FormGuidID, string StudentID, string CurriculumID, string ClassID, string StartDateTime, string EndDateTime, bool IsActive)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            //Insert data fot QuestionsForm
            CMD = new SqlCommand("INSERT INTO QuestionsFormSettings " +
                                 "(" +
                                    "QuestionsFormSettingsGuidID, FormGuidID, StudentID, CurriculumID, ClassID, StratDateTime, EndDateTime, IsActive " +
                                 ") VALUES (" +
                                    "@QuestionsFormSettingsGuidID, @FormGuidID, @StudentID, @CurriculumID, @ClassID, @StratDateTime, @EndDateTime, @IsActive " +
                                 ")", con);
            CMD.Parameters.AddWithValue("@QuestionsFormSettingsGuidID", SystemBase.GenerateGuidID("QuestionsFormSettingsGuidID", "QuestionsFormSettings"));
            CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
            CMD.Parameters.AddWithValue("@StudentID", StudentID);
            CMD.Parameters.AddWithValue("@CurriculumID", CurriculumID);
            CMD.Parameters.AddWithValue("@ClassID", ClassID);
            CMD.Parameters.AddWithValue("@StratDateTime", StartDateTime);
            CMD.Parameters.AddWithValue("@EndDateTime", EndDateTime);
            CMD.Parameters.AddWithValue("@IsActive", IsActive);
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }

        private void RetriveFormSettings()
        {
            DataTable FormInfoDB =
                SystemBase.GetDataTble($"SELECT * " +
                                       $"FROM QuestionsFormSettings " +
                                       $"WHERE FormGuidID = '{hdfgid.Value.Trim()}'");
            if (FormInfoDB != null)
            {
                if (FormInfoDB.Rows.Count > 0)
                {
                    string StratDateTime = FormInfoDB.Rows[0]["StratDateTime"].ToString();
                    string[] StratDateTimeArr = StratDateTime.Split(' ');
                    string StartDateDate = Convert.ToDateTime(StratDateTimeArr[0].ToString()).ToString("yyyy-MM-dd");
                    string EndDateTime = FormInfoDB.Rows[0]["EndDateTime"].ToString();
                    string[] EndDateTimeArr = EndDateTime.Split(' ');
                    //string EndDateDate = Convert.ToDateTime(EndDateTimeArr[0].ToString()).ToString("yyyy-MM-dd");

                    TxtDate.Text = StartDateDate;
                    TxtTime.Text = StratDateTimeArr[1];
                    TxtEndTime.Text = EndDateTimeArr[1];
                    ChkIsActive.Checked = FormInfoDB.Rows[0]["IsActive"].ToString() == "True" ? true : false;

                    string[] AllStds = hdallstds.Value.Split('|');
                    DrpStudentsSelections.SelectedValue = "1";
                    if (FormInfoDB.Rows.Count < AllStds.Length - 1) DrpStudentsSelections.SelectedValue = "2";
                    DrpStudentsSelections_IndxChange(null, null);



                    foreach (DataRow Student in FormInfoDB.Rows)
                    {
                        CheckBox chkBox;
                        chkBox = (CheckBox)PnlChks.FindControl(
                                String.Concat("ChkSel_", Student["StudentID"], "_", Student["CurriculumID"], "_", Student["ClassID"])) as CheckBox;
                        if (chkBox != null) chkBox.Checked = true;
                    }
                }
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormsSettings.aspx", true);
        }
    }
}