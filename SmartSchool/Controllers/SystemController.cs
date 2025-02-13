using Business.Base;
using Common;
using Common.Helpers;
using Objects;
using SmartSchool.Models.System;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class SystemController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        // GET: System
        #region Actions

        public ActionResult Index()
        {
            return View();
        }
        //System 

        public ActionResult SystemSettings()
        {
            return View();
        }

        public ActionResult SystemSchoolBusSettings()
        {
            var SystemSettingPerSchoolManager = factory.CreateSystemSettingsperSchoolsManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var result = SystemSettingPerSchoolManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            SystemSettingsModel model = new SystemSettingsModel();

            model.SchoolID = SchoolID;
            model.BusScheduleDateType = result.BusScheduleDateType;
            model.BusScheduleType = result.BusScheduleType;
            model.NumberofBusTrips = result.NumberofBusTrips;
            model.BusAttendantAssigningMethod = result.BusAttendantAssigningMethod;

            return View(model);
        }

        public ActionResult SystemSchoolSettings()
        {
            var SystemSettingPerSchoolManager = factory.CreateSystemSettingsperSchoolsManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var result = SystemSettingPerSchoolManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            SystemSettingsModel model = new SystemSettingsModel();

            model.SchoolID = SchoolID;
            model.OptionalArabicFields = result.OptionalArabicFields;
            model.OptionalEnglishFields = result.OptionalEnglishFields;
            model.OptionalFamilyDetails = result.OptionalFamilyDetails;
            model.OptionalThirdName = result.OptionalThirdName;
            model.TimetableType = result.TimetableType;
            model.SubjectDistributionMethod = result.SubjectDistributionMethod;
            model.SubjectforClassorSection = result.SubjectforClassorSection;
            model.CurrencyOptions = result.CurrencyOptions;

            return View(model);
        }

        public ActionResult SchoolSettings()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            SystemSettingsModel model = new SystemSettingsModel();

            model.SchoolID = SchoolID;
            model.NumberofClassRooms = schoolSettings.NumberofClassRooms;
            model.NumberofChairsPerClass = schoolSettings.NumberofChairsPerClass;
            model.NumberofSessionsPerDay = schoolSettings.NumberofSessionsPerDay;
            model.NumberofSessionsPerTeacher = schoolSettings.NumberofSessionsPerTeacher;
            model.AllowedNumberofStudentsPerYardArea = schoolSettings.AllowedNumberofStudentsPerYardArea;
            model.AllowedNumberofStudentsPerClassroomArea = schoolSettings.AllowedNumberofStudentsPerClassroomArea;
            model.WeekStartingDay = schoolSettings.WeekStartingDay;
            model.StartingTime = schoolSettings.StartingTime.Value.ToString("HH:mm");
            model.FirstClassStartingTime = schoolSettings.FirstClassStartingTime.Value.ToString("HH:mm");
            model.SessionDuration = schoolSettings.SessionDuration;
            model.BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            model.BreakDuration = schoolSettings.BreakDuration;
            model.NumberofSemesters = schoolSettings.NumberofSemesters;
            model.MultiDependantsDiscount = schoolSettings.MultiDependantsDiscount;
            /* i used the ADO here cause there is a problem in the .edmx file so it 
             can't read the new attributes. */
            string query = "SELECT BreakAfterSession, NumberofExamsPerSemester, MaxDiscountNumber FROM SchoolSettings " +
                "WHERE SchoolID = " + SchoolID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        model.BreakAfterSession = Convert.ToInt32(reader["BreakAfterSession"].ToString());
                        model.NumberofExamsPerSemester = Convert.ToInt32(reader["NumberofExamsPerSemester"].ToString());
                        model.MaxDiscountNumber = reader["MaxDiscountNumber"] != DBNull.Value
                            ? (int?)Convert.ToInt32(reader["MaxDiscountNumber"]) : null;
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            return View(model);
        }
        #endregion

        #region AJAX post
        [HttpPost]
        public JsonResult UpdateSystemSettingsPerSchool(SystemSettingsModel model)
        {
            try
            {
                var SystemSettingsPerSchoolManager = factory.CreateSystemSettingsperSchoolsManager();
                var Result = SystemSettingsPerSchoolManager.Find(a => a.SchoolID == model.SchoolID).FirstOrDefault();
                if (Result != null)
                {
                    Result.OptionalArabicFields = model.OptionalArabicFields;
                    Result.OptionalEnglishFields = model.OptionalEnglishFields;
                    Result.OptionalFamilyDetails = model.OptionalFamilyDetails;
                    Result.OptionalThirdName = model.OptionalThirdName;
                    Result.CurrencyOptions = model.CurrencyOptions;
                    Result.TimetableType = model.TimetableType;
                    Result.SubjectDistributionMethod = model.SubjectDistributionMethod;
                    Result.SubjectforClassorSection = model.SubjectforClassorSection;
                    SystemSettingsPerSchoolManager.Update(Result);

                }
                else
                {
                    SystemSettingsperSchool obj = new SystemSettingsperSchool();
                    obj.SchoolID = model.SchoolID;
                    obj.OptionalArabicFields = model.OptionalArabicFields;
                    obj.OptionalEnglishFields = model.OptionalEnglishFields;
                    obj.OptionalFamilyDetails = model.OptionalFamilyDetails;
                    obj.OptionalThirdName = model.OptionalThirdName;
                    obj.CurrencyOptions = model.CurrencyOptions;
                    obj.TimetableType = model.TimetableType;
                    obj.SubjectDistributionMethod = model.SubjectDistributionMethod;
                    obj.SubjectforClassorSection = model.SubjectforClassorSection;
                    SystemSettingsPerSchoolManager.Add(obj);
                }

                return Json(new { Success = true });

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });

            }

        }

        [HttpPost]
        public JsonResult UpdateBusSystemSettings(SystemSettingsModel model)
        {
            try
            {
                var SystemSettingsPerSchoolManager = factory.CreateSystemSettingsperSchoolsManager();
                var Result = SystemSettingsPerSchoolManager.Find(a => a.SchoolID == model.SchoolID).FirstOrDefault();
                if (Result != null)
                {
                    Result.NumberofBusTrips = model.NumberofBusTrips;
                    Result.BusScheduleType = model.BusScheduleType;
                    Result.BusScheduleDateType = model.BusScheduleDateType;
                    Result.BusAttendantAssigningMethod = model.BusAttendantAssigningMethod;
                    SystemSettingsPerSchoolManager.Update(Result);
                }
                else
                {
                    SystemSettingsperSchool obj = new SystemSettingsperSchool();
                    obj.SchoolID = model.SchoolID;
                    obj.NumberofBusTrips = model.NumberofBusTrips;
                    obj.BusScheduleType = model.BusScheduleType;
                    obj.BusScheduleDateType = model.BusScheduleDateType;
                    obj.BusAttendantAssigningMethod = model.BusAttendantAssigningMethod;
                    SystemSettingsPerSchoolManager.Add(obj);
                }

                return Json(new { Success = true });

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });

            }

        }

        [HttpPost]
        public JsonResult UpdateSchoolSettings(SystemSettingsModel model)
        {
            try
            {
                string query = "";
                var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
                var result = SchoolSettingsManager.Find(a => a.SchoolID == model.SchoolID).FirstOrDefault();
                string startingtime = model.StartingTime;
                var time = TimeSpan.Parse(startingtime);
                var StartingdateTime = DateTime.Today.Add(time);
                string Firstclasstime = model.FirstClassStartingTime;
                var firstime = TimeSpan.Parse(Firstclasstime);
                var FirstClassStartingTime = DateTime.Today.Add(firstime);
                if (result != null)
                {
                    query = "UPDATE SchoolSettings SET " +
                        "SchoolID = @SchoolID, NumberofClassRooms = @NumberofClassRooms, " +
                        "NumberofChairsPerClass = @NumberofChairsPerClass, " +
                        "NumberofSessionsPerDay = @NumberofSessionsPerDay, " +
                        "NumberofSessionsPerTeacher = @NumberofSessionsPerTeacher, " +
                        "AllowedNumberofStudentsPerYardArea = @AllowedNumberofStudentsPerYardArea, " +
                        "AllowedNumberofStudentsPerClassroomArea = @AllowedNumberofStudentsPerClassroomArea," +
                        " WeekStartingDay = @WeekStartingDay, StartingTime = @StartingTime, " +
                        "FirstClassStartingTime = @FirstClassStartingTime, SessionDuration = @SessionDuration, " +
                        "BreakBetweenSessionsDuration = @BreakBetweenSessionsDuration, " +
                        "BreakDuration = @BreakDuration, NumberofSemesters = @NumberofSemesters, " +
                        "NumberofExamsPerSemester = @NumberofExamsPerSemester, " +
                        "MultiDependantsDiscount = @MultiDependantsDiscount, " +
                        "BreakAfterSession = @BreakAfterSession, MaxDiscountNumber = @MaxDiscountNumber " +
                        "WHERE SchoolID = @SchoolID";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SchoolID", model.SchoolID);
                            comm.Parameters.AddWithValue("@NumberofClassRooms", model.NumberofClassRooms);
                            comm.Parameters.AddWithValue("@NumberofChairsPerClass", model.NumberofChairsPerClass);
                            comm.Parameters.AddWithValue("@NumberofSessionsPerDay", model.NumberofSessionsPerDay);
                            comm.Parameters.AddWithValue("@NumberofSessionsPerTeacher", model.NumberofSessionsPerTeacher);
                            comm.Parameters.AddWithValue("@AllowedNumberofStudentsPerYardArea", model.AllowedNumberofStudentsPerYardArea);
                            comm.Parameters.AddWithValue("@AllowedNumberofStudentsPerClassroomArea", model.AllowedNumberofStudentsPerClassroomArea);
                            comm.Parameters.AddWithValue("@WeekStartingDay", model.WeekStartingDay);
                            comm.Parameters.AddWithValue("@StartingTime", model.StartingTime);
                            comm.Parameters.AddWithValue("@FirstClassStartingTime", model.FirstClassStartingTime);
                            comm.Parameters.AddWithValue("@SessionDuration", model.SessionDuration);
                            comm.Parameters.AddWithValue("@BreakBetweenSessionsDuration", model.BreakBetweenSessionsDuration);
                            comm.Parameters.AddWithValue("@BreakDuration", model.BreakDuration);
                            comm.Parameters.AddWithValue("@NumberofSemesters", model.NumberofSemesters);
                            comm.Parameters.AddWithValue("@NumberofExamsPerSemester", model.NumberofExamsPerSemester);
                            comm.Parameters.AddWithValue("@MultiDependantsDiscount", model.MultiDependantsDiscount);
                            comm.Parameters.AddWithValue("@BreakAfterSession", model.BreakAfterSession);
                            comm.Parameters.AddWithValue("@MaxDiscountNumber", model.MaxDiscountNumber.HasValue ? (object)model.MaxDiscountNumber : DBNull.Value);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else
                {
                    query = "INSERT INTO SchoolSettings " +
                        "(SchoolID, NumberofClassRooms, NumberofChairsPerClass, NumberofSessionsPerDay, " +
                        "NumberofSessionsPerTeacher, AllowedNumberofStudentsPerYardArea, " +
                        "AllowedNumberofStudentsPerClassroomArea, WeekStartingDay, StartingTime, " +
                        "FirstClassStartingTime, SessionDuration, BreakBetweenSessionsDuration, BreakDuration, " +
                        "NumberofSemesters,NumberofExamsPerSemester, MultiDependantsDiscount, BreakAfterSession, MaxDiscountNumber) " +
                        "VALUES (@SchoolID, @NumberofClassRooms, @NumberofChairsPerClass, " +
                        "@NumberofSessionsPerDay, @NumberofSessionsPerTeacher, " +
                        "@AllowedNumberofStudentsPerYardArea, @AllowedNumberofStudentsPerClassroomArea, " +
                        "@WeekStartingDay, @StartingTime, @FirstClassStartingTime, @SessionDuration, " +
                        "@BreakBetweenSessionsDuration, @BreakDuration, @NumberofSemesters, " +
                        "@NumberofExamsPerSemester, @MultiDependantsDiscount, @BreakAfterSession, @MaxDiscountNumber)";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SchoolID", model.SchoolID);
                            comm.Parameters.AddWithValue("@NumberofClassRooms", model.NumberofClassRooms);
                            comm.Parameters.AddWithValue("@NumberofChairsPerClass", model.NumberofChairsPerClass);
                            comm.Parameters.AddWithValue("@NumberofSessionsPerDay", model.NumberofSessionsPerDay);
                            comm.Parameters.AddWithValue("@NumberofSessionsPerTeacher", model.NumberofSessionsPerTeacher);
                            comm.Parameters.AddWithValue("@AllowedNumberofStudentsPerYardArea", model.AllowedNumberofStudentsPerYardArea);
                            comm.Parameters.AddWithValue("@AllowedNumberofStudentsPerClassroomArea", model.AllowedNumberofStudentsPerClassroomArea);
                            comm.Parameters.AddWithValue("@WeekStartingDay", model.WeekStartingDay);
                            comm.Parameters.AddWithValue("@StartingTime", model.StartingTime);
                            comm.Parameters.AddWithValue("@FirstClassStartingTime", model.FirstClassStartingTime);
                            comm.Parameters.AddWithValue("@SessionDuration", model.SessionDuration);
                            comm.Parameters.AddWithValue("@BreakBetweenSessionsDuration", model.BreakBetweenSessionsDuration);
                            comm.Parameters.AddWithValue("@BreakDuration", model.BreakDuration);
                            comm.Parameters.AddWithValue("@NumberofSemesters", model.NumberofSemesters);
                            comm.Parameters.AddWithValue("@NumberofExamsPerSemester", model.NumberofExamsPerSemester);
                            comm.Parameters.AddWithValue("@MultiDependantsDiscount", model.MultiDependantsDiscount);
                            comm.Parameters.AddWithValue("@BreakAfterSession", model.BreakAfterSession);
                            comm.Parameters.AddWithValue("@MaxDiscountNumber", model.MaxDiscountNumber);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }
                return Json(new { Success = true });

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message });
            }
        }
        #endregion
    }
}