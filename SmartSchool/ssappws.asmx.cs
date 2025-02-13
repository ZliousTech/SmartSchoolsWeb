using Common.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;

namespace SmartSchool
{
    /// <summary>
    /// Summary description for ssappws
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ssappws : System.Web.Services.WebService
    {
        [WebMethod]
        public string DeleteQuestionsForm(string FGID, string Lang)
        {
            string DeleteQuestionsFormRes = "success";

            DataTable FormInfoDB =
                SystemBase.GetDataTble($"SELECT FormCount = COUNT(QB.FormGuidID) " +
                                       $"FROM QuestionsBank QB " +
                                       $"INNER JOIN Questions Q ON " +
                                       $"(Q.FormGuidID = QB.FormGuidID) " +
                                       $"WHERE QB.FormGuidID = '{FGID}'");
            if (FormInfoDB != null)
            {
                if (FormInfoDB.Rows.Count > 0)
                {
                    if (int.Parse(FormInfoDB.Rows[0]["FormCount"].ToString()) > 0)
                    {
                        DeleteQuestionsFormRes = Lang.Contains("en")
                            ?
                            "The form cannot be deleted, there are questions associated with this form."
                            :
                            "لا يمكن حذف النموذج، هناك أسئلة مرتبطة بهذا النموذج.";

                        return DeleteQuestionsFormRes;
                    }
                }
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("UPDATE QuestionsBank SET " +
                                 "IsDeleted = @IsDeleted " +
                                 "WHERE FormGuidID = @FormGuidID", con);
            CMD.Parameters.AddWithValue("@FormGuidID", FGID);
            CMD.Parameters.AddWithValue("@IsDeleted", "True");
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            return DeleteQuestionsFormRes;
        }

        [WebMethod]
        public string DeleteQuestion(string QGID, string Lang)
        {
            string DeleteQuestionRes = "success";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("UPDATE Questions SET " +
                                 "IsDeleted = @IsDeleted " +
                                 "WHERE QuestionGuidID = @QuestionGuidID", con);
            CMD.Parameters.AddWithValue("@QuestionGuidID", QGID);
            CMD.Parameters.AddWithValue("@IsDeleted", "True");
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            return DeleteQuestionRes;
        }

        [WebMethod]
        public string GetFormSettingsInfo(string FGID)
        {
            string FormSettings = " | | | | ";

            DataTable FormSettingsInfoDB =
                SystemBase.GetDataTble($"SELECT TOP 1 QB.QuestionsFormTitle, QFS.StratDateTime, QFS.EndDateTime, QFS.IsActive " +
                                       $"FROM QuestionsFormSettings QFS " +
                                       $"INNER JOIN QuestionsBank QB ON " +
                                       $"(QB.FormGuidID = QFS.FormGuidID) " +
                                       $"WHERE QFS.FormGuidID = '{FGID}'");
            if (FormSettingsInfoDB != null)
            {
                if (FormSettingsInfoDB.Rows.Count > 0)
                {
                    string FormTitle = FormSettingsInfoDB.Rows[0]["QuestionsFormTitle"].ToString();
                    string[] StartDateTimeArr = FormSettingsInfoDB.Rows[0]["StratDateTime"].ToString().Split(' ');
                    string[] EndDateTimeArr = FormSettingsInfoDB.Rows[0]["EndDateTime"].ToString().Split(' ');
                    string StartDate = StartDateTimeArr[0];
                    string StartTime = String.Concat(StartDateTimeArr[1], " ", StartDateTimeArr[2]);
                    string EndTime = String.Concat(EndDateTimeArr[1], " ", EndDateTimeArr[2]) ;
                    string IsActive = FormSettingsInfoDB.Rows[0]["IsActive"].ToString() == "True" ? "Yes" : "No";

                    FormSettings = String.Concat(FormTitle, "|", StartDate, "|", StartTime, "|", EndTime, "|", IsActive);
                }
            }

            return FormSettings;
        }

        [WebMethod]
        public string[] GetIncludedStudentsInfo(string FGID, string Lang)
        {
            string StudentName = Lang.Contains("en") ? "StudentEnglishName" : "StudentArabicName";
            List<string> Students = new List<string>();

            DataTable StudentsInfoDB =
                SystemBase.GetDataTble($"SELECT QFS.StudentID, StudentName = S.{StudentName} " +
                                       $"FROM QuestionsFormSettings QFS " +
                                       $"INNER JOIN Student S ON " +
                                       $"(S.StudentID = QFS.StudentID) " +
                                       $"WHERE QFS.FormGuidID = '{FGID}'");
            if (StudentsInfoDB != null)
            {
                if (StudentsInfoDB.Rows.Count > 0)
                {
                    foreach (DataRow Student in StudentsInfoDB.Rows)
                    {
                        Students.Add(String.Concat(Student["StudentID"].ToString(), "|", Student["StudentName"].ToString()));
                    }
                }
            }

            return Students.ToArray();
        }

        [WebMethod]
        public string saveStudentQuestionsFormData(string StudentID, string FormGuidID, object[] QuestionIDInfo, object[] QuestionValueInfo)
        {
            string SaveRes = "failed";
            StudentID = StudentID.Trim();
            FormGuidID = FormGuidID.Trim();
            List<string> QuestionsAnswers = new List<string>();
            for (int i = 0; i < QuestionIDInfo.Length; i++)
            {
                string[] QuestionInfo = QuestionIDInfo[i].ToString().Split('_');
                InsertUpdateQuestionAnswer(QuestionInfo[0].ToString().Trim(), StudentID, FormGuidID, QuestionInfo[1].ToString().Trim(), QuestionValueInfo[i].ToString().Trim());
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            CMD = new SqlCommand("UPDATE QuestionsFormSettings SET " +
                                 "IsStudentFinished = @IsStudentFinished " +
                                 "WHERE FormGuidID = @FormGuidID AND StudentID = @StudentID", con);
            CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
            CMD.Parameters.AddWithValue("@StudentID", StudentID);
            CMD.Parameters.AddWithValue("@IsStudentFinished", "True");
            CMD.ExecuteNonQuery();

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
            SaveRes = "success";
            return SaveRes;
        }

        private void InsertUpdateQuestionAnswer(string QuestionTypName, string StudentID, string FormGuidID, string QuestionGuidID, string QuestionValue)
        {
            string AnswerTextEssay = "";
            string AnswerTextShortAnswer = "";
            string AnswerNumberOne = "False";
            string AnswerNumberTwo = "False";
            string AnswerNumberThree = "False";
            string AnswerNumberFour = "False";
            string AnswerNumberFive = "False";
            string AnswerNumberSix = "False";
            string AnswerTrue = "False";
            string AnswerFalse = "False";
            string AnswerYes = "False";
            string AnswerNo = "False";
            switch (QuestionTypName)
            {
                case "TxtEssay":
                    AnswerTextEssay = QuestionValue;
                    break;
                case "TxtShortAnswer":
                    AnswerTextShortAnswer = QuestionValue;
                    break;
                case "RadChoice01":
                    AnswerNumberOne = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadChoice02":
                    AnswerNumberTwo = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadChoice03":
                    AnswerNumberThree = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadChoice04":
                    AnswerNumberFour = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadChoice05":
                    AnswerNumberFive = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadChoice06":
                    AnswerNumberSix = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadTrue":
                    AnswerTrue = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadFalse":
                    AnswerFalse = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadYes":
                    AnswerYes = QuestionValue == "True" ? "True" : "False";
                    break;
                case "RadNo":
                    AnswerNo = QuestionValue == "True" ? "True" : "False";
                    break;
            }

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == ConnectionState.Closed) con.Open();
            SqlCommand CMD;

            //Do update if avaliavle 
            DataTable QuestionInfoDB =
                SystemBase.GetDataTble($"SELECT COUNT(QuestionGuidID) " +
                                       $"FROM QuestionsStudentsAnswers " +
                                       $"WHERE StudentID = '{StudentID}' AND QuestionGuidID = '{QuestionGuidID}'");
            if (QuestionInfoDB != null)
            {
                if (QuestionInfoDB.Rows.Count > 0)
                {
                    if (int.Parse(QuestionInfoDB.Rows[0][0].ToString()) > 0) //Update data
                    {
                        switch (QuestionTypName)
                        {
                            case "TxtEssay":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerTextEssay = @AnswerTextEssay " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerTextEssay", AnswerTextEssay);
                                CMD.ExecuteNonQuery();
                                break;

                            case "TxtShortAnswer":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerTextShortAnswer = @AnswerTextShortAnswer " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerTextShortAnswer", AnswerTextShortAnswer);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice01":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberOne = @AnswerNumberOne " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberOne", AnswerNumberOne);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice02":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberTwo = @AnswerNumberTwo " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberTwo", AnswerNumberTwo);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice03":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberThree = @AnswerNumberThree " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberThree", AnswerNumberThree);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice04":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberFour = @AnswerNumberFour " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberFour", AnswerNumberFour);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice05":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberFive = @AnswerNumberFive " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberFive", AnswerNumberFive);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadChoice06":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNumberSix = @AnswerNumberSix " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNumberSix", AnswerNumberSix);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadTrue":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerTrue = @AnswerTrue " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerTrue", AnswerTrue);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadFalse":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerFalse = @AnswerFalse " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerFalse", AnswerFalse);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadYes":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerYes = @AnswerYes " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerYes", AnswerYes);
                                CMD.ExecuteNonQuery();
                                break;

                            case "RadNo":
                                CMD = new SqlCommand("UPDATE QuestionsStudentsAnswers SET " +
                                                     "AnswerNo = @AnswerNo " +
                                                     "WHERE QuestionGuidID = @QuestionGuidID AND StudentID = @StudentID", con);
                                CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                                CMD.Parameters.AddWithValue("@StudentID", StudentID);
                                CMD.Parameters.AddWithValue("@AnswerNo", AnswerNo);
                                CMD.ExecuteNonQuery();
                                break;
                        }
                    }
                    else //Insert data
                    {
                        CMD = new SqlCommand("INSERT INTO QuestionsStudentsAnswers " +
                                             "(" +
                                                "QuestionStudentAnswersGuidID, StudentID, FormGuidID, QuestionGuidID, AnswerTextEssay, AnswerTextShortAnswer, " +
                                             "   AnswerNumberOne, AnswerNumberTwo, AnswerNumberThree, AnswerNumberFour, AnswerNumberFive, AnswerNumberSix, " +
                                             "   AnswerTrue, AnswerFalse, AnswerYes, AnswerNo " +
                                             ") VALUES (" +
                                                "@QuestionStudentAnswersGuidID, @StudentID, @FormGuidID, @QuestionGuidID, @AnswerTextEssay, @AnswerTextShortAnswer, " +
                                             "   @AnswerNumberOne, @AnswerNumberTwo, @AnswerNumberThree, @AnswerNumberFour, @AnswerNumberFive, @AnswerNumberSix, " +
                                             "   @AnswerTrue, @AnswerFalse, @AnswerYes, @AnswerNo " +
                                             ")", con);
                        CMD.Parameters.AddWithValue("@QuestionStudentAnswersGuidID", SystemBase.GenerateGuidID("QuestionStudentAnswersGuidID", "QuestionsStudentsAnswers"));
                        CMD.Parameters.AddWithValue("@StudentID", StudentID);
                        CMD.Parameters.AddWithValue("@FormGuidID", FormGuidID);
                        CMD.Parameters.AddWithValue("@QuestionGuidID", QuestionGuidID);
                        CMD.Parameters.AddWithValue("@AnswerTextEssay", AnswerTextEssay);
                        CMD.Parameters.AddWithValue("@AnswerTextShortAnswer", AnswerTextShortAnswer);
                        CMD.Parameters.AddWithValue("@AnswerNumberOne", AnswerNumberOne);
                        CMD.Parameters.AddWithValue("@AnswerNumberTwo", AnswerNumberTwo);
                        CMD.Parameters.AddWithValue("@AnswerNumberThree", AnswerNumberThree);
                        CMD.Parameters.AddWithValue("@AnswerNumberFour", AnswerNumberFour);
                        CMD.Parameters.AddWithValue("@AnswerNumberFive", AnswerNumberFive);
                        CMD.Parameters.AddWithValue("@AnswerNumberSix", AnswerNumberSix);
                        CMD.Parameters.AddWithValue("@AnswerTrue", AnswerTrue);
                        CMD.Parameters.AddWithValue("@AnswerFalse", AnswerFalse);
                        CMD.Parameters.AddWithValue("@AnswerYes", AnswerYes);
                        CMD.Parameters.AddWithValue("@AnswerNo", AnswerNo);
                        CMD.ExecuteNonQuery();
                    }
                }
            }

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
