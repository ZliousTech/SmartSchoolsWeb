using Business.Base;
using Common.Helpers;
using DataAccess;
using Microsoft.Ajax.Utilities;
using SmartSchool.Models.HomeWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class HomeWorkController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities _context = new SmartSchoolsEntities();

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
        public ActionResult PrepareHomeWork(PrepareHomeWork model, string staffID)
        {
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.TeacherID = staffID;
            var sectionID = model.SectionID;
            var subjecID = model.SubjectID;
            List<HomeWork> homeWorks = new List<HomeWork>();
            string query = "SELECT h.HomeWorkID, c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "s.SectionArabicName, s.SectionEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "h.HomeWorkTitle, " +
                "h.HomeWorkDeadLine, h.HomeWorkNote FROM HomeWork h " +
                "INNER JOIN SchoolClasses c " +
                "ON h.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Sections s " +
                "ON h.SectionID = s.SectionID " +
                "INNER JOIN Subjects j " +
                "ON h.SubjectID = j.SubjectID " +
                "WHERE h.TeacherID = @TeacherID ";
            query += sectionID == null || sectionID == 0 ? "" : "AND h.SectionID = " + sectionID + " ";
            query += subjecID == null || subjecID == 0 ? "" : "AND j.SubjectID = " + subjecID + " ";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@TeacherID", staffID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            homeWorks.Add(new HomeWork()
                            {
                                HomeWorkID = (int)reader["HomeWorkID"],
                                SchoolClassName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SchoolClassEnglishName"].ToString()) ?
                                reader["SchoolClassEnglishName"].ToString() : reader["SchoolClassArabicName"].ToString() : !string.IsNullOrWhiteSpace(reader["SchoolClassArabicName"].ToString()) ?
                                reader["SchoolClassArabicName"].ToString() : reader["SchoolClassEnglishName"].ToString(),
                                SectionName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SectionEnglishName"].ToString()) ?
                                reader["SectionEnglishName"].ToString() : reader["SectionArabicName"].ToString() : !string.IsNullOrWhiteSpace(reader["SectionArabicName"].ToString()) ?
                                reader["SectionArabicName"].ToString() : reader["SectionEnglishName"].ToString(),
                                SubjectName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SubjectEnglishName"].ToString()) ?
                                reader["SubjectEnglishName"].ToString() : reader["SubjectArabicName"].ToString() : !string.IsNullOrWhiteSpace(reader["SubjectArabicName"].ToString()) ?
                                reader["SubjectArabicName"].ToString() : reader["SubjectEnglishName"].ToString(),
                                HomeWorkTitle = reader["HomeWorkTitle"].ToString(),
                                HomeWorkDeadLine = (DateTime)reader["HomeWorkDeadLine"],
                                HomeWorkNote = reader["HomeWorkNote"].ToString(),
                                TeacherID = staffID
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            model.HomeWork = homeWorks;
            return View(model);
        }

        public ActionResult AddHomeWork(HomeWork model, string staffID)
        {
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            return View(model);
        }

        [HttpPost]
        public JsonResult GetTeacherSections(int schoolClassID, string staffID)
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

        [HttpPost]
        public JsonResult GetTeacherSubjects(int schoolClassID, string staffID)
        {
            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == staffID).ToList();

            var subjectsManager = factory.CreateSubjectsManager();
            var subjects = subjectsManager.Find(s => s.SchoolID == SchoolID && s.SchoolClassID == schoolClassID).ToList();

            var teacherSubjectsList = (from t in teacherTimeTableItems
                                       join s in subjects
                                       on t.SubjectID equals s.SubjectID
                                       where t.TeacherID == staffID
                                       select new SelectListItem
                                       {
                                           Text = CurrentLanguage == Languges.English ? s.SubjectEnglishName : s.SubjectArabicName,
                                           Value = s.SubjectID.ToString()
                                       }).DistinctBy(s => s.Value).OrderBy(s => s.Value).ToList();

            return Json(teacherSubjectsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult NewHomeWork(HomeWork model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.HomeWorkAttachment) && model.HomeWorkAttachment != "null")
                {
                    string[] attachmentParts = model.HomeWorkAttachment.Split(',');

                    if (attachmentParts.Length > 1)
                    {
                        string base64String = attachmentParts[1];

                        int base64Length = base64String.Length;
                        int estimatedFileSizeInBytes = (int)(base64Length / 1.37); // divide the base64 length by 1.37 to estimate the original file size in bytes.

                        if (estimatedFileSizeInBytes > maxFileSizeInBytes)
                            return Json(new { Success = false, Message = "File size exceeds the maximum allowed size." }, JsonRequestBehavior.AllowGet);

                        string query = "INSERT INTO HomeWork " +
                            "(SchoolClassID, SectionID, SubjectID, HomeWorkTitle, HomeWorkDeadLine, " +
                            "HomeWorkAttachment, [.Ext], HomeWorkNote, TeacherID) " +
                            "VALUES (@SchoolClassID, @SectionID, @SubjectID, @HomeWorkTitle, " +
                            "@HomeWorkDeadLine, @HomeWorkAttachment, @Ext, @HomeWorkNote, @TeacherID)";

                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                                comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                                comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                                comm.Parameters.AddWithValue("@HomeWorkTitle", model.HomeWorkTitle);
                                comm.Parameters.AddWithValue("@HomeWorkDeadLine", model.HomeWorkDeadLine);
                                comm.Parameters.AddWithValue("@HomeWorkAttachment", base64String);
                                comm.Parameters.AddWithValue("@TeacherID", model.TeacherID);

                                if (!string.IsNullOrEmpty(model.HomeWorkNote))
                                    comm.Parameters.AddWithValue("@HomeWorkNote", model.HomeWorkNote);
                                else
                                    comm.Parameters.AddWithValue("@HomeWorkNote", DBNull.Value);

                                // The model.Ext now is holding the file path.
                                string filePath = @"" + model.Ext + "";
                                string fileName = Path.GetFileName(filePath);
                                string fileExtension = Path.GetExtension(fileName);
                                comm.Parameters.AddWithValue("@Ext", fileExtension);

                                comm.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        return Json(new { Success = false, Message = "Invalid file format." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Success = false, Message = "No file uploaded." }, JsonRequestBehavior.AllowGet);
                }

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


        public ActionResult EditHomeWork(int homeWorkID, string staffID)
        {
            HomeWork model = new HomeWork();
            model.TeacherID = staffID;
            model.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);

            string query = "SELECT * FROM HomeWork WHERE HomeWorkID = @HomeWorkID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@HomeWorkID", homeWorkID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            model.HomeWorkID = (int)reader["HomeWorkID"];
                            model.SchoolClassID = (int)reader["SchoolClassID"];
                            model.SectionID = (int)reader["SectionID"];
                            model.SubjectID = (int)reader["SubjectID"];
                            model.HomeWorkTitle = reader["HomeWorkTitle"].ToString();
                            model.HomeWorkDeadLine = (DateTime)reader["HomeWorkDeadLine"];
                            model.HomeWorkNote = reader["HomeWorkNote"].ToString();
                            model.HomeWorkAttachment = reader["HomeWorkAttachment"]?.ToString() ?? null;
                            model.Ext = reader[".Ext"]?.ToString() ?? null;
                            model.TeacherID = reader["TeacherID"].ToString();
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateHomeWork(HomeWork model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.HomeWorkAttachment))
                {
                    string[] attachmentParts = model.HomeWorkAttachment.Split(',');

                    if (attachmentParts.Length > 1)
                    {
                        string base64String = attachmentParts[1];
                        byte[] fileData = Convert.FromBase64String(base64String);

                        int fileSizeInBytes = fileData.Length;

                        if (fileSizeInBytes > maxFileSizeInBytes)
                            return Json(new { Success = false, Message = "File size exceeds the maximum allowed size." }, JsonRequestBehavior.AllowGet);
                    }
                }

                string query = "UPDATE HomeWork SET " +
                    "SchoolClassID = @SchoolClassID, SectionID = @SectionID, SubjectID = @SubjectID, " +
                    "HomeWorkTitle = @HomeWorkTitle, HomeWorkDeadLine = @HomeWorkDeadLine, " +
                    "HomeWorkNote = @HomeWorkNote ";
                query += !string.IsNullOrEmpty(model.HomeWorkAttachment) && model.HomeWorkAttachment != "null" ? ", HomeWorkAttachment = CONVERT(VARBINARY(MAX), @HomeWorkAttachment), [.Ext] = @Ext " : "";
                query += "WHERE HomeWorkID = @HomeWorkID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@HomeWorkTitle", model.HomeWorkTitle);
                        comm.Parameters.AddWithValue("@HomeWorkDeadLine", model.HomeWorkDeadLine);

                        if (!string.IsNullOrEmpty(model.HomeWorkNote))
                            comm.Parameters.AddWithValue("@HomeWorkNote", model.HomeWorkNote);
                        else
                            comm.Parameters.AddWithValue("@HomeWorkNote", DBNull.Value);

                        byte[] fileData = null;
                        if (!string.IsNullOrEmpty(model.HomeWorkAttachment) && model.HomeWorkAttachment != "null")
                        {
                            string[] attachmentParts = model.HomeWorkAttachment.Split(',');

                            if (attachmentParts.Length > 1)
                            {
                                string base64String = attachmentParts[1];
                                fileData = Convert.FromBase64String(base64String);

                                int fileSizeInBytes = fileData.Length;

                                if (fileSizeInBytes > maxFileSizeInBytes)
                                    return Json(new { Success = false, Message = "File size exceeds the maximum allowed size." }, JsonRequestBehavior.AllowGet);

                                comm.Parameters.AddWithValue("@HomeWorkAttachment", base64String);

                                // The model.Ext now is holding the file path.
                                string filePath = @"" + model.Ext + "";
                                string fileName = Path.GetFileName(filePath);
                                string fileExtension = Path.GetExtension(fileName);
                                comm.Parameters.AddWithValue("@Ext", fileExtension);
                            }
                            else
                            {
                                comm.Parameters.Add("@HomeWorkAttachment", SqlDbType.VarBinary).Value = DBNull.Value;
                            }
                        }

                        comm.Parameters.AddWithValue("@HomeWorkID", model.HomeWorkID);
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

        public ActionResult DownloadFile(int homeWorkID)
        {
            byte[] fileBytes;
            string homeWorkTitle, fileExtension;

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                string query = "SELECT HomeWorkTitle, HomeWorkAttachment, [.Ext] FROM HomeWork " +
                    "WHERE HomeWorkID = @HomeWorkID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@HomeWorkID", homeWorkID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["HomeWorkAttachment"] != DBNull.Value && reader[".Ext"] != DBNull.Value)
                            {
                                homeWorkTitle = reader["HomeWorkTitle"].ToString();
                                string base64String = reader["HomeWorkAttachment"].ToString();
                                fileBytes = Convert.FromBase64String(base64String);
                                fileExtension = reader[".Ext"].ToString();
                            }
                            else
                                return HttpNotFound();
                        }
                        else
                            return HttpNotFound();
                    }
                }
            }

            string contentType = "application/octet-stream";
            return File(fileBytes, contentType, homeWorkTitle.Replace(" ", "-") + "-HomeWork" + fileExtension);
        }

        [HttpPost]
        public JsonResult DeleteHomeWork(int homeworkID)
        {
            try
            {
                string query = "DELETE FROM HomeWork WHERE HomeWorkID = @HomeWorkID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@HomeWorkID", homeworkID);
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