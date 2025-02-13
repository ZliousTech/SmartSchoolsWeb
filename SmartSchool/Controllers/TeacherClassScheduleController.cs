using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using SmartSchool.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class TeacherClassScheduleController : BaseController
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: TeacherClassSchedule
        public ActionResult Index(string StaffID)
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
            //var staffInfoManager = factory.CreateStaffsManager();

            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            var TimetableType = SystemSettingsperSchoolManger.Find(a => a.SchoolID == SchoolID).Select(a => a.TimetableType).FirstOrDefault();

            model.TimetableType = TimetableType;
            model.schoolID = SchoolID;
            model.WeekStartingDay = schoolSettings.WeekStartingDay;
            model.NumberofSessionsPerDay = schoolSettings.NumberofSessionsPerDay;
            model.SessionDuration = schoolSettings.SessionDuration;
            model.BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            model.BreakDuration = schoolSettings.BreakDuration;
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

                // break after session 4 ::todo from setting 
                // get SessionOrderDay (4) and add the BreakDuration 
                if (i == 4)
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
    }
}