using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Objects;
using Objects.DTO;
using SmartSchool.Models.ClassSchedule;
using SmartSchool.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class ClassScheduleController : BaseController
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        Random random = new Random();

        // GET: ClassSchedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrepareClassSchedule()
        {
            ManualScheduleModel model = new ManualScheduleModel();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            model.Classes = GetSchoolClassesBySchoolID(SchoolID);
            return View(model);
        }

        [HttpPost]
        public ActionResult PrepareClassSchedule(ManualScheduleModel model)
        {
            var ClassManager = factory.CreateSchoolClasssManager();
            var SectionsManager = factory.CreateSectionsManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            var subjects = (from a in context.Subjects
                            join b in context.SchoolClasses on a.SchoolClassID equals b.SchoolClassID
                            where a.SchoolID == SchoolID && a.SchoolClassID == model.SchoolClassID //&& a.SectionID == -1
                            //&&  a.SectionID==model.SectionID

                            select new ManualScheduleDTO
                            {
                                SubjectID = a.SubjectID,
                                SubjectName = CurrentLanguage == Languges.English ? a.SubjectEnglishName : a.SubjectArabicName,
                                SubjectNumOfSession = a.NumberOfSessionsPerWeek.Value
                            }).ToList();
            model.ManualScheduleDTO = subjects;
            var classroom = ClassManager.Find(a => a.SchoolClassID == model.SchoolClassID && a.SchoolID == SchoolID).FirstOrDefault();
            model.ClassName = CurrentLanguage == Languges.English ? classroom.SchoolClassArabicName : classroom.SchoolClassEnglishName;

            //------------ Error in logic
            //var section = SectionsManager.Find(a => a.SectionID == model.SectionID && a.SchoolID == SchoolID).FirstOrDefault();
            // Should be "SchoolClassID" not SectionID
            var section = SectionsManager.Find(a => a.SchoolClassID == model.SchoolClassID && a.SchoolID == SchoolID).FirstOrDefault();

            model.SectionName = CurrentLanguage == Languges.English ? section.SectionEnglishName : section.SectionArabicName;
            model.Classes = GetSchoolClassesBySchoolID(SchoolID);
            return View(model);
        }

        public ActionResult TimeTableManual(int SchoolClassID, int SectionID)
        {
            ManualScheduleModel model = new ManualScheduleModel();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var SystemSettingsperSchoolManger = factory.CreateSystemSettingsperSchoolsManager();
            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            var TimetableType = SystemSettingsperSchoolManger.Find(a => a.SchoolID == SchoolID).Select(a => a.TimetableType).FirstOrDefault();
            model.TimetableType = TimetableType;
            model.SchoolID = SchoolID;
            model.WeekStartingDay = schoolSettings.WeekStartingDay;
            model.NumberofSessionsPerDay = schoolSettings.NumberofSessionsPerDay;
            model.SessionDuration = schoolSettings.SessionDuration;
            model.BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            model.BreakDuration = schoolSettings.BreakDuration;
            model.BreakAfterSession = 4; // the default value
            /* i used the ADO here cause there is a problem in the .edmx file so it 
             can't read the new attributes. */
            string query = "SELECT BreakAfterSession FROM SchoolSettings " +
                "WHERE SchoolID = " + SchoolID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                        model.BreakAfterSession = Convert.ToInt32(reader["BreakAfterSession"].ToString());
                }
                conn.Close();
                conn.Dispose();
            }
            model.WeekDaysList = new List<LookupDTO>();
            model.SessionsPerDayWithStartTimeList = new List<LookupDTO>();
            DateTime ClassStartingTime = schoolSettings.FirstClassStartingTime.Value;
            TimeSpan ClassTime = ClassStartingTime.TimeOfDay;
            model.SchoolClassID = SchoolClassID;
            model.SectionID = SectionID;
            model.SessionNameList = SessionNameList();

            if (model.WeekStartingDay == 1)
            {
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 1,//WeekStartingDay
                    DescriptionAR = "الاحد",
                    Description = "Sunday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 2,
                    DescriptionAR = "الاثنين",
                    Description = "Monday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 3,
                    DescriptionAR = "الثلاثاء",
                    Description = "Tuesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 4,
                    DescriptionAR = "الاربعاء",
                    Description = "Wednesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 5,
                    DescriptionAR = "الخميس",
                    Description = "Thursday"
                });
            }
            else //other val of WeekStartingDay ::todo
            {
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 1,//WeekStartingDay
                    DescriptionAR = "الاحد",
                    Description = "Sunday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 2,
                    DescriptionAR = "الاثنين",
                    Description = "Monday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 3,
                    DescriptionAR = "الثلاثاء",
                    Description = "Tuesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 4,
                    DescriptionAR = "الاربعاء",
                    Description = "Wednesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 5,
                    DescriptionAR = "الخميس",
                    Description = "Thursday"
                });
            }

            for (int i = 1; i <= model.NumberofSessionsPerDay.Value; i++)
            {
                model.SessionsPerDayWithStartTimeList.Add(
                    new LookupDTO
                    {
                        ID = i, // formla to set sessiondayorder
                        Description = model.SessionNameList.Where(c => c.ID == i).Select(c => c.Description).FirstOrDefault() + "\n" + ClassTime.ToString(@"hh\:mm"),
                        DescriptionAR = model.SessionNameList.Where(c => c.ID == i).Select(c => c.DescriptionAR).FirstOrDefault() + "\n" + ClassTime.ToString(@"hh\:mm")
                    }
              );
                // break after session model.BreakAfterSession ::todo from setting 
                // get SessionOrderDay (model.BreakAfterSession) and add the BreakDuration 
                if (i == model.BreakAfterSession)
                {
                    ClassTime = ClassTime.Add(TimeSpan.FromMinutes(model.SessionDuration.Value
                        + model.BreakBetweenSessionsDuration.Value + model.BreakDuration.Value));
                }
                else
                {
                    ClassTime = ClassTime.Add(TimeSpan.FromMinutes(model.SessionDuration.Value + model.BreakBetweenSessionsDuration.Value));

                }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult AddSession(string StaffID, int SubjectID, int DayID, int SessionID, int SectionID, int SchoolClassID, bool SharedSession)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var _SchoolYear = context.SystemSettings.Select(c => c.CurrentAcademicYear).FirstOrDefault();

            var ManualTimeTableManager = factory.CreateManualTimetablesManager();
            var TimeTableItemsManager = factory.CreateTimetableItemsManager();
            var SessionManager = factory.CreateSessionsManager();
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var SubjectsManager = factory.CreateSubjectsManager();
            var TeacherManager = factory.CreateTeachersManager();
            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();

            DateTime ClassStartingTime = schoolSettings.FirstClassStartingTime.Value;
            TimeSpan ClassTime = ClassStartingTime.TimeOfDay;
            var SessionDuration = schoolSettings.SessionDuration;
            var BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            var BreakDuration = schoolSettings.BreakDuration;
            var BreakAfterSession = -2;
            TimeSpan sessionTime;
            var SessionDayorder = SessionID - 1;
            var HaveSession = false;
            var MaxNumberOfSession = false;
            var TeacherMaxNumberOfSession = false;
            /* i used the ADO here cause there is a problem in the .edmx file so it 
             can't read the new attributes. */
            string query = "SELECT BreakAfterSession FROM SchoolSettings " +
                "WHERE SchoolID = " + SchoolID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                        BreakAfterSession = Convert.ToInt32(reader["BreakAfterSession"].ToString());
                }
                conn.Close();
                conn.Dispose();
            }
            if (SessionID >= BreakAfterSession + 1)
            {
                sessionTime = ClassTime.Add(TimeSpan.FromMinutes(SessionDayorder * (SessionDuration.Value + BreakBetweenSessionsDuration.Value) + BreakDuration.Value));

            }
            else
            {
                sessionTime = ClassTime.Add(TimeSpan.FromMinutes(SessionDayorder * (SessionDuration.Value + BreakBetweenSessionsDuration.Value)));
            }
            var ManualResult = GetClassManualTimetable(SchoolClassID, SectionID, DayID - 1, SessionID, SchoolID);
            DateTime DateSessionTime =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + sessionTime;
            if (ManualResult == null)
            {
                var TeacherSession = GetTeacherManualTimetable(StaffID, DayID - 1, SessionID, SchoolID);
                if (TeacherSession != null && SharedSession == false)
                {
                    HaveSession = true;
                    return Json(new { TeacherSession, MaxNumberOfSession, TeacherMaxNumberOfSession, HaveSession }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var SubjectNumOfSession = SubjectsManager.Find(a => a.SubjectID == SubjectID).Select(a => a.NumberOfSessionsPerWeek).FirstOrDefault();
                    int SubjectCount = TimeTableItemsManager.Find(a => a.SubjectID == SubjectID && a.SectionID == SectionID && a.SchoolClassID == SchoolClassID && a.SchoolID == SchoolID && a.SchoolYear == _SchoolYear).ToList().Count();
                    var TeacherNumOfSession = TeacherManager.Find(a => a.StaffID == StaffID).Select(a => a.NumberofSessins).FirstOrDefault();
                    var TeacherSessionCount = ManualTimeTableManager.Find(a => a.TeacherID == StaffID && a.SchoolID == SchoolID && a.SchoolYear == _SchoolYear).ToList().Count();

                    if (TeacherSessionCount >= TeacherNumOfSession)
                    {
                        TeacherMaxNumberOfSession = true;
                        return Json(new { HaveSession, TeacherMaxNumberOfSession, MaxNumberOfSession }, JsonRequestBehavior.AllowGet);

                    }
                    if (SubjectCount < SubjectNumOfSession)
                    {
                        TimetableItem obj1 = new TimetableItem();
                        obj1.SchoolID = SchoolID;
                        obj1.SchoolYear = _SchoolYear;
                        obj1.SchoolClassID = SchoolClassID;
                        obj1.SectionID = SectionID;
                        obj1.SubjectID = SubjectID;
                        obj1.TeacherID = StaffID;
                        var SubjectColor = TimeTableItemsManager.Find(a => a.SchoolID == SchoolID && a.SubjectID == SubjectID).Select(a => a.ItemRGBColor).FirstOrDefault();
                        if (!string.IsNullOrEmpty(SubjectColor))
                        {
                            obj1.ItemRGBColor = SubjectColor;
                        }
                        else
                        {
                            var AllSubjectsColor = TimeTableItemsManager.Find(a => a.SchoolID == SchoolID && a.SchoolClassID == a.SchoolClassID).Select(a => a.ItemRGBColor).ToList();
                            if (AllSubjectsColor != null)
                            {
                            generateColor:
                                // 150 resulting in darker shades compared to a full range of 0 - 255.
                                int red = random.Next(150);
                                int green = random.Next(150);
                                int blue = random.Next(150);
                                string newColor = String.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
                                if (AllSubjectsColor.Contains(newColor))
                                    goto generateColor;
                                obj1.ItemRGBColor = newColor;
                            }
                        }
                        TimeTableItemsManager.Add(obj1);

                        Session obj2 = new Session();
                        obj2.SchoolID = SchoolID;
                        obj2.SchoolYear = _SchoolYear;
                        obj2.SchoolClassID = SchoolClassID;
                        obj2.SectionID = SectionID;
                        obj2.WeekDay = DayID - 1;
                        obj2.SessionDayOrder = SessionID;
                        obj2.Available = 1;
                        obj2.SessionTime = DateSessionTime;
                        SessionManager.Add(obj2);

                        ManualTimetable obj3 = new ManualTimetable();
                        obj3.SchoolID = SchoolID;
                        obj3.SchoolYear = _SchoolYear;
                        obj3.SchoolClassID = SchoolClassID;
                        obj3.SectionID = SectionID;
                        obj3.TeacherID = StaffID;
                        obj3.TimetableItemID = obj1.TimetableItemID;
                        obj3.SessionID = obj2.SessionID;
                        ManualTimeTableManager.Add(obj3);

                        var result = TeacherManager.Find(t => t.StaffID == StaffID).FirstOrDefault();
                        if (result != null)
                        {
                            result.TimeTableSessions += 1;
                            TeacherManager.Update(result);
                        }

                        string HexColor = obj1.ItemRGBColor;
                        return Json(new { HaveSession, HexColor, TeacherMaxNumberOfSession, MaxNumberOfSession }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        MaxNumberOfSession = true;
                        return Json(new { HaveSession, MaxNumberOfSession, TeacherMaxNumberOfSession }, JsonRequestBehavior.AllowGet);

                    }
                }
            }

            return Json(new { HaveSession, TeacherMaxNumberOfSession, MaxNumberOfSession }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSession(string StaffID, int SubjectID, int DayID, int SessionID, int SectionID, int SchoolClassID)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            var ManualTimeTableManager = factory.CreateManualTimetablesManager();
            var TimeTableItemsManager = factory.CreateTimetableItemsManager();
            var SessionManager = factory.CreateSessionsManager();

            var ManualResult = GetClassManualTimetable(SchoolClassID, SectionID, DayID - 1, SessionID, SchoolID);
            if (ManualResult != null)
            {
                var ManualTable = ManualTimeTableManager.Find(a => a.TimetableEntityID == ManualResult.TimetableEntityID).FirstOrDefault();
                var TableItems = TimeTableItemsManager.Find(a => a.TimetableItemID == ManualTable.TimetableItemID).FirstOrDefault();
                var Session = SessionManager.Find(a => a.SessionID == ManualTable.SessionID).FirstOrDefault();

                SessionManager.Delete(Session);
                TimeTableItemsManager.Delete(TableItems);
                ManualTimeTableManager.Delete(ManualTable);

                var TeacherManager = factory.CreateTeachersManager();
                var result = TeacherManager.Find(t => t.StaffID == StaffID).FirstOrDefault();
                if (result != null)
                {
                    result.TimeTableSessions -= 1;
                    TeacherManager.Update(result);
                }

            }

            return Json(true, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetSections(int SchoolClassID)
        {
            var Sections = GetSectionsBySchoolClassID(SchoolClassID);
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTeachersBysubjectID(int SubjectID, int SchoolClassID)//, int SectionID)
        {
            //var Teachers = from a in context.TeacherExperiences
            //               join b in context.Staffs on a.TeacherID equals b.StaffID
            //               where a.SubjectID == SubjectID
            //               select new TeacherDTO
            //               {
            //                   StaffID = a.TeacherID,
            //                   TeacherName = CurrentLanguage == Languges.English ? b.StaffEnglishName : b.StaffArabicName
            //               };

            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            //var Teachers = from a in context.TeacherSubjects
            //               join b in context.Staffs on a.TeacherID equals b.StaffID
            //               join c in context.StaffJobDetails on b.StaffID equals c.StaffID
            //               where a.SubjectID == SubjectID && c.SchoolID == _SchoolID
            //               select new TeacherDTO
            //               {
            //                   StaffID = a.TeacherID,
            //                   TeacherName = CurrentLanguage == Languges.English ? b.StaffEnglishName : b.StaffArabicName,
            //                   SectionID = a.SectionID,
            //                   SectionCode = a.SectionCode

            //               };

            var Teachers = from a in context.TeacherExperiences
                           join b in context.Staffs on a.TeacherID equals b.StaffID
                           join c in context.StaffJobDetails on b.StaffID equals c.StaffID
                           join d in context.Teachers on a.TeacherID equals d.StaffID
                           join e in context.Subjects on a.SubjectID equals e.SubjectID
                           //join d in context.Sections on e.SectionID equals d.SectionID
                           where a.SchoolClassID == SchoolClassID && a.SubjectID == SubjectID && c.SchoolID == _SchoolID
                           select new TeacherDTO
                           {
                               StaffID = a.TeacherID,
                               TeacherName = CurrentLanguage == Languges.English ?
                               b.StaffEnglishName : b.StaffArabicName,
                               NumberofSessins = d.NumberofSessins,
                               TimeTableSessions = d.TimeTableSessions,
                               //SectionID = d.SectionID,
                               //SectionCode = d.SectionCode
                           };
            string HexColor = context.TimetableItems.Where(a => a.SubjectID == SubjectID).Select(a => a.ItemRGBColor).FirstOrDefault();

            return Json(new { Teachers, HexColor }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TeachersClassSchedule()
        {
            TeacherRegisterModel model = new TeacherRegisterModel();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string schoolyear = getCurrentAcademicYear();
            var StaffManager = factory.CreateStaffsManager();

            var AllTeachers = StaffManager.GetTeachers(SchoolID, lang, schoolyear);
            var TeachersList = (from Teachers in AllTeachers
                                select new LookupDTO
                                {
                                    Description = Teachers.TeacherName.ToString(),
                                    DescriptionAR = Teachers.TeacherName.ToString(),
                                    id = Teachers.StaffID
                                }).ToList();
            model.TeacherNameList = TeachersList;

            return View(model);
        }

        public ActionResult GetTimeTable(string StaffID)
        {
            StaffID = Decrypt(StaffID);
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            SmartSchoolsEntities db = new SmartSchoolsEntities();
            TeacherRegisterModel model = new TeacherRegisterModel();
            var qTeacherNameInfo =
                (from Stf in db.Staffs
                 join StfJ in db.StaffJobDetails on Stf.StaffID equals StfJ.StaffID
                 where StfJ.SchoolID == SchoolID && Stf.StaffID == StaffID
                 select new TeacherNameInfo
                 {
                     TeacherArName = Stf.StaffArabicName,
                     TeacherEnName = Stf.StaffEnglishName
                 }).FirstOrDefault();
            model.TeacherName = lang == "en" ? qTeacherNameInfo.TeacherEnName : qTeacherNameInfo.TeacherArName;

            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var SystemSettingsperSchoolManger = factory.CreateSystemSettingsperSchoolsManager();
            var staffInfoManager = factory.CreateStaffsManager();

            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            var TimetableType = SystemSettingsperSchoolManger.Find(a => a.SchoolID == SchoolID).Select(a => a.TimetableType).FirstOrDefault();

            model.TimetableType = TimetableType;
            model.schoolID = SchoolID;
            model.WeekStartingDay = schoolSettings.WeekStartingDay;
            model.NumberofSessionsPerDay = schoolSettings.NumberofSessionsPerDay;
            model.SessionDuration = schoolSettings.SessionDuration;
            model.BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            model.BreakDuration = schoolSettings.BreakDuration;
            model.BreakAfterSession = 4; // the default value
            /* i used the ADO here cause there is a problem in the .edmx file so it 
             can't read the new attributes. */
            string query = "SELECT BreakAfterSession FROM SchoolSettings " +
                "WHERE SchoolID = " + SchoolID;
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                        model.BreakAfterSession = Convert.ToInt32(reader["BreakAfterSession"].ToString());
                }
                conn.Close();
                conn.Dispose();
            }
            model.TeacherID = StaffID;
            model.WeekDaysList = new List<LookupDTO>();
            model.SessionsPerDayWithStartTimeList = new List<LookupDTO>();
            DateTime ClassStartingTime = schoolSettings.FirstClassStartingTime.Value;
            TimeSpan ClassTime = ClassStartingTime.TimeOfDay;

            model.SessionNameList = SessionNameList();

            if (model.WeekStartingDay == 1)
            {
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 1,//WeekStartingDay
                    DescriptionAR = "الاحد",
                    Description = "Sunday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 2,
                    DescriptionAR = "الاثنين",
                    Description = "Monday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 3,
                    DescriptionAR = "الثلاثاء",
                    Description = "Tuesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 4,
                    DescriptionAR = "الاربعاء",
                    Description = "Wednesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 5,
                    DescriptionAR = "الخميس",
                    Description = "Thursday"
                });
            }
            else //other val of WeekStartingDay ::todo
            {
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 1,//WeekStartingDay
                    DescriptionAR = "الاحد",
                    Description = "Sunday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 2,
                    DescriptionAR = "الاثنين",
                    Description = "Monday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 3,
                    DescriptionAR = "الثلاثاء",
                    Description = "Tuesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 4,
                    DescriptionAR = "الاربعاء",
                    Description = "Wednesday"
                });
                model.WeekDaysList.Add(new LookupDTO
                {
                    ID = 5,
                    DescriptionAR = "الخميس",
                    Description = "Thursday"
                });
            }

            for (int i = 1; i <= model.NumberofSessionsPerDay.Value; i++)
            {
                model.SessionsPerDayWithStartTimeList.Add(
                    new LookupDTO
                    {
                        ID = i, // formla to set sessiondayorder
                        Description = model.SessionNameList.Where(c => c.ID == i).Select(c => c.Description).FirstOrDefault() + "\n" + ClassTime.ToString(@"hh\:mm"),
                        DescriptionAR = model.SessionNameList.Where(c => c.ID == i).Select(c => c.DescriptionAR).FirstOrDefault() + "\n" + ClassTime.ToString(@"hh\:mm")
                    });

                // break after session model.BreakAfterSession ::todo from setting 
                // get SessionOrderDay (model.BreakAfterSession) and add the BreakDuration 
                if (i == model.BreakAfterSession)
                {
                    ClassTime = ClassTime.Add(TimeSpan.FromMinutes(model.SessionDuration.Value
                        + model.BreakBetweenSessionsDuration.Value + model.BreakDuration.Value));
                }
                else
                {
                    ClassTime = ClassTime.Add(TimeSpan.FromMinutes(model.SessionDuration.Value + model.BreakBetweenSessionsDuration.Value));

                }
            }

            return View(model);
            //return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }

    class TeacherNameInfo
    {
        public string TeacherArName { get; set; }
        public string TeacherEnName { get; set; }
    }
}