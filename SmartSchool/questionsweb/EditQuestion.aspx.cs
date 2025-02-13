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
    public partial class EditQuestion : System.Web.UI.Page
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
                        if (String.IsNullOrEmpty(Request.QueryString["FGID"]) || String.IsNullOrEmpty(Request.QueryString["QGID"]))
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
                    string QuestionGuidID = Request.QueryString["QGID"];
                    Hdqgid.Value = QuestionGuidID;

                    SetCulture();
                    SetLookups();
                    SetASPElementsTxt();
                    if (RetriveQuestionInfo() == "IsApprovedTrue")
                    {
                        BtnSave.Attributes.Add("disabled", "disabled");
                        LblMsg.Text =
                            hdclutrueName.Value.Contains("en")
                            ?
                            "You cannot modify the question data because it has been approved"
                            :
                            "لا تستطيع تعديل بيانات السؤال كونه تم اعتماده";

                        ShowMsg(hdclutrueName.Value.Contains("en")
                            ?
                            "You cannot modify the question data because it has been approved"
                            :
                            "لا تستطيع تعديل بيانات السؤال كونه تم اعتماده", "info");
                        return;
                    }
                    DivQuestionTime.Visible = ShowQuestionTimeTextDiv(QuestionGuidID);

                    HdIsThereStudentsAnsweredTheQuestion.Value = "False";
                    if (IsThereStudentsAnsweredTheQuestion(QuestionGuidID))
                    {
                        HdIsThereStudentsAnsweredTheQuestion.Value = "True";
                        DrpQuestionType.Attributes.Add("disabled", "disabled");
                        if (DivQuestionTime.Visible) TxtQuestionTime.Attributes.Add("disabled", "disabled");
                    }
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
            string QuestionTypeName = "QuestionTypeArabicText";
            string AnswersText = "AnswersTextAr";

            if (hdclutrueName.Value.Contains("en"))
            {
                QuestionTypeName = "QuestionTypeEnglishText";
                AnswersText = "AnswersTextEn";
            }

            DrpQuestionType.Items.Clear();
            DrpQuestionType.DataSource =
                SystemBase.GetDataTble($"SELECT QuestionTypeID, QuestionTypeName = {QuestionTypeName} " +
                                       $"FROM QuestionTypes " +
                                       $"WHERE QuestionTypeID <= 5 " +
                                       $"ORDER BY QuestionTypeID ASC");
            DrpQuestionType.DataValueField = "QuestionTypeID";
            DrpQuestionType.DataTextField = "QuestionTypeName";
            DrpQuestionType.DataBind();
            DrpQuestionType_IndxChange(null, null);

            DrpTrueFalse.Items.Clear();
            DrpTrueFalse.Items.Clear();
            DrpTrueFalse.DataSource =
                SystemBase.GetDataTble($"SELECT AnswersTextID, AnswersText = {AnswersText} " +
                                       $"FROM QuestionAnswersTrueFalseText " +
                                       $"ORDER BY AnswersTextID ASC");
            DrpTrueFalse.DataValueField = "AnswersTextID";
            DrpTrueFalse.DataTextField = "AnswersText";
            DrpTrueFalse.DataBind();

            DrpYesNo.Items.Clear();
            DrpYesNo.DataSource =
                SystemBase.GetDataTble($"SELECT AnswersTextID, AnswersText = {AnswersText} " +
                                       $"FROM QuestionAnswersTrueFalseText " +
                                       $"ORDER BY AnswersTextID ASC");
            DrpYesNo.DataValueField = "AnswersTextID";
            DrpYesNo.DataTextField = "AnswersText";
            DrpYesNo.DataBind();

            DrpNumberofChoices.Items.Clear();
            DrpNumberofChoices.DataSource =
                SystemBase.GetDataTble($"SELECT AnswersTextID, AnswersText = {AnswersText} " +
                                       $"FROM QuestionAnswersMultipleChoicesText " +
                                       $"ORDER BY AnswersTextID ASC");
            DrpNumberofChoices.DataValueField = "AnswersTextID";
            DrpNumberofChoices.DataTextField = "AnswersText";
            DrpNumberofChoices.DataBind();
            DrpNumberofChoices_IndxChange(null, null);
        }

        private void SetASPElementsTxt()
        {
            try
            {
                LblQuesFrmTit.Text =
                    SystemBase.GetDataTble($"SELECT QB.QuestionsFormTitle " +
                                           $"FROM Questions Q " +
                                           $"INNER JOIN QuestionsBank QB ON " +
                                           $"(QB.FormGuidID = Q.FormGuidID) " +
                                           $"WHERE QuestionGuidID = '{Hdqgid.Value}'").Rows[0]["QuestionsFormTitle"].ToString();
            }
            catch (Exception exp) { Console.WriteLine(exp); SystemBase.Logout(); }

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

        protected void DrpQuestionType_IndxChange(object sender, EventArgs e)
        {
            TxtMultiChoicesAnswer01.Text = TxtMultiChoicesAnswer02.Text = TxtMultiChoicesAnswer03.Text =
            TxtMultiChoicesAnswer04.Text = TxtMultiChoicesAnswer05.Text = TxtMultiChoicesAnswer06.Text = "";

            TxtAnswerText.Text = "";
            DivAnswerText.Visible = false;
            DivMultiChoices.Visible = false;
            DivMultiChoicesAnswers.Visible = false;
            DivTrueFalse.Visible = false;
            DivYesNo.Visible = false;

            switch (DrpQuestionType.SelectedValue.Trim())
            {
                case "3": //Multi Choice
                    DivMultiChoices.Visible = true;
                    DivMultiChoicesAnswers.Visible = true;
                    break;

                case "4": //True False
                    DivTrueFalse.Visible = true;
                    break;

                case "5": //Yes No
                    DivYesNo.Visible = true;
                    break;

                default:
                    DivAnswerText.Visible = true;
                    break;
            }
        }

        protected void DrpNumberofChoices_IndxChange(object sender, EventArgs e)
        {
            DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = DivMultiChoicesAnswer03.Visible =
            DivMultiChoicesAnswer04.Visible = DivMultiChoicesAnswer05.Visible = DivMultiChoicesAnswer06.Visible = false;
            switch (DrpNumberofChoices.SelectedValue.Trim())
            {
                case "1":
                    TxtMultiChoicesAnswer03.Text = TxtMultiChoicesAnswer04.Text = TxtMultiChoicesAnswer05.Text = TxtMultiChoicesAnswer06.Text = "";

                    DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = true;
                    break;

                case "2":
                    TxtMultiChoicesAnswer04.Text = TxtMultiChoicesAnswer05.Text = TxtMultiChoicesAnswer06.Text = "";

                    DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = DivMultiChoicesAnswer03.Visible = true;
                    break;

                case "3":
                    TxtMultiChoicesAnswer05.Text = TxtMultiChoicesAnswer06.Text = "";

                    DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = DivMultiChoicesAnswer03.Visible =
                    DivMultiChoicesAnswer04.Visible = true;
                    break;

                case "4":
                    TxtMultiChoicesAnswer06.Text = "";

                    DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = DivMultiChoicesAnswer03.Visible =
                    DivMultiChoicesAnswer04.Visible = DivMultiChoicesAnswer05.Visible = true;
                    break;

                case "5":
                    DivMultiChoicesAnswer01.Visible = DivMultiChoicesAnswer02.Visible = DivMultiChoicesAnswer03.Visible =
                    DivMultiChoicesAnswer04.Visible = DivMultiChoicesAnswer05.Visible = DivMultiChoicesAnswer06.Visible = true;
                    break;
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect($"ViewQuestions.aspx?FGID={hdfgid.Value}", true);
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string CheckInputs_res = CheckInputs();
            if (CheckInputs_res != "Success")
            {
                ShowMsg(CheckInputs_res, "error");
                return;
            }

            string QuestionGuidID = Hdqgid.Value;
            int QuestionTypeID = int.Parse(DrpQuestionType.SelectedValue.Trim());
            double QuestionGrade = double.Parse(TxtQuestionGrade.Text.Trim());
            string QuestionText = TxtQuestionText.Text.Trim();
            string AnswerText = TxtAnswerText.Text.Trim();
            int NumberOfChoicesID = int.Parse(DrpNumberofChoices.SelectedValue.Trim());
            string AnswerNumberOne = TxtMultiChoicesAnswer01.Text.Trim();
            string AnswerNumberTwo = TxtMultiChoicesAnswer02.Text.Trim();
            string AnswerNumberThree = TxtMultiChoicesAnswer03.Text.Trim();
            string AnswerNumberFour = TxtMultiChoicesAnswer04.Text.Trim();
            string AnswerNumberFive = TxtMultiChoicesAnswer05.Text.Trim();
            string AnswerNumberSix = TxtMultiChoicesAnswer06.Text.Trim();
            int TheCorrectAnswer = int.Parse(TxtTheCorrectAnswer.Text.Trim());
            string TheCorrectAnswerTrueFals = DrpTrueFalse.SelectedValue.Trim();
            string TheCorrectAnswerYesNo = DrpYesNo.SelectedValue.Trim();
            int QuestionTime = int.Parse(TxtQuestionTime.Text.Trim());

            //Update the question information to questions table.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("UPDATE Questions SET " +
                                 "QuestionTypeID = @QuestionTypeID, " +
                                 "QuestionGrade = @QuestionGrade, " +
                                 "QuestionText = @QuestionText, " +
                                 "AnswerText = @AnswerText, " +
                                 "NumberOfChoicesID = @NumberOfChoicesID, " +
                                 "AnswerNumberOne = @AnswerNumberOne, " +
                                 "AnswerNumberTwo = @AnswerNumberTwo, " +
                                 "AnswerNumberThree = @AnswerNumberThree, " +
                                 "AnswerNumberFour = @AnswerNumberFour, " +
                                 "AnswerNumberFive = @AnswerNumberFive, " +
                                 "AnswerNumberSix = @AnswerNumberSix, " +
                                 "TheCorrectAnswer = @TheCorrectAnswer, " +
                                 "TheCorrectAnswerTrueFals = @TheCorrectAnswerTrueFals, " +
                                 "TheCorrectAnswerYesNo = @TheCorrectAnswerYesNo, " +
                                 "QuestionTime = @QuestionTime " +
                                 "WHERE QuestionGuidID = @QuestionGuidID", con);
            CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
            CMD.Parameters.AddWithValue("@QuestionTypeID", QuestionTypeID);
            CMD.Parameters.AddWithValue("@QuestionGrade", QuestionGrade);
            CMD.Parameters.AddWithValue("@QuestionText", QuestionText);
            CMD.Parameters.AddWithValue("@AnswerText", AnswerText);
            CMD.Parameters.AddWithValue("@NumberOfChoicesID", NumberOfChoicesID);
            CMD.Parameters.AddWithValue("@AnswerNumberOne", AnswerNumberOne);
            CMD.Parameters.AddWithValue("@AnswerNumberTwo", AnswerNumberTwo);
            CMD.Parameters.AddWithValue("@AnswerNumberThree", AnswerNumberThree);
            CMD.Parameters.AddWithValue("@AnswerNumberFour", AnswerNumberFour);
            CMD.Parameters.AddWithValue("@AnswerNumberFive", AnswerNumberFive);
            CMD.Parameters.AddWithValue("@AnswerNumberSix", AnswerNumberSix);
            CMD.Parameters.AddWithValue("@TheCorrectAnswer", TheCorrectAnswer);
            CMD.Parameters.AddWithValue("@TheCorrectAnswerTrueFals", TheCorrectAnswerTrueFals);
            CMD.Parameters.AddWithValue("@TheCorrectAnswerYesNo", TheCorrectAnswerYesNo);
            CMD.Parameters.AddWithValue("@QuestionTime", QuestionTime);
            CMD.ExecuteNonQuery();

            //Update QuestionStudentsDeservedGrade if there is an students answered
            if (HdIsThereStudentsAnsweredTheQuestion.Value == "True")
            {
                CMD = new SqlCommand("UPDATE QuestionStudentsDeservedGrade SET " +
                                     "QuestionGrade = @QuestionGrade, " +
                                     "QuestionDeservedGrade = @QuestionDeservedGrade " +
                                     "WHERE QuestionGuidID = @QuestionGuidID", con);
                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                CMD.Parameters.AddWithValue("@QuestionGrade", QuestionGrade);
                CMD.Parameters.AddWithValue("@QuestionDeservedGrade", 0);
                CMD.ExecuteNonQuery();
            }

            //Update QuestionBank set form Duration = Sum of questions time.
            if (DivQuestionTime.Visible)
            {
                DataTable QuestionsFormDurationMinutesDB =
                SystemBase.GetDataTble($"SELECT QuestionsFormDurationMinutes = SUM(QuestionTime) " +
                                       $"FROM Questions " +
                                       $"WHERE FormGuidID = '{hdfgid.Value}'");
                if (QuestionsFormDurationMinutesDB != null)
                {
                    if (QuestionsFormDurationMinutesDB.Rows.Count > 0)
                    {
                        int QuestionsFormDurationMinutes = int.Parse(QuestionsFormDurationMinutesDB.Rows[0]["QuestionsFormDurationMinutes"].ToString());
                        CMD = new SqlCommand("UPDATE QuestionsBank SET " +
                                             "QuestionsFormDurationMinutes = @QuestionsFormDurationMinutes " +
                                             "WHERE FormGuidID = @FormGuidID", con);
                        CMD.Parameters.AddWithValue("@FormGuidID", hdfgid.Value);
                        CMD.Parameters.AddWithValue("@QuestionsFormDurationMinutes", QuestionsFormDurationMinutes);
                        CMD.ExecuteNonQuery();
                    }
                }
            }

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            if (HdIsThereStudentsAnsweredTheQuestion.Value == "True")
            {
                ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "The data has been edited. Note: This question has been answered by students, you must correct the question again. Please wait while you are being redirected."
                :
                "تم تعديل البيانات. ملاحظة: تم الاجابة على هذا السؤال من قبل الطلاب، عليك اعادة تصحيح السؤال مرة أخرى. يرجى الانتظار أثناء إعادة توجيهك.",
                "success");
            }
            else
            {
                ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "The data has been edited. Please wait while you are being redirected."
                :
                "تم نعديل البيانات. يرجى الانتظار حتى يتم إعادة توجيهك.",
                "success");
            }

            Response.AddHeader("REFRESH", $"2;URL=ViewQuestions.aspx?FGID={hdfgid.Value}");
        }

        private string CheckInputs()
        {
            string Msg = "Success";

            if (DrpQuestionType.Items.Count == 0)
            {
                return hdclutrueName.Value.Contains("en") ? "Please select at least one question type" : "يرجى تحديد نوع سؤال واحد على الأقل";
            }

            if (String.IsNullOrEmpty(TxtQuestionGrade.Text))
            {
                return hdclutrueName.Value.Contains("en") ? "Please check the question grade is not empty" : "يرجى التأكد من أن درجة السؤال ليست فارغة";
            }
            if (!SystemBase.RegIsMatch(TxtQuestionGrade.Text, "NUMBER-OR-DECIMAL"))
            {
                return hdclutrueName.Value.Contains("en") ? "The question grade is invalid" : "درجة السؤال غير صالحة";
            }
            if (double.Parse(TxtQuestionGrade.Text) <= 0)
            {
                return hdclutrueName.Value.Contains("en") ? "The question grade must be greater than zero" : "يجب أن تكون درجة السؤال أكبر من الصفر";
            }

            if (DivQuestionTime.Visible)
            {
                if (String.IsNullOrEmpty(TxtQuestionTime.Text))
                {
                    return hdclutrueName.Value.Contains("en") ? "Please check the question time is not empty" : "يرجى التأكد من أن وقت السؤال ليس فارغاً";
                }
                if (!SystemBase.RegIsMatch(TxtQuestionTime.Text.Trim(), "NUMBER"))
                {
                    return hdclutrueName.Value.Contains("en") ? "The question time is invalid" : "وقت السؤال غير صحيح";
                }
                if (int.Parse(TxtQuestionTime.Text.Trim()) <= 0)
                {
                    return hdclutrueName.Value.Contains("en") ? "The question time must be greater than zero" : "يجب أن يكون وقت السؤال أكبر من الصفر";
                }
            }

            if (String.IsNullOrEmpty(TxtQuestionText.Text)) //Question Text
            {
                return hdclutrueName.Value.Contains("en") ? "Please check the question text is not empty" : "يرجى التأكد من أن نص السؤال ليس فارغاً";
            }

            switch (DrpQuestionType.SelectedValue)
            {
                case "3": //Multiple Choices
                    if (DrpNumberofChoices.Items.Count == 0)
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please select at least one of number of choices" : "يرجى تحديد واحد على الأقل من عدد من الاختيارات";
                    }

                    if (String.IsNullOrEmpty(TxtTheCorrectAnswer.Text))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please check the correct answer is not empty" : "يرجى التأكد من أن الإجابة الصحيحة ليست فارغة";
                    }
                    if (!SystemBase.RegIsMatch(TxtTheCorrectAnswer.Text.Trim(), "NUMBER"))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please check the correct answer is number" : "يرجى التحقق من الإجابة الصحيحة هي الرقم";
                    }
                    if (int.Parse(TxtTheCorrectAnswer.Text) < 1 || int.Parse(TxtTheCorrectAnswer.Text) > int.Parse(DrpNumberofChoices.SelectedValue) + 1)
                    {
                        return hdclutrueName.Value.Contains("en") ? "The correct answer is incorrect" : "الإجابة الصحيحة غير صحيحة";
                    }

                    switch (DrpNumberofChoices.SelectedValue)
                    {
                        case "1":
                            if (String.IsNullOrEmpty(TxtMultiChoicesAnswer01.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer02.Text))
                            {
                                return hdclutrueName.Value.Contains("en") ? "Please check the answers are not empty" : "يرجى التأكد من أن الإجابات ليست فارغة";
                            }
                            break;

                        case "2":
                            if (String.IsNullOrEmpty(TxtMultiChoicesAnswer01.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer02.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer03.Text))
                            {
                                return hdclutrueName.Value.Contains("en") ? "Please check the answers are not empty" : "يرجى التأكد من أن الإجابات ليست فارغة";
                            }
                            break;

                        case "3":
                            if (String.IsNullOrEmpty(TxtMultiChoicesAnswer01.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer02.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer03.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer04.Text))
                            {
                                return hdclutrueName.Value.Contains("en") ? "Please check the answers are not empty" : "يرجى التأكد من أن الإجابات ليست فارغة";
                            }
                            break;

                        case "4":
                            if (String.IsNullOrEmpty(TxtMultiChoicesAnswer01.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer02.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer03.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer04.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer05.Text))
                            {
                                return hdclutrueName.Value.Contains("en") ? "Please check the answers are not empty" : "يرجى التأكد من أن الإجابات ليست فارغة";
                            }
                            break;

                        case "5":
                            if (String.IsNullOrEmpty(TxtMultiChoicesAnswer01.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer02.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer03.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer04.Text) ||
                                String.IsNullOrEmpty(TxtMultiChoicesAnswer05.Text) || String.IsNullOrEmpty(TxtMultiChoicesAnswer06.Text))
                            {
                                return hdclutrueName.Value.Contains("en") ? "Please check the answers are not empty" : "يرجى التأكد من أن الإجابات ليست فارغة";
                            }
                            break;
                    }
                    break;

                case "4"://True / False
                    if (DrpTrueFalse.Items.Count == 0)
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please select at least one of the correct answer" : "يرجى اختيار إجابة واحدة على الأقل من الإجابات الصحيحة";
                    }
                    break;

                case "5"://Yes / No
                    if (DrpYesNo.Items.Count == 0)
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please select at least one of the correct answer" : "يرجى اختيار إجابة واحدة على الأقل من الإجابات الصحيحة";
                    }
                    break;

                default://Other types
                    if (String.IsNullOrEmpty(TxtAnswerText.Text))
                    {
                        return hdclutrueName.Value.Contains("en") ? "Please check the answer text is not empty" : "يرجى التأكد من أن نص الإجابة ليس فارغا";
                    }
                    if (!SystemBase.RegIsMatch(TxtAnswerText.Text, "REGULAR_TEXT"))
                    {
                        return hdclutrueName.Value.Contains("en") ? "The answer text is incorrect" : "نص الإجابة غير صحيح";
                    }
                    break;
            }

            return Msg;
        }

        private bool ShowQuestionTimeTextDiv(string QuestionGuidID)
        {
            bool ShowQuestionTime = false;

            try
            {
                ShowQuestionTime =
                    int.Parse(SystemBase.GetDataTble($"SELECT QB.QuestionsFormStyleID " +
                                                     $"FROM Questions Q " +
                                                     $"INNER JOIN QuestionsBank QB ON " +
                                                     $"(QB.FormGuidID = Q.FormGuidID) " +
                                                     $"WHERE Q.QuestionGuidID = '{QuestionGuidID}'").Rows[0]["QuestionsFormStyleID"].ToString()) == 3
                                                     ?
                                                     true
                                                     :
                                                     false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (!ShowQuestionTime) TxtQuestionTime.Text = "0";
            return ShowQuestionTime;
        }

        private string RetriveQuestionInfo()
        {
            string RetriveRes = "success";

            string QuestionGID = Hdqgid.Value;
            DataTable QuestionIfnoDB =
                SystemBase.GetDataTble($"SELECT * " +
                                       $"FROM Questions " +
                                       $"WHERE QuestionGuidID = '{QuestionGID}'");
            if (QuestionIfnoDB != null)
            {
                if (QuestionIfnoDB.Rows.Count > 0)
                {
                    if (QuestionIfnoDB.Rows[0]["IsApproved"].ToString().Contains("False"))
                    {
                        DrpQuestionType.SelectedValue = QuestionIfnoDB.Rows[0]["QuestionTypeID"].ToString();
                        DrpQuestionType_IndxChange(null, null);
                        DrpNumberofChoices.SelectedValue = QuestionIfnoDB.Rows[0]["NumberOfChoicesID"].ToString();
                        DrpNumberofChoices_IndxChange(null, null);

                        DrpTrueFalse.SelectedValue = QuestionIfnoDB.Rows[0]["TheCorrectAnswerTrueFals"].ToString();
                        DrpYesNo.SelectedValue = QuestionIfnoDB.Rows[0]["TheCorrectAnswerYesNo"].ToString();

                        TxtAnswerText.Text = QuestionIfnoDB.Rows[0]["AnswerText"].ToString();
                        TxtTheCorrectAnswer.Text = QuestionIfnoDB.Rows[0]["TheCorrectAnswer"].ToString();
                        TxtQuestionGrade.Text = QuestionIfnoDB.Rows[0]["QuestionGrade"].ToString();

                        TxtMultiChoicesAnswer01.Text = QuestionIfnoDB.Rows[0]["AnswerNumberOne"].ToString();
                        TxtMultiChoicesAnswer02.Text = QuestionIfnoDB.Rows[0]["AnswerNumberTwo"].ToString();
                        TxtMultiChoicesAnswer03.Text = QuestionIfnoDB.Rows[0]["AnswerNumberThree"].ToString();
                        TxtMultiChoicesAnswer04.Text = QuestionIfnoDB.Rows[0]["AnswerNumberFour"].ToString();
                        TxtMultiChoicesAnswer05.Text = QuestionIfnoDB.Rows[0]["AnswerNumberFive"].ToString();
                        TxtMultiChoicesAnswer06.Text = QuestionIfnoDB.Rows[0]["AnswerNumberSix"].ToString();

                        TxtQuestionTime.Text = QuestionIfnoDB.Rows[0]["QuestionTime"].ToString();

                        TxtQuestionText.Text = QuestionIfnoDB.Rows[0]["QuestionText"].ToString();
                    }
                    else
                    {
                        RetriveRes = "IsApprovedTrue";
                    }
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

            return RetriveRes;
        }

        private bool IsThereStudentsAnsweredTheQuestion(string QuestionGuidID)
        {
            bool ThereIsAnswer = false;

            DataTable QuestionsStudentsAnswersInfo =
                SystemBase.GetDataTble($"SELECT * " +
                                       $"FROM QuestionsStudentsAnswers " +
                                       $"WHERE QuestionGuidID = '{QuestionGuidID}'");
            if(QuestionsStudentsAnswersInfo != null)
            {
                if (QuestionsStudentsAnswersInfo.Rows.Count > 0)
                {
                    ThereIsAnswer = true;
                }
            }

            return ThereIsAnswer;
        }
    }
}