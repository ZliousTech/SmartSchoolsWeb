using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartSchool.questionsweb
{
    public partial class ViewQuestions : System.Web.UI.Page
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

                    SetCulture();
                    SetLookups();
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
                    GetQuestionsList();
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

        private void GetQuestionsList()
        {
            string QuestionTypeText = "QuestionTypeArabicText";

            if (hdclutrueName.Value.Contains("en"))
            {
                QuestionTypeText = "QuestionTypeEnglishText";
            }

            DataTable QuestionsFormInfoDB =
                SystemBase.GetDataTble($"SELECT QuestionsFormTitle " +
                                       $"FROM QuestionsBank " +
                                       $"WHERE FormGuidID = '{hdfgid.Value}' AND IsDeleted = 'False'");
            if (QuestionsFormInfoDB != null)
            {
                if (QuestionsFormInfoDB.Rows.Count > 0)
                {
                    LblQuestionsFormTitle.Text = QuestionsFormInfoDB.Rows[0]["QuestionsFormTitle"].ToString();
                }
                else
                {
                    SystemBase.Logout();
                }
            }
            else
            {
                SystemBase.Logout();
            }

            ReptQuestionsList.DataSource =
                SystemBase.GetDataTble($"SELECT Q.QuestionGuidID, QuestionTypeText = QT.{QuestionTypeText}, Q.QuestionText, " +
                                       $"       Q.QuestionGrade, Q.IsApproved " +
                                       $"FROM Questions Q " +
                                       $"INNER JOIN QuestionTypes QT ON " +
                                       $"(QT.QuestionTypeID = Q.QuestionTypeID) " +
                                       $"WHERE Q.FormGuidID = '{hdfgid.Value}' AND Q.IsDeleted = 'False'");
            ReptQuestionsList.DataBind();
        }
    }
}