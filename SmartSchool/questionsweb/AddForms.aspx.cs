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
    public partial class AddForms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);

                    SetCulture();
                    SetLookups();

                    /*For test*/
                    //string StaffID = SystemBase.GetCookie((int)clsenumration.UserData.StaffID);
                    //ShowMsg(StaffID, "info"); //10000 admin Manager -1
                    //DataTable StaffInfoDB =
                    //    SystemBase.GetDataTble("SELECT * FROM Users WHERE StaffID = '202310780000428'");
                    //string UserName = StaffInfoDB.Rows[0]["UserName"].ToString();
                    //string UPassword = SystemBase.DecryptF(StaffInfoDB.Rows[0]["Password"].ToString());
                    //ShowMsg($"UserName: {UserName}, Password: {UPassword}", "info");
                    /*eof For test*/
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
            SetASPElementsTxt();
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
            string SchoolID = SystemBase.GetCookie((int)clsenumration.UserData.SchoolID);
            string StaffID = SystemBase.GetCookie((int)clsenumration.UserData.StaffID);
            string QuestionsFormTypeName = "QuestionsFormTypeNameAr";
            string CurriculumName = "CurriculumArabicName";
            string QuestionsFormStylesName = "QuestionsFormStyleNameAr";
            string SemesterName = "SemesterArabicName";
            if (hdclutrueName.Value.Contains("en"))
            {
                QuestionsFormTypeName = "QuestionsFormTypeNameEn";
                CurriculumName = "CurriculumEnglishName";
                QuestionsFormStylesName = "QuestionsFormStyleNameEn";
                SemesterName = "SemesterEnglishName";
            }

            DrpFormTypes.Items.Clear();
            DrpFormTypes.DataSource =
                SystemBase.GetDataTble($"SELECT QuestionsFormTypeID, QuestionsFormTypeName = {QuestionsFormTypeName} " +
                                       $"FROM QuestionsFormTypes " +
                                       $"ORDER BY QuestionsFormTypeID ASC");
            DrpFormTypes.DataValueField = "QuestionsFormTypeID";
            DrpFormTypes.DataTextField = "QuestionsFormTypeName";
            DrpFormTypes.DataBind();

            DrpCurriculums.Items.Clear();
            DrpCurriculums.DataSource =
                SystemBase.GetDataTble($"SELECT DISTINCT CM.CurriculumID, CurriculumName = CM.{CurriculumName} " +
                                       $"FROM Curriculums CM " +
                                       $"INNER JOIN TeacherExperiences TXP ON " +
                                       $"(TXP.Curriculum = CM.CurriculumID) " +
                                       $"WHERE CM.SchoolID = {SchoolID} " +
                                       $"  AND TXP.TeacherID = '{StaffID}' " +
                                       $"ORDER BY CM.CurriculumID ASC");
            DrpCurriculums.DataValueField = "CurriculumID";
            DrpCurriculums.DataTextField = "CurriculumName";
            DrpCurriculums.DataBind();

            DrpCurriculums_IndxChange(null, null);


            DrpQuestionsFormStyles.Items.Clear();
            DrpQuestionsFormStyles.DataSource =
                SystemBase.GetDataTble($"SELECT QuestionsFormStyleID, QuestionsFormStyleName = {QuestionsFormStylesName} " +
                                       $"FROM QuestionsFormStyles " +
                                       $"ORDER BY QuestionsFormStyleID ASC");
            DrpQuestionsFormStyles.DataValueField = "QuestionsFormStyleID";
            DrpQuestionsFormStyles.DataTextField = "QuestionsFormStyleName";
            DrpQuestionsFormStyles.DataBind();
            DrpQuestionsFormStyles_IndxChange(null, null);

            DrpSemesters.Items.Clear();
            DrpSemesters.DataSource =
                SystemBase.GetDataTble($"SELECT ID, SemesterName = {SemesterName} " +
                                       $"FROM Semesters " +
                                       $"WHERE SchoolID = '{SchoolID}'" +
                                       $"ORDER BY ID ASC");
            DrpSemesters.DataValueField = "ID";
            DrpSemesters.DataTextField = "SemesterName";
            DrpSemesters.DataBind();

        }

        private void SetASPElementsTxt()
        {
            BtnSave.Text = Resources.Resource.SaveAndContinueAddQuestions;
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

        protected void DrpCurriculums_IndxChange(object sender, EventArgs e)
        {
            if (DrpCurriculums.Items.Count <= 0)
            {
                DrpClasses.Items.Clear();
                DrpSubjects.Items.Clear();
                return;
            }

            string SchoolClassName = "SchoolClassArabicName";
            if (hdclutrueName.Value.Contains("en"))
            {
                SchoolClassName = "SchoolClassEnglishName";
            }

            DrpClasses.Items.Clear();
            DrpClasses.DataSource =
                SystemBase.GetDataTble($"SELECT DISTINCT SC.SchoolClassID, SchoolClassName = SC.{SchoolClassName} " +
                                       $"FROM SchoolClasses SC " +
                                       $"INNER JOIN TeacherExperiences TXP ON " +
                                       $"(TXP.SchoolClassID = SC.SchoolClassID) " +
                                       $"WHERE SC.SchoolID = {SystemBase.GetCookie((int)clsenumration.UserData.SchoolID)} " +
                                       $"  AND SC.CurriculumID = {DrpCurriculums.SelectedValue} " +
                                       $"  AND TXP.TeacherID = '{SystemBase.GetCookie((int)clsenumration.UserData.StaffID)}' " +
                                       $"ORDER BY SC.SchoolClassID ASC");
            DrpClasses.DataValueField = "SchoolClassID";
            DrpClasses.DataTextField = "SchoolClassName";
            DrpClasses.DataBind();

            DrpClasses_IndxChange(null, null);
        }

        protected void DrpClasses_IndxChange(object sender, EventArgs e)
        {
            if (DrpClasses.Items.Count <= 0)
            {
                DrpSubjects.Items.Clear();
                return;
            }

            string SubjectName = "SubjectArabicName";
            if (hdclutrueName.Value.Contains("en"))
            {
                SubjectName = "SubjectEnglishName";
            }

            DrpSubjects.Items.Clear();
            DrpSubjects.DataSource =
                SystemBase.GetDataTble($"SELECT DISTINCT SBJ.SubjectID, SubjectName = SBJ.{SubjectName} " +
                                       $"FROM Subjects SBJ " +
                                       $"INNER JOIN TeacherExperiences TXP ON " +
                                       $"(TXP.SubjectID = SBJ.SubjectID) " +
                                       $"WHERE SBJ.SchoolID = {SystemBase.GetCookie((int)clsenumration.UserData.SchoolID)} " +
                                       $"  AND SBJ.SchoolClassID = {DrpClasses.SelectedValue} " +
                                       $"  AND TXP.TeacherID = '{SystemBase.GetCookie((int)clsenumration.UserData.StaffID)}' " +
                                       $"ORDER BY SBJ.SubjectID ASC");
            DrpSubjects.DataValueField = "SubjectID";
            DrpSubjects.DataTextField = "SubjectName";
            DrpSubjects.DataBind();
        }

        protected void DrpQuestionsFormStyles_IndxChange(object sender, EventArgs e)
        {
            TxtTimeForQuestionsFormMinutes.Text = "0";
            DivTimeForQuestionsForm.Visible = false;

            switch (DrpQuestionsFormStyles.SelectedValue)
            {
                case "1":
                    LblQuestionsFormStyles.Text = hdclutrueName.Value.Contains("en") ? "All questions appear to students at once." : "تظهر جميع الأسئلة للطلاب مرة واحدة.";
                    DivTimeForQuestionsForm.Visible = true;
                    break;
                case "2":
                    LblQuestionsFormStyles.Text = hdclutrueName.Value.Contains("en") ? "Present the questions to students one at a time, and allowing the student to move back and forth between the questions." : "اعرض الأسئلة على الطلاب واحدًا تلو الآخر، واسمح للطالب بالتنقل ذهابًا وإيابًا بين الأسئلة.";
                    DivTimeForQuestionsForm.Visible = true;
                    break;
                case "3":
                    LblQuestionsFormStyles.Text = hdclutrueName.Value.Contains("en") ? "Present the questions to students one at a time during a specified time, and do not allow the student to move back and forth between questions." : "عرض الأسئلة على الطلاب واحدًا تلو الآخر خلال وقت محدد، ولا يسمح للطالب بالتنقل ذهابًا وإيابًا بين الأسئلة.";
                    break;
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("quesbank.aspx", true);
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckInputs_res = CheckInputs();
            if (CheckInputs_res != "Success")
            {
                ShowMsg(CheckInputs_res, "error");
                return;
            }

            string SchoolID = SystemBase.GetCookie((int)clsenumration.UserData.SchoolID);
            string StaffID = SystemBase.GetCookie((int)clsenumration.UserData.StaffID);
            string QuestionBankGuidID = SystemBase.GenerateGuidID("QuestionBankGuidID", "QuestionsBank");
            string FormGuidID = QuestionBankGuidID;

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("INSERT INTO QuestionsBank " +
                                 "(" +
                                    "QuestionBankGuidID, SchoolID, StaffID, FormGuidID, QuestionsFormStyleID, QuestionsFormDurationMinutes, QuestionsFormTypeID, CurriculumID, SchoolClassID, SubjectID, SemesterID, QuestionsFormTitle, QuestionsFormDescription, IsApproved " +
                                 ") VALUES (" +
                                    "@QuestionBankGuidID, @SchoolID, @StaffID, @FormGuidID, @QuestionsFormStyleID, @QuestionsFormDurationMinutes, @QuestionsFormTypeID, @CurriculumID, @SchoolClassID, @SubjectID, @SemesterID, @QuestionsFormTitle, @QuestionsFormDescription, @IsApproved " +
                                 ") SELECT SCOPE_IDENTITY()", con);
            CMD.Parameters.AddWithValue("@QuestionBankGuidID", QuestionBankGuidID);
            CMD.Parameters.AddWithValue("@SchoolID", SchoolID);
            CMD.Parameters.AddWithValue("@StaffID", StaffID);
            CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
            CMD.Parameters.AddWithValue("@QuestionsFormStyleID", DrpQuestionsFormStyles.SelectedValue);
            CMD.Parameters.AddWithValue("@QuestionsFormDurationMinutes", TxtTimeForQuestionsFormMinutes.Text.Trim());
            CMD.Parameters.AddWithValue("@QuestionsFormTypeID", DrpFormTypes.SelectedValue);
            CMD.Parameters.AddWithValue("@CurriculumID", DrpCurriculums.SelectedValue);
            CMD.Parameters.AddWithValue("@SchoolClassID", DrpClasses.SelectedValue);
            CMD.Parameters.AddWithValue("@SubjectID", DrpSubjects.SelectedValue);
            CMD.Parameters.AddWithValue("@SemesterID", DrpSemesters.SelectedValue);
            CMD.Parameters.AddWithValue("@QuestionsFormTitle", TxtFormTitle.Text.Trim());
            CMD.Parameters.AddWithValue("@QuestionsFormDescription", TxtFormDescription.Text.Trim());
            CMD.Parameters.AddWithValue("@IsApproved", "False");
            CMD.ExecuteNonQuery();
            //int QuestionBankID = Convert.ToInt32(CMD.ExecuteScalar());

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "The data has been saved. Please wait while you are being redirected."
                :
                "تم حفظ البيانات. يرجى الانتظار حتى يتم إعادة توجيهك.",
                "success");

            //Response.Redirect($"AddQuestions.aspx?QBID={QuestionBankID}", true);
            Response.AddHeader("REFRESH", $"2;URL=AddQuestions.aspx?QBGID={QuestionBankGuidID}");
        }

        private string CheckInputs()
        {
            string Msg = "Success";

            if (DrpQuestionsFormStyles.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one questions form style" : "يرجى تحديد نمط واحد على الأقل لنموذج الأسئلة";
            }

            switch (DrpQuestionsFormStyles.SelectedValue)
            {
                case "1":
                case "2":
                    if (String.IsNullOrEmpty(TxtTimeForQuestionsFormMinutes.Text))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please check the questions form duration in minutes is not empty" : "يرجى التحقق من أن مدة نموذج الأسئلة بالدقائق ليست فارغة";
                    }

                    if (!SystemBase.RegIsMatch(TxtTimeForQuestionsFormMinutes.Text, "NUMBER"))
                    {
                        return hdclutrueName.Value.Contains("en") ? "The questions form duration in minutes is incorrect" : "مدة نموذج الأسئلة بالدقائق غير صحيحة";
                    }

                    if (int.Parse(TxtTimeForQuestionsFormMinutes.Text) <= 0)
                    {
                        return hdclutrueName.Value.Contains("en") ? "The questions form duration in minutes must be greater than zero" : "يجب أن تكون مدة نموذج الأسئلة بالدقائق أكبر من الصفر";
                    }
                    break;

                case "3":
                    TxtTimeForQuestionsFormMinutes.Text = "0";
                    break;
            }

            if (DrpFormTypes.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one questions form type" : "يرجى تحديد نوع واحد على الأقل لنموذج الأسئلة";
            }

            if (DrpCurriculums.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one curriculum" : "يرجى تحديد منهاج واحد على الأقل";
            }

            if (DrpClasses.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one class" : "يرجى تحديد صف واحد على الأقل";
            }

            if (DrpSubjects.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one subject" : "يرجى تحديد مادة واحدة على الأقل";
            }

            if (DrpSemesters.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one semester" : "يرجى تحديد فصل دراسي واحد على الأقل";
            }

            if (String.IsNullOrEmpty(TxtFormTitle.Text) || !SystemBase.RegIsMatch(TxtFormTitle.Text.Trim(), "REGULAR_TEXT"))
            {
                return hdclutrueName.Value.Contains("en") ? "Please check the title is not empty" : "يرجى التحقق من أن العنوان ليس فارغا";
            }

            if (String.IsNullOrEmpty(TxtFormDescription.Text) || !SystemBase.RegIsMatch(TxtFormDescription.Text.Trim(), "REGULAR_TEXT"))
            {
                return hdclutrueName.Value.Contains("en") ? "Please check the description is not empty" : "يرجى التحقق من أن الوصف ليس فارغا";
            }

            return Msg;
        }
    }
}