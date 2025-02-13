using Business.Base;
using Common.Helpers;
using SmartSchool.Models.TeacherStudentAttendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class TeacherStudentAttendanceController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

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

        public ActionResult Index(TeacherStudentAttendanceView model, int? SectionID)
        {
            model.Attendance.SectionID = SectionID;
            model.Attendance.SchoolClassesList = PopulateSchoolClasses();
            if (SectionID != null)
            {
                var StudentsManager = factory.CreateStudentsManager();
                var Students = StudentsManager.GetAll().ToList();
                var StudentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
                var StudentsSchoolDetails = StudentSchoolDetailsManager.Find(s => s.SectionID == SectionID).ToList();
                var SchoolClassManager = factory.CreateSchoolClasssManager();
                var SchoolClass = SchoolClassManager.Find(s => s.SchoolID == SchoolID).ToList();
                var SectionsManager = factory.CreateSectionsManager();
                var Sections = SectionsManager.Find(s => s.SchoolID == SchoolID).ToList();
                var AttendanceManager = factory.CreateAttendancesManager();
                var Attendances = AttendanceManager.Find(a => a.SchoolID == SchoolID).ToList();
                var students = from a in Students
                               join b in StudentsSchoolDetails
                               on a.StudentID equals b.StudentID
                               join c in Attendances
                               on a.StudentID equals c.StudentID
                               join d in SchoolClass
                               on b.ClassID equals d.SchoolClassID
                               join e in Sections
                               on b.ClassID equals e.SchoolClassID
                               where c.AttendanceDate == DateTime.Now.Date.ToString()
                               select new TeacherStudentAttendances()
                               {
                                   StudentID = a.StudentID,
                                   StudentName = CurrentLanguage == Languges.English ? a.StudentEnglishName : a.StudentArabicName,
                                   StudentClass = CurrentLanguage == Languges.English ? d.SchoolClassEnglishName : d.SchoolClassArabicName,
                                   StudentSection = CurrentLanguage == Languges.English ? e.SectionEnglishName : e.SectionArabicName,
                                   StudentTotalAbsence = 0
                               };
                model.TeacherStudentAttendance = students.ToList();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetSchoolClasses()
        {
            var SchoolClasses = GetSchoolClassesBySchoolID(SchoolID);
            return Json(SchoolClasses, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSections(int SchoolClassID)
        {
            var Sections = GetSectionsBySchoolClassID(SchoolClassID);
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }
    }
}