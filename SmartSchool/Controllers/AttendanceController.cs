using Business.Base;
using Common.Helpers;
using DataAccess;
using Microsoft.Ajax.Utilities;
using Microsoft.Data.OData;
using Microsoft.Graph;
using Newtonsoft.Json;
using Objects;
using SmartSchool.Models.Attendance;
using SmartSchool.Models.TeacherStudentAttendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Configuration;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using static Common.clsenumration;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace SmartSchool.Controllers
{
    public class AttendanceController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities _context = new SmartSchoolsEntities();
        private List<string> weekdays = new List<string>() { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" };
        private Dictionary<int, string> sessionProperties = new Dictionary<int, string>
        {
            { 1, "FirstSession" },
            { 2, "SecondSession" },
            { 3, "ThirdSession" },
            { 4, "FourthSession" },
            { 5, "FifthSession" },
            { 6, "SixthSession" },
            { 7, "SeventhSession" },
            { 8, "EighthSession" }
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManualAttendance()
        {
            return View();
        }

        public ActionResult AutomaticAttendance()
        {
            return View();
        }

        private List<SelectListItem> PopulateSchoolClasses()
        {
            var SchoolClassesManager = factory.CreateSchoolClasssManager();
            var SchoolClasses = SchoolClassesManager.Find(s => s.SchoolID == SchoolID).ToList();
            var classesList = (from s in SchoolClasses
                               select new SelectListItem
                               {
                                   Text = CurrentLanguage == Languges.English ? s.SchoolClassEnglishName : s.SchoolClassArabicName,
                                   Value = s.SchoolClassID.ToString()
                               }).ToList();
            return classesList;
        }

        public ActionResult PrepareAttendance(PrepareAttendanceView model)
        {
            var SectionID = model.Attendance.SectionID;
            var AttendanceType = model.Attendance.AttendanceType;
            model.Attendance.SchoolClassesList = PopulateSchoolClasses();
            model.Attendance.AttendanceTypeTextName = (from a in _context.AttendanceTypes
                                                       select new SelectListItem
                                                       {
                                                           Text = CurrentLanguage == Languges.English ?
                                                               a.AttedanceTypeEnglishText : a.AttedanceTypeArabicText,
                                                           Value = a.AttendanceTypeID.ToString()
                                                       }).ToList();
            var StudentsManager = factory.CreateStudentsManager();
            var Students = StudentsManager.GetAll().ToList();
            var StudentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
            var StudentsSchoolDetails = StudentSchoolDetailsManager.Find(s => s.SchoolID == SchoolID && SectionID != null ? s.SectionID == SectionID : true).ToList();
            var SectionsManager = factory.CreateSectionsManager();
            var Sections = SectionsManager.Find(s => SectionID != null ? s.SectionID == SectionID : true).ToList();
            var SchoolClassesManager = factory.CreateSchoolClasssManager();
            var SchoolClasses = SchoolClassesManager.Find(s => s.SchoolID == SchoolID).ToList();
            var students = from a in Students
                           join b in StudentsSchoolDetails
                           on a.StudentID equals b.StudentID
                           join c in Sections
                           on b.SectionID equals c.SectionID
                           join d in SchoolClasses
                           on c.SchoolClassID equals d.SchoolClassID
                           where (b.SchoolID == SchoolID && SectionID != null ? b.SectionID == SectionID : true)
                           select new TeacherStudentAttendances()
                           {
                               StudentID = a.StudentID,
                               StudentName = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(a.StudentEnglishName) ? a.StudentEnglishName : a.StudentArabicName) : a.StudentArabicName,
                               StudentEnglishName = a.StudentEnglishName,
                               StudentClass = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(d.SchoolClassEnglishName) ? d.SchoolClassEnglishName : d.SchoolClassArabicName) : d.SchoolClassArabicName,
                               StudentEnglisClass = d.SchoolClassEnglishName,
                               StudentSection = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(c.SectionEnglishName) ? c.SectionEnglishName : c.SectionArabicName) : c.SectionArabicName,
                               StudentEnglisSection = c.SectionEnglishName,
                               StudentTotalAbsence =
                               (
                                from attendance in _context.Attendances
                                where attendance.StudentID == a.StudentID &&
                                (attendance.AttendanceType == 3)
                                select attendance
                               ).Count(),
                               StudenTotalPartialAttendace =
                               (
                                from attendance in _context.Attendances
                                where attendance.StudentID == a.StudentID &&
                                (attendance.AttendanceType == 2)
                                select attendance
                               ).Count()
                           };

            model.StudentAttendances = students.Where(s => AttendanceType == 0 || AttendanceType == null ? true : (AttendanceType == 1 ? s.StudentTotalAbsence == 0 : s.StudentTotalAbsence > 0)).ToList();
            return View(model);
        }


        public ActionResult PrepareAutoAttendance(string date)
        {
            date = string.IsNullOrEmpty(date) ? DateTime.Now.ToString("MM-dd-yyyy") : date;
            string formattedDateValue = "";
            if (!string.IsNullOrEmpty(date))
            {
                DateTime parsedDate;
                if (DateTime.TryParseExact(date, "MM-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    formattedDateValue = parsedDate.ToString("yyyy-MM-dd");
                }
            }
            ViewBag.Date = formattedDateValue;
            IEnumerable <ReviewAutoAttendanceVM> model = GetUsersDailyAttendance(date, "true");

            if (model != null && model.Any())
            {
                string staffIdList = string.Join(",", model.Select(m => $"'{m.staffID}'"));
                string query = $"SELECT StudentID, StudentArabicName, StudentEnglishName FROM Student WHERE StudentID IN ({staffIdList})";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            var studentNames = new Dictionary<string, string>();
                            while (reader.Read())
                            {
                                string studentID = reader["StudentID"].ToString();
                                string studentName = CurrentLanguage == Languges.English ?
                                    reader["StudentEnglishName"].ToString() : reader["StudentArabicName"].ToString();
                                studentNames[studentID] = studentName;
                            }

                            foreach (var item in model)
                            {
                                if (studentNames.ContainsKey(item.staffID))
                                {
                                    item.staffID = studentNames[item.staffID];
                                }
                            }
                        }
                    }
                } 
            }

            return View(model);
        }

        public ActionResult AttendanceTraking(string date)
        {
            if (string.IsNullOrEmpty(date) || date.Equals("aN/aN/NaN"))
            {
                var currentDate = DateTime.Now;
                ViewBag.Date = currentDate.ToString("yyyy-MM-dd");
                date = currentDate.ToString("MM/dd/yyyy");
            }
            else
            {
                DateTime formatedDate;
                if(DateTime.TryParseExact(date, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out formatedDate))
                {
                    ViewBag.Date = formatedDate.ToString("yyyy-MM-dd");
                }
            }

            IEnumerable<AttendanceTrackingVM> model = GetUsersRecentFollowUpData(SchoolID.ToString(), date);

            return View(model);
        }

        public ActionResult TrakingDetails(string userId, string studentName, string tagId, string fromDate, string toDate)
        {
            var currentDate = DateTime.Now;

            if (string.IsNullOrEmpty(fromDate) || fromDate.Equals("aN/aN/NaN"))
            {
                ViewBag.FromDate = currentDate.ToString("yyyy-MM-dd");
                fromDate = currentDate.ToString("MM/dd/yyyy");
            }
            else
            {
                DateTime formatedDate;
                if (DateTime.TryParseExact(fromDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out formatedDate))
                {
                    ViewBag.FromDate = formatedDate.ToString("yyyy-MM-dd");
                }
            }
            if (string.IsNullOrEmpty(toDate) || toDate.Equals("aN/aN/NaN"))
            {
                ViewBag.ToDate = currentDate.ToString("yyyy-MM-dd");
                toDate = currentDate.ToString("MM/dd/yyyy");
            }
            else
            {
                DateTime formatedDate;
                if (DateTime.TryParseExact(toDate, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out formatedDate))
                {
                    ViewBag.ToDate = formatedDate.ToString("yyyy-MM-dd");
                }
            }
            userId = DecryptURL(userId);
            IEnumerable<TrakingDetailsVM> model = GetUserFollowUpData(userId, fromDate, toDate);
            ViewBag.StudentName = studentName;
            ViewBag.TagId = tagId;
            ViewBag.UserId = userId;

            return View(model);
        }

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

        public ActionResult IndexAttendanceByTeacher(string staffID)
        {
            PrepareAttendanceView model = new PrepareAttendanceView();
            model.StaffID = staffID;
            return View(model);
        }

        public string GetCountryTimeZoneDateTimeStr()
        {
            try
            {
                var schoolBranch = factory.CreateSchoolBranchsManager().Find(s => s.SchoolID == SchoolID).FirstOrDefault();
                var countryName = factory.CreateCountrysManager().Find(c => c.ID == schoolBranch.Country).FirstOrDefault()?.EnglishName;

                using (var client = new HttpClient())
                {
                    string SmartTimeAPIUri = WebConfigurationManager.AppSettings["SmartTimeAPIUri"].ToString();
                    client.BaseAddress = new Uri($"{SmartTimeAPIUri}/CountriesTimeZones/");
                    //client.BaseAddress = new Uri($"{SmartTimeBaseUri.Uri()}/CountriesTimeZones/");
                    var endpoint = $"GetTimeZoneByID/{countryName} Standard Time";

                    var response = client.GetAsync(endpoint).Result;

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonString = response.Content.ReadAsStringAsync().Result;
                        dynamic jsonResult = JsonConvert.DeserializeObject(jsonString);

                        if (jsonResult != null && jsonResult.status == "true")
                        {
                            return jsonResult.data.dateTimeStr;
                        }
                    }

                    return string.Empty; // Return an empty string if there's no valid response or if the response is not successful
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public ActionResult PrepareAttendanceByTeacher(PrepareAttendanceView model, string staffID)
        {
            model.StaffID = staffID;
            var SectionID = model.Attendance.SectionID;
            var AttendanceType = model.Attendance.AttendanceType;
            model.Attendance.SchoolClassesList = PopulateTeacherSchoolClasses(model.StaffID);
            model.Attendance.AttendanceTypeTextName = (from a in _context.AttendanceTypes
                                                       select new SelectListItem
                                                       {
                                                           Text = CurrentLanguage == Languges.English ?
                                                               a.AttedanceTypeEnglishText : a.AttedanceTypeArabicText,
                                                           Value = a.AttendanceTypeID.ToString()
                                                       }).ToList();

            var StudentsManager = factory.CreateStudentsManager();
            var Students = StudentsManager.GetAll().ToList();

            var StudentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
            var StudentsSchoolDetails = StudentSchoolDetailsManager.Find(s => s.SchoolID == SchoolID).ToList();

            var timeTableItemsManager = factory.CreateTimetableItemsManager();
            var teacherTimeTableItems = timeTableItemsManager.Find(t => t.TeacherID == model.StaffID).ToList();

            var SectionsManager = factory.CreateSectionsManager();
            var Sections = SectionsManager.Find(s => s.SchoolID == SchoolID).ToList();

            var SchoolClassesManager = factory.CreateSchoolClasssManager();
            var SchoolClasses = SchoolClassesManager.Find(s => s.SchoolID == SchoolID).ToList();

            var SystemSettingsperSchoolsManager = factory.CreateSystemSettingsperSchoolsManager();
            var SystemSettingsperSchool = SystemSettingsperSchoolsManager.Find(s => s.SchoolID == SchoolID).FirstOrDefault();

            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var SchoolSettings = SchoolSettingsManager.Find(s => s.SchoolID == SchoolID).FirstOrDefault();

            var currentTime = GetCountryTimeZoneDateTimeStr();


            if (SectionID == null && !string.IsNullOrEmpty(currentTime))
            {
                List<TeacherStudentAttendances> students = new List<TeacherStudentAttendances>();
                string TimetableType = SystemSettingsperSchool.TimetableType == -1 ?
                    "ManualTimetable" : "AutomaticTimetable";

                /* This query Getting the time from Server */
                //string query = "SELECT a.StudentID, " +
                //    "a.StudentEnglishName, a.StudentArabicName, d.SchoolClassEnglishName, " +
                //    "d.SchoolClassArabicName, c.SectionID, c.SectionEnglishName, c.SectionArabicName, " +
                //    "f.SessionDayOrder " +
                //    "FROM Student a " +
                //    "INNER JOIN StudentSchoolDetails b " +
                //    "ON a.StudentID = b.StudentID " +
                //    "INNER JOIN Sections c " +
                //    "ON b.SectionID = c.SectionID " +
                //    "INNER JOIN SchoolClasses d " +
                //    "ON c.SchoolClassID = d.SchoolClassID " +
                //    "INNER JOIN " + TimetableType + " e " +
                //    "ON c.SectionID = e.SectionID " +
                //    "INNER JOIN [Sessions] f " +
                //    "ON f.SessionID = e.SessionID " +
                //    "WHERE e.TeacherID = @TeacherID " +
                //    "AND CONVERT(VARCHAR(8), GETDATE(), 108) >= CONVERT(VARCHAR(8), f.SessionTime, 108) " +
                //    "AND CONVERT(VARCHAR(8), GETDATE(), 108) <= CONVERT(VARCHAR(8), DATEADD(MINUTE, " + SchoolSettings.SessionDuration + ", " +
                //    "f.SessionTime),  108) " +
                //    "AND DATENAME(WEEKDAY, f.WeekDay - 1) = DATENAME(WEEKDAY, GETDATE());";

                /* This query Getting the time from Api */
                string query = "SELECT a.StudentID, " +
                    "a.StudentEnglishName, a.StudentArabicName, d.SchoolClassEnglishName, " +
                    "d.SchoolClassArabicName, c.SectionID, c.SectionEnglishName, c.SectionArabicName, " +
                    "f.SessionDayOrder " +
                    "FROM Student a " +
                    "INNER JOIN StudentSchoolDetails b " +
                    "ON a.StudentID = b.StudentID " +
                    "INNER JOIN Sections c " +
                    "ON b.SectionID = c.SectionID " +
                    "INNER JOIN SchoolClasses d " +
                    "ON c.SchoolClassID = d.SchoolClassID " +
                    "INNER JOIN " + TimetableType + " e " +
                    "ON c.SectionID = e.SectionID " +
                    "INNER JOIN [Sessions] f " +
                    "ON f.SessionID = e.SessionID " +
                    "WHERE e.TeacherID = @TeacherID " +
                    "AND FORMAT(CONVERT(datetime, '" + currentTime + "', 101), 'HH:mm:ss') >= CONVERT(VARCHAR(8), f.SessionTime, 108) " +
                    "AND FORMAT(CONVERT(datetime, '" + currentTime + "', 101), 'HH:mm:ss') <= CONVERT(VARCHAR(8), DATEADD(MINUTE, " + SchoolSettings.SessionDuration + ", " +
                    "f.SessionTime),  108) " +
                    "AND DATENAME(WEEKDAY, f.WeekDay - 1) = DATENAME(WEEKDAY, GETDATE());";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@TeacherID", model.StaffID);
                        comm.Parameters.AddWithValue("@SessionDuration", SchoolSettings.SessionDuration);

                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                students.Add(new TeacherStudentAttendances()
                                {
                                    StudentID = reader["StudentID"].ToString(),
                                    StudentName = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(reader["StudentEnglishName"].ToString()) ? reader["StudentEnglishName"].ToString() : reader["StudentArabicName"].ToString()) : reader["StudentArabicName"].ToString(),
                                    StudentEnglishName = reader["StudentEnglishName"].ToString(),
                                    StudentClass = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(reader["SchoolClassEnglishName"].ToString()) ? reader["SchoolClassEnglishName"].ToString() : reader["SchoolClassArabicName"].ToString()) : reader["SchoolClassArabicName"].ToString(),
                                    StudentEnglisClass = reader["SchoolClassEnglishName"].ToString(),
                                    StudentSection = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(reader["SectionEnglishName"].ToString()) ? reader["SectionEnglishName"].ToString() : reader["SectionArabicName"].ToString()) : reader["SectionArabicName"].ToString(),
                                    StudentEnglisSection = reader["SectionEnglishName"].ToString(),
                                    SectionID = (int)reader["SectionID"],
                                });
                                model.SessionDayOrder = (int)reader["SessionDayOrder"];
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                var distinctStudents = students.GroupBy(s => s.StudentID).Select(s => s.First()).ToList();
                distinctStudents = SectionID == null ? distinctStudents : distinctStudents.Where(s => s.SectionID == SectionID).ToList();
                model.StudentAttendances = distinctStudents.Where(s => AttendanceType == 0 || AttendanceType == null ? true : (AttendanceType == 1 ? s.StudentTotalAbsence == 0 : s.StudentTotalAbsence > 0)).ToList();
            }
            else
            {
                var students = from a in Students
                               join b in StudentsSchoolDetails
                               on a.StudentID equals b.StudentID
                               join c in Sections
                               on b.SectionID equals c.SectionID
                               join d in SchoolClasses
                               on c.SchoolClassID equals d.SchoolClassID
                               join e in teacherTimeTableItems
                               on c.SectionID equals e.SectionID
                               select new TeacherStudentAttendances()
                               {
                                   StudentID = a.StudentID,
                                   StudentName = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(a.StudentEnglishName) ? a.StudentEnglishName : a.StudentArabicName) : a.StudentArabicName,
                                   StudentEnglishName = a.StudentEnglishName,
                                   StudentClass = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(d.SchoolClassEnglishName) ? d.SchoolClassEnglishName : d.SchoolClassArabicName) : d.SchoolClassArabicName,
                                   StudentEnglisClass = d.SchoolClassEnglishName,
                                   StudentSection = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(c.SectionEnglishName) ? c.SectionEnglishName : c.SectionArabicName) : c.SectionArabicName,
                                   StudentEnglisSection = c.SectionEnglishName,
                                   SectionID = c.SectionID,
                               };
                var distinctStudents = students.GroupBy(s => s.StudentID).Select(s => s.First()).ToList();
                distinctStudents = SectionID == null ? distinctStudents : distinctStudents.Where(s => s.SectionID == SectionID).ToList();
                model.StudentAttendances = distinctStudents.Where(s => AttendanceType == 0 || AttendanceType == null ? true : (AttendanceType == 1 ? s.StudentTotalAbsence == 0 : s.StudentTotalAbsence > 0)).ToList();
            }

            return View(model);
        }

        public ActionResult ReviewAttendance(PrepareAttendanceView model)
        {
            var SectionID = model.Attendance.SectionID;
            var AttendanceType = model.Attendance.AttendanceType;
            if (model.Attendance.AttendanceDate == null)
                model.Attendance.AttendanceDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            else
                model.Attendance.AttendanceDate = DateTime.Parse(model.Attendance.AttendanceDate).ToString("dd/MM/yyyy");
            model.Attendance.SchoolClassesList = PopulateSchoolClasses();
            model.Attendance.AttendanceTypeTextName = (from a in _context.AttendanceTypes
                                                       select new SelectListItem
                                                       {
                                                           Text = CurrentLanguage == Languges.English ?
                                                               a.AttedanceTypeEnglishText : a.AttedanceTypeArabicText,
                                                           Value = a.AttendanceTypeID.ToString()
                                                       }).ToList();

            var Attendance = factory.CreateAttendancesManager().Find(a => a.SchoolID == SchoolID && a.SchoolYear == DateTime.Now.Year.ToString() && a.AttendanceDate == model.Attendance.AttendanceDate).ToList();
            var Students = factory.CreateStudentsManager().GetAll().ToList();
            var StudentSchoolDetails = factory.CreateStudentSchoolDetailsManager().Find(d => d.SchoolID == SchoolID && SectionID == null ? true : d.SectionID == SectionID).ToList();
            var SchoolClasses = factory.CreateSchoolClasssManager().Find(c => c.SchoolID == SchoolID).ToList();
            var Sections = factory.CreateSectionsManager().Find(s => s.SchoolID == SchoolID && SectionID == null ? true : s.SectionID == SectionID).ToList();

            var students = (from a in Attendance
                            join st in Students
                            on a.StudentID equals st.StudentID
                            join d in StudentSchoolDetails
                            on st.StudentID equals d.StudentID
                            join s in Sections
                            on d.SectionID equals s.SectionID
                            join c in SchoolClasses
                            on s.SchoolClassID equals c.SchoolClassID
                            select new TeacherStudentAttendances
                            {
                                StudentID = a.StudentID,
                                StudentName = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(st.StudentEnglishName) ? st.StudentEnglishName : st.StudentArabicName) : st.StudentArabicName,
                                StudentClass = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(c.SchoolClassEnglishName) ? c.SchoolClassEnglishName : c.SchoolClassArabicName) : c.SchoolClassArabicName,
                                StudentSection = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(s.SectionEnglishName) ? s.SectionEnglishName : s.SectionArabicName) : s.SectionArabicName,
                                StudentTotalAbsence = (
                                    from attendance in _context.Attendances
                                    where attendance.StudentID == a.StudentID &&
                                        (attendance.AttendanceType == 3)
                                    select attendance
                                ).Count(),
                                StudenTotalPartialAttendace =
                               (
                                from attendance in _context.Attendances
                                where attendance.StudentID == a.StudentID &&
                                (attendance.AttendanceType == 2)
                                select attendance
                               ).Count()
                            }).ToList();

            model.StudentAttendances = students.DistinctBy(s => s.StudentID).Where(s => AttendanceType == 0 || AttendanceType == null ? true : (AttendanceType == 1 ? s.StudentTotalAbsence == 0 : s.StudentTotalAbsence > 0)).ToList();
            return View(model);
        }

        public ActionResult ReviewAttendanceByTeacher(PrepareAttendanceView model, string staffID)
        {
            model.StaffID = staffID;
            var SectionID = model.Attendance.SectionID;
            var AttendanceType = model.Attendance.AttendanceType;
            if (model.Attendance.AttendanceDate == null)
                model.Attendance.AttendanceDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
            else
                model.Attendance.AttendanceDate = DateTime.Parse(model.Attendance.AttendanceDate).ToString("dd/MM/yyyy");
            model.Attendance.SchoolClassesList = PopulateTeacherSchoolClasses(staffID);
            model.Attendance.AttendanceTypeTextName = (from a in _context.AttendanceTypes
                                                       select new SelectListItem
                                                       {
                                                           Text = CurrentLanguage == Languges.English ?
                                                               a.AttedanceTypeEnglishText : a.AttedanceTypeArabicText,
                                                           Value = a.AttendanceTypeID.ToString()
                                                       }).ToList();
            var Attendance = factory.CreateAttendancesManager().Find(a => a.SchoolID == SchoolID && a.SchoolYear == DateTime.Now.Year.ToString() && a.AttendanceDate == model.Attendance.AttendanceDate).ToList();
            var Students = factory.CreateStudentsManager().GetAll().ToList();
            var StudentSchoolDetails = factory.CreateStudentSchoolDetailsManager().Find(d => d.SchoolID == SchoolID && SectionID == null ? true : d.SectionID == SectionID).ToList();
            var SchoolClasses = factory.CreateSchoolClasssManager().Find(c => c.SchoolID == SchoolID).ToList();
            var Sections = factory.CreateSectionsManager().Find(s => s.SchoolID == SchoolID && SectionID == null ? true : s.SectionID == SectionID).ToList();

            var students = (from a in Attendance
                            join st in Students
                            on a.StudentID equals st.StudentID
                            join d in StudentSchoolDetails
                            on st.StudentID equals d.StudentID
                            join s in Sections
                            on d.SectionID equals s.SectionID
                            join c in SchoolClasses
                            on s.SchoolClassID equals c.SchoolClassID
                            select new TeacherStudentAttendances
                            {
                                StudentID = a.StudentID,
                                StudentName = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(st.StudentEnglishName) ? st.StudentEnglishName : st.StudentArabicName) : st.StudentArabicName,
                                StudentClass = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(c.SchoolClassEnglishName) ? c.SchoolClassEnglishName : c.SchoolClassArabicName) : c.SchoolClassArabicName,
                                StudentSection = CurrentLanguage == Languges.English ? (!string.IsNullOrWhiteSpace(s.SectionEnglishName) ? s.SectionEnglishName : s.SectionArabicName) : s.SectionArabicName,
                                StudentTotalAbsence = (
                                    from attendance in _context.Attendances
                                    where attendance.StudentID == a.StudentID &&
                                        (attendance.AttendanceType == 3)
                                    select attendance
                                ).Count(),
                                StudenTotalPartialAttendace =
                               (
                                from attendance in _context.Attendances
                                where attendance.StudentID == a.StudentID &&
                                (attendance.AttendanceType == 2)
                                select attendance
                               ).Count()
                            }).ToList();

            model.StudentAttendances = students.DistinctBy(s => s.StudentID).Where(s => AttendanceType == 0 || AttendanceType == null ? true : (AttendanceType == 1 ? s.StudentTotalAbsence == 0 : s.StudentTotalAbsence > 0)).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult GetSchoolClasses()
        {
            var SchoolClasses = GetSchoolClassesBySchoolID(SchoolID).OrderBy(sc => sc.id);
            return Json(SchoolClasses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSections(int SchoolClassID)
        {
            var Sections = GetSectionsBySchoolClassID(SchoolClassID);
            return Json(Sections, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetTeacherDays(string staffID, string sectionID)
        {
            var schoolyear = factory.CreateSystemSettingsManager().GetAll().FirstOrDefault().CurrentAcademicYear;
            var timeTableItems = factory.CreateTimetableItemsManager()
                .Find(t => t.TeacherID == staffID && t.SectionID.ToString() == sectionID).ToList();
            var manualTimetables = factory.CreateManualTimetablesManager()
                .Find(m => m.SchoolID == SchoolID && m.SchoolYear == schoolyear
                && m.TeacherID == staffID && m.SectionID.ToString() == sectionID).ToList();
            var sessions = factory.CreateSessionsManager()
                .Find(s => s.SchoolID == SchoolID && s.SchoolYear == schoolyear
                && s.SectionID.ToString() == sectionID).ToList();

            var teachertableinfo = (from a in timeTableItems
                                    join b in manualTimetables
                                    on a.TeacherID equals b.TeacherID
                                    join c in sessions
                                    on b.SessionID equals c.SessionID
                                    where a.TeacherID == staffID
                                    && a.SectionID.ToString() == sectionID
                                    select new SelectListItem()
                                    {
                                        Text = R.GetResource(weekdays[(int)c.WeekDay]),
                                        Value = c.WeekDay.ToString()
                                    }).DistinctBy(t => t.Value).OrderBy(t => t.Value).ToList();
            return Json(teachertableinfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeacherSessionOrders(string staffID, string sectionID, string weekDay)
        {
            var schoolyear = factory.CreateSystemSettingsManager().GetAll().FirstOrDefault().CurrentAcademicYear;
            var timeTableItems = factory.CreateTimetableItemsManager()
                .Find(t => t.TeacherID == staffID && t.SectionID.ToString() == sectionID).ToList();
            var manualTimetables = factory.CreateManualTimetablesManager()
                .Find(m => m.SchoolID == SchoolID && m.SchoolYear == schoolyear
                && m.TeacherID == staffID && m.SectionID.ToString() == sectionID).ToList();
            var sessions = factory.CreateSessionsManager()
                .Find(s => s.SchoolID == SchoolID && s.SchoolYear == schoolyear
                && s.SectionID.ToString() == sectionID).ToList();

            var teachertableinfo = (from a in timeTableItems
                                    join b in manualTimetables
                                    on a.TeacherID equals b.TeacherID
                                    join c in sessions
                                    on b.SessionID equals c.SessionID
                                    where a.TeacherID == staffID
                                    && a.SectionID.ToString() == sectionID
                                    && c.WeekDay.ToString() == weekDay
                                    select new SelectListItem()
                                    {
                                        Text = c.SessionDayOrder.ToString(),
                                        Value = c.SessionDayOrder.ToString()
                                    }).DistinctBy(t => t.Value).OrderBy(t => t.Value).ToList();
            return Json(teachertableinfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddAttendance(string StudentID)
        {
            Attendances model = new Attendances();
            model.StudentID = Decrypt(StudentID);
            var AttendanceTypesManager = factory.CreateAttendanceTypesManager();
            var AttendanceTypes = AttendanceTypesManager.GetAll().ToList();
            model.AttendanceTypeTextName = (from a in AttendanceTypes
                                            select new SelectListItem
                                            {
                                                Text = CurrentLanguage == Languges.English ?
                                                    a.AttedanceTypeEnglishText : a.AttedanceTypeArabicText,
                                                Value = a.AttendanceTypeID.ToString()
                                            }).ToList();
            var AbsenceReasonsManager = factory.CreateAbsenceReasonsManager();
            var AbsenceReasons = AbsenceReasonsManager.GetAll().ToList();
            model.AbsenceReasonTextName = (from a in AbsenceReasons
                                           select new SelectListItem
                                           {
                                               Text = CurrentLanguage == Languges.English ?
                                                    a.AbsenceReasonEnglishText : a.AbsenceReasonArabicText,
                                               Value = a.AbsenceReasonID.ToString()
                                           }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddAttendance(Attendance model)
        {
            try
            {
                string schoolEnglishName = string.Empty;
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    string statement = "SELECT SchoolEnglishName From SchoolBranches WHERE SchoolID = @SchoolID";
                    using (SqlCommand comm = new SqlCommand(statement, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                schoolEnglishName = reader[0].ToString();
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                var AttendanceManager = factory.CreateAttendancesManager();
                var studentID = Decrypt(model.StudentID);
                var attendanceDate = model.AttendanceDate;
                var studentAttendance = AttendanceManager.Find(a => a.StudentID == studentID
                        && a.AttendanceDate == attendanceDate).FirstOrDefault();
                if (studentAttendance != null)
                {
                    studentAttendance.SchoolID = SchoolID;
                    studentAttendance.SchoolYear = DateTime.UtcNow.Year.ToString();
                    studentAttendance.StudentID = studentID;
                    studentAttendance.AttendanceDate = attendanceDate;
                    studentAttendance.AttendanceType = model.AttendanceType;
                    studentAttendance.AbsenceReason = model.AbsenceReason;
                    studentAttendance.Description = model.Description;
                    studentAttendance.ParentInformed = model.ParentInformed;
                    studentAttendance.ParentMessage = model.ParentMessage;
                    studentAttendance.AttendanceNote = model.AttendanceNote;
                    studentAttendance.FirstSession = model.FirstSession;
                    studentAttendance.SecondSession = model.SecondSession;
                    studentAttendance.ThirdSession = model.ThirdSession;
                    studentAttendance.FourthSession = model.FourthSession;
                    studentAttendance.FifthSession = model.FifthSession;
                    studentAttendance.SixthSession = model.SixthSession;
                    studentAttendance.SeventhSession = model.SeventhSession;
                    studentAttendance.EighthSession = model.EighthSession;
                    if (model.FirstSession == 1 && model.SecondSession == 1 && model.ThirdSession == 1 &&
                        model.FourthSession == 1 && model.FifthSession == 1 && model.SixthSession == 1 &&
                        model.SeventhSession == 1 && model.EighthSession == 1)
                    {
                        studentAttendance.AttendanceType = 3; // 3 for Absent.
                    }
                    else
                        studentAttendance.AttendanceType = 2; // 2 for Partial Attendace.

                    AttendanceManager.Update(studentAttendance);
                }
                else
                {
                    Attendance attendance = new Attendance();
                    attendance.SchoolID = SchoolID;
                    attendance.SchoolYear = DateTime.UtcNow.Year.ToString();
                    attendance.StudentID = studentID;
                    attendance.AttendanceDate = attendanceDate;
                    attendance.AttendanceType = model.AttendanceType;
                    attendance.AbsenceReason = model.AbsenceReason;
                    attendance.Description = model.Description;
                    attendance.ParentInformed = model.ParentInformed;
                    attendance.ParentMessage = model.ParentMessage;
                    attendance.AttendanceNote = model.AttendanceNote;
                    attendance.FirstSession = model.FirstSession;
                    attendance.SecondSession = model.SecondSession;
                    attendance.ThirdSession = model.ThirdSession;
                    attendance.FourthSession = model.FourthSession;
                    attendance.FifthSession = model.FifthSession;
                    attendance.SixthSession = model.SixthSession;
                    attendance.SeventhSession = model.SeventhSession;
                    attendance.EighthSession = model.EighthSession;
                    if (model.FirstSession == 1 && model.SecondSession == 1 && model.ThirdSession == 1 &&
                       model.FourthSession == 1 && model.FifthSession == 1 && model.SixthSession == 1 &&
                       model.SeventhSession == 1 && model.EighthSession == 1)
                    {
                        attendance.AttendanceType = 3; // 3 for Absent.
                    }
                    else
                        attendance.AttendanceType = 2; // 2 for Partial Attendace.

                    AttendanceManager.Add(attendance);
                }

                if (model.ParentInformed == 1)
                {
                    string query = "";
                    dynamic guardian = null;
                    string studentName = string.Empty;
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        query = "SELECT DeviceRegistrationCode, OwnerID FROM DeviceRegistrar WHERE OwnerID = " +
                            "(SELECT DISTINCT CAST(GuardianID AS NVARCHAR(6)) FROM StudentGuardDetails WHERE StudentID = @StudentID);" +
                            "SELECT StudentEnglishName From Student WHERE StudentID = @StudentID";
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", studentID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    guardian = new
                                    {
                                        guardiantoken = reader["DeviceRegistrationCode"].ToString(),
                                        guardianID = reader["OwnerID"].ToString()
                                    };
                                }

                                reader.NextResult();

                                if (reader.Read())
                                {
                                    studentName = reader["StudentEnglishName"].ToString();
                                }
                            }
                        }

                    }
                    if (guardian != null)
                    {
                        string notificationBuilder = $"{studentName} \nis absent by {schoolEnglishName} school \n";
                        notificationBuilder += !string.IsNullOrEmpty(model.ParentMessage) ? $"Message to parent: \n\"{model.ParentMessage}\"\n" : "";
                        notificationBuilder += $"{Convert.ToDateTime(attendanceDate).ToString("yyyy-MM-dd")}";

                        query = "INSERT INTO Notifications " +
                            "(DesitinationID, DestinationType, NotificationTypeID, NotificationText, SourceType, [Status]) " +
                            "VALUES ('" + guardian.guardianID + "', 19, " + (int)NotificationsTypes.Attendance + ", N'" + notificationBuilder + "', 15, 2);";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.ExecuteNonQuery();
                            }
                        }

                        var data = new
                        {
                            to = guardian.guardiantoken,

                            data = new
                            {
                                type = "Attendance",
                                title = "Attendance Notification",
                                message = notificationBuilder
                            }
                        };
                        using (var client = new WebClient())
                        {
                            // Supprot arabic text 
                            var dataString = JsonConvert.SerializeObject(data);
                            client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                            client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                            byte[] byteData = Encoding.UTF8.GetBytes(dataString);
                            client.UploadData("https://fcm.googleapis.com/fcm/send", "POST", byteData);
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
        public JsonResult AddQuickAttendance(string AbsenceList, string ParentInformList, string date)
        {
            try
            {
                string schoolEnglishName = string.Empty;
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    string statement = "SELECT SchoolEnglishName From SchoolBranches WHERE SchoolID = @SchoolID";
                    using (SqlCommand comm = new SqlCommand(statement, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                schoolEnglishName = reader[0].ToString();
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                var Absences = AbsenceList.Split(',').ToList();
                var ParentInforms = ParentInformList.Split(',').ToList();
                string studentId, studentName, schoolClassName, sectionName;
                studentId = studentName = schoolClassName = sectionName = string.Empty;
                List<dynamic> guardians = new List<dynamic>();
                List<string> notifications = new List<string>();
                if (Absences.Any())
                {
                    var AttendanceManager = factory.CreateAttendancesManager();
                    foreach (var item in Absences)
                    {
                        var record = item.Split('$');
                        studentId = record[0];
                        studentName = record[1];
                        schoolClassName = record[2];
                        sectionName = record[3];
                        var studentAttendance = AttendanceManager.Find(a => a.StudentID == studentId
                                && a.AttendanceDate == date).FirstOrDefault();
                        if (studentAttendance != null)
                        {
                            studentAttendance.SchoolID = SchoolID;
                            studentAttendance.SchoolYear = DateTime.UtcNow.Year.ToString();
                            studentAttendance.StudentID = studentId;
                            studentAttendance.AttendanceDate = date;
                            studentAttendance.AttendanceType = 3; // 3 for Absent.
                            studentAttendance.AbsenceReason = 8; // 8  for Other.
                            studentAttendance.ParentInformed = ParentInforms.Contains(studentId) ? 1 : 0;
                            studentAttendance.FirstSession = 0;
                            studentAttendance.SecondSession = 0;
                            studentAttendance.ThirdSession = 0;
                            studentAttendance.FourthSession = 0;
                            studentAttendance.FifthSession = 0;
                            studentAttendance.SixthSession = 0;
                            studentAttendance.SeventhSession = 0;
                            studentAttendance.EighthSession = 0;
                            AttendanceManager.Update(studentAttendance);
                        }
                        else
                        {
                            Attendance obj = new Attendance();
                            obj.SchoolID = SchoolID;
                            obj.SchoolYear = DateTime.UtcNow.Year.ToString();
                            obj.StudentID = studentId;
                            obj.AttendanceDate = date;
                            obj.AttendanceType = 3; // 3 for Absent.
                            obj.AbsenceReason = 8; // 8  for Other.
                            obj.ParentInformed = ParentInforms.Contains(studentId) ? 1 : 0;
                            obj.FirstSession = 0;
                            obj.SecondSession = 0;
                            obj.ThirdSession = 0;
                            obj.FourthSession = 0;
                            obj.FifthSession = 0;
                            obj.SixthSession = 0;
                            obj.SeventhSession = 0;
                            obj.EighthSession = 0;
                            AttendanceManager.Add(obj);
                        }

                        if (ParentInforms.Any())
                        {
                            if (ParentInforms.Contains(studentId))
                            {
                                string query = "";
                                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                                {
                                    conn.Open();
                                    query = "SELECT DeviceRegistrationCode, OwnerID FROM DeviceRegistrar WHERE OwnerID = " +
                                        "(SELECT DISTINCT CAST(GuardianID AS NVARCHAR(6)) FROM StudentGuardDetails WHERE StudentID = @StudentID)";
                                    using (SqlCommand comm = new SqlCommand(query, conn))
                                    {
                                        comm.Parameters.AddWithValue("@StudentID", studentId);
                                        using (SqlDataReader reader = comm.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                guardians.Add(new
                                                {
                                                    guardiantoken = reader[0].ToString(),
                                                    guardianID = reader[1].ToString()
                                                });

                                                notifications.Add($"{studentName} \n is absent by {schoolEnglishName} school. \n {Convert.ToDateTime(date).ToString("yyyy-MM-dd")}");
                                            }
                                        }
                                    }

                                }
                            } 
                        }
                    }

                    if (guardians.Any())
                    {
                        StringBuilder queryBuilder = new StringBuilder();
                        for (int i = 0; i < notifications.Count(); i++)
                        {
                            queryBuilder.Append("INSERT INTO Notifications " +
                               "(DesitinationID, DestinationType, NotificationTypeID, NotificationText, SourceType, [Status]) " +
                               "VALUES ('" + guardians[i].guardianID + "', 19, " + (int)NotificationsTypes.Attendance + ", N'" + notifications[i] + "', 15, 2);");

                            var data = new
                            {
                                to = guardians[i].guardiantoken,

                                data = new
                                {
                                    type = "Attendance",
                                    title = "Attendance Notification",
                                    message = notifications[i]
                                }
                            };
                            using (var client = new WebClient())
                            {
                                //var dataString = JsonConvert.SerializeObject(data);
                                //client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                                //client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                                //client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                                //Task.Delay(100);
                                //client.UploadString(new Uri("https://fcm.googleapis.com/fcm/send"), dataString);


                                // Supprot arabic text 
                                var dataString = JsonConvert.SerializeObject(data);
                                client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                                client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                                byte[] byteData = Encoding.UTF8.GetBytes(dataString);
                                client.UploadData("https://fcm.googleapis.com/fcm/send", "POST", byteData);
                            }
                        }
                        if (notifications.Any())
                        {
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand comm = new SqlCommand(queryBuilder.ToString(), conn))
                                {
                                    comm.ExecuteNonQuery();
                                }
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
        public JsonResult AddTeacherQuickAttendance(string AbsenceList, string ParentInformList, string SessionNumber, string teacherId)
        {
            try
            {
                string teacherEnglishName = string.Empty;
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

                var attendanceManager = factory.CreateAttendancesManager();
                var Absences = AbsenceList.Split(',').ToList();
                var ParentInforms = ParentInformList.Split(',').ToList();
                var currentDate = DateTime.Now.Date.ToString("dd/MM/yyyy");
                string studentId, studentName, schoolClassName, sectionName;
                studentId = studentName = schoolClassName = sectionName = string.Empty;
                List<dynamic> guardians = new List<dynamic>();
                List<string> notifications = new List<string>();
                if (Absences.Count > 0)
                {
                    foreach (var item in Absences)
                    {
                        var record = item.Split('$');
                        studentId = record[0];
                        studentName = record[1];
                        schoolClassName = record[2];
                        sectionName = record[3];
                        var studentAttendance = attendanceManager.Find(a => a.StudentID == studentId
                        && a.AttendanceDate == currentDate).FirstOrDefault();
                        if (studentAttendance != null)
                        {
                            studentAttendance.SchoolID = SchoolID;
                            studentAttendance.SchoolYear = DateTime.UtcNow.Year.ToString();
                            studentAttendance.StudentID = studentId;
                            studentAttendance.AttendanceDate = currentDate;
                            studentAttendance.AbsenceReason = 8; // 8  for Other.
                            studentAttendance.ParentInformed = ParentInforms.Contains(studentId) ? 1 : 0;
                            int sessionNumber = int.Parse(SessionNumber);
                            if (sessionProperties.ContainsKey(sessionNumber))
                            {
                                var propertyName = sessionProperties[sessionNumber];
                                var property = typeof(Attendance).GetProperty(propertyName);
                                property?.SetValue(studentAttendance, 0);
                            }
                            if (studentAttendance.FirstSession == 1 && studentAttendance.SecondSession == 1 &&
                                studentAttendance.ThirdSession == 1 && studentAttendance.FourthSession == 1 &&
                                studentAttendance.FifthSession == 1 && studentAttendance.SixthSession == 1 &&
                                studentAttendance.SeventhSession == 1 && studentAttendance.EighthSession == 1)
                            {
                                studentAttendance.AttendanceType = 3; // 3 for Absent.
                            }
                            else
                                studentAttendance.AttendanceType = 2; // 2 for Partial Attendace.

                            attendanceManager.Update(studentAttendance);
                        }
                        else
                        {
                            Attendance obj = new Attendance();
                            obj.SchoolID = SchoolID;
                            obj.SchoolYear = DateTime.UtcNow.Year.ToString();
                            obj.StudentID = studentId;
                            obj.AttendanceDate = currentDate;
                            obj.AbsenceReason = 8; // 8  for Other.
                            obj.ParentInformed = ParentInforms.Contains(studentId) ? 1 : 0;
                            int sessionNumber = int.Parse(SessionNumber);
                            if (sessionProperties.ContainsKey(sessionNumber))
                            {
                                var propertyName = sessionProperties[sessionNumber];
                                var property = typeof(Attendance).GetProperty(propertyName);
                                property?.SetValue(obj, 0);
                            }
                            if (obj.FirstSession == 1 && obj.SecondSession == 1 && obj.ThirdSession == 1 &&
                                obj.FourthSession == 1 && obj.FifthSession == 1 && obj.SixthSession == 1 &&
                                obj.SeventhSession == 1 && obj.EighthSession == 1)
                            {
                                obj.AttendanceType = 3; // 3 for Absent.
                            }
                            else
                                obj.AttendanceType = 2; // 2 for Partial Attendace.

                            attendanceManager.Add(obj);
                        }

                        if (ParentInforms.Contains(studentId))
                        {
                            string query = "";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                query = "SELECT DeviceRegistrationCode, OwnerID FROM DeviceRegistrar WHERE OwnerID = " +
                                    "(SELECT DISTINCT CAST(GuardianID AS NVARCHAR(6)) FROM StudentGuardDetails WHERE StudentID = @StudentID)";
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    comm.Parameters.AddWithValue("@StudentID", studentId);
                                    using (SqlDataReader reader = comm.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            guardians.Add(new
                                            {
                                                guardiantoken = reader[0].ToString(),
                                                guardianID = reader[1].ToString()
                                            });
                                        }
                                    }
                                }

                            }
                            notifications.Add($"{studentName} \n is absent by {teacherEnglishName}");
                        }
                    }

                    StringBuilder queryBuilder = new StringBuilder();
                    for (int i = 0; i < notifications.Count(); i++)
                    {
                        queryBuilder.Append("INSERT INTO Notifications " +
                           "(DesitinationID, DestinationType, NotificationTypeID, NotificationText, SourceType, [Status]) " +
                           "VALUES ('" + guardians[i].guardianID + "', 19, " + (int)NotificationsTypes.Attendance + ", N'" + notifications[i] + "', 15, 2);");

                        var data = new
                        {
                            to = guardians[i].guardiantoken,

                            data = new
                            {
                                type = "Attendance",
                                title = "Attendance Notification",
                                message = notifications[i]
                            }
                        };
                        using (var client = new WebClient())
                        {
                            //var dataString = JsonConvert.SerializeObject(data);
                            //client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                            //client.Headers.Add(HttpRequestHeader.Accept, "application/json");
                            //client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                            //Task.Delay(100);
                            //client.UploadString(new Uri("https://fcm.googleapis.com/fcm/send"), dataString);


                            // Supprot arabic text 
                            var dataString = JsonConvert.SerializeObject(data);
                            client.Headers.Add(HttpRequestHeader.ContentType, "application/json; charset=utf-8");
                            client.Headers.Add("Authorization", "key=AAAAhKNFcBE:APA91bGKl1Vd69fNuf1XJ7jNTjeWM6Pz9UbO5L95YXf9b03LxBBvYW_O7E-KzrjMGV8gp6qkwlfdUn3mkXu6DXMUOxWXSNUTO1ZH2m3nZSKcd1iKxFoycQPICp-tld7e6BpbluAVXlnA");
                            byte[] byteData = Encoding.UTF8.GetBytes(dataString);
                            client.UploadData("https://fcm.googleapis.com/fcm/send", "POST", byteData);
                        }
                    }
                    if (notifications.Any())
                    {
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(queryBuilder.ToString(), conn))
                            {
                                comm.ExecuteNonQuery();
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

        public IEnumerable<ReviewAutoAttendanceVM> GetUsersDailyAttendance(string date, string isCustomerUser)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string SmartTimeAPIUri = WebConfigurationManager.AppSettings["SmartTimeAPIUri"].ToString();
                    client.BaseAddress = new Uri($"{SmartTimeAPIUri}/SmartSchoolUsers/");
                    //client.BaseAddress = new Uri($"{SmartTimeBaseUri.Uri()}/SmartSchoolUsers/");
                    var endpoint = $"GetUsersDailyAttendance/{SchoolID}/{date}/{isCustomerUser}";

                    var response = client.GetAsync(endpoint).Result;

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        string jsonString = response.Content.ReadAsStringAsync().Result;
                        dynamic jsonResult = JsonConvert.DeserializeObject(jsonString);

                        if (jsonResult != null && jsonResult.status == "true")
                        {
                            var data = JsonConvert.DeserializeObject<List<ReviewAutoAttendanceVM>>(jsonResult.data.ToString());
                            return data;
                        }
                    }

                    return new List<ReviewAutoAttendanceVM>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<TrakingDetailsVM> GetUserFollowUpData(string userID, string fromDate, string toDate)
        {
            using (var client = new HttpClient())
            {
                string SmartTimeAPIUri = WebConfigurationManager.AppSettings["SmartTimeAPIUri"].ToString();
                client.BaseAddress = new Uri($"{SmartTimeAPIUri}/UHFReadersData/");
                //client.BaseAddress = new Uri($"{SmartTimeBaseUri.Uri()}/UHFReadersData/");

                var endpoint = "GetUserFollowUpData";

                var requestData = new
                {
                    userID = userID,
                    fromDate = fromDate,
                    toDate = toDate
                };

                var jsonString = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = client.PostAsync(endpoint, content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseJsonString = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonResult = JsonConvert.DeserializeObject(responseJsonString);

                    if (jsonResult != null && jsonResult.status == "true")
                    {
                        var data = JsonConvert.DeserializeObject<List<TrakingDetailsVM>>(jsonResult.data.ToString());
                        return data;
                    }
                }

                return new List<TrakingDetailsVM>();
            }
        }

        public IEnumerable<AttendanceTrackingVM> GetUsersRecentFollowUpData(string schoolId, string date)
        {
            using (var client = new HttpClient())
            {
                string SmartTimeAPIUri = WebConfigurationManager.AppSettings["SmartTimeAPIUri"].ToString();
                client.BaseAddress = new Uri($"{SmartTimeAPIUri}/UHFReadersData/");
                //client.BaseAddress = new Uri($"{SmartTimeBaseUri.Uri()}/UHFReadersData/");

                var endpoint = "GetUsersRecentFollowUpData";

                var requestData = new
                {
                    schoolID = schoolId,
                    dayDate = date
                };

                var jsonString = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = client.PostAsync(endpoint, content).Result;

                if (response != null && response.IsSuccessStatusCode)
                {
                    string responseJsonString = response.Content.ReadAsStringAsync().Result;
                    dynamic jsonResult = JsonConvert.DeserializeObject(responseJsonString);

                    if (jsonResult != null && jsonResult.status == "true")
                    {
                        var data = JsonConvert.DeserializeObject<List<AttendanceTrackingVM>>(jsonResult.data.ToString());
                        return data;
                    }
                }

                return new List<AttendanceTrackingVM>();
            }
        }
    }
}