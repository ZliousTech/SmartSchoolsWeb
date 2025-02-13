using Business.Base;
using Common.Helpers;
using SmartSchool.Models.Calendar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class CalendarController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        // GET: Calendar
        public ActionResult PrepareCalendar()
        {
            string query = "SELECT a.* FROM AcademicCalendar a " +
                "INNER JOIN AcademicCalendars b " +
                "ON a.CalendarID = b.CalendarID " +
                "WHERE b.SchoolID IN (1000000, " + SchoolID + ") OR (b.SchoolID = -1 AND b.CompanyID = " + CompanyID + ")";
            List<Models.Calendar.AcademicCalendar> model = new List<Models.Calendar.AcademicCalendar>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new Models.Calendar.AcademicCalendar()
                            {
                                OccasionID = int.Parse(reader["OccasionID"].ToString()),
                                ArabicHeader = reader["ArabicHeader"].ToString(),
                                EnglishHeader = reader["EnglishHeader"].ToString(),
                                ArabicDescription = reader["ArabicDescription"].ToString(),
                                EnglishDescription = reader["EnglishDescription"].ToString(),
                                OccasionType = int.Parse(reader["OccasionType"].ToString()),
                                StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                NumberofDays = int.Parse(reader["NumberofDays"].ToString()),
                                Vacation = reader["Vacation"].ToString() == "0" ? false : true,
                                CalendarID = int.Parse(reader["CalendarID"].ToString()),
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return View(model);
        }

        public ActionResult AddNewAcademicCalendar(AcademicCalendarView model)
        {
            var CountriesManager = factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll().ToList();
            model.AcademicCalendars.CountryNames = (from c in Countries
                                                    select new SelectListItem
                                                    {
                                                        Text = CurrentLanguage == Languges.English ?
                                                        (!string.IsNullOrWhiteSpace(c.EnglishName) ? c.EnglishName : c.ArabicName) :
                                                        (!string.IsNullOrWhiteSpace(c.ArabicName) ? c.ArabicName : c.EnglishName),
                                                        Value = c.ID.ToString()
                                                    }).ToList();
            model.AcademicCalendars.SchoolYear = DateTime.Now.Year.ToString() + " - " + (DateTime.Now.Year + 1).ToString();
            var OccasionTypesManager = factory.CreateOccasionTypesManager();
            var OccasionTypes = OccasionTypesManager.GetAll().ToList();
            model.AcademicCalendar.OccasionTypeNames = (from o in OccasionTypes
                                                        select new SelectListItem
                                                        {
                                                            Text = CurrentLanguage == Languges.English ?
                                                        (!string.IsNullOrWhiteSpace(o.OccasionTypeEnglishName) ? o.OccasionTypeEnglishName : o.OccasionTypeArabicName) :
                                                        (!string.IsNullOrWhiteSpace(o.OccasionTypeArabicName) ? o.OccasionTypeArabicName : o.OccasionTypeEnglishName),
                                                            Value = o.OccasionID.ToString()
                                                        }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult NewAcademicCalendar(AcademicCalendarView model)
        {
            int insertedCalendar = 0;
            string query = "";

            try
            {
                query = "INSERT INTO AcademicCalendars " +
                        "(CountryID, SchoolID, CompanyID, SchoolYear) " +
                        "VALUES (@CountryID, @SchoolID, @CompanyID, @SchoolYear);" +
                        "SELECT SCOPE_IDENTITY() AS Result";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@CountryID", model.AcademicCalendars.CountryID);
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@CompanyID", CompanyID);
                        comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year);
                        insertedCalendar = int.Parse(comm.ExecuteScalar().ToString());
                    }
                    conn.Close();
                    conn.Dispose();
                }
                if (insertedCalendar != 0)
                {
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
                    query = "INSERT INTO AcademicCalendar " +
                    "(Photo, ArabicHeader, EnglishHeader, ArabicDescription, EnglishDescription, OccasionType, " +
                    "StartingDate, NumberofDays, Vacation, CalendarID) " +
                    "VALUES (@Photo, @ArabicHeader, @EnglishHeader, @ArabicDescription, @EnglishDescription," +
                    " @OccasionType, @StartingDate, @NumberofDays, @Vacation, @CalendarID)";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@Photo", imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Holyday-event.jpg")) : imgByte);
                            comm.Parameters.AddWithValue("@ArabicHeader", model.AcademicCalendar.ArabicHeader);
                            comm.Parameters.AddWithValue("@EnglishHeader", model.AcademicCalendar.EnglishHeader);
                            comm.Parameters.AddWithValue("@ArabicDescription", model.AcademicCalendar.ArabicDescription);
                            comm.Parameters.AddWithValue("@EnglishDescription", model.AcademicCalendar.EnglishDescription);
                            comm.Parameters.AddWithValue("@OccasionType", model.AcademicCalendar.OccasionType);
                            comm.Parameters.AddWithValue("@StartingDate", model.AcademicCalendar.StartingDate);
                            comm.Parameters.AddWithValue("@NumberofDays", model.AcademicCalendar.NumberofDays);
                            comm.Parameters.AddWithValue("@Vacation", model.AcademicCalendar.Vacation);
                            comm.Parameters.AddWithValue("@CalendarID", insertedCalendar);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditAcademicCalendar(int OccasionID)
        {
            AcademicCalendarView model = new AcademicCalendarView();
            var CountriesManager = factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll().ToList();
            model.AcademicCalendars.CountryNames = (from c in Countries
                                                    select new SelectListItem
                                                    {
                                                        Text = CurrentLanguage == Languges.English ?
                                                        (!string.IsNullOrWhiteSpace(c.EnglishName) ? c.EnglishName : c.ArabicName) :
                                                        (!string.IsNullOrWhiteSpace(c.ArabicName) ? c.ArabicName : c.EnglishName),
                                                        Value = c.ID.ToString()
                                                    }).ToList();
            var OccasionTypesManager = factory.CreateOccasionTypesManager();
            var OccasionTypes = OccasionTypesManager.GetAll().ToList();
            model.AcademicCalendar.OccasionTypeNames = (from o in OccasionTypes
                                                        select new SelectListItem
                                                        {
                                                            Text = CurrentLanguage == Languges.English ?
                                                        (!string.IsNullOrWhiteSpace(o.OccasionTypeEnglishName) ? o.OccasionTypeEnglishName : o.OccasionTypeArabicName) :
                                                        (!string.IsNullOrWhiteSpace(o.OccasionTypeArabicName) ? o.OccasionTypeArabicName : o.OccasionTypeEnglishName),
                                                            Value = o.OccasionID.ToString()
                                                        }).ToList();

            string query = "";

            query = "SELECT * FROM AcademicCalendar WHERE OccasionID = " + OccasionID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.AcademicCalendar.OccasionID = OccasionID;
                            byte[] imageData = null;
                            if (!(reader["Photo"] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader["Photo"];
                                model.AcademicCalendar.Photo = imageData;
                            }
                            model.AcademicCalendar.ArabicHeader = reader["ArabicHeader"].ToString();
                            model.AcademicCalendar.EnglishHeader = reader["EnglishHeader"].ToString();
                            model.AcademicCalendar.ArabicDescription = reader["ArabicDescription"].ToString();
                            model.AcademicCalendar.EnglishDescription = reader["EnglishDescription"].ToString();
                            model.AcademicCalendar.OccasionType = int.Parse(reader["OccasionType"].ToString());
                            model.AcademicCalendar.StartingDate = DateTime.Parse(reader["StartingDate"].ToString().Split(' ')[0]);
                            model.AcademicCalendar.NumberofDays = int.Parse(reader["NumberofDays"].ToString());
                            model.AcademicCalendar.Vacation = Convert.ToBoolean(reader["Vacation"]);
                            model.AcademicCalendar.CalendarID = int.Parse(reader["CalendarID"].ToString());
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            if (model.AcademicCalendar.CalendarID != null)
            {
                query = "SELECT * FROM AcademicCalendars WHERE CalendarID = " + model.AcademicCalendar.CalendarID;
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                model.AcademicCalendars.CalendarID = int.Parse(reader["CalendarID"].ToString());
                                model.AcademicCalendars.CountryID = reader["CountryID"].ToString();
                                model.AcademicCalendars.SchoolID = int.Parse(reader["SchoolID"].ToString());
                                model.AcademicCalendars.CompanyID = int.Parse(reader["CompanyID"].ToString());
                                model.AcademicCalendars.SchoolYear = reader["SchoolYear"].ToString() + " - " +
                                    (int.Parse(reader["SchoolYear"].ToString()) + 1);
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult EditCalendar(AcademicCalendarView model)
        {
            try
            {
                if (model.AcademicCalendars.SchoolID == SchoolID && model.AcademicCalendars.CompanyID == CompanyID)
                {
                    byte[] imgByte = null, oldImg = null;
                    bool isUpdated = false;
                    string query = "";

                    query = "UPDATE AcademicCalendars SET " +
                            "CountryID = @CountryID, SchoolID = @SchoolID, " +
                            "CompanyID = @CompanyID, SchoolYear = @SchoolYear " +
                            "WHERE CalendarID = @CalendarID";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@CalendarID", model.AcademicCalendars.CalendarID);
                            comm.Parameters.AddWithValue("@CountryID", model.AcademicCalendars.CountryID);
                            comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                            comm.Parameters.AddWithValue("@CompanyID", CompanyID);
                            comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year);
                            isUpdated = comm.ExecuteNonQuery() == 1 ? true : false;
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                    query = "SELECT Photo FROM AcademicCalendar WHERE OccasionID = " + model.AcademicCalendar.OccasionID;
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
                                        model.AcademicCalendar.Photo = oldImg;
                                    }
                                }
                            }
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                    if (isUpdated)
                    {
                        query = "UPDATE AcademicCalendar SET " +
                       "Photo = @Photo, ArabicHeader = @ArabicHeader, EnglishHeader = @EnglishHeader, " +
                       "ArabicDescription = @ArabicDescription, EnglishDescription = @EnglishDescription, " +
                       "OccasionType = @OccasionType, StartingDate = @StartingDate, NumberofDays = @NumberofDays, " +
                       "Vacation = @Vacation, CalendarID = @CalendarID " +
                       "WHERE OccasionID = @OccasionID";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@OccasionID", model.AcademicCalendar.OccasionID);
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
                                comm.Parameters.AddWithValue("@Photo", imgByte == null ? model.AcademicCalendar.Photo : imgByte);
                                comm.Parameters.AddWithValue("@ArabicHeader", model.AcademicCalendar.ArabicHeader);
                                comm.Parameters.AddWithValue("@EnglishHeader", model.AcademicCalendar.EnglishHeader);
                                comm.Parameters.AddWithValue("@ArabicDescription", model.AcademicCalendar.ArabicDescription);
                                comm.Parameters.AddWithValue("@EnglishDescription", model.AcademicCalendar.EnglishDescription);
                                comm.Parameters.AddWithValue("@OccasionType", model.AcademicCalendar.OccasionType);
                                comm.Parameters.AddWithValue("@StartingDate", model.AcademicCalendar.StartingDate);
                                comm.Parameters.AddWithValue("@NumberofDays", model.AcademicCalendar.NumberofDays);
                                comm.Parameters.AddWithValue("@Vacation", model.AcademicCalendar.Vacation);
                                comm.Parameters.AddWithValue("@CalendarID", model.AcademicCalendars.CalendarID);
                                comm.ExecuteNonQuery();
                            }
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    Success = false,
                    Message = CurrentLanguage == Languges.English ?
                    "Can't Update or Delete the admin Calendars" : "لا يمكن تعديل او حذف التقويم الخاص بالإدارة"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteCalendar(int CalendarID)
        {
            try
            {
                int schoolID = 0, companyID = 0;
                string query = "";

                query = "SELECT SchoolID, CompanyID FROM AcademicCalendars WHERE CalendarID = " + CalendarID;
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                schoolID = int.Parse(reader["SchoolID"].ToString());
                                companyID = int.Parse(reader["CompanyID"].ToString());
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                if (schoolID == SchoolID && companyID == CompanyID)
                {
                    bool IsDeleted = false;

                    query = "DELETE FROM AcademicCalendars WHERE CalendarID = " + CalendarID;
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        SqlCommand comm = new SqlCommand(query, conn);
                        IsDeleted = comm.ExecuteNonQuery() == 1 ? true : false;
                        conn.Close();
                        conn.Dispose();
                    }
                    if (IsDeleted)
                    {
                        query = "DELETE FROM AcademicCalendar WHERE CalendarID = " + CalendarID;
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            SqlCommand comm = new SqlCommand(query, conn);
                            comm.ExecuteNonQuery();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    Success = false,
                    Message = CurrentLanguage == Languges.English ?
                    "Can't Update or Delete the admin Calendars" : "لا يمكن تعديل او حذف التقويم الخاص بالإدارة"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCalenadarsByMonth(int month, string staffID, string studentID)
        {
            try
            {
                string query = "";

                List<Models.Calendar.AcademicCalendar> academicCalendar = new List<Models.Calendar.AcademicCalendar>();
                query = "SELECT a.* FROM AcademicCalendar a " +
                "INNER JOIN AcademicCalendars b " +
                "ON a.CalendarID = b.CalendarID " +
                "WHERE (MONTH(a.StartingDate) = " + month + " AND YEAR(a.StartingDate) = YEAR(GETDATE())) AND " +
                "b.SchoolID IN (1000000, " + SchoolID + ") OR (b.SchoolID = -1 AND b.CompanyID = " + CompanyID + ") " +
                "ORDER BY a.StartingDate";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                academicCalendar.Add(new Models.Calendar.AcademicCalendar()
                                {
                                    Photo = (byte[])reader["Photo"],
                                    ArabicHeader = reader["ArabicHeader"].ToString(),
                                    EnglishHeader = reader["EnglishHeader"].ToString(),
                                    ArabicDescription = reader["ArabicDescription"].ToString(),
                                    EnglishDescription = reader["EnglishDescription"].ToString(),
                                    StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                });
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                List<Models.Activity.TeacherActivity> teacherActivity = new List<Models.Activity.TeacherActivity>();
                query = "SELECT * FROM TeacherActivity WHERE TeacherID = '" + staffID + "' AND " +
                    "(MONTH(StartingDate) = " + month + "  AND YEAR(StartingDate) = YEAR(GETDATE()))";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                teacherActivity.Add(new Models.Activity.TeacherActivity
                                {
                                    Photo = (byte[])reader["Photo"],
                                    ArabicHeader = reader["ArabicHeader"].ToString(),
                                    EnglishHeader = reader["EnglishHeader"].ToString(),
                                    ArabicDescription = reader["ArabicDescription"].ToString(),
                                    EnglishDescription = reader["EnglishDescription"].ToString(),
                                    StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                });
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                List<Models.Activity.TeacherActivity> studentActivity = new List<Models.Activity.TeacherActivity>();
                query = "SELECT * FROM  TeacherActivity WHERE ID IN " +
                    "(SELECT TeacherActivityID FROM TeacherStudentsActivity WHERE StudentID = '" + studentID + "') " +
                    "AND (MONTH(StartingDate) = " + month + "  AND YEAR(StartingDate) = YEAR(GETDATE()))";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                studentActivity.Add(new Models.Activity.TeacherActivity
                                {
                                    Photo = (byte[])reader["Photo"],
                                    ArabicHeader = reader["ArabicHeader"].ToString(),
                                    EnglishHeader = reader["EnglishHeader"].ToString(),
                                    ArabicDescription = reader["ArabicDescription"].ToString(),
                                    EnglishDescription = reader["EnglishDescription"].ToString(),
                                    StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                });
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true, AcademicCalendar = academicCalendar, TeacherActivity = teacherActivity, StudentActivity = studentActivity }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CalendarMore()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCalenadarsbySchoolID()
        {
            try
            {
                List<Models.Calendar.AcademicCalendar> academicCalendar = new List<Models.Calendar.AcademicCalendar>();
                string query = "SELECT a.* FROM AcademicCalendar a " +
                "INNER JOIN AcademicCalendars b " +
                "ON a.CalendarID = b.CalendarID " +
                "WHERE (YEAR(a.StartingDate) = YEAR(GETDATE())) AND " +
                "b.SchoolID IN (1000000, " + SchoolID + ") OR (b.SchoolID = -1 AND b.CompanyID = " + CompanyID + ") " +
                "ORDER BY a.StartingDate";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                academicCalendar.Add(new Models.Calendar.AcademicCalendar()
                                {
                                    OccasionID = (int)reader["OccasionID"],
                                    Photo = (byte[])reader["Photo"],
                                    ArabicHeader = reader["ArabicHeader"].ToString(),
                                    EnglishHeader = reader["EnglishHeader"].ToString(),
                                    ArabicDescription = reader["ArabicDescription"].ToString(),
                                    EnglishDescription = reader["EnglishDescription"].ToString(),
                                    StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                }); ;
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true, AcademicCalendar = academicCalendar }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetFilteredCalenadar(string eventDate, string eventHeader, string eventMonth)
        {
            try
            {
                List<Models.Calendar.AcademicCalendar> academicCalendar = new List<Models.Calendar.AcademicCalendar>();

                // Initial base query
                string query = "SELECT a.* FROM AcademicCalendar a " +
                               "INNER JOIN AcademicCalendars b ON a.CalendarID = b.CalendarID WHERE 1=1";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        // Add conditions only if parameters are not empty or null
                        if (!string.IsNullOrEmpty(eventDate))
                        {
                            query += " AND DAY(StartingDate) = DAY(@EventDate) AND MONTH(StartingDate) = MONTH(@EventDate) AND YEAR(StartingDate) = YEAR(@EventDate)";
                            comm.Parameters.AddWithValue("@EventDate", eventDate);
                        }

                        if (!string.IsNullOrEmpty(eventHeader))
                        {
                            query += " AND (ArabicHeader LIKE '%' + @EventHeader + '%' OR EnglishHeader LIKE '%' + @EventHeader + '%')";
                            comm.Parameters.AddWithValue("@EventHeader", eventHeader);
                        }

                        if (!string.IsNullOrEmpty(eventMonth))
                        {
                            string[] splitEventMonth = eventMonth.Split('-');
                            query += " AND YEAR(StartingDate) = @EventYear AND MONTH(StartingDate) = @EventMonth";
                            comm.Parameters.AddWithValue("@EventYear", splitEventMonth[0]);
                            comm.Parameters.AddWithValue("@EventMonth", splitEventMonth[1]);
                        }

                        // Add SchoolID and CompanyID conditions
                        query += " AND (b.SchoolID IN (1000000, @SchoolID) OR (b.SchoolID = -1 AND b.CompanyID = @CompanyID)) ORDER BY a.StartingDate";

                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@CompanyID", CompanyID);

                        comm.CommandText = query; // Update the command text with the modified query

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                academicCalendar.Add(new Models.Calendar.AcademicCalendar()
                                {
                                    OccasionID = (int)reader["OccasionID"],
                                    Photo = (byte[])reader["Photo"],
                                    ArabicHeader = reader["ArabicHeader"].ToString(),
                                    EnglishHeader = reader["EnglishHeader"].ToString(),
                                    ArabicDescription = reader["ArabicDescription"].ToString(),
                                    EnglishDescription = reader["EnglishDescription"].ToString(),
                                    StartingDate = DateTime.Parse(reader["StartingDate"].ToString()),
                                });
                            }
                        }
                    }
                    conn.Close();
                }

                return Json(new { Success = true, AcademicCalendar = academicCalendar });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }

        public ActionResult CalendarDetails(int OccasionID)
        {
            Models.Calendar.AcademicCalendar
                                model = new Models.Calendar.AcademicCalendar();
            string query = "SELECT * FROM AcademicCalendar WHERE OccasionID = " + OccasionID;

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            model.OccasionID = (int)reader["OccasionID"];
                            model.Photo = (byte[])reader["Photo"];
                            model.ArabicHeader = reader["ArabicHeader"].ToString();
                            model.EnglishHeader = reader["EnglishHeader"].ToString();
                            model.ArabicDescription
                                = reader["ArabicDescription"].ToString();
                            model.EnglishDescription
                                = reader["EnglishDescription"].ToString();
                            model.StartingDate
                                = DateTime.Parse(reader["StartingDate"].ToString());
                            model.NumberofDays = (int)reader["NumberofDays"];
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return View(model);
        }
    }
}