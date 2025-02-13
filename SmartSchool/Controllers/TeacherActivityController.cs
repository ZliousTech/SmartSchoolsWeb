using Business.Base;
using Common.Helpers;
using Microsoft.Ajax.Utilities;
using SmartSchool.Models.Activity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class TeacherActivityController : BaseController
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

        private List<SelectListItem> OccasionTypesForTeacher()
        {
            var occasionTypesManager = factory.CreateOccasionTypesManager();
            var occasionTypes = occasionTypesManager.Find(o => o.OccasionID == 8).ToList(); // 8 for School Activities in database.

            var occasionTypesList = (from o in occasionTypes
                                     select new SelectListItem
                                     {
                                         Value = o.OccasionID.ToString(),
                                         Text = CurrentLanguage == Languges.English ? o.OccasionTypeEnglishName : o.OccasionTypeArabicName
                                     }).ToList();

            return occasionTypesList;
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

        public ActionResult PrepareTeacherActivity(PrepareTeacherActivity model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);

            List<TeacherActivity> teacherActivities = new List<TeacherActivity>();
            string query = "SELECT t.ID, t.ArabicHeader, t.EnglishHeader, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, t.NumberofDays, t.StartingDate " +
                "FROM TeacherActivity t " +
                "INNER JOIN SchoolClasses c ON t.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections s ON t.SectionID = s.SectionID " +
                "WHERE t.TeacherID = @TeacherID ";

            query += model.SchoolClassID != null && model.SchoolClassID != 0 ?
                " AND t.SchoolClassID = " + model.SchoolClassID.ToString() : "";
            query += model.SectionID != null && model.SectionID != 0 ?
                " AND t.SectionID = " + model.SectionID.ToString() : "";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teacherActivities.Add(new TeacherActivity()
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                SchoolClassName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SchoolClassEnglishName"].ToString()) ?
                                reader["SchoolClassEnglishName"].ToString()
                                : reader["SchoolClassArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SchoolClassArabicName"].ToString()) ?
                                reader["SchoolClassArabicName"].ToString()
                                : reader["SchoolClassEnglishName"].ToString(),
                                SectionName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SectionEnglishName"].ToString()) ?
                                reader["SectionEnglishName"].ToString()
                                : reader["SectionArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SectionArabicName"].ToString()) ?
                                reader["SectionArabicName"].ToString()
                                : reader["SectionEnglishName"].ToString(),
                                Header = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["EnglishHeader"].ToString()) ?
                                reader["EnglishHeader"].ToString()
                                : reader["ArabicHeader"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["ArabicHeader"].ToString()) ?
                                reader["ArabicHeader"].ToString()
                                : reader["EnglishHeader"].ToString(),
                                StartingDate = (DateTime)reader["StartingDate"],
                                NumberofDays = (int)reader["NumberofDays"]
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            model.TeacherActivities = teacherActivities;
            return View(model);
        }

        public ActionResult AddNewTeacherActivity(TeacherActivity model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.OccasionTypeNames = OccasionTypesForTeacher();
            model.OccasionType = Convert.ToInt32(model.OccasionTypeNames.ToList().First().Value);
            return View(model);
        }

        public ActionResult EditTeacherActivity(int activityID, string staffID)
        {
            TeacherActivity model = new TeacherActivity();
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.OccasionTypeNames = OccasionTypesForTeacher();
            model.OccasionType = Convert.ToInt32(model.OccasionTypeNames.ToList().First().Value);

            string query = "";

            query = "SELECT * FROM TeacherActivity WHERE ID = " + activityID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.ID = activityID;
                            byte[] imageData = null;
                            if (!(reader["Photo"] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader["Photo"];
                                model.Photo = imageData;
                            }
                            model.SchoolClassID = (int)reader["SchoolClassID"];
                            model.SectionID = (int)reader["SectionID"];
                            model.ArabicHeader = reader["ArabicHeader"].ToString();
                            model.ArabicHeader = reader["ArabicHeader"].ToString();
                            model.EnglishHeader = reader["EnglishHeader"].ToString();
                            model.ArabicDescription = reader["ArabicDescription"].ToString();
                            model.EnglishDescription = reader["EnglishDescription"].ToString();
                            model.OccasionType = int.Parse(reader["OccasionType"].ToString());
                            model.StartingDate = DateTime.Parse(reader["StartingDate"].ToString().Split(' ')[0]);
                            model.NumberofDays = int.Parse(reader["NumberofDays"].ToString());
                            model.Vacation = Convert.ToBoolean(reader["Vacation"]);
                            model.TeacherID = reader["TeacherID"].ToString();
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return View(model);
        }

        public ActionResult GetListStudents(int sectionID)
        {
            var studentsActivity = new List<StudentsActivity>();
            string query = "SELECT t.StudentID, t.StudentArabicName, t.StudentEnglishName, " +
                "s.SectionID, s.SectionArabicName, s.SectionEnglishName " +
                "FROM Student t " +
                "INNER JOIN StudentSchoolDetails d ON d.StudentID = t.StudentID " +
                "INNER JOIN Sections s ON d.SectionID = s.SectionID " +
                "WHERE d.SectionID = @SectionID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SectionID", sectionID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentsActivity.Add(new StudentsActivity()
                            {
                                StudentID = reader["StudentID"].ToString(),
                                StudentName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["StudentEnglishName"].ToString()) ?
                                reader["StudentEnglishName"].ToString()
                                : reader["StudentArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["StudentArabicName"].ToString()) ?
                                reader["StudentArabicName"].ToString()
                                : reader["StudentEnglishName"].ToString(),
                                SectionID = (int)reader["SectionID"],
                                SectionName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SectionEnglishName"].ToString()) ?
                                reader["SectionEnglishName"].ToString()
                                : reader["SectionArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SectionArabicName"].ToString()) ?
                                reader["SectionArabicName"].ToString()
                                : reader["SectionEnglishName"].ToString(),
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return View(studentsActivity);
        }

        [HttpPost]
        public JsonResult NewTeacherActivity(TeacherActivity model, string students)
        {
            try
            {
                int insertedActivityID = 0;
                string query = "";
                byte[] imgByte = null;

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader != null && fileuploader.ContentLength > 0)
                    {
                        imgByte = new Byte[fileuploader.ContentLength];
                        //force the control to load data in array
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }
                }

                query = "INSERT INTO TeacherActivity " +
                "(SchoolID, SchoolClassID, SectionID, Photo, ArabicHeader, EnglishHeader, ArabicDescription, EnglishDescription, " +
                "OccasionType, StartingDate, NumberofDays, Vacation, TeacherID) " +
                "VALUES (@SchoolID, @SchoolClassID, @SectionID, @Photo, @ArabicHeader, @EnglishHeader, " +
                "@ArabicDescription, @EnglishDescription, @OccasionType, @StartingDate, @NumberofDays, @Vacation, @TeacherID); " +
                "SELECT SCOPE_IDENTITY() AS RESULT";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@Photo", imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Activities.jpg")) : imgByte);
                        comm.Parameters.AddWithValue("@ArabicHeader", model.ArabicHeader);
                        comm.Parameters.AddWithValue("@EnglishHeader", model.EnglishHeader);
                        comm.Parameters.AddWithValue("@ArabicDescription", model.ArabicDescription);
                        comm.Parameters.AddWithValue("@EnglishDescription", model.EnglishDescription);
                        comm.Parameters.AddWithValue("@OccasionType", model.OccasionType);
                        comm.Parameters.AddWithValue("@StartingDate", model.StartingDate);
                        comm.Parameters.AddWithValue("@NumberofDays", model.NumberofDays);
                        comm.Parameters.AddWithValue("@Vacation", model.Vacation);
                        comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);
                        insertedActivityID = Convert.ToInt16(comm.ExecuteScalar());
                    }
                    conn.Close();
                    conn.Dispose();
                }

                if (insertedActivityID != 0)
                {
                    foreach (var item in students.Split(','))
                    {
                        var studentID = item.Split('$')[0];
                        if (!string.IsNullOrWhiteSpace(studentID))
                        {
                            query = "INSERT INTO TeacherStudentsActivity " +
                                                "(TeacherActivityID, StudentID) " +
                                                "VALUES(@TeacherActivityID, @StudentID)";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    comm.Parameters.AddWithValue("@TeacherActivityID", insertedActivityID);
                                    comm.Parameters.AddWithValue("@StudentID", studentID);
                                    comm.ExecuteNonQuery();
                                }
                                conn.Close();
                                conn.Dispose();
                            }
                        }
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateTeacherActivity(TeacherActivity model, string students)
        {
            try
            {
                byte[] imgByte = null, oldImg = null;
                string query = "";

                query = "SELECT Photo FROM TeacherActivity WHERE ID = " + model.ID;
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (!(reader["Photo"] is DBNull))
                                {
                                    // Get the image data from the reader
                                    oldImg = (byte[])reader["Photo"];
                                    model.Photo = oldImg;
                                }
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                query = "UPDATE TeacherActivity SET " +
                "SchoolID = @SchoolID, SchoolClassID = @SchoolClassID, SectionID = @SectionID, Photo = @Photo, " +
                "ArabicHeader = @ArabicHeader, EnglishHeader = @EnglishHeader, " +
                "ArabicDescription = @ArabicDescription, EnglishDescription = @EnglishDescription, " +
                "OccasionType = @OccasionType, StartingDate = @StartingDate, NumberofDays = @NumberofDays, " +
                "Vacation = @Vacation, TeacherID = @TeacherID WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", model.ID);
                        if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                        {
                            var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                            if (fileuploader != null && fileuploader.ContentLength > 0)
                            {
                                imgByte = new Byte[fileuploader.ContentLength];
                                //force the control to load data in array
                                fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                            }
                        }
                        comm.Parameters.AddWithValue("@Photo", imgByte == null ? model.Photo : imgByte);
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@ArabicHeader", model.ArabicHeader);
                        comm.Parameters.AddWithValue("@EnglishHeader", model.EnglishHeader);
                        comm.Parameters.AddWithValue("@ArabicDescription", model.ArabicDescription);
                        comm.Parameters.AddWithValue("@EnglishDescription", model.EnglishDescription);
                        comm.Parameters.AddWithValue("@OccasionType", model.OccasionType);
                        comm.Parameters.AddWithValue("@StartingDate", model.StartingDate);
                        comm.Parameters.AddWithValue("@NumberofDays", model.NumberofDays);
                        comm.Parameters.AddWithValue("@Vacation", model.Vacation);
                        comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                query = "DELETE FROM TeacherStudentsActivity WHERE TeacherActivityID = @ID;";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", model.ID);
                        comm.ExecuteNonQuery();
                    }

                    conn.Close();
                    conn.Dispose();
                }

                foreach (var item in students.Split(','))
                {
                    var studentID = item.Split('$')[0];
                    if (!string.IsNullOrEmpty(studentID))
                    {
                        query = "INSERT INTO TeacherStudentsActivity " +
                                        "(TeacherActivityID, StudentID) " +
                                        "VALUES(@TeacherActivityID, @StudentID)";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@TeacherActivityID", model.ID);
                                comm.Parameters.AddWithValue("@StudentID", studentID);
                                comm.ExecuteNonQuery();
                            }
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ActivityStudents(int activityID)
        {
            try
            {
                List<string> teacherStudentsActivity = new List<string>();
                string query = "SELECT StudentID FROM TeacherStudentsActivity " +
                    "WHERE TeacherActivityID = @TeacherActivityID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@TeacherActivityID", activityID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                teacherStudentsActivity.Add(reader["StudentID"].ToString());
                            }
                        }
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true, teacherStudentsActivity = teacherStudentsActivity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteTeacherActivity(int ID)
        {
            try
            {
                string query = "DELETE FROM TeacherActivity WHERE ID = @ID; " +
                    "DELETE FROM TeacherStudentsActivity WHERE TeacherActivityID = @ID;";

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