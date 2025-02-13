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
    public partial class AddQuestions : System.Web.UI.Page
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
                        if (String.IsNullOrEmpty(Request.QueryString["QBGID"]))
                        {
                            SystemBase.Logout();
                        }
                    }
                    catch (Exception exp)
                    {
                        Console.WriteLine(Convert.ToString(exp));
                        SystemBase.Logout();
                    }
                    string QuestionBankGuidID = Request.QueryString["QBGID"];
                    Hdqbgid.Value = QuestionBankGuidID;

                    SetCulture();
                    SetLookups();
                    //DivQuestionTime.Visible = ShowQuestionTimeTextDiv(QuestionBankGuidID) ? true : false;
                    DivQuestionTime.Visible = ShowQuestionTimeTextDiv(QuestionBankGuidID);
                    GetFormQuestionsList();
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
                                       $"WHERE QuestionTypeID <= 5 AND QuestionTypeID <> 2" +
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
                                       $"FROM QuestionAnswersYesNoText " +
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
                    SystemBase.GetDataTble($"SELECT QuestionsFormTitle " +
                                           $"FROM QuestionsBank " +
                                           $"WHERE QuestionBankGuidID = '{Hdqbgid.Value}'").Rows[0]["QuestionsFormTitle"].ToString();
            }
            catch (Exception exp) { Console.WriteLine(exp); SystemBase.Logout(); }

            BtnAdd.Text = Resources.Resource.Add;
            BtnCancel.Text = Resources.Resource.Cancel;
            BtnPickFromQuestionBank.Text = Resources.Resource.PickFromQuestionsBank;
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

        protected void BtnPickFromQuestionBank_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("quesbank.aspx", true);
        }

        protected void BtnAdd_Click(object sender, EventArgs e)
        {
            string CheckInputs_res = CheckInputs();
            if (CheckInputs_res != "Success")
            {
                ShowMsg(CheckInputs_res, "error");
                return;
            }

            string QuestionGuidID = SystemBase.GenerateGuidID("QuestionGuidID", "Questions");
            int QuestionBankID = int.Parse(SystemBase.GetIDByGuidIDFor("QuestionBankID", "QuestionBankGuidID", Hdqbgid.Value, "QuestionsBank"));
            string FormGuidID = Hdqbgid.Value;
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
            int TheCorrectAnswer = String.IsNullOrEmpty(TxtTheCorrectAnswer.Text.Trim()) ? 1 : int.Parse(TxtTheCorrectAnswer.Text.Trim());
            string TheCorrectAnswerTrueFals = DrpTrueFalse.SelectedValue.Trim();
            string TheCorrectAnswerYesNo = DrpYesNo.SelectedValue.Trim();
            int QuestionTime = int.Parse(TxtQuestionTime.Text.Trim());
            string CreateDate = DateTime.Now.ToString("MM/dd/yyyy");

            //Insert the question information to questions table.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("INSERT INTO Questions " +
                                 "(" +
                                    "QuestionGuidID, QuestionBankID, FormGuidID, QuestionTypeID, QuestionGrade, QuestionText, AnswerText, NumberOfChoicesID, AnswerNumberOne, AnswerNumberTwo, AnswerNumberThree, AnswerNumberFour, AnswerNumberFive, AnswerNumberSix, TheCorrectAnswer, TheCorrectAnswerTrueFals, TheCorrectAnswerYesNo, QuestionTime, CreateDate " +
                                 ") VALUES (" +
                                    "@QuestionGuidID, @QuestionBankID, @FormGuidID, @QuestionTypeID, @QuestionGrade, @QuestionText, @AnswerText, @NumberOfChoicesID, @AnswerNumberOne, @AnswerNumberTwo, @AnswerNumberThree, @AnswerNumberFour, @AnswerNumberFive, @AnswerNumberSix, @TheCorrectAnswer, @TheCorrectAnswerTrueFals, @TheCorrectAnswerYesNo, @QuestionTime, @CreateDate " +
                                 ") SELECT SCOPE_IDENTITY()", con);
            CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
            CMD.Parameters.AddWithValue("@QuestionBankID", QuestionBankID);
            CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
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
            CMD.Parameters.AddWithValue("@CreateDate", CreateDate);
            CMD.ExecuteNonQuery();
            //int QuestionID = Convert.ToInt32(CMD.ExecuteScalar());

            //Update QuestionBank set form Duration = Sum of questions time.
            if (DivQuestionTime.Visible)
            {
                DataTable QuestionsFormDurationMinutesDB =
                SystemBase.GetDataTble($"SELECT QuestionsFormDurationMinutes = SUM(QuestionTime) " +
                                       $"FROM Questions " +
                                       $"WHERE FormGuidID = '{FormGuidID}'");
                if (QuestionsFormDurationMinutesDB != null)
                {
                    if (QuestionsFormDurationMinutesDB.Rows.Count > 0)
                    {
                        int QuestionsFormDurationMinutes = int.Parse(QuestionsFormDurationMinutesDB.Rows[0]["QuestionsFormDurationMinutes"].ToString());
                        CMD = new SqlCommand("UPDATE QuestionsBank SET " +
                                             "QuestionsFormDurationMinutes = @QuestionsFormDurationMinutes " +
                                             "WHERE FormGuidID = @FormGuidID", con);
                        CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
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

            ShowMsg(hdclutrueName.Value.Contains("en")
                ?
                "The data has been added. Please wait while you are being redirected."
                :
                "تم اضافة البيانات. يرجى الانتظار حتى يتم إعادة توجيهك.",
                "success");

            GetFormQuestionsList();
            Page.Response.Redirect(Page.Request.Url.ToString(), false);
            Context.ApplicationInstance.CompleteRequest();
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

            if (String.IsNullOrEmpty(TxtQuestionText.Text))
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

        private bool ShowQuestionTimeTextDiv(string QuestionBankGuidID)
        {
            bool ShowQuestionTime = false;

            try
            {
                ShowQuestionTime =
                    int.Parse(SystemBase.GetDataTble($"SELECT QuestionsFormStyleID " +
                                                     $"FROM QuestionsBank " +
                                                     $"WHERE QuestionBankGuidID = '{QuestionBankGuidID}'").Rows[0]["QuestionsFormStyleID"].ToString()) == 3
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

        private void GetFormQuestionsList()
        {
            string SchoolID = SystemBase.GetCookie((int)clsenumration.UserData.SchoolID);
            string StaffID = SystemBase.GetCookie((int)clsenumration.UserData.StaffID);

            ReptFormsQuestionsList.DataSource =
                SystemBase.GetDataTble($"SELECT QB.QuestionsFormTitle, QB.QuestionsFormDescription, QuestionTypeText = QT.QuestionTypeEnglishText, Q.QuestionText, Q.QuestionGrade " +
                                       $"FROM QuestionsBank QB " +
                                       $"INNER JOIN Questions Q ON " +
                                       $"(Q.QuestionBankID = QB.QuestionBankID) " +
                                       $"INNER JOIN QuestionTypes QT ON " +
                                       $"(QT.QuestionTypeID = Q.QuestionTypeID) " +
                                       $"WHERE QB.SchoolID = {SchoolID} AND QB.StaffID = '{StaffID}' AND QuestionBankGuidID = '{Hdqbgid.Value}'" +
                                       $"ORDER BY QB.QuestionBankID, Q.QuestionID ASC");

            ReptFormsQuestionsList.DataBind();
        }
    }
}