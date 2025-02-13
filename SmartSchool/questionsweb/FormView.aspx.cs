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
    public partial class FormView : System.Web.UI.Page
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
                    hdformstylid.Value = QuestionsFormInfo[0];
                    LblQuestionsFormTitle.Text = QuestionsFormInfo[1];
                    hdformdurationmin.Value = QuestionsFormInfo[2]; //QuestionsFormDurationMinutes
                    SpanQuestionsFormDurationMinutes.Visible = false;
                    SetCulture();

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
            if (hdformstylid.Value != "1")
            {
                ShowHideQuestions();
            }
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
            BtnDone.Text = Resources.Resource.Done;
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
            return "0| ";
        }

        private string[] GetQuestionsGuidIDsForQuestionsForm(string FormGuidID)
        {
            List<string> QuestionsGuidIDs = new List<string>();

            DataTable QuestionsInfoDB =
                SystemBase.GetDataTble($"SELECT Q.QuestionGuidID " +
                                       $"FROM QuestionsBank QB " +
                                       $"INNER JOIN Questions Q ON " +
                                       $"(Q.FormGuidID = QB.FormGuidID) " +
                                       $"WHERE QB.FormGuidID = '{FormGuidID}' AND Q.IsDeleted = 'False' " +
                                       $"ORDER BY NEWID()");
            if (QuestionsInfoDB != null)
            {
                if (QuestionsInfoDB.Rows.Count > 0)
                {
                    foreach (DataRow QuestionGuidIDRow in QuestionsInfoDB.Rows)
                    {
                        QuestionsGuidIDs.Add(QuestionGuidIDRow["QuestionGuidID"].ToString());
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
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_AllAtOnesStyle(QuestioGuidID, QuestionTypeID, NumberOfQuestion);
                        break;

                    case "2": //One by one 
                        LblQuestionsFormDurationMinutes.Text = hdformdurationmin.Value;
                        SpanQuestionsFormDurationMinutes.Visible = true;
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_OneByOneStyle(QuestioGuidID, QuestionTypeID, NumberOfQuestion, TotalNumberOfQuestion);
                        break;

                    case "3": //One by one with specific time
                        LblQuestionsFormDurationMinutes.Text = hdformdurationmin.Value;
                        SpanQuestionsFormDurationMinutes.Visible = true;
                        QuestionTypeID = GetQuestionTypeID(QuestioGuidID);
                        AddQuestionToPanel_OneByOneStyleWithSpecificQuestionTime(QuestioGuidID, QuestionTypeID, NumberOfQuestion, TotalNumberOfQuestion);
                        break;
                }
            }
        }

        private void ShowHideQuestions()
        {
            //Pnl.Attributes.Add("style", "display: none;");

            if (!IsPostBack)
            {
                string[] QuestionsGuidIDs = hdQuestionsGuidIDs.Value.Split('|');
                int NumberOfQuestion = 0;
                foreach (string QuestioGuidID in QuestionsGuidIDs)
                {
                    if (String.IsNullOrEmpty(QuestioGuidID)) continue;

                    NumberOfQuestion += 1;
                    Panel Pnl = new Panel();
                    Pnl = (Panel)DivPanelGroup.FindControl(String.Concat("Pnl", "_", QuestioGuidID, "_", NumberOfQuestion));

                    if (NumberOfQuestion == 1)
                    {
                        Pnl.Visible = true;
                        hdCurrQuesToShow.Value = NumberOfQuestion.ToString();
                    }
                    else
                    {
                        Pnl.Visible = false;
                    }
                }
            }
            else
            {
                int NextQuesNo = int.Parse(hdCurrQuesToShow.Value);
                string[] QuestionsGuidIDs = hdQuestionsGuidIDs.Value.Split('|');
                int TotalNumberOfQuestion = int.Parse(hdTotalNumberOfQuestion.Value);
                if (NextQuesNo > TotalNumberOfQuestion)
                {
                    hdCurrQuesToShow.Value = TotalNumberOfQuestion.ToString();
                    NextQuesNo = int.Parse(hdCurrQuesToShow.Value);
                    ShowMsg(hdclutrueName.Value.Contains("en")
                        ?
                        "You have reached the end of the questions"
                        :
                        "لقد وصلت إلى نهاية الأسئلة", "info");
                    //return;
                }
                if (NextQuesNo <= 0)
                {
                    hdCurrQuesToShow.Value = "1";
                    NextQuesNo = int.Parse(hdCurrQuesToShow.Value);
                    ShowMsg(hdclutrueName.Value.Contains("en")
                        ?
                        "You have reached the start of the questions"
                        :
                        "لقد وصلت إلى بداية الأسئلة", "info");
                    //return;
                }

                int NumberOfQuestion = 0;
                foreach (string QuestioGuidID in QuestionsGuidIDs)
                {
                    if (String.IsNullOrEmpty(QuestioGuidID)) continue;
                    NumberOfQuestion += 1;
                    Panel Pnl = new Panel();
                    Pnl = (Panel)DivPanelGroup.FindControl(String.Concat("Pnl", "_", QuestioGuidID, "_", NumberOfQuestion));
                    Pnl.Visible = false;

                    if (NumberOfQuestion == NextQuesNo) Pnl.Visible = true;
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

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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

                Button BtnNext = new Button();
                BtnNext.ID = $"BtnNext_{QuestionGuidID}_{NumberOfQuestion}";
                //BtnNext.Attributes["runat"] = "server";
                BtnNext.Attributes["class"] = "btn btn-success";
                BtnNext.Text = Resources.Resource.Next;
                BtnNext.Click += new EventHandler(BtnNext_Click);

                Button BtnPrev = new Button();
                BtnPrev.ID = $"BtnPrev_{QuestionGuidID}_{NumberOfQuestion}";
                //BtnPrev.Attributes["runat"] = "server";
                BtnPrev.Attributes["class"] = "btn btn-info";
                BtnPrev.Text = Resources.Resource.Previous;
                BtnPrev.Click += new EventHandler(BtnPrev_Click);

                panel.Controls.Add(new LiteralControl("<div class=\"panel-footer\">"));
                panel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                panel.Controls.Add(new LiteralControl("<tr>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:150px; text-align: {Resources.Resource.CSSQuestionTxtsRightLeft};\">"));
                panel.Controls.Add(new LiteralControl($"{Resources.Resource.QuestionGrade}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionGrade"]}</span>"));
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl("<td></td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:50px; text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(BtnPrev);
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"width:50px; text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(BtnNext);
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

                        RadioButton RadChoice02 = new RadioButton();
                        RadChoice02.ID = $"RadChoice02_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Attributes["runat"] = "server";
                        RadChoice02.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice02.Text = QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        RadChoice02.Font.Size = 10;
                        RadChoice02.Font.Bold = false;
                        RadChoice02.Checked = false;
                        RadChoice02.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberTwo"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice03 = new RadioButton();
                        RadChoice03.ID = $"RadChoice03_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Attributes["runat"] = "server";
                        RadChoice03.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice03.Text = QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString();
                        RadChoice03.Font.Size = 10;
                        RadChoice03.Font.Bold = false;
                        RadChoice03.Checked = false;
                        RadChoice03.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberThree"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice04 = new RadioButton();
                        RadChoice04.ID = $"RadChoice04_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Attributes["runat"] = "server";
                        RadChoice04.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice04.Text = QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString();
                        RadChoice04.Font.Size = 10;
                        RadChoice04.Font.Bold = false;
                        RadChoice04.Checked = false;
                        RadChoice04.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFour"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice05 = new RadioButton();
                        RadChoice05.ID = $"RadChoice05_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Attributes["runat"] = "server";
                        RadChoice05.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice05.Text = QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString();
                        RadChoice05.Font.Size = 10;
                        RadChoice05.Font.Bold = false;
                        RadChoice05.Checked = false;
                        RadChoice05.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberFive"].ToString().Trim()) ? false : true;

                        RadioButton RadChoice06 = new RadioButton();
                        RadChoice06.ID = $"RadChoice06_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Attributes["runat"] = "server";
                        RadChoice06.GroupName = $"MutliChoice_{QuestionGuidID}_{NumberOfQuestion}";
                        RadChoice06.Text = QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString();
                        RadChoice06.Font.Size = 10;
                        RadChoice06.Font.Bold = false;
                        RadChoice06.Checked = false;
                        RadChoice06.Visible = String.IsNullOrEmpty(QuestionInfoDB.Rows[0]["AnswerNumberSix"].ToString().Trim()) ? false : true;

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadFalse = new RadioButton();
                        RadFalse.ID = $"RadFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Attributes["runat"] = "server";
                        RadFalse.GroupName = $"TrueFalse_{QuestionGuidID}_{NumberOfQuestion}";
                        RadFalse.Text = Resources.Resource.False;
                        RadFalse.Font.Size = 10;
                        RadFalse.Font.Bold = false;
                        RadFalse.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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
                        //RadTrue.Checked += new EventHandler(Radio_Checked);

                        RadioButton RadNo = new RadioButton();
                        RadNo.ID = $"RadNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Attributes["runat"] = "server";
                        RadNo.GroupName = $"YesNo_{QuestionGuidID}_{NumberOfQuestion}";
                        RadNo.Text = Resources.Resource.No;
                        RadNo.Font.Size = 10;
                        RadNo.Font.Bold = false;
                        RadNo.Checked = false;
                        //RadFalse.Checked += new EventHandler(Radio_Checked);

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

                Button BtnNext = new Button();
                BtnNext.ID = $"BtnNext_{QuestionGuidID}_{NumberOfQuestion}";
                //BtnNext.Attributes["runat"] = "server";
                BtnNext.Attributes["class"] = "btn btn-success";
                BtnNext.Text = Resources.Resource.Next;
                BtnNext.Click += new EventHandler(BtnNext_Click);

                Button BtnPrev = new Button();
                BtnPrev.ID = $"BtnPrev_{QuestionGuidID}_{NumberOfQuestion}";
                //BtnPrev.Attributes["runat"] = "server";
                BtnPrev.Attributes["class"] = "btn btn-info";
                BtnPrev.Text = Resources.Resource.Previous;
                BtnPrev.Click += new EventHandler(BtnPrev_Click);
                BtnPrev.Visible = false;

                panel.Controls.Add(new LiteralControl("<div class=\"panel-footer\">"));
                panel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.QuestionGrade}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionGrade"]}</span></td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.QuestionTime}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionTime"]}</span> {Resources.Resource.Minutes}</td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td>{Resources.Resource.TimeRemainingForTheQuestion}: <span style=\"color: orangered;\">{QuestionInfoDB.Rows[0]["QuestionTime"]}</span> {Resources.Resource.Minutes}</td>"));
                panel.Controls.Add(new LiteralControl("<td></td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("<tr>"));
                panel.Controls.Add(new LiteralControl($"<td style=\"text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(BtnPrev);
                panel.Controls.Add(new LiteralControl("</td>"));

                panel.Controls.Add(new LiteralControl($"<td style=\"text-align: {Resources.Resource.CSSQuestionBtnsRightLeft};\">"));
                panel.Controls.Add(BtnNext);
                panel.Controls.Add(new LiteralControl("</td>"));
                panel.Controls.Add(new LiteralControl("</tr>"));

                panel.Controls.Add(new LiteralControl("</table>"));

                //panel.Controls.Add(new LiteralControl("<table style=\"width: 100%;\">"));
                //panel.Controls.Add(new LiteralControl("<tr>"));
                //panel.Controls.Add(new LiteralControl("<td style=\"width:250px; text-align: left;\">"));
                //panel.Controls.Add(new LiteralControl($"{Resources.Resource.QuestionGrade}: {QuestionInfoDB.Rows[0]["QuestionGrade"]}"));
                //panel.Controls.Add(new LiteralControl("</td>"));

                //panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));
                //panel.Controls.Add(new LiteralControl($"<td style=\"width:250px; text-align: left;\">{Resources.Resource.QuestionTime}: {QuestionInfoDB.Rows[0]["QuestionTime"]} {Resources.Resource.Minutes}</td>"));
                //panel.Controls.Add(new LiteralControl("<td style=\"width:30px;\"></td>"));

                //panel.Controls.Add(new LiteralControl($"<td style=\"width:300px; text-align: left;\">{Resources.Resource.TimeRemainingForTheQuestion}: {QuestionInfoDB.Rows[0]["QuestionTime"]} {Resources.Resource.Minutes}</td>"));

                //panel.Controls.Add(new LiteralControl("<td></td>"));

                //panel.Controls.Add(new LiteralControl("<td style=\"width:50px; text-align: right;\">"));
                //panel.Controls.Add(BtnPrev);
                //panel.Controls.Add(new LiteralControl("</td>"));

                //panel.Controls.Add(new LiteralControl("<td style=\"width:50px; text-align: right;\">"));
                //panel.Controls.Add(BtnNext);
                //panel.Controls.Add(new LiteralControl("</td>"));

                //panel.Controls.Add(new LiteralControl("</tr>"));
                //panel.Controls.Add(new LiteralControl("</table>"));
                panel.Controls.Add(new LiteralControl("</div>"));
                panel.Controls.Add(new LiteralControl("</div>"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        protected void BtnNext_Click(object sender, EventArgs e)
        {
            Button _Button = (Button)sender;
            string BtnID = _Button.ID.ToString();
            if (_Button == null) return;

            hdCurrQuesToShow.Value = (int.Parse(hdCurrQuesToShow.Value) + 1).ToString();

            ShowHideQuestions();
        }

        protected void BtnPrev_Click(object sender, EventArgs e)
        {
            Button _Button = (Button)sender;
            string BtnID = _Button.ID.ToString();
            if (_Button == null) return;

            hdCurrQuesToShow.Value = (int.Parse(hdCurrQuesToShow.Value) - 1).ToString();

            ShowHideQuestions();
        }

        protected void BtnDone_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormsView.aspx", true);
        }
    }
}