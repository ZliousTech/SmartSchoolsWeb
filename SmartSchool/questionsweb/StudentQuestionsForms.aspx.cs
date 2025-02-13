using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartSchool.questionsweb
{
    public partial class StudentQuestionsForms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);

                    hdSchoolID.Value = SystemBase.GetCookie((int)clsenumration.UserData.SchoolID);
                    hdStaffID.Value = SystemBase.GetCookie((int)clsenumration.UserData.StaffID);
                    hdStdID.Value = SystemBase.GetCookie((int)clsenumration.UserData.StudentID);

                    SetCulture();
                    SetLookups();
                    GetQuestionsFormsList();

                    Response.CacheControl = "no-cache";
                    Response.AddHeader("Pragma", "no-cache");
                    Response.Expires = -1;
                    Response.Headers.Add("Refresh", "60"); //Refresh every one minutes.
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    //Server.TransferRequest(Request.Url.AbsolutePath, false);
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

        }

        private void SetASPElementsTxt()
        {
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

        private void GetQuestionsFormsList()
        {
            string StaffName = "StaffArabicName";
            string QuestionsFormTypeName = "QuestionsFormTypeNameAr";
            string CurriculumName = "CurriculumArabicName";
            string SchoolClassName = "SchoolClassArabicName";
            string SubjectName = "SubjectArabicName";
            string SemesterName = "SemesterArabicName";
            if (hdclutrueName.Value.Contains("en"))
            {
                StaffName = "StaffEnglishName";
                QuestionsFormTypeName = "QuestionsFormTypeNameEn";
                CurriculumName = "CurriculumEnglishName";
                SchoolClassName = "SchoolClassEnglishName";
                SubjectName = "SubjectEnglishName";
                SemesterName = "SemesterEnglishName";
            }

            ReptFormsQuestionsList.DataSource =
                SystemBase.GetDataTble($"SELECT QB.FormGuidID, QB.QuestionsFormTitle, QB.QuestionsFormDescription, QB.IsApproved, StaffName = REPLACE(STF.{StaffName}, '-', ' '), " +
                                       $"       QuestionsFormTypeName = QFT.{QuestionsFormTypeName}, CurriculumName = CRC.{CurriculumName}, " +
                                       $"       SchoolClassName = SCC.{SchoolClassName}, SubjectName = SUB.{SubjectName}, SemesterName = SMT.{SemesterName}, QFS.StratDateTime, QFS.EndDateTime, " +
                                       $"       QFS.IsStudentFinished, StdQFEndDateTime = (DATEADD(mi, QB.QuestionsFormDurationMinutes, QFS.StratDateTime)), " +
                                       $"       CASE " +
                                       $"           WHEN QFS.IsStudentFinished = 'True' THEN 'FINISHED' " +
                                       $"           WHEN GETDATE() > QFS.EndDateTime THEN 'CLOSED' " +
                                       $"           WHEN GETDATE() >= QFS.StratDateTime THEN 'YES' " +
                                       $"           ELSE 'NO' " +
                                       $"       END AS IsFormOpened " +
                                       $"FROM QuestionsBank QB " +
                                       $"INNER JOIN Staff STF ON " +
                                       $"(STF.StaffID = QB.StaffID) " +
                                       $"INNER JOIN QuestionsFormSettings QFS ON " +
                                       $"(QFS.FormGuidID = QB.FormGuidID) " +
                                       $"INNER JOIN QuestionsFormTypes QFT ON " +
                                       $"(QFT.QuestionsFormTypeID = QB.QuestionsFormTypeID) " +
                                       $"INNER JOIN Curriculums CRC ON " +
                                       $"(CRC.CurriculumID = QB.CurriculumID) " +
                                       $"INNER JOIN SchoolClasses SCC ON " +
                                       $"(SCC.SchoolClassID = QB.SchoolClassID) " +
                                       $"INNER JOIN Subjects SUB ON " +
                                       $"(SUB.SubjectID = QB.SubjectID) " +
                                       $"INNER JOIN Semesters SMT ON " +
                                       $"(SMT.ID = QB.SemesterID) " +
                                       $"WHERE QB.SchoolID = {hdSchoolID.Value} AND QB.IsDeleted = 'False' AND QFS.StudentID = '{hdStdID.Value}' AND QFS.IsActive = 'True' " +
                                       $"ORDER BY QB.QuestionBankID ASC");
            ReptFormsQuestionsList.DataBind();
        }
    }
}