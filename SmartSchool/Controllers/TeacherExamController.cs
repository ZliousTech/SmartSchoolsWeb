using Business.Base;
using Common.Helpers;
using Microsoft.Ajax.Utilities;
using SmartSchool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class TeacherExamController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        private List<SelectListItem> PopulateTeacherSchoolClasses(string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID).ToList();

            var SchoolClassesManager = factory.CreateSchoolClasssManager();
            var SchoolClasses = SchoolClassesManager.Find(s => s.SchoolID == SchoolID).ToList();

            var classesList = teacherTimeTableItems
                .Join(SchoolClasses, t => t.SchoolClassID, s => s.SchoolClassID, (t, s) => new
                {
                    SchoolClassID = s.SchoolClassID,
                    SchoolClassName = CurrentLanguage == Languges.English ? s.SchoolClassEnglishName : s.SchoolClassArabicName
                })
                .DistinctBy(c => c.SchoolClassID)
                .Select(c => new SelectListItem
                {
                    Text = c.SchoolClassName,
                    Value = c.SchoolClassID.ToString()
                }).OrderBy(c => c.Value).ToList();
            return classesList;
        }

        public List<SelectListItem> PopulateSemesters()
        {
            List<SelectListItem> semesters = new List<SelectListItem>();
            string query = "SELECT ID, SemesterArabicName, SemesterEnglishName FROM Semesters " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semesters.Add(new SelectListItem()
                            {
                                Value = reader["ID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["SemesterEnglishName"].ToString() : reader["SemesterArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return semesters;
        }

        public List<SelectListItem> PopulateExamTypes()
        {
            List<SelectListItem> examTypes = new List<SelectListItem>();
            string query = "SELECT * FROM ExamTypes";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            examTypes.Add(new SelectListItem()
                            {
                                Value = reader["ExamTypeID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["TypeEnglishName"].ToString() : reader["TypeArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return examTypes;
        }

        public List<SelectListItem> PopulateExamTitles(bool isAddForm = false)
        {
            List<SelectListItem> examTypes = new List<SelectListItem>();
            string query = "SELECT * FROM ExamTitles ";
            query += isAddForm ? "WHERE ID = (SELECT TOP 1 ID FROM ExamTitles ORDER BY ID DESC)" : "";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            examTypes.Add(new SelectListItem()
                            {
                                Value = reader["ID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["ExamTitleEnglishName"].ToString() : reader["ExamTitleArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return examTypes;
        }

        [HttpPost]
        public JsonResult GetTeacherSections(int? schoolClassID, string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID
            && t.SchoolClassID == schoolClassID).ToList();

            var sectionsManager = factory.CreateSectionsManager();
            var sections = sectionsManager.Find(a => a.SchoolClassID == schoolClassID).ToList();

            var teacherSectionsList = teacherTimeTableItems
                .Join(sections, t => t.SectionID, s => s.SectionID, (t, s) => new
                {
                    SectionID = s.SectionID,
                    SectionName = CurrentLanguage == Languges.English ? string.IsNullOrWhiteSpace(s.SectionEnglishName.ToString()) ?
                                    s.SectionArabicName.ToString() : s.SectionEnglishName.ToString() :
                                    string.IsNullOrWhiteSpace(s.SectionArabicName.ToString()) ?
                                    s.SectionEnglishName.ToString() : s.SectionArabicName.ToString()
                })
                .DistinctBy(c => c.SectionID)
                .Select(c => new SelectListItem
                {
                    Text = c.SectionName,
                    Value = c.SectionID.ToString()
                }).OrderBy(c => c.Value).ToList();
            return Json(teacherSectionsList, JsonRequestBehavior.AllowGet);
        }

        public List<int> GetTeacherSections(string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID).ToList();

            var sectionsManager = factory.CreateSectionsManager();
            var sections = sectionsManager.Find(a => a.SchoolID == SchoolID).ToList();

            var teacherSectionsList = teacherTimeTableItems
                .Join(sections, t => t.SectionID, s => s.SectionID, (t, s) => new
                {
                    SectionID = s.SectionID
                })
                .Distinct()
                .Select(c => c.SectionID).OrderBy(c => c).ToList();
            return teacherSectionsList;
        }

        [HttpPost]
        public JsonResult GetTeacherSubjects(int? sectionID, string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID && t.SectionID == sectionID).ToList();

            var subjectsManager = factory.CreateSubjectsManager();
            var subjects = subjectsManager.Find(s => s.SchoolID == SchoolID).ToList();

            var teacherSubjectsList = (from t in teacherTimeTableItems
                                       join s in subjects
                                       on t.SubjectID equals s.SubjectID
                                       where t.TeacherID == staffID
                                       select new SelectListItem
                                       {
                                           Text = CurrentLanguage == Languges.English ?
                                                    string.IsNullOrWhiteSpace(s.SubjectEnglishName) ? s.SubjectArabicName : s.SubjectEnglishName :
                                                    string.IsNullOrWhiteSpace(s.SubjectArabicName) ? s.SubjectEnglishName : s.SubjectArabicName,
                                           Value = s.SubjectID.ToString()
                                       }).DistinctBy(s => s.Value).OrderBy(s => s.Text).ToList();

            return Json(teacherSubjectsList, JsonRequestBehavior.AllowGet);
        }

        public List<int> GetTeacherSubjects(string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID).ToList();

            var subjectsManager = factory.CreateSubjectsManager();
            var subjects = subjectsManager.Find(s => s.SchoolID == SchoolID).ToList();

            var teacherSubjectsList = (from t in teacherTimeTableItems
                                       join s in subjects
                                       on t.SubjectID equals s.SubjectID
                                       where t.TeacherID == staffID
                                       select s.SubjectID)
                                       .Distinct().OrderBy(s => s).ToList();

            return teacherSubjectsList;
        }

        public ActionResult PrepareTeacherExams(PrepareExam model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();

            List<Exam> exams = new List<Exam>();
            string query = "SELECT e.ID, e.TotalGrades, e.ExamDate, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionID, s.SectionArabicName, s.SectionEnglishName, " +
                "m.SemesterArabicName, m.SemesterEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "t.TypeArabicName, t.TypeEnglishName, " +
                "l.ExamTitleArabicName, l.ExamTitleEnglishName " +
                "FROM Exams e " +
                "INNER JOIN SchoolClasses c ON e.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections S ON e.SectionID = S.SectionID " +
                "INNER JOIN Semesters m ON e.SemesterID = m.ID " +
                "INNER JOIN Subjects j ON e.SubjectID = j.SubjectID " +
                "INNER JOIN ExamTypes t ON e.ExamTypeID = t.ExamTypeID " +
                "INNER JOIN ExamTitles l ON e.ExamTitleID = l.ID " +
                "WHERE e.SchoolID = @SchoolID AND e.SchoolYear = @SchoolYear AND " +
                "(e.TeacherID = '-1' ";

            query += model.TeacherID != null ? "OR e.TeacherID = '" + model.TeacherID + "') AND " +
                "e.SectionID IN (" + string.Join(",", GetTeacherSections(model.TeacherID)) + ") AND " +
                "e.SubjectID IN (" + string.Join(",", GetTeacherSubjects(model.TeacherID)) + ")" : ")";

            query += model.SchoolClassID != null && model.SchoolClassID != 0 ? " AND e.SchoolClassID = " + model.SchoolClassID : "";
            query += model.SectionID != null && model.SectionID != 0 ? " AND e.SectionID = " + model.SectionID : "";
            query += model.SubjectID != null && model.SubjectID != 0 ? " AND e.SubjectID = " + model.SubjectID : "";
            query += model.SemesterID != null && model.SemesterID != 0 ? " AND e.SemesterID = " + model.SemesterID : "";
            query += model.ExamTypeID != null && model.ExamTypeID != 0 ? " AND e.ExamTypeID = " + model.ExamTypeID : "";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam()
                            {
                                ID = Convert.ToInt16(reader["ID"]),
                                ExamTitleName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["ExamTitleEnglishName"].ToString()) ?
                                reader["ExamTitleEnglishName"].ToString()
                                : reader["ExamTitleArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["ExamTitleArabicName"].ToString()) ?
                                reader["ExamTitleArabicName"].ToString()
                                : reader["ExamTitleEnglishName"].ToString(),
                                SchoolClassName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SchoolClassEnglishName"].ToString()) ?
                                reader["SchoolClassEnglishName"].ToString()
                                : reader["SchoolClassArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SchoolClassArabicName"].ToString()) ?
                                reader["SchoolClassArabicName"].ToString()
                                : reader["SchoolClassEnglishName"].ToString(),
                                SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString(),
                                SectionID = Convert.ToInt16(reader["SectionID"]),
                                SectionName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SectionEnglishName"].ToString()) ?
                                reader["SectionEnglishName"].ToString()
                                : reader["SectionArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SectionArabicName"].ToString()) ?
                                reader["SectionArabicName"].ToString()
                                : reader["SectionEnglishName"].ToString(),
                                SectionEnglisName = reader["SectionEnglishName"].ToString(),
                                SubjectName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SubjectEnglishName"].ToString()) ?
                                reader["SubjectEnglishName"].ToString()
                                : reader["SubjectArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SubjectArabicName"].ToString()) ?
                                reader["SubjectArabicName"].ToString()
                                : reader["SubjectEnglishName"].ToString(),
                                SubjectEnglisName = reader["SubjectEnglishName"].ToString(),
                                SemesterName = CurrentLanguage == Languges.English ?
                                reader["SemesterEnglishName"].ToString() :
                                reader["SemesterEnglishName"].ToString(),
                                ExamTypeName = CurrentLanguage == Languges.English ?
                                reader["TypeEnglishName"].ToString() :
                                reader["TypeArabicName"].ToString(),
                                ExamDate = reader.IsDBNull(reader.GetOrdinal("ExamDate")) ? (DateTime?)null : Convert.ToDateTime(reader["ExamDate"]),
                                TotalGrades = (decimal)reader["TotalGrades"]
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            model.Exams = exams;

            return View(model);
        }

        public ActionResult EditTeacherExamDate(int ID, string staffID)
        {
            Exam model = new Exam();
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();
            model.ExamTitelsList = PopulateExamTitles();

            string query = "SELECT * FROM Exams WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.ID = (int)reader["ID"];
                            model.SchoolClassID = (int)reader["SchoolClassID"];
                            model.SectionID = (int)reader["SectionID"];
                            model.SubjectID = (int)reader["SubjectID"];
                            model.SemesterID = (int)reader["SemesterID"];
                            model.ExamTypeID = (int)reader["ExamTypeID"];
                            model.ExamTitleID = (int)reader["ExamTitleID"];
                            model.TotalGrades = (decimal)reader["TotalGrades"];
                            model.IsCounted = (bool)reader["IsCounted"];
                            model.SchoolYear = reader["SchoolYear"].ToString();
                            model.ExamDate = reader["ExamDate"] != DBNull.Value ? (DateTime?)reader["ExamDate"] : null;

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return View(model);
        }

        public JsonResult UpdateExamDate(string info, string teacherId)
        {
            /* info parameter: is a string with format "examDate$examId$schoolClassName$sectionName$subjectName$sectionId" 
            *  Note:
            *  you should split the string with (',') to get each record.
            */
            try
            {
                DateTime examDate = default(DateTime);
                int examID, sectionId = default(int);
                string teacherEnglishName, schoolClassName, sectionName, subjectName, notificationBuilder;
                teacherEnglishName = schoolClassName = sectionName = subjectName = notificationBuilder = string.Empty;

                foreach (var item in info.Split(','))
                {
                    examDate = Convert.ToDateTime(item.Split('$')[0]);
                    examID = Convert.ToInt16(item.Split('$')[1]);

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand("UPDATE Exams SET ExamDate = @ExamDate WHERE ID = @ExamID", conn))
                        {
                            comm.Parameters.AddWithValue("@ExamDate", examDate);
                            comm.Parameters.AddWithValue("@ExamID", examID);

                            comm.ExecuteNonQuery();
                        }
                    }
                }

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    string statement = "SELECT StaffEnglishName FROM Staff WHERE StaffID = @StaffID";
                    using (SqlCommand comm = new SqlCommand(statement, conn))
                    {
                        comm.Parameters.AddWithValue("@StaffID", teacherId);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                teacherEnglishName = reader[0].ToString();
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                foreach (var item in info.Split(','))
                {
                    examDate = Convert.ToDateTime(item.Split('$')[0]);
                    examID = Convert.ToInt16(item.Split('$')[1]);
                    schoolClassName = item.Split('$')[2];
                    sectionName = item.Split('$')[3];
                    subjectName = item.Split('$')[4];
                    sectionId = Convert.ToInt16(item.Split('$')[5]);
                    notificationBuilder = "Mr / Mis " + teacherEnglishName + " Updated the exam set it to " + schoolClassName +
                            " " + sectionName + " for " + subjectName + " Subject in " + examDate.ToString("yyyy-MM-dd");
                    AddPushNotification(notificationBuilder, "Exam", sectionId);
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult IsTeacherExam(int ID)
        {
            var isTeacherExam = false;
            string query = "SELECT TeacherID FROM Exams WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isTeacherExam = reader["TeacherID"].ToString() == "-1" ? false : true;
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return Json(new { isTeacherExam = isTeacherExam }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExamDateForNotification(int id)
        {
            try
            {
                dynamic examInfo = new ExpandoObject(); ;
                string query = "SELECT c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                    "s.SectionArabicName, s.SectionEnglishName, " +
                    "j.SubjectArabicName, j.SubjectEnglishName, " +
                    "f.StaffArabicName, f.StaffEnglishName " +
                    "FROM Exams e " +
                    "INNER JOIN SchoolClasses c ON e.SchoolClassID = c.SchoolClassID " +
                    "INNER JOIN Sections s ON e.SectionID = s.SectionID " +
                    "INNER JOIN Subjects j ON e.SubjectID = j.SubjectID " +
                    "LEFT JOIN Staff f ON e.TeacherID = f.StaffID " +
                    "WHERE ID = @ID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                examInfo.SchoolClassName = CurrentLanguage == Languges.English ?
                                    reader["SchoolClassEnglishName"].ToString() : reader["SchoolClassArabicName"].ToString();
                                examInfo.SectionName = CurrentLanguage == Languges.English ?
                                    reader["SectionEnglishName"].ToString() : reader["SectionArabicName"].ToString();
                                examInfo.SubjectName = CurrentLanguage == Languges.English ?
                                    reader["SubjectEnglishName"].ToString() : reader["SubjectArabicName"].ToString();
                                examInfo.StaffName = CurrentLanguage == Languges.English ?
                                    (reader["StaffEnglishName"] != DBNull.Value ? reader["StaffEnglishName"].ToString() : "School Manager") :
                                    (reader["StaffArabicName"] != DBNull.Value ? reader["StaffArabicName"].ToString() : "مدير المدرسة");
                            }
                        }
                    }
                }

                return Json(new { Success = true, ExamInformation = examInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditExamDate(Exam model, int ID)
        {
            try
            {
                string query = "UPDATE Exams SET ExamDate = @ExamDate WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ExamDate", model.ExamDate);
                        comm.Parameters.AddWithValue("@ID", ID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddTeacherExam(Exam model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();
            model.ExamTitelsList = PopulateExamTitles(true); // GET Just Quiz.
            model.ExamTitleID = int.Parse(model.ExamTitelsList.First().Value);
            return View(model);
        }

        [HttpPost]
        public JsonResult NewTeacherExam(Exam model)
        {
            try
            {
                int insertedExamID = -1;
                string query = "";

                query = "INSERT INTO Exams " +
                    "(SchoolID, SchoolClassID, SectionID, SubjectID, SemesterID, ExamTypeID, " +
                    "ExamTitleID, TotalGrades, TeacherID, IsCounted, ExamDate) " +
                    "VALUES(@SchoolID, @SchoolClassID, @SectionID, @SubjectID, " +
                    "@SemesterID, @ExamTypeID, @ExamTitleID, @TotalGrades, @TeacherID, @IsCounted, @ExamDate); SELECT SCOPE_IDENTITY() AS RESULT";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                        comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                        comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                        comm.Parameters.AddWithValue("@TotalGrades", model.TotalGrades);
                        comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);
                        comm.Parameters.AddWithValue("@IsCounted", model.IsCounted);
                        comm.Parameters.AddWithValue("@ExamDate", model.ExamDate);
                        insertedExamID = Convert.ToInt32(comm.ExecuteScalar());
                    }

                    conn.Close();
                    conn.Dispose();
                }

                #region
                // insert the Grades for each student in exam section => zero by default.
                List<string> studentsIDs = new List<string>();
                query = "SELECT s.StudentID FROM Student s " +
                    "INNER JOIN StudentSchoolDetails d ON d.StudentID = s.StudentID " +
                    "WHERE d.SectionID = @SectionID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                studentsIDs.Add(reader["StudentID"].ToString());
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                foreach (var studentID in studentsIDs)
                {
                    query = "INSERT INTO Grades " +
                      "(SchoolID, ExamID, StudentID) " +
                      "VALUES(@SchoolID, @ExamID, '" + studentID + "'); ";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                            comm.Parameters.AddWithValue("@ExamID", insertedExamID);
                            comm.ExecuteNonQuery();
                        }

                        conn.Close();
                        conn.Dispose();
                    }
                }
                #endregion



                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDataForNotification(string staffID, int schoolClassID, int sectionID, int subjectID)
        {
            try
            {
                List<object> informations = new List<object>();

                string query = "SELECT StaffArabicName, StaffEnglishName FROM Staff " +
                               "WHERE StaffID = @StaffID; ";
                query += "SELECT SchoolClassArabicName, SchoolClassEnglishName FROM SchoolClasses " +
                         "WHERE SchoolClassID = @SchoolClassID; ";
                query += "SELECT SectionArabicName, SectionEnglishName FROM Sections " +
                         "WHERE SectionID = @SectionID; ";
                query += "SELECT SubjectArabicName, SubjectEnglishName FROM Subjects " +
                         "WHERE SubjectID = @SubjectID; ";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StaffID", staffID);
                        comm.Parameters.AddWithValue("@SchoolClassID", schoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", sectionID);
                        comm.Parameters.AddWithValue("@SubjectID", subjectID);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    StaffArabicName = reader["StaffArabicName"].ToString(),
                                    StaffEnglishName = reader["StaffEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SchoolClassArabicName = reader["SchoolClassArabicName"].ToString(),
                                    SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SectionArabicName = reader["SectionArabicName"].ToString(),
                                    SectionEnglishName = reader["SectionEnglishName"].ToString()
                                });
                            }

                            reader.NextResult();

                            if (reader.Read())
                            {
                                informations.Add(new
                                {
                                    SubjectArabicName = reader["SubjectArabicName"].ToString(),
                                    SubjectEnglishName = reader["SubjectEnglishName"].ToString()
                                });
                            }
                        }
                    }
                }

                return Json(new { Success = true, Informations = informations }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditTeacherExam(Exam model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();
            model.ExamTitelsList = PopulateExamTitles(true); // GET Just Quiz.
            model.ExamTitleID = int.Parse(model.ExamTitelsList.First().Value);

            string query = "SELECT * FROM Exams WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", model.ID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.ID = (int)reader["ID"];
                            model.SchoolClassID = (int)reader["SchoolClassID"];
                            model.SectionID = (int)reader["SectionID"];
                            model.SubjectID = (int)reader["SubjectID"];
                            model.SemesterID = (int)reader["SemesterID"];
                            model.ExamTypeID = (int)reader["ExamTypeID"];
                            model.ExamTitleID = (int)reader["ExamTitleID"];
                            model.TotalGrades = (decimal)reader["TotalGrades"];
                            model.SchoolYear = reader["SchoolYear"].ToString();
                            model.ExamDate = reader["ExamDate"] != DBNull.Value ? (DateTime?)reader["ExamDate"] : null;

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult EditExam(Exam model)
        {
            try
            {
                string query = "UPDATE Exams SET " +
                  "SchoolID = @SchoolID, SchoolClassID = @SchoolClassID, SectionID = @SectionID, " +
                  "SubjectID = @SubjectID, SemesterID = @SemesterID, ExamTypeID = @ExamTypeID, " +
                  "ExamTitleID = @ExamTitleID, TotalGrades = @TotalGrades, TeacherID = @TeacherID, " +
                  "ExamDate = @ExamDate " +
                  "WHERE ID = @ID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", model.ID);
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                        comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                        comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                        comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);
                        comm.Parameters.AddWithValue("@TotalGrades", model.TotalGrades);
                        comm.Parameters.AddWithValue("@ExamDate", model.ExamDate);
                        comm.ExecuteNonQuery();
                    }

                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteExam(int ID)
        {
            try
            {
                string query = "";
                bool isSchoolExam = false;

                query = "SELECT TeacherID FROM Exams WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                                isSchoolExam = reader["TeacherID"].ToString() == "-1" ? true : false;
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                if (isSchoolExam)
                    return Json(new { Success = false, Message = R.GetResource("Youcan'tdeleteanexamaddedbyschool") }, JsonRequestBehavior.AllowGet);

                query = "DELETE FROM Exams WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", ID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}