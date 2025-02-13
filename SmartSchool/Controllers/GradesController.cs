using Business.Base;
using Common.Helpers;
using Microsoft.Ajax.Utilities;
using SmartSchool.Models.Grades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class GradesController : BaseController
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
        public ActionResult PrepareGrades(PrepareGrades model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();
            model.ExamTitelsList = PopulateExamTitles();

            var nullCheckItems = new object[] { model.SemesterID, model.SectionID, model.SubjectID, model.ExamTypeID, model.ExamTitleID };
            if (nullCheckItems.Any(item => item == null))
            {
                model.Grades = new List<Grades>();
                return View(model);
            }

            List<Grades> stuentsGrades = new List<Grades>();
            string query = "SELECT DISTINCT g.ID, g.ExamID, g.StudentID, g.GradeValue, " +
                "s.StudentArabicName, StudentEnglishName, e.TotalGrades FROM Grades g " +
                "INNER JOIN Exams e ON g.ExamID = e.ID " +
                "INNER JOIN StudentSchoolDetails d ON d.SectionID = e.SectionID " +
                "INNER JOIN Student s ON s.StudentID = d.StudentID " +
                "WHERE SemesterID = @SemesterID AND e.SectionID = @SectionID AND SubjectID = @SubjectID " +
                "AND ExamTypeID = @ExamTypeID AND ExamTitleID = @ExamTitleID " +
                "AND (g.StudentID = s.StudentID)";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                    comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                    comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                    comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                    comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stuentsGrades.Add(new Grades()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                StudentID = reader["StudentID"].ToString(),
                                ExamID = Convert.ToInt32(reader["ExamID"].ToString()),
                                StudentName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["StudentEnglishName"].ToString()) ?
                                reader["StudentEnglishName"].ToString()
                                : reader["StudentArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["StudentArabicName"].ToString()) ?
                                reader["StudentArabicName"].ToString()
                                : reader["StudentEnglishName"].ToString(),
                                GradeValue = (decimal)reader["GradeValue"],
                                ExamMaxGrade = (decimal)reader["TotalGrades"]
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            model.Grades = stuentsGrades;
            return View(model);
        }

        public JsonResult AddUpdateStudentGrade(string info)
        {
            try
            {
                bool gradeCount = false, isExamStarted = false;
                int gradeID = 0, examID = 0;
                decimal gradeValue = 0;
                string query = "", studentID = "", gradeInfo = "";
                List<string> values = new List<string>();

                foreach (var item in info.Split(','))
                {
                    gradeInfo = item.Split('_')[1];
                    values = gradeInfo.Split('$').ToList();
                    gradeID = Convert.ToInt32(values[0]);
                    studentID = values[1];
                    examID = Convert.ToInt32(values[2]);
                    gradeValue = Convert.ToDecimal(values[3]);

                    query = "SELECT ExamDate FROM Exams WHERE ID = @ExamID";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@ExamID", examID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    isExamStarted = reader.IsDBNull(0) ? false : (DateTime.Now.Date >= Convert.ToDateTime(reader["ExamDate"].ToString()) ? true : false);
                                }
                            }
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    if (isExamStarted)
                    {
                        query = "SELECT Count(ID) FROM Grades WHERE ID = @ID";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@ID", gradeID);
                                gradeCount = (int)comm.ExecuteScalar() != 0 ? true : false;
                            }
                            conn.Close();
                            conn.Dispose();
                        }

                        if (gradeCount)
                        {
                            query = "UPDATE Grades SET SchoolID = @SchoolID, SchoolYear = @SchoolYear, " +
                                "ExamID = @ExamID, StudentID = @StudentID, GradeValue = @GradeValue " +
                                "WHERE ID = @ID";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                                    comm.Parameters.AddWithValue("@ExamID", examID);
                                    comm.Parameters.AddWithValue("@StudentID", studentID);
                                    comm.Parameters.AddWithValue("@GradeValue", gradeValue);
                                    comm.Parameters.AddWithValue("@ID", gradeID);
                                    comm.ExecuteNonQuery();
                                }
                                conn.Close();
                                conn.Dispose();
                            }
                        }
                        else // for the new students in case the student accepted to the school after exam is created 😪
                        {
                            query = "INSERT INTO Grades " +
                                "(SchoolID, SchoolYear, ExamID, StudentID, GradeValue)  " +
                                "VALUES(@SchoolID, @SchoolYear, @ExamID, @StudentID, @GradeValue)";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                                    comm.Parameters.AddWithValue("@ExamID", examID);
                                    comm.Parameters.AddWithValue("@StudentID", studentID);
                                    comm.Parameters.AddWithValue("@GradeValue", gradeValue);
                                    comm.ExecuteNonQuery();
                                }
                                conn.Close();
                                conn.Dispose();
                            }
                        }
                    }
                    else
                        return Json(new { Success = false, Message = "This exam doesn't have Date (You must add start date to the exam befor set the grades)! , or the exam doesn't started yet!" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteStudentGrade(int ID)
        {
            try
            {
                string query = "DELETE FROM Grades WHERE ID = @ID";
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