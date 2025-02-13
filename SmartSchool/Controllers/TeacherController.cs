using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Microsoft.Ajax.Utilities;
using Objects;
using Objects.DTO;
using SmartSchool.Models.Teacher;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class TeacherController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllTeachers()
        {

            var StaffManager = factory.CreateStaffsManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            string schoolyear = getCurrentAcademicYear();

            var AllTeachers = StaffManager.GetTeachers(SchoolID, lang, schoolyear);
            return View(AllTeachers);
        }

        public ActionResult EditTeacher(string StaffID)
        {

            var _StaffID = Decrypt(StaffID);
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            TeacherRegisterModel model = new TeacherRegisterModel();
            model.StaffID = _StaffID;
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var ContactDetailsManager = factory.CreateStaffContactDetailsManager();
            var StaffJobDetailsManager = factory.CreateStaffJobDetailsManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var QualificationManger = factory.CreateQualificationsManager();
            var SpecializationManger = factory.CreateSpecializationsManager();
            var QualificationsManager = factory.CreateQualificationsManager();
            var SpecializationsManager = factory.CreateSpecializationsManager();
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var StaffManager = factory.CreateStaffsManager();
            var ClassroomsManager = factory.CreateClassroomsManager();
            var TeacherManager = factory.CreateTeachersManager();
            var ContactDetails = ContactDetailsManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();
            var staff = StaffManager.Find(c => c.StaffID == model.StaffID).FirstOrDefault();

            string schoolyear = getCurrentAcademicYear();
            var TeacherInfo = TeacherManager.Find(c => c.StaffID == model.StaffID).FirstOrDefault();
            var AllTeachers = StaffManager.GetTeachers(SchoolID, lang, schoolyear);
            var TeachersList = (from Teachers in AllTeachers
                                select new LookupDTO
                                {
                                    Description = Teachers.TeacherName.ToString(),
                                    DescriptionAR = Teachers.TeacherName.ToString(),
                                    id = Teachers.StaffID
                                }).ToList();

            model.TeacherNameList = TeachersList;
            //int AssistanceStaffID;
            //bool isParsed = int.TryParse(TeacherInfo.AssistantTeacher, out AssistanceStaffID);
            model.StrAssistanceStaffID =
                (TeacherInfo.AssistantTeacher == null || TeacherInfo.AssistantTeacher.ToString().Length == 0)
                ?
                "-1"
                :
                TeacherInfo.AssistantTeacher;
            model.NumberofSessins = TeacherInfo.NumberofSessins;
            model.TimeTableSessions = TeacherInfo.TimeTableSessions.Value;
            model.JobNumber = model.StaffID;
            model.Email = ContactDetails.Email;
            model.MobileNo = ContactDetails.MobileNo;
            model.TeacherName = ViewBag.CurrentLanguage == Languges.English ? staff.StaffEnglishName : staff.StaffArabicName;

            var AllClassroom = ClassroomsManager.Find(x => x.SchoolID == SchoolID).ToList();

            var ClassroomList = (from Classroom in AllClassroom
                                 select new LookupDTO
                                 {
                                     Description = "Building:" + Classroom.BuildingArabicName.ToString() + "Room:" + Classroom.CLassRoomNumber,
                                     DescriptionAR = "المبنى:" + " " + Classroom.BuildingEnglishName.ToString() + " " + "الغرفه:" + "  " + Classroom.CLassRoomNumber,
                                     ID = Classroom.ClassRoomID
                                 }).ToList();

            model.ClassroomList = ClassroomList;
            model.ClassroomID = TeacherInfo.Classroom.Value;

            var JobDetailsInfo = StaffJobDetailsManager.Find(a => a.SchoolID == SchoolID && a.StaffID == _StaffID).FirstOrDefault();
            var DepartmentID = Convert.ToInt32(JobDetailsInfo.Department);
            var Specialization = SpecializationManger.Find(c => c.SpecializationID == JobDetailsInfo.Specialization).FirstOrDefault();

            var Department = DepartmentManager.Find(c => c.SchoolID == SchoolID && c.DepartmentID == DepartmentID).FirstOrDefault();
            var Qualification = QualificationManger.Find(c => c.QualificationID == JobDetailsInfo.Qualification).FirstOrDefault();

            var SchoolBranch = SchoolBranchsManager.GetAll();
            var SchoolBranchList = (from School in SchoolBranch
                                    select new LookupDTO
                                    {
                                        Description = School.SchoolEnglishName.ToString(),
                                        DescriptionAR = School.SchoolArabicName.ToString(),
                                        ID = (School.SchoolID)
                                    }).ToList();
            model.schoolList = SchoolBranchList;
            model.schoolID = SchoolID;
            string DecyptedStaffID = Decrypt(StaffID);
            var TeacherExperiencesManger = factory.CreateTeacherExperiencesManager();
            var TeacherExperiences = TeacherExperiencesManger.Find(c => c.SchoolID == SchoolID && c.TeacherID == DecyptedStaffID).ToList();
            var ClassesManger = factory.CreateSchoolClasssManager();
            var Classes = ClassesManger.Find(c => c.SchoolID == SchoolID).ToList();
            model.SchoolClassList = (from c in Classes
                                     where c.SchoolID == SchoolID
                                     select new LookupDTO
                                     {
                                         Description = c.SchoolClassEnglishName.ToString(),
                                         DescriptionAR = c.SchoolClassArabicName.ToString(),
                                         ID = (c.SchoolClassID)
                                     }).ToList();
            model.SchoolClassList2 = (from t in TeacherExperiences
                                      join c in Classes
                                      on t.SchoolClassID equals c.SchoolClassID
                                      where c.SchoolID == SchoolID && t.TeacherID == DecyptedStaffID
                                      select new LookupDTO
                                      {
                                          Description = c.SchoolClassEnglishName.ToString(),
                                          DescriptionAR = c.SchoolClassArabicName.ToString(),
                                          ID = (c.SchoolClassID)
                                      }).DistinctBy(c => c.ID).OrderBy(c => c.ID).ToList();
            var Deparments = DepartmentManager.Find(a => a.SchoolID == SchoolID);

            var DepartmentList = (from dep in Deparments
                                  select new LookupDTO
                                  {
                                      Description = dep.DepartmentEnglishName.ToString(),
                                      DescriptionAR = dep.DepartmentArabicName.ToString(),
                                      ID = (dep.DepartmentID)
                                  }).ToList();
            model.DepartmentsList = DepartmentList;
            model.DepartmentID = Department.DepartmentID;
            var Qualifications = QualificationsManager.GetAll();
            var QualificationList = (from q in Qualifications
                                     select new LookupDTO
                                     {
                                         Description = q.QualificationEnglishName.ToString(),
                                         DescriptionAR = q.QualificationArabicName.ToString(),
                                         ID = (q.QualificationID)
                                     }).ToList();
            model.QualificationList = QualificationList;
            model.QualificationID = Qualification.QualificationID;

            var Spelizations = SpecializationsManager.GetAll();
            var SpelizationsList = (from s in Spelizations
                                    select new LookupDTO
                                    {
                                        Description = s.SpecializationEnglishName.ToString(),
                                        DescriptionAR = s.SpecializationArabicName.ToString(),
                                        ID = (s.SpecializationID)
                                    }).ToList();
            model.SpecializationList = SpelizationsList;
            model.SpecializationID = Specialization.SpecializationID;
            model.YearOfExperience = JobDetailsInfo.YearOfExperience.ToString();
            model.YearofGraduation = JobDetailsInfo.YearofGraduation.ToString();
            model.Institution = JobDetailsInfo.Institution != null ?
                JobDetailsInfo.Institution.ToString() : "";
            model.DateOfJoining = JobDetailsInfo.DateOfJoining.Value.ToShortDateString();

            return View(model);
        }

        [HttpPost]
        public JsonResult RemoveExperience(int TeacherExperienceID)
        {
            try
            {
                var TeacherExperiencesManager = factory.CreateTeacherExperiencesManager();
                var TeacherExperience = TeacherExperiencesManager.GetAll().Where(c => c.TeacherExperienceID == TeacherExperienceID).FirstOrDefault();


                TeacherExperiencesManager.Delete(TeacherExperience);


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult TeacherExperience(int SchoolClassID, int SubjectID, string TotalYearsOfExperience, int Rate, string StaffID)
        {
            try
            {

                var TeacherExperiencesManager = factory.CreateTeacherExperiencesManager();
                var SchoolClasssManager = factory.CreateSchoolClasssManager();
                var _CurriculumID = SchoolClasssManager.Find(c => c.SchoolClassID == SchoolClassID).Select(c => c.CurriculumID).FirstOrDefault();

                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                string schoolyear = getCurrentAcademicYear();

                TeacherExperience te = new TeacherExperience
                {

                    SchoolID = _SchoolID,
                    Curriculum = _CurriculumID,
                    SchoolYear = schoolyear,
                    TeacherID = StaffID,
                    SchoolClassID = SchoolClassID,
                    SubjectID = SubjectID,
                    YearsofExperience = TotalYearsOfExperience,
                    //TeacherWeight = Rate,
                    //AppraisalValue = Convert.ToInt32(Rate),

                };
                TeacherExperiencesManager.Add(te);


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TeacherExperiencesList(string ID)
        {
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string _schoolyear = getCurrentAcademicYear();
            var query = new List<TeacherExperiences>();
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
            {
                query = (from te in entities.TeacherExperiences
                         join SC in entities.SchoolClasses
                         on te.SchoolClassID equals SC.SchoolClassID
                         join su in entities.Subjects
                         on te.SubjectID equals su.SubjectID
                         //join Appraisal in entities.AppraisalValues on te.AppraisalValue.Value equals Appraisal.AppraisalValueID
                         where te.TeacherID == ID && te.SchoolID == _SchoolID && te.SchoolYear == _schoolyear
                         select new TeacherExperiences
                         {
                             TeacherExperienceID = te.TeacherExperienceID,
                             SchoolClassName = lang == "ar" ? SC.SchoolClassArabicName : SC.SchoolClassEnglishName,
                             SubjectName = lang == "ar" ? su.SubjectArabicName : su.SubjectEnglishName,
                             YearsOfExperience = te.YearsofExperience,
                             //AppraisalValueText = lang == "ar" ? Appraisal.AppraisalValueTextinArabic : Appraisal.AppraisalValueTextinEnglish,
                             //AppraisalValue = te.AppraisalValue.Value
                         }).OrderByDescending(t => t.TeacherExperienceID).ToList();
            }


            return View(query);
        }

        public ActionResult TeacherSubjectsList(string ID, int SchoolClassID)//, int SectionId,string SectionCode)
        {
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string _schoolyear = getCurrentAcademicYear();
            var query = new List<TeacherSubjects>();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
            {
                //query = (from ts in entities.TeacherExperiences
                //         join SC in entities.SchoolClasses
                //         on ts.SchoolClassID equals SC.SchoolClassID
                //         where ts.TeacherID == ID && ts.SchoolID == _SchoolID && ts.SchoolYear == _schoolyear
                //         && (SchoolClassID != 0 ? ts.SchoolClassID == SchoolClassID : 1 == 1)
                //         && (SectionId != 0 ? ts.SectionCode.Contains(SectionCode) : 1 == 1)
                //         select new TeacherSubjects
                //         {
                //             TeacherID = ts.TeacherID,
                //             SchoolClassName = lang == "ar" ? SC.SchoolClassArabicName : SC.SchoolClassEnglishName,
                //             SubjectName = lang == "ar" ?
                //             entities.Subjects.Where(c => c.SchoolClassID == ts.SchoolClassID).Select(c => c.SubjectArabicName).FirstOrDefault()
                //             :
                //             entities.Subjects.Where(c => c.SchoolClassID == ts.SchoolClassID).Select(c => c.SubjectEnglishName).FirstOrDefault(),
                //             NumberOfSessionsPerWeek = entities.Subjects.Where(c => c.SchoolClassID == ts.SchoolClassID).Select(c => c.NumberOfSessionsPerWeek).FirstOrDefault(),
                //             SectionCode = ts.SectionCode
                //         }).ToList();

                query = (from te in entities.TeacherExperiences
                         join sc in entities.SchoolClasses
                         on te.SchoolClassID equals sc.SchoolClassID
                         join su in entities.Subjects
                         on te.SubjectID equals su.SubjectID
                         where te.TeacherID == ID && te.SchoolID == _SchoolID && te.SchoolYear == _schoolyear
                         && (SchoolClassID != 0 ? te.SchoolClassID == SchoolClassID : 1 == 1)
                         select new TeacherSubjects
                         {
                             TeacherID = te.TeacherID,
                             SchoolClassID = sc.SchoolClassID,
                             SchoolClassName = lang == "ar" ? sc.SchoolClassArabicName : sc.SchoolClassEnglishName,
                             SubjectName = lang == "ar" ? su.SubjectEnglishName : su.SubjectArabicName,
                             NumberOfSessionsPerWeek = su.NumberOfSessionsPerWeek,
                         }).OrderBy(sc => sc.SchoolClassID).ToList();
            }
            return View(query);
        }

        [HttpPost]
        public JsonResult EditTeacherInformation(TeacherContactInfo model)
        {
            try
            {
                //update table Teacher 
                var TeacherManager = factory.CreateTeachersManager();
                var StaffContactDetails = factory.CreateStaffContactDetailsManager();
                var staffManager = factory.CreateStaffsManager();
                var TeacherToUpdate = TeacherManager.Find(c => c.StaffID == model.StaffID).FirstOrDefault();

                TeacherToUpdate.NumberofSessins = model.NumberofSessins;
                TeacherToUpdate.Classroom = model.ClassroomID;
                TeacherToUpdate.TimeTableSessions = model.TimeTableSessions;
                TeacherToUpdate.AssistantTeacher = model.AssistanceStaffID;
                TeacherManager.Update(TeacherToUpdate);

                //Update teacher StaffContactDetails Mobile and Email 
                var TeacherContactDetailsToUpdate = StaffContactDetails.Find(c => c.StaffID == model.StaffID).FirstOrDefault();
                TeacherContactDetailsToUpdate.Email = model.Email;
                TeacherContactDetailsToUpdate.MobileNo = model.MobileNo;

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditTeacherJobDetails(StaffJobDetails model)
        {

            try
            {
                int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                var StaffJobDetailsManager = factory.CreateStaffJobDetailsManager();
                var JobDetails = StaffJobDetailsManager.Find(c => c.StaffID == model.StaffID && c.SchoolID == _SchoolID).FirstOrDefault();

                JobDetails.Institution = model.Institution;
                JobDetails.Department = model.DepartmentID;
                JobDetails.Qualification = model.QualificationID;
                JobDetails.Specialization = model.SpecializationID;
                JobDetails.YearofGraduation = model.YearofGraduation;
                JobDetails.YearOfExperience = model.TotalYearsOfExperience;
                JobDetails.DateOfJoining = model.DateOfJoining;



                StaffJobDetailsManager.Update(JobDetails);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RegisterTeacher()
        {
            TeacherRegisterModel model = new TeacherRegisterModel();
            PrepareRegisterModel(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult RegisterTeacher(TeacherContactInfo model)
        {
            try
            {
                var TeacherManager = factory.CreateTeachersManager();

                bool exist = TeacherManager.Find(c => c.StaffID == model.StaffID).FirstOrDefault() != null ? true : false;
                if (!exist)
                {
                    Teacher Teacher = new Teacher
                    {
                        StaffID = model.StaffID,
                        NumberofSessins = model.NumberofSessins,
                        TimeTableSessions = model.TimeTableSessions,
                        Classroom = model.ClassroomID,
                        AssistantTeacher = model.AssistanceStaffID


                    };
                    TeacherManager.Add(Teacher);
                }
                else
                {
                    return Json(new { Success = false, Exist = exist }, JsonRequestBehavior.AllowGet);

                }


                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetInformation(string StaffID)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            TeacherRegisterModel JobDetails = new TeacherRegisterModel();
            var ContactDetailsManager = factory.CreateStaffContactDetailsManager();
            var StaffJobDetailsManager = factory.CreateStaffJobDetailsManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var QualificationManger = factory.CreateQualificationsManager();
            var SpecializationManger = factory.CreateSpecializationsManager();
            var JobDetailsInfo = StaffJobDetailsManager.Find(a => a.SchoolID == SchoolID && a.StaffID == StaffID).FirstOrDefault();
            var ContactDetails = ContactDetailsManager.Find(a => a.StaffID == StaffID).FirstOrDefault();
            var DepartmentID = Convert.ToInt32(JobDetailsInfo.Department);
            var Specialization = SpecializationManger.Find(c => c.SpecializationID == JobDetailsInfo.Specialization).FirstOrDefault();

            var Department = DepartmentManager.Find(c => c.SchoolID == SchoolID && c.DepartmentID == DepartmentID).FirstOrDefault();
            var Qualification = QualificationManger.Find(c => c.QualificationID == JobDetailsInfo.Qualification).FirstOrDefault();
            JobDetails.Department = ViewBag.CurrentLanguage == Languges.English ? Department.DepartmentEnglishName : Department.DepartmentArabicName;
            JobDetails.Specialization = ViewBag.CurrentLanguage == Languges.English ? Specialization.SpecializationEnglishName : Specialization.SpecializationArabicName;
            JobDetails.YearOfExperience = JobDetailsInfo.YearOfExperience.ToString();
            JobDetails.YearofGraduation = JobDetailsInfo.YearofGraduation.ToString();
            JobDetails.Institution = JobDetailsInfo.Institution != null ?
                JobDetailsInfo.Institution.ToString() : "";
            JobDetails.DateOfJoining = JobDetailsInfo.DateOfJoining.Value.ToShortDateString();
            return Json(new { Success = true, data = ContactDetails, JobDetailsData = JobDetails }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTimeTable(string StaffID)
        {
            TeacherRegisterModel model = new TeacherRegisterModel();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var SystemSettingsperSchoolManger = factory.CreateSystemSettingsperSchoolsManager();

            var schoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            var TimetableType = SystemSettingsperSchoolManger.Find(a => a.SchoolID == SchoolID).Select(a => a.TimetableType).FirstOrDefault();

            model.TimetableType = TimetableType;
            model.schoolID = SchoolID;
            model.WeekStartingDay = schoolSettings.WeekStartingDay;
            model.NumberofSessionsPerDay = schoolSettings.NumberofSessionsPerDay;
            model.SessionDuration = schoolSettings.SessionDuration;
            model.BreakBetweenSessionsDuration = schoolSettings.BreakBetweenSessionsDuration;
            model.BreakDuration = schoolSettings.BreakDuration;
            model.BreakAfterSession = 4;// is the default value.
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

        }

        [HttpPost]
        public JsonResult GetSections(int SchoolClassID, string StaffID)
        {
            var TeacherExperiencesManager = factory.CreateTeacherExperiencesManager();
            var TeacherExperiences = TeacherExperiencesManager.Find(t => t.TeacherID == StaffID).ToList();
            var SubjectsManager = factory.CreateSubjectsManager();
            var Subjects = SubjectsManager.Find(s => s.SchoolClassID == SchoolClassID).ToList();
            var SectionsManager = factory.CreateSectionsManager();
            var Sections = SectionsManager.Find(a => a.SchoolClassID == SchoolClassID).ToList();
            var sectionsList = (from t in TeacherExperiences
                                join su in Subjects
                                on t.SubjectID equals su.SubjectID
                                join se in Sections
                                on su.SectionID equals se.SectionID
                                select new LookupDTO
                                {
                                    Description = CurrentLanguage == Languges.English ?
                                    se.SectionEnglishName.ToString() : se.SectionArabicName.ToString(),
                                    DescriptionAR = se.SectionArabicName.ToString(),
                                    ID = (se.SectionID)
                                }).ToList();
            return Json(sectionsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubjects(int SchoolClassID)
        {
            var Subjects = GetSubjectsBySchoolClassID(SchoolClassID);
            return Json(Subjects, JsonRequestBehavior.AllowGet);
        }

        private void PrepareRegisterModel(TeacherRegisterModel model)
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var StaffManager = factory.CreateStaffsManager();
            var ClassroomsManager = factory.CreateClassroomsManager();
            var TeacherManager = factory.CreateTeachersManager();
            var SchoolBranch = SchoolBranchsManager.GetAll();
            var SchoolBranchList = (from School in SchoolBranch
                                    select new LookupDTO
                                    {
                                        Description = School.SchoolEnglishName.ToString(),
                                        DescriptionAR = School.SchoolArabicName.ToString(),
                                        ID = (School.SchoolID)
                                    }).ToList();
            model.schoolList = SchoolBranchList;
            model.schoolID = SchoolID;
            string schoolyear = getCurrentAcademicYear();

            var AllTeachers = (from Staff in context.Staffs
                               join StaffJob in context.StaffJobDetails
                               on Staff.StaffID equals StaffJob.StaffID
                               // 6 means its teacher, 12 means its first teacher.
                               where (StaffJob.Designation == 6 || StaffJob.Designation == 12) &&
                               StaffJob.SchoolID == SchoolID
                               select new TeacherDTO
                               {
                                   StaffID = Staff.StaffID,
                                   TeacherName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") :
                                   Staff.StaffArabicName.Replace("-", " "),

                               }).ToList();
            var TeachersList = (from Teachers in AllTeachers
                                select new LookupDTO
                                {
                                    Description = Teachers.TeacherName.ToString(),
                                    DescriptionAR = Teachers.TeacherName.ToString(),
                                    id = Teachers.StaffID
                                }).ToList();
            var registeredTechers = StaffManager.GetTeachers(SchoolID, lang, schoolyear).Select(t => t.StaffID).ToList();
            model.TeacherNameList = TeachersList.Where(t => !registeredTechers.Contains(t.id)).ToList();

            var AllEmployee = StaffManager.GetEmployees(SchoolID, lang);
            var AllTeacherIDs = TeacherManager.GetAll().Select(c => c.StaffID).ToList();
            var EmployeeList = (from Employee in AllEmployee
                                select new LookupDTO
                                {
                                    Description = Employee.EmployeeName.ToString(),
                                    DescriptionAR = Employee.EmployeeName.ToString(),
                                    id = Employee.StaffID
                                }).Where(e => !AllTeacherIDs.Contains(e.id)).ToList();
            model.EmployeeList = EmployeeList;

            model.TimeTableSessions = 0;

            var AllClassroom = ClassroomsManager.Find(x => x.SchoolID == SchoolID).ToList();
            var ClassroomList = (from Classroom in AllClassroom
                                 select new LookupDTO
                                 {
                                     Description = "Building:" + Classroom.BuildingArabicName.ToString() + "Room:" + Classroom.CLassRoomNumber,
                                     DescriptionAR = "المبنى:" + " " + Classroom.BuildingEnglishName.ToString() + " " + "الغرفه:" + "  " + Classroom.CLassRoomNumber,
                                     ID = Classroom.ClassRoomID
                                 }).ToList();
            model.ClassroomList = ClassroomList;

            var ClassesManger = factory.CreateSchoolClasssManager();
            var Classes = ClassesManger.Find(c => c.SchoolID == SchoolID).ToList();
            model.SchoolClassList = (from c in Classes
                                     select new LookupDTO
                                     {
                                         Description = c.SchoolClassEnglishName.ToString(),
                                         DescriptionAR = c.SchoolClassArabicName.ToString(),
                                         ID = (c.SchoolClassID)
                                     }).ToList();
        }
    }
}