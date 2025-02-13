using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

namespace SmartSchool.questionsweb
{
    public partial class ViewStudentForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);
                    hdStdid.Value = SystemBase.GetCookie((int)clsenumration.UserData.StudentID);

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
                    hdformstylid.Value = QuestionsFormInfo[0];
                    LblQuestionsFormTitle.Text = QuestionsFormInfo[1];
                    hdformdurationmin.Value = QuestionsFormInfo[2]; //QuestionsFormDurationMinutes
                    SpanQuestionsFormDurationMinutes.Visible = false;
                    SetCulture();

                    string[] StdFormStartsEndsAt = GetStdFormStartsEndsAt(hdfgid.Value);
                    LblStartsAtExactly.Text = StdFormStartsEndsAt[0];
                    //hdEndsAtExactly.Value = Convert.ToDateTime(StdFormStartsEndsAt[1]).AddSeconds(30).ToString("MM/dd/yyyy HH:mm:ss");
                    hdEndsAtExactly.Value = Convert.ToDateTime(StdFormStartsEndsAt[1]).ToString("MM/dd/yyyy HH:mm:ss");
                    LblEndsAtExactly.Text = hdEndsAtExactly.Value;

                    string[] StdFormStartsEndsAtExactly = GetStdFormStartsEndsAtExactly(hdfgid.Value, hdStdid.Value);
                    LblStdStartsAtExactly.Text = StdFormStartsEndsAtExactly[0];
                    hdStdEndsAtExactly.Value = Convert.ToDateTime(StdFormStartsEndsAtExactly[1]).ToString("MM/dd/yyyy HH:mm:ss");
                    LblStdEndsAtExactly.Text = hdStdEndsAtExactly.Value;

                    DateTime _hdEndsAtExactly = Convert.ToDateTime(StdFormStartsEndsAt[1]);
                    DateTime _hdStdEndsAtExactly = Convert.ToDateTime(StdFormStartsEndsAtExactly[1]);
                    if (_hdStdEndsAtExactly > _hdEndsAtExactly)
                    {
                        hdStdEndsAtExactly.Value = hdEndsAtExactly.Value;
                        LblStdEndsAtExactly.Text = hdStdEndsAtExactly.Value;
                    }

                    string[] QuestionsGuidIDs = GetQuestionsGuidIDsForQuestionsForm(hdfgid.Value);
                    int TotalNumberOfQuestion = QuestionsGuidIDs.Length;
                    hdTotalNumberOfQuestion.Value = TotalNumberOfQuestion.ToString();
                    LblNumberOfQuestions.Text = hdTotalNumberOfQuestion.Value;
                    foreach (string QuestioGuidID in QuestionsGuidIDs)
                    {
                        hdQuestionsGuidIDs.Value += String.Concat(QuestioGuidID, "|");
                    }
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
            SetASPElementsTxt();

            /* Start adding the questions */
            ShowQuestionsForm();
            /* eof Start adding the questions */
        }

        private void SetCulture()
        {
            string CultureName = SystemBase.GetCurrentCultureName("_culture");
            if (String.IsNullOrEmpty(CultureName)) CultureName = "ar";
            SystemBase.SetCulture("_culture", CultureName);
            hdclutrueName.Value = CultureName;
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

        private string[] GetStdFormStartsEndsAt(string FormGuidID)
        {
            List<string> FormStartsEndsAt = new List<string>();

            DataTable StdFormStartEndDB =
                SystemBase.GetDataTble($"SELECT TOP 1 StratDateTime, EndDateTime " +
                                       $"FROM QuestionsFormSettings QFS " +
                                       $"INNER JOIN QuestionsBank QB ON " +
                                       $"(QB.FormGuidID = QFS.FormGuidID) " +
                                       $"WHERE QFS.FormGuidID = '{FormGuidID}'");
            if (StdFormStartEndDB != null)
            {
                if (StdFormStartEndDB.Rows.Count > 0)
                {
                    FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["StratDateTime"].ToString());
                    FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["EndDateTime"].ToString());
                }
            }

            return FormStartsEndsAt.ToArray();
        }

        private string[] GetStdFormStartsEndsAtExactly(string FormGuidID, string StudentID)
        {
            List<string> FormStartsEndsAt = new List<string>();
            bool IsStudentStartEndFormDateTimeUpdated = false;

            DataTable StdFormStartEndDB =
                SystemBase.GetDataTble($"SELECT StudentStartFormAt, StudentEndFormAt = (DATEADD(mi, QB.QuestionsFormDurationMinutes, QFS.StudentStartFormAt)) " +
                                       $"FROM QuestionsFormSettings QFS " +
                                       $"INNER JOIN QuestionsBank QB ON " +
                                       $"(QB.FormGuidID = QFS.FormGuidID) " +
                                       $"WHERE QFS.FormGuidID = '{FormGuidID}' AND StudentID = '{StudentID}'");
            if (StdFormStartEndDB != null)
            {
                if (StdFormStartEndDB.Rows.Count > 0)
                {
                    if (StdFormStartEndDB.Rows[0]["StudentStartFormAt"].ToString() == "01/01/1900 00:00:00")
                    {
                        //Update data
                        IsStudentStartEndFormDateTimeUpdated = UpdateStudentDataSetting(FormGuidID, StudentID, DateTime.Now);
                    }
                    else
                    {
                        FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["StudentStartFormAt"].ToString());
                        FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["StudentEndFormAt"].ToString());
                    }
                }
            }

            if (IsStudentStartEndFormDateTimeUpdated)
            {
                StdFormStartEndDB =
                    SystemBase.GetDataTble($"SELECT StudentStartFormAt, StudentEndFormAt = (DATEADD(mi, QB.QuestionsFormDurationMinutes, QFS.StudentStartFormAt)) " +
                                           $"FROM QuestionsFormSettings QFS " +
                                           $"INNER JOIN QuestionsBank QB ON " +
                                           $"(QB.FormGuidID = QFS.FormGuidID) " +
                                           $"WHERE QFS.FormGuidID = '{FormGuidID}' AND StudentID = '{StudentID}'");
                if (StdFormStartEndDB != null)
                {
                    if (StdFormStartEndDB.Rows.Count > 0)
                    {
                        FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["StudentStartFormAt"].ToString());
                        FormStartsEndsAt.Add(StdFormStartEndDB.Rows[0]["StudentEndFormAt"].ToString());
                    }
                }
            }

            return FormStartsEndsAt.ToArray();
        }

        private bool UpdateStudentDataSetting(string FormGuidID, string StudentID, DateTime StdStartAtDateTime)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("UPDATE QuestionsFormSettings SET " +
                                 "StudentStartFormAt = @StudentStartFormAt " +
                                 "WHERE FormGuidID = @FormGuidID AND StudentID = @StudentID", con);
            CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
            CMD.Parameters.AddWithValue("@StudentID", StudentID);
            CMD.Parameters.AddWithValue("@StudentStartFormAt", StdStartAtDateTime);
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            return true;
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
            return "0| ";
        }

        private string[] GetQuestionsGuidIDsForQuestionsForm(string FormGuidID)
        {
            List<string> QuestionsGuidIDs = new List<string>();

            DataTable QuestionsInfoDB =
                SystemBase.GetDataTble($"SELECT Q.QuestionGuidID, Q.QuestionTime " +
                                       $"FROM QuestionsBank QB " +
                                       $"INNER JOIN Questions Q ON " +
                                       $"(Q.FormGuidID = QB.FormGuidID) " +
                                       $"WHERE QB.FormGuidID = '{FormGuidID}' AND Q.IsDeleted = 'False' " +
                                       $"ORDER BY NEWID()");
            if (QuestionsInfoDB != null)
            {
                if (QuestionsInfoDB.Rows.Count > 0)
                {
                    hdQuestionsTime.Value = "";
                    foreach (DataRow QuestionGuidIDRow in QuestionsInfoDB.Rows)
                    {
                        QuestionsGuidIDs.Add(QuestionGuidIDRow["QuestionGuidID"].ToString());
                        hdQuestionsTime.Value += QuestionGuidIDRow["QuestionTime"].ToString() + "|";
                    }
                }
                else
                {
                    DivQuestionsDemonstration.Visible = false;
                    ShowMsg(hdclutrueName.Value.Contains("en")
                        ?
                        "There are no questions to demonstration."
                        :
                        "لا يوجد أسئلة لعرضها.", "info");
                }
            }
            else
            {
                DivQuestionsDemonstration.Visible = false;
                ShowMsg(hdclutrueName.Value.Contains("en")
                    ?
                    "There are no questions to demonstration."
                    :
                    "لا يوجد أسئلة لعرضها.", "info");
            }

            return QuestionsGuidIDs.ToArray();
        }

        private string GetQuestionTypeID(string QuestionGuidID)
        {
            DataTable QuestionTypeIDDB =
                SystemBase.GetDataTble($"SELECT QuestionTypeID " +
                                       $"FROM Questions " +
                                       $"WHERE QuestionGuidID = '{QuestionGuidID}'");

            if (QuestionTypeIDDB != null)
            {
                if (QuestionTypeIDDB.Rows.Count > 0)
                {
                    return QuestionTypeIDDB.Rows[0]["QuestionTypeID"].ToString();
                }
            }

            return "0";
        }

        private void ShowQuestionsForm()
        {
            hdAllControls.Value = "";
            string QuestionTypeID;
            string[] QuestionsGuidIDs = hdQuestionsGuidIDs.Value.Split('|');
            int NumberOfQuestion = 0;
            int TotalNumberOfQuestion = int.Parse(hdTotalNumberOfQuestion.Value);
            foreach (string QuestioGuidID in QuestionsGuidIDs)
            {
                if (String.IsNullOrEmpty(QuestioGuidID)) continue;

                NumberOfQuestion += 1;
                switch (hdformstylid.Value)
                {
                    case "1": //All at ones
                        LblQuestionsFormDurationMinutes.Text = hdformdurationmin.Value;
                        SpanQuestionsFormDurationMinutes.Visible = true;
                        SpanQuestionsFormStartsEnds.Visible = true;
                        SpanStudentStartEndFormInfo.Visible = true;
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_AllAtOnesStyle(QuestioGuidID, QuestionTypeID, NumberOfQuestion);
                        break;

                    case "2": //One by one 
                        LblQuestionsFormDurationMinutes.Text = hdformdurationmin.Value;
                        SpanQuestionsFormDurationMinutes.Visible = true;
                        SpanQuestionsFormStartsEnds.Visible = true;
                        SpanStudentStartEndFormInfo.Visible = true;
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_OneByOneStyle(QuestioGuidID, QuestionTypeID, NumberOfQuestion, TotalNumberOfQuestion);
                        break;

                    case "3": //One by one with specific time
                        LblQuestionsFormDurationMinutes.Text = hdformdurationmin.Value;
                        SpanQuestionsFormDurationMinutes.Visible = true;
                        SpanQuestionsFormStartsEnds.Visible = true;
                        SpanStudentStartEndFormInfo.Visible = true;
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_OneByOneStyleWithSpecificQuestionTime(QuestioGuidID, QuestionTypeID, NumberOfQuestion, TotalNumberOfQuestion);
                        break;
                }
            }
        }

        private void AddQuestionToPanel_AllAtOnesStyle(string QuestionGuidID, string QuestionTypeID, int NumberOfQuestion)
        {
            string QuestionText;

            try
            {
                DataTable QuestionInfoDB =
                    SystemBase.GetDataTble($"SELECT * " +
                                           $"FROM Questions " +
                                           $"WHERE QuestionGuidID = '{QuestionGuidID}'");
                if (QuestionInfoDB != null)
                {
                    if (QuestionInfoDB.Rows.Count > 0)
                    {
                        QuestionText = QuestionInfoDB.Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                Panel panel = new Panel();
                panel.ID = $"Pnl_{QuestionGuidID}_{NumberOfQuestion}";
                panel.Attributes["runat"] = "server";

                DivPanelGroup.Controls.Add(panel);

                panel.Controls.Add(new LiteralControl("<div class=\"panel panel-primary\">"));
                panel.Controls.Add(new LiteralControl($"<div class=\"panel-heading\">" +
                                                      $"{Resources.Resource.Question} # {NumberOfQuestion}: {QuestionText.Replace("<p>", "").Replace("</p>", "")} </div>"));

                //Createing the controls for each question type.
                switch (QuestionTypeID)
                {
                    case "1": //مقالي
                        TextBox TxtEssay = new TextBox();
                        TxtEssay.ID = $"TxtEssay_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtEssay.Attributes["runat"] = "server";
                        TxtEssay.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtEssay.Text = String.Empty;
                        TxtEssay.Font.Size = 12;
                        TxtEssay.Font.Bold = true;
                        hdAllControls.Value += TxtEssay.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtEssay);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "2": //الإجابة القصيرة
                        TextBox TxtShortAnswer = new TextBox();
                        TxtShortAnswer.ID = $"TxtShortAnswer_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtShortAnswer.Attributes["runat"] = "server";
                        TxtShortAnswer.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtShortAnswer.Text = String.Empty;
                        TxtShortAnswer.Font.Size = 12;
                        TxtShortAnswer.Font.Bold = false;
                        hdAllControls.Value += TxtShortAnswer.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtShortAnswer);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "3": //خيارات متعددة
                        int NumberOfChoices = int.Parse(QuestionInfoDB.Rows[0]["NumberOfChoicesID"].ToString());
                        RadioButton RadChoice01 = new RadioButton();
                        RadChoice01.ID = $"RadChoice01_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Attributes["runat"] = "server";
                        RadChoice01.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Text = QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString();
                        RadChoice01.Font.Size = 10;
                        RadChoice01.Font.Bold = false;
                        RadChoice01.Checked = false;
                        RadChoice01.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString().Trim()) ? false : true;
                        if (RadChoice01.Visible) hdAllControls.Value += RadChoice01.ID + "|";

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;
                        if (RadChoice02.Visible) hdAllControls.Value += RadChoice02.ID + "|";

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;
                        if (RadChoice03.Visible) hdAllControls.Value += RadChoice03.ID + "|";

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;
                        if (RadChoice04.Visible) hdAllControls.Value += RadChoice04.ID + "|";

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;
                        if (RadChoice05.Visible) hdAllControls.Value += RadChoice05.ID + "|";

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;
                        if (RadChoice06.Visible) hdAllControls.Value += RadChoice06.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice01);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice02);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice03);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice04);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice05);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice06);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));

                        break;

                    case "4": //صواب / خطأ
                        RadioButton RadTrue = new RadioButton();
                        RadTrue.ID = $"RadTrue_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Attributes["runat"] = "server";
                        RadTrue.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Text = Resources.Resource.True;
                        RadTrue.Font.Size = 10;
                        RadTrue.Font.Bold = false;
                        RadTrue.Checked = false;
                        hdAllControls.Value += RadTrue.ID + "|";

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        hdAllControls.Value += RadFalse.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadTrue);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadFalse);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "5": //نعم / لا
                        RadioButton RadYes = new RadioButton();
                        RadYes.ID = $"RadYes_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Attributes["runat"] = "server";
                        RadYes.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Text = Resources.Resource.Yes;
                        RadYes.Font.Size = 10;
                        RadYes.Font.Bold = false;
                        RadYes.Checked = false;
                        hdAllControls.Value += RadYes.ID + "|";

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        hdAllControls.Value += RadNo.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadYes);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadNo);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;
                }

                panel.Controls.Add(new LiteralControl($"<div class=\"panel-footer\">" +
                                                     $"<table>" +
                                                     $"<tr>" +
                                                     $"<td>" +
                                                     $"{Resources.Resource.QuestionGrade}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionGrade"]}</span>" +
                                                     $"</td>" +
                                                     $"</tr>" +
                                                     $"</table>" +
                                                     $"</div>"));
                panel.Controls.Add(new LiteralControl("</div>"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void AddQuestionToPanel_OneByOneStyle(string QuestionGuidID, string QuestionTypeID, int NumberOfQuestion, int TotalNumberOfQuestion)
        {
            string QuestionText;

            try
            {
                DataTable QuestionInfoDB =
                    SystemBase.GetDataTble($"SELECT * " +
                                           $"FROM Questions " +
                                           $"WHERE QuestionGuidID = '{QuestionGuidID}'");
                if (QuestionInfoDB != null)
                {
                    if (QuestionInfoDB.Rows.Count > 0)
                    {
                        QuestionText = QuestionInfoDB.Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                Panel panel = new Panel();
                panel.ID = $"Pnl_{QuestionGuidID}_{NumberOfQuestion}";
                panel.Attributes["runat"] = "server";

                DivPanelGroup.Controls.Add(panel);

                panel.Controls.Add(new LiteralControl("<div class=\"panel panel-primary\">"));
                panel.Controls.Add(new LiteralControl($"<div class=\"panel-heading\">" +
                                                      $"{Resources.Resource.Question} # {NumberOfQuestion}: {QuestionText.Replace("<p>", "").Replace("</p>", "")} </div>"));

                //Createing the controls for each question type.
                switch (QuestionTypeID)
                {
                    case "1": //مقالي
                        TextBox TxtEssay = new TextBox();
                        TxtEssay.ID = $"TxtEssay_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtEssay.Attributes["runat"] = "server";
                        TxtEssay.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtEssay.Text = String.Empty;
                        TxtEssay.Font.Size = 12;
                        TxtEssay.Font.Bold = true;
                        hdAllControls.Value += TxtEssay.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtEssay);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "2": //الإجابة القصيرة
                        TextBox TxtShortAnswer = new TextBox();
                        TxtShortAnswer.ID = $"TxtShortAnswer_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtShortAnswer.Attributes["runat"] = "server";
                        TxtShortAnswer.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtShortAnswer.Text = String.Empty;
                        TxtShortAnswer.Font.Size = 12;
                        TxtShortAnswer.Font.Bold = false;
                        hdAllControls.Value += TxtShortAnswer.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtShortAnswer);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "3": //خيارات متعددة
                        int NumberOfChoices = int.Parse(QuestionInfoDB.Rows[0]["NumberOfChoicesID"].ToString());
                        RadioButton RadChoice01 = new RadioButton();
                        RadChoice01.ID = $"RadChoice01_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Attributes["runat"] = "server";
                        RadChoice01.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Text = QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString();
                        RadChoice01.Font.Size = 10;
                        RadChoice01.Font.Bold = false;
                        RadChoice01.Checked = false;
                        RadChoice01.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString().Trim()) ? false : true;
                        if (RadChoice01.Visible) hdAllControls.Value += RadChoice01.ID + "|";

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;
                        if (RadChoice02.Visible) hdAllControls.Value += RadChoice02.ID + "|";

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;
                        if (RadChoice03.Visible) hdAllControls.Value += RadChoice03.ID + "|";

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;
                        if (RadChoice04.Visible) hdAllControls.Value += RadChoice04.ID + "|";

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;
                        if (RadChoice05.Visible) hdAllControls.Value += RadChoice05.ID + "|";

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;
                        if (RadChoice06.Visible) hdAllControls.Value += RadChoice06.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice01);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice02);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice03);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice04);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice05);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice06);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));

                        break;

                    case "4": //صواب / خطأ
                        RadioButton RadTrue = new RadioButton();
                        RadTrue.ID = $"RadTrue_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Attributes["runat"] = "server";
                        RadTrue.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Text = Resources.Resource.True;
                        RadTrue.Font.Size = 10;
                        RadTrue.Font.Bold = false;
                        RadTrue.Checked = false;
                        hdAllControls.Value += RadTrue.ID + "|";

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        hdAllControls.Value += RadFalse.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadTrue);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadFalse);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "5": //نعم / لا
                        RadioButton RadYes = new RadioButton();
                        RadYes.ID = $"RadYes_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Attributes["runat"] = "server";
                        RadYes.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Text = Resources.Resource.Yes;
                        RadYes.Font.Size = 10;
                        RadYes.Font.Bold = false;
                        RadYes.Checked = false;
                        hdAllControls.Value += RadYes.ID + "|";

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        hdAllControls.Value += RadNo.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadYes);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadNo);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;
                }

                panel.Controls.Add(new LiteralControl("<div class=\"panel-footer\">"));
                panel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                panel.Controls.Add(new LiteralControl("<tr>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:150px; text-align: {Resources.Resource.CSSQuestionTxtsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl($"{Resources.Resource.QuestionGrade}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionGrade"]}</span>"));
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl("<td></td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:50px; text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl("<a href=\"javascript:0\" onclick = BtnPrevClicked('" + $"BtnPrev_{QuestionGuidID}_{NumberOfQuestion}" + "') class=\"btn btn-info\">" + Resources.Resource.Previous + "</a>"));
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:50px; text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl("<a href=\"javascript:0\" onclick = BtnNextClicked('" + $"BtnNext_{QuestionGuidID}_{NumberOfQuestion}" + "') class=\"btn btn-success\">" + Resources.Resource.Next + "</a>"));
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl("</tr>"));
                panel.Controls.Add(new LiteralControl("</table>"));
                panel.Controls.Add(new LiteralControl("</div>"));
                panel.Controls.Add(new LiteralControl("</div>"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void AddQuestionToPanel_OneByOneStyleWithSpecificQuestionTime(string QuestionGuidID, string QuestionTypeID, int NumberOfQuestion, int TotalNumberOfQuestion)
        {
            string QuestionText;

            try
            {
                DataTable QuestionInfoDB =
                    SystemBase.GetDataTble($"SELECT * " +
                                           $"FROM Questions " +
                                           $"WHERE QuestionGuidID = '{QuestionGuidID}'");
                if (QuestionInfoDB != null)
                {
                    if (QuestionInfoDB.Rows.Count > 0)
                    {
                        QuestionText = QuestionInfoDB.Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                Panel panel = new Panel();
                panel.ID = $"Pnl_{QuestionGuidID}_{NumberOfQuestion}";
                panel.Attributes["runat"] = "server";

                DivPanelGroup.Controls.Add(panel);

                panel.Controls.Add(new LiteralControl("<div class=\"panel panel-primary\">"));
                panel.Controls.Add(new LiteralControl($"<div class=\"panel-heading\">" +
                                                      $"{Resources.Resource.Question} # {NumberOfQuestion}: {QuestionText.Replace("<p>", "").Replace("</p>", "")} </div>"));

                //Createing the controls for each question type.
                switch (QuestionTypeID)
                {
                    case "1": //مقالي
                        TextBox TxtEssay = new TextBox();
                        TxtEssay.ID = $"TxtEssay_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtEssay.Attributes["runat"] = "server";
                        TxtEssay.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtEssay.Text = String.Empty;
                        TxtEssay.Font.Size = 12;
                        TxtEssay.Font.Bold = true;
                        hdAllControls.Value += TxtEssay.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtEssay);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "2": //الإجابة القصيرة
                        TextBox TxtShortAnswer = new TextBox();
                        TxtShortAnswer.ID = $"TxtShortAnswer_{QuestionGuidID}_{NumberOfQuestion}";
                        TxtShortAnswer.Attributes["runat"] = "server";
                        TxtShortAnswer.Attributes["class"] = "form-control summernote";
                        //TxtEssay.TextMode = TextBoxMode.MultiLine;
                        TxtShortAnswer.Text = String.Empty;
                        TxtShortAnswer.Font.Size = 12;
                        TxtShortAnswer.Font.Bold = false;
                        hdAllControls.Value += TxtShortAnswer.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<label style=\"font-size: 12px;\">" + Resources.Resource.Answer + "</label>"));
                        panel.Controls.Add(TxtShortAnswer);
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "3": //خيارات متعددة
                        int NumberOfChoices = int.Parse(QuestionInfoDB.Rows[0]["NumberOfChoicesID"].ToString());
                        RadioButton RadChoice01 = new RadioButton();
                        RadChoice01.ID = $"RadChoice01_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Attributes["runat"] = "server";
                        RadChoice01.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice01.Text = QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString();
                        RadChoice01.Font.Size = 10;
                        RadChoice01.Font.Bold = false;
                        RadChoice01.Checked = false;
                        RadChoice01.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberOne"].ToString().Trim()) ? false : true;
                        if (RadChoice01.Visible) hdAllControls.Value += RadChoice01.ID + "|";

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;
                        if (RadChoice02.Visible) hdAllControls.Value += RadChoice02.ID + "|";

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;
                        if (RadChoice03.Visible) hdAllControls.Value += RadChoice03.ID + "|";

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;
                        if (RadChoice04.Visible) hdAllControls.Value += RadChoice04.ID + "|";

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;
                        if (RadChoice05.Visible) hdAllControls.Value += RadChoice05.ID + "|";

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;
                        if (RadChoice06.Visible) hdAllControls.Value += RadChoice06.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice01);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice02);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice03);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice04);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice05);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadChoice06);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));

                        break;

                    case "4": //صواب / خطأ
                        RadioButton RadTrue = new RadioButton();
                        RadTrue.ID = $"RadTrue_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Attributes["runat"] = "server";
                        RadTrue.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadTrue.Text = Resources.Resource.True;
                        RadTrue.Font.Size = 10;
                        RadTrue.Font.Bold = false;
                        RadTrue.Checked = false;
                        hdAllControls.Value += RadTrue.ID + "|";

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        hdAllControls.Value += RadFalse.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadTrue);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadFalse);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;

                    case "5": //نعم / لا
                        RadioButton RadYes = new RadioButton();
                        RadYes.ID = $"RadYes_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Attributes["runat"] = "server";
                        RadYes.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadYes.Text = Resources.Resource.Yes;
                        RadYes.Font.Size = 10;
                        RadYes.Font.Bold = false;
                        RadYes.Checked = false;
                        hdAllControls.Value += RadYes.ID + "|";

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        hdAllControls.Value += RadNo.ID + "|";

                        panel.Controls.Add(new LiteralControl("<div class=\"panel-body\">"));
                        panel.Controls.Add(new LiteralControl("<table>"));
                        panel.Controls.Add(new LiteralControl("<tr>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadYes);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                        panel.Controls.Add(new LiteralControl("<td>"));
                        panel.Controls.Add(RadNo);
                        panel.Controls.Add(new LiteralControl("</td>"));
                        panel.Controls.Add(new LiteralControl("</tr>"));
                        panel.Controls.Add(new LiteralControl("</table>"));
                        panel.Controls.Add(new LiteralControl("</div>"));
                        break;
                }

                panel.Controls.Add(new LiteralControl("<div class=\"panel-footer\">"));
                panel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.QuestionGrade}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionGrade"]}</span></td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                //hdQuestionsTime.Value
                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.QuestionTime}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionTime"]}</span> {Resources.Resource.Minutes}</td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                //panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.TimeRemainingForTheQuestion}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionTime"]}</span> {Resources.Resource.Minutes}</td>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.TimeRemainingForTheQuestion}: <span style=\"color: orangered; font-size:small;\"><label id=\"LblTimeRemaining_{QuestionGuidID}_{NumberOfQuestion}\"></label></span> {Resources.Resource.Minutes}</td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td style=\"text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl("<a href=\"javascript:0\" onclick = BtnNextClicked('" + $"BtnNext_{QuestionGuidID}_{NumberOfQuestion}" + "') class=\"btn btn-success\">" + Resources.Resource.Next + "</a>"));
                panel.Controls.Add(new LiteralControl("</td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("</table>"));
                panel.Controls.Add(new LiteralControl("</div>"));
                panel.Controls.Add(new LiteralControl("</div>"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
    }
}