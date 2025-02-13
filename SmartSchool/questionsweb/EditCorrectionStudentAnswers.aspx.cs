using Common;
using Common.Base;
using Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartSchool.questionsweb
{
    public partial class EditCorrectionStudentAnswers : System.Web.UI.Page
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

                        if (String.IsNullOrEmpty(Request.QueryString["STDID"]))
                        {
                            SystemBase.Logout();
                        }

                        if (String.IsNullOrEmpty(Request.QueryString["n"]))
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
                    string StudentGuidID = Request.QueryString["STDID"];
                    hdstdgid.Value = StudentGuidID;
                    string StudentName = Request.QueryString["n"];
                    StdName.Text = StudentName;
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
            SetASPElementsTxt();

            RetriveQuestionsList();
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

        private void RetriveQuestionsList()
        {
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

            string QuestionTypeName = "QuestionTypeArabicText";
            if (hdclutrueName.Value.Contains("en"))
            {
                QuestionTypeName = "QuestionTypeEnglishText";
            }

            DataTable QuestionStudentsDeservedGradeInfo;

            DataTable QuestionsInfoDB =
                SystemBase.GetDataTble($"SELECT Q.QuestionGuidID, Q.QuestionText, Q.QuestionGrade, QT.QuestionTypeEnglishText, QuestionTypeName = QT.{QuestionTypeName}, " +
                                       $"       Q.AnswerText, QSA.AnswerTextEssay, QSA.AnswerTextShortAnswer, " +
                                       $"       Q.AnswerNumberOne, Q.AnswerNumberTwo, Q.AnswerNumberThree, Q.AnswerNumberFour, Q.AnswerNumberFive, Q.AnswerNumberSix, Q.TheCorrectAnswer, " +
                                       $"       StdAnswerNumberOne = QSA.AnswerNumberOne, StdAnswerNumberTwo = QSA.AnswerNumberTwo, StdAnswerNumberThree = QSA.AnswerNumberThree, StdAnswerNumberFour = QSA.AnswerNumberFour, StdAnswerNumberFive = QSA.AnswerNumberFive, StdAnswerNumberSix = QSA.AnswerNumberSix, " +
                                       $"       Q.TheCorrectAnswerTrueFals, QSA.AnswerTrue, QSA.AnswerFalse, " +
                                       $"       Q.TheCorrectAnswerYesNo, QSA.AnswerYes, QSA.AnswerNo " +
                                       $"FROM Questions Q " +
                                       $"INNER JOIN QuestionsStudentsAnswers QSA ON " +
                                       $"(QSA.QuestionGuidID = Q.QuestionGuidID) " +
                                       $"INNER JOIN QuestionTypes QT ON " +
                                       $"(QT.QuestionTypeID = Q.QuestionTypeID) " +
                                       $"WHERE QSA.FormGuidID = '{hdfgid.Value}' AND QSA.StudentID = '{hdstdgid.Value}'");
            if (QuestionsInfoDB != null)
            {
                if (QuestionsInfoDB.Rows.Count > 0)
                {
                    foreach (DataRow row in QuestionsInfoDB.Rows)
                    {
                        switch (row["QuestionTypeEnglishText"].ToString())
                        {
                            case "Essay":
                                hdQuestionGuidIDsGrads.Value += $"{row["QuestionGuidID"]}_{row["QuestionGrade"]}|";

                                TextBox TxtEssay = new TextBox();
                                TxtEssay.ID = $"Txt_{row["QuestionGuidID"]}";
                                TxtEssay.Attributes["runat"] = "server";
                                TxtEssay.Attributes["class"] = "form-control";
                                TxtEssay.Attributes["style"] = "width: 150px";
                                //TxtEssay.Text = String.Empty;
                                TxtEssay.Font.Size = 12;
                                TxtEssay.Font.Bold = true;

                                MainPanel.Controls.Add(new LiteralControl("<div class=\"light-component inner\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<hr />"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionText}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.TheCorrectAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.StudentAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerTextEssay"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                                MainPanel.Controls.Add(new LiteralControl("<tr>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.QuestionGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionGrade"]}</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\"></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-warning\">{Resources.Resource.DeservedGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td>"));

                                QuestionStudentsDeservedGradeInfo =
                                    SystemBase.GetDataTble($"SELECT QuestionDeservedGrade " +
                                                           $"FROM QuestionStudentsDeservedGrade " +
                                                           $"WHERE StudentID = '{hdstdgid.Value}' AND FormGuidID = '{hdfgid.Value}' AND QuestionGuidID = '{row["QuestionGuidID"]}'");
                                if (QuestionStudentsDeservedGradeInfo != null)
                                {
                                    if (QuestionStudentsDeservedGradeInfo.Rows.Count > 0)
                                    {
                                        TxtEssay.Text = QuestionStudentsDeservedGradeInfo.Rows[0]["QuestionDeservedGrade"].ToString();
                                    }
                                }
                                MainPanel.Controls.Add(TxtEssay);
                                MainPanel.Controls.Add(new LiteralControl(" </td>"));
                                MainPanel.Controls.Add(new LiteralControl(" </tr>"));
                                MainPanel.Controls.Add(new LiteralControl("</table>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                break;

                            case "Short Answer":
                                hdQuestionGuidIDsGrads.Value += $"{row["QuestionGuidID"]}_{row["QuestionGrade"]}|";

                                TextBox TxtShortAnswer = new TextBox();
                                TxtShortAnswer.ID = $"Txt_{row["QuestionGuidID"]}";
                                TxtShortAnswer.Attributes["runat"] = "server";
                                TxtShortAnswer.Attributes["class"] = "form-control";
                                TxtShortAnswer.Attributes["style"] = "width: 150px";
                                //TxtShortAnswer.Text = String.Empty;
                                TxtShortAnswer.Font.Size = 12;
                                TxtShortAnswer.Font.Bold = true;

                                MainPanel.Controls.Add(new LiteralControl("<div class=\"light-component inner\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<hr />"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}: {row["QuestionTypeName"]}: {Resources.Resource.QuestionText}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.TheCorrectAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.StudentAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerTextShortAnswer"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                                MainPanel.Controls.Add(new LiteralControl("<tr>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.QuestionGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionGrade"]}</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\"></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-warning\">{Resources.Resource.DeservedGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td>"));

                                QuestionStudentsDeservedGradeInfo =
                                    SystemBase.GetDataTble($"SELECT QuestionDeservedGrade " +
                                                           $"FROM QuestionStudentsDeservedGrade " +
                                                           $"WHERE StudentID = '{hdstdgid.Value}' AND FormGuidID = '{hdfgid.Value}' AND QuestionGuidID = '{row["QuestionGuidID"]}'");
                                if (QuestionStudentsDeservedGradeInfo != null)
                                {
                                    if (QuestionStudentsDeservedGradeInfo.Rows.Count > 0)
                                    {
                                        TxtShortAnswer.Text = QuestionStudentsDeservedGradeInfo.Rows[0]["QuestionDeservedGrade"].ToString();
                                    }
                                }
                                MainPanel.Controls.Add(TxtShortAnswer);
                                MainPanel.Controls.Add(new LiteralControl(" </td>"));
                                MainPanel.Controls.Add(new LiteralControl(" </tr>"));
                                MainPanel.Controls.Add(new LiteralControl("</table>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                break;

                            case "Multiple Choice":
                                hdQuestionGuidIDsGrads.Value += $"{row["QuestionGuidID"]}_{row["QuestionGrade"]}|";

                                int CorrectAnswerMChoiceNo = 0;
                                int StudentAnswerMChoiceNo = 0;

                                TextBox TxtMultipleChoice = new TextBox();
                                TxtMultipleChoice.ID = $"Txt_{row["QuestionGuidID"]}";
                                TxtMultipleChoice.Attributes["runat"] = "server";
                                TxtMultipleChoice.Attributes["class"] = "form-control";
                                TxtMultipleChoice.Attributes["style"] = "width: 150px";
                                TxtMultipleChoice.Attributes["readonly"] = "readonly";
                                //TxtMultipleChoice.Text = String.Empty;
                                TxtMultipleChoice.Font.Size = 12;
                                TxtMultipleChoice.Font.Bold = true;

                                MainPanel.Controls.Add(new LiteralControl("<div class=\"light-component inner\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<hr />"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}: {row["QuestionTypeName"]}: {Resources.Resource.QuestionText}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.TheCorrectAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                switch (row["TheCorrectAnswer"].ToString().Trim())
                                {
                                    case "1":
                                        CorrectAnswerMChoiceNo = 1;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberOne"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                    case "2":
                                        CorrectAnswerMChoiceNo = 2;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberTwo"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                    case "3":
                                        CorrectAnswerMChoiceNo = 3;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberThree"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                    case "4":
                                        CorrectAnswerMChoiceNo = 4;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberFour"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                    case "5":
                                        CorrectAnswerMChoiceNo = 5;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberFive"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                    case "6":
                                        CorrectAnswerMChoiceNo = 6;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberSix"]} ({Resources.Resource.AnswerNo}.: {row["TheCorrectAnswer"]})</label>"));
                                        break;
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.StudentAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                if (row["StdAnswerNumberOne"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 1;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberOne"]} ({Resources.Resource.AnswerNo}.: 1)</label>"));
                                }
                                else if (row["StdAnswerNumberTwo"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 2;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberTwo"]} ({Resources.Resource.AnswerNo}.: 2)</label>"));
                                }
                                else if (row["StdAnswerNumberThree"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 3;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberThree"]} ({Resources.Resource.AnswerNo}.: 3)</label>"));
                                }
                                else if (row["StdAnswerNumberFour"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 4;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberFour"]} ({Resources.Resource.AnswerNo}.: 4)</label>"));
                                }
                                else if (row["StdAnswerNumberFive"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 5;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberFive"]} ({Resources.Resource.AnswerNo}.: 5)</label>"));
                                }
                                else if (row["StdAnswerNumberSix"].ToString().Trim() == "True")
                                {
                                    StudentAnswerMChoiceNo = 6;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>{row["AnswerNumberSix"]} ({Resources.Resource.AnswerNo}.: 6)</label>"));
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                                MainPanel.Controls.Add(new LiteralControl("<tr>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.QuestionGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionGrade"]}</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\"></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-warning\">{Resources.Resource.DeservedGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td>"));
                                TxtMultipleChoice.Text = row["QuestionGrade"].ToString();
                                if (StudentAnswerMChoiceNo != CorrectAnswerMChoiceNo || StudentAnswerMChoiceNo == 0) TxtMultipleChoice.Text = "0";
                                MainPanel.Controls.Add(TxtMultipleChoice);
                                MainPanel.Controls.Add(new LiteralControl(" </td>"));
                                MainPanel.Controls.Add(new LiteralControl(" </tr>"));
                                MainPanel.Controls.Add(new LiteralControl("</table>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                break;

                            case "True / False":
                                hdQuestionGuidIDsGrads.Value += $"{row["QuestionGuidID"]}_{row["QuestionGrade"]}|";

                                int CorrectAnswerTFChoiceNo = 0;
                                int StudentAnswerTFChoiceNo = 0;

                                TextBox TxtTrueFalse = new TextBox();
                                TxtTrueFalse.ID = $"Txt_{row["QuestionGuidID"]}";
                                TxtTrueFalse.Attributes["runat"] = "server";
                                TxtTrueFalse.Attributes["class"] = "form-control";
                                TxtTrueFalse.Attributes["style"] = "width: 150px";
                                TxtTrueFalse.Attributes["readonly"] = "readonly";
                                //TxtTrueFalse.Text = String.Empty;
                                TxtTrueFalse.Font.Size = 12;
                                TxtTrueFalse.Font.Bold = true;

                                MainPanel.Controls.Add(new LiteralControl("<div class=\"light-component inner\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<hr />"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}: {row["QuestionTypeName"]}: {Resources.Resource.QuestionText}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.TheCorrectAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                switch (row["TheCorrectAnswerTrueFals"].ToString().Trim())
                                {
                                    case "1":
                                        CorrectAnswerTFChoiceNo = 1;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>True ({Resources.Resource.AnswerNo}.: 1)</label>"));
                                        break;
                                    case "2":
                                        CorrectAnswerTFChoiceNo = 2;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>False ({Resources.Resource.AnswerNo}.: 2)</label>"));
                                        break;
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.StudentAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                if (row["AnswerTrue"].ToString().Trim() == "True")
                                {
                                    StudentAnswerTFChoiceNo = 1;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>True ({Resources.Resource.AnswerNo}.: 1)</label>"));
                                }
                                else if (row["AnswerFalse"].ToString().Trim() == "True")
                                {
                                    StudentAnswerTFChoiceNo = 2;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>False ({Resources.Resource.AnswerNo}.: 2))</label>"));
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                                MainPanel.Controls.Add(new LiteralControl("<tr>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.QuestionGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionGrade"]}</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\"></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-warning\">{Resources.Resource.DeservedGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td>"));
                                TxtTrueFalse.Text = row["QuestionGrade"].ToString();
                                if (StudentAnswerTFChoiceNo != CorrectAnswerTFChoiceNo || StudentAnswerTFChoiceNo == 0) TxtTrueFalse.Text = "0";
                                MainPanel.Controls.Add(TxtTrueFalse);
                                MainPanel.Controls.Add(new LiteralControl(" </td>"));
                                MainPanel.Controls.Add(new LiteralControl(" </tr>"));
                                MainPanel.Controls.Add(new LiteralControl("</table>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                break;

                            case "Yes / No":
                                hdQuestionGuidIDsGrads.Value += $"{row["QuestionGuidID"]}_{row["QuestionGrade"]}|";

                                int CorrectAnswerYNChoiceNo = 0;
                                int StudentAnswerYNChoiceNo = 0;

                                TextBox TxtYesNo = new TextBox();
                                TxtYesNo.ID = $"Txt_{row["QuestionGuidID"]}";
                                TxtYesNo.Attributes["runat"] = "server";
                                TxtYesNo.Attributes["class"] = "form-control";
                                TxtYesNo.Attributes["style"] = "width: 150px";
                                TxtYesNo.Attributes["readonly"] = "readonly";
                                //TxtYesNo.Text = String.Empty;
                                TxtYesNo.Font.Size = 12;
                                TxtYesNo.Font.Bold = true;

                                MainPanel.Controls.Add(new LiteralControl("<div class=\"light-component inner\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<hr />"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}: {row["QuestionTypeName"]}: {Resources.Resource.QuestionText}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionText"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.QuestionType}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionTypeName"]}</label>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-info\">{Resources.Resource.TheCorrectAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                switch (row["TheCorrectAnswerYesNo"].ToString().Trim())
                                {
                                    case "1":
                                        CorrectAnswerYNChoiceNo = 1;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>Yes ({Resources.Resource.AnswerNo}.: 1)</label>"));
                                        break;
                                    case "2":
                                        CorrectAnswerYNChoiceNo = 2;
                                        MainPanel.Controls.Add(new LiteralControl($"<label>No ({Resources.Resource.AnswerNo}.: 2)</label>"));
                                        break;
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.StudentAnswer}:</label>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                if (row["AnswerYes"].ToString().Trim() == "True")
                                {
                                    StudentAnswerYNChoiceNo = 1;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>Yes ({Resources.Resource.AnswerNo}.: 1)</label>"));
                                }
                                else if (row["AnswerNo"].ToString().Trim() == "True")
                                {
                                    StudentAnswerYNChoiceNo = 2;
                                    MainPanel.Controls.Add(new LiteralControl($"<label>No ({Resources.Resource.AnswerNo}.: 2))</label>"));
                                }
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"col-md-12\">"));
                                MainPanel.Controls.Add(new LiteralControl("<div class=\"form-group\">"));
                                MainPanel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                                MainPanel.Controls.Add(new LiteralControl("<tr>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-danger\">{Resources.Resource.QuestionGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label>{row["QuestionGrade"]}</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 20px;\"></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td style=\"width: 120px;\">"));
                                MainPanel.Controls.Add(new LiteralControl($"<label class=\"text-warning\">{Resources.Resource.DeservedGrade}:</label></td>"));
                                MainPanel.Controls.Add(new LiteralControl("<td>"));
                                TxtYesNo.Text = row["QuestionGrade"].ToString();
                                if (StudentAnswerYNChoiceNo != CorrectAnswerYNChoiceNo || StudentAnswerYNChoiceNo == 0) TxtYesNo.Text = "0";
                                MainPanel.Controls.Add(TxtYesNo);
                                MainPanel.Controls.Add(new LiteralControl(" </td>"));
                                MainPanel.Controls.Add(new LiteralControl(" </tr>"));
                                MainPanel.Controls.Add(new LiteralControl("</table>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                MainPanel.Controls.Add(new LiteralControl("</div>"));
                                break;
                        }
                    }
                }
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckInputsRes = CheckInputs();
            if (CheckInputsRes != "success")
            {
                ShowMsg(CheckInputsRes, "error");
                return;
            }

            //Insert data here 
            string[] QuestionGuidIDsGrads = hdQuestionGuidIDsGrads.Value.Split('|');
            for (int i = 0; i < QuestionGuidIDsGrads.Length - 1; i++)
            {
                string[] QuestionGuidIDGrade = QuestionGuidIDsGrads[i].Split('_');

                string QuestionGradeGuidID = SystemBase.GenerateGuidID("QuestionGradeGuidID", "QuestionStudentsDeservedGrade");
                string StudentID = hdstdgid.Value;
                string FormGuidID = hdfgid.Value;
                string QuestionGuidID = QuestionGuidIDGrade[0];
                string QuestionGrad = QuestionGuidIDGrade[1];
                string QuestionDeservedGrade = "0";

                TextBox TxtBox = new TextBox();
                TxtBox = (TextBox)MainPanel.FindControl(String.Concat("Txt_", QuestionGuidID));
                if (TxtBox != null)
                {
                    QuestionDeservedGrade = TxtBox.Text;
                }

                //Insert the questions (question by question) to QuestionStudentsDeservedGrade Table
                string InsertRes = UpdateStudentQuestionAnswer(QuestionGradeGuidID, StudentID, FormGuidID, QuestionGuidID,
                                                               Convert.ToDouble(QuestionGrad), Convert.ToDouble(QuestionDeservedGrade));
                //if (InsertRes != "success")
                //{
                //    ShowMsg(InsertRes, "error");
                //}
            }

            ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "The data has been added. Please wait while you are being redirected."
                :
                "تم اضافة البيانات. يرجى الانتظار حتى يتم إعادة توجيهك.",
                "success");

            Response.AddHeader("REFRESH", $"2;URL=StudentsList.aspx?FGID={hdfgid.Value}");
        }

        private string CheckInputs()
        {
            string CheckRes = "success";

            string[] QuestionGuidIDsGrads = hdQuestionGuidIDsGrads.Value.Split('|');
            for (int i = 0; i < QuestionGuidIDsGrads.Length - 1; i++)
            {
                string[] QuestionGuidIDGrade = QuestionGuidIDsGrads[i].Split('_');
                string QuestionGuiID = QuestionGuidIDGrade[0];
                double QuestionGrade = double.Parse(QuestionGuidIDGrade[1]);

                TextBox TxtBox = new TextBox();
                TxtBox = (TextBox)MainPanel.FindControl(String.Concat("Txt_", QuestionGuiID));
                if (TxtBox != null)
                {
                    string TxtBoxVal = TxtBox.Text;
                    if (String.IsNullOrEmpty(TxtBoxVal))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Not all grades due are entered correctly" : "لم يتم إدخال جميع الدرجات المستحقة بشكل صحيح";
                    }

                    if (!SystemBase.RegIsMatch(TxtBoxVal, "NUMBER-OR-DECIMAL"))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Not all grades due are entered correctly" : "لم يتم إدخال جميع الدرجات المستحقة بشكل صحيح";
                    }

                    if (double.Parse(TxtBoxVal) < 0)
                    {
                        return hdclutrueName.Value.Contains("en") ? "There are unacceptable grades, which may be less or greater than the question grade" : "يوجد درجات غير مسموح فيها، قد تكون أقل أو أكبر من درجة السؤال";
                    }

                    if (double.Parse(TxtBoxVal) > QuestionGrade)
                    {
                        return hdclutrueName.Value.Contains("en") ? "There are unacceptable grades, which may be less or greater than the question grade" : "يوجد درجات غير مسموح فيها، قد تكون أقل أو أكبر من درجة السؤال";
                    }
                }
            }

            return CheckRes;
        }

        private string UpdateStudentQuestionAnswer(string QuestionGradeGuidID, string StudentID, string FormGuidID, string QuestionGuidID,
                                                   double QuestionGrade, double QuestionDeservedGrade)
        {
            string InsertRes = "success";

            DataTable StudentQuestionAnswerDB =
                SystemBase.GetDataTble($"SELECT QuestionCount = COUNT(QuestionGradeID) " +
                                       $"FROM QuestionStudentsDeservedGrade " +
                                       $"WHERE StudentID = '{StudentID}' AND FormGuidID = '{FormGuidID}' AND QuestionGuidID = '{QuestionGuidID}'");
            if (StudentQuestionAnswerDB != null)
            {
                if (StudentQuestionAnswerDB.Rows.Count > 0)
                {
                    if (int.Parse(StudentQuestionAnswerDB.Rows[0]["QuestionCount"].ToString()) > 0)
                    {
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
                        if (con.State == ConnectionState.Closed) con.Open();
                        SqlCommand CMD;

                        CMD = new SqlCommand("UPDATE QuestionStudentsDeservedGrade SET " +
                                             "QuestionDeservedGrade = @QuestionDeservedGrade " +
                                             "WHERE StudentID = @StudentID AND FormGuidID = @FormGuidID AND QuestionGuidID = @QuestionGuidID", con);
                        CMD.Parameters.AddWithValue("@StudentID", StudentID);
                        CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
                        CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                        CMD.Parameters.AddWithValue("@QuestionDeservedGrade", QuestionDeservedGrade);
                        CMD.ExecuteNonQuery();

                        if (con.State == ConnectionState.Open)
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }
                }
            }

            return InsertRes;
        }
    }
}