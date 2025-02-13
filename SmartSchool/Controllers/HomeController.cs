using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Objects;
using Objects.DTO;
using SmartSchool.Models.Home;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static Common.clsenumration;

namespace SmartSchool.Controllers
{

    [Authorize]
    public class HomeController : BaseController
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        public ActionResult Index()
        {
            Dashboardstatisticsmodel model = new Dashboardstatisticsmodel();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            model.StaffID = GetCookie((int)clsenumration.UserData.StaffID);
            model.CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var schoolBranchManager = factory.CreateSchoolBranchsManager();
            if (SchoolID == -1)
            {
                var newSchool = schoolBranchManager.Find(a => a.IsNew == true && a.CompanyID == model.CompanyID).ToList();
                if (newSchool != null)
                {
                    if (newSchool.Count > 0)
                    {
                        return RedirectToAction("RegistrationStepTwo", "SchoolRegistration");
                    }
                }
            }
            model.StaffType = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserType));
            model.PrevSchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.PreviousSchoolID));
            model.SchoolID = SchoolID;
            model.SchoolList = GetSchoolsByCompanyID(model.CompanyID);
            var SystemSettingManager = factory.CreateSystemSettingsManager();
            model.AcademicYear = SystemSettingManager.GetAll().Select(a => a.CurrentAcademicYear).FirstOrDefault();
            if (model.PrevSchoolID == -1)
            {
                model.CompanySchoolID = SchoolID;
            }
            var BusInfoManager = factory.CreateBusInfosManager();
            int _SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var StaffManager = factory.CreateStaffsManager();
            var StaffDetailsManager = factory.CreateStaffJobDetailsManager();
            var StudentManager = factory.CreateStudentsManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var AttendanceManger = factory.CreateAttendancesManager();
            var employeeresult = StaffManager.GetEmployeeStatisticsByDesgination(SchoolID);
            var studentresult = StudentManager.GetStudentStatistics(SchoolID);
            model.NumOfEmployees = employeeresult.Count();
            model.NumOfFirstTeacher = employeeresult.Where(a => a.DesignationID == 12).Count();
            model.NumOfSupervisorTeacher = employeeresult.Where(a => a.DesignationID == 24).Count();
            model.NumOfTeachers = model.NumOfFirstTeacher + model.NumOfSupervisorTeacher + employeeresult.Where(a => a.DesignationID == 6).Count();
            model.NumOfBusEscort = employeeresult.Where(a => a.DesignationID == 14).Count();
            model.NumOfCleanWorker = employeeresult.Where(a => a.DesignationID == 11).Count();
            model.NumOfAccountants = employeeresult.Where(a => a.DesignationID == 8).Count();
            int ManagerDepartmentID = DepartmentManager.Find(a => a.SchoolID == SchoolID && a.DepartmentEnglishName.Contains("Manager")).Select(a => a.DepartmentID).FirstOrDefault();
            model.NumOfAdministrative = StaffDetailsManager.Find(a => a.Department == ManagerDepartmentID).Select(a => a.StaffID).ToList().Count();
            model.NumOfYoungerStudents = studentresult.Where(a => a.AgeClassificationID == 1).Count();
            model.NumOfElderStudents = studentresult.Where(a => a.AgeClassificationID == 2).Count();
            model.NumOfStudents = model.NumOfYoungerStudents + model.NumOfElderStudents;
            string datenow = DateTime.Now.Date.ToShortDateString();
            model.NumOfFullAttendance = AttendanceManger.Find(a => a.AttendanceType == 1 && a.SchoolID == SchoolID && a.AttendanceDate == datenow).ToList().Count();
            model.NumOfPartialAttendance = AttendanceManger.Find(a => a.AttendanceType == 2 && a.SchoolID == SchoolID && a.AttendanceDate == datenow).ToList().Count();
            model.NumOfAbsent = AttendanceManger.Find(a => a.AttendanceType == 3 && a.SchoolID == SchoolID && a.AttendanceDate == datenow).ToList().Count();

            ////var Requests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
            ////                join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
            ////                on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
            ////                where ExternalStudentSchoolDetail.SchoolID == SchoolID
            ////                select ExternalGuardStudentsRequest).ToList();
            ////model.PendingRequests = Requests.Where(a => a.RequestStatus == (int)clsenumration.RequestStatus.pending).ToList().Count();

            //model.NumberOfBuses = BusInfoManager.Find(c => c.SchoolID == _SchoolID).ToList().Count();

            model.ShowNotification = false;
            model.ShowQeustionsForms = false;
            int UserType = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserType));
            if (UserType == (int)UserTypes.student)
            {
                bool IsActive = false;
                string StudentID = GetCookie((int)clsenumration.UserData.StudentID);
                model.StudentID = StudentID;

                model.StudentSectionID = factory.CreateStudentSchoolDetailsManager().Find(s => s.StudentID == StudentID).FirstOrDefault().SectionID;
                model.Photo = factory.CreateStudentsManager().Find(s => s.StudentID == StudentID).FirstOrDefault().Photo;
                var base64 = "";
                var imgSrc = "../../AppContent/Images/avatar.jpeg";
                if (model.Photo != null)
                {
                    base64 = Convert.ToBase64String(model.Photo);
                    imgSrc = String.Format("data:image/*;base64,{0}", base64);
                }
                Session["UserImage"] = imgSrc;

                var notificationManager = factory.CreateNotificationsWebsManager();
                var notifcation = notificationManager.Find(a => a.ToID == StudentID && a.SchoolID == SchoolID).Take(10).OrderByDescending(a => a.CreationDate);
                model.ShowNotification = true;
                model.ShowQeustionsForms = true;
                List<NotificationDTO> notificationList = new List<NotificationDTO>();
                var MsteamsSessionAttendesManager = factory.CreateMSTeams_SessionAttendessManager();
                var MsteamsSessionsManager = factory.CreateMSTeams_SessionssManager();

                foreach (var item in notifcation)
                {
                    var sessiondetails = MsteamsSessionsManager.Find(a => a.MSTeamSessionID == item.ModuleID).FirstOrDefault();
                    var currenttime = DateTime.Now;
                    if (currenttime >= sessiondetails.StartTime.Value && currenttime <= sessiondetails.StartTime.Value.AddMinutes(60))
                        IsActive = true;
                    else
                        IsActive = false;
                    NotificationDTO obj = new NotificationDTO();
                    obj.ArabicDescription = item.ArabicDescription;
                    obj.EnglishDescription = item.EnglishDescription;
                    obj.CreatedByID = item.CreatedByID;
                    obj.CreationDate = item.CreationDate;
                    obj.Link = item.Link;
                    obj.NotifcationID = item.NotifcationID;
                    obj.ToID = item.ToID;
                    obj.ToGroupID = item.ToGroupID;
                    obj.Visited = item.Visited;
                    obj.NotificationTypeID = item.NotificationTypeID;
                    obj.SchoolID = item.SchoolID;
                    obj.IsActive = IsActive;
                    notificationList.Add(obj);
                }

                model.NotifcationWeb = notificationList;
                model.ExternalGuardianDashboard = false;
            }
            PreparemMenuModel(model);
            if (model.ExternalGuardianDashboard)
                return RedirectToAction("ParentRegistrationRequests", "Registration");

            return View(model);
        }

        private void PreparemMenuModel(Dashboardstatisticsmodel model)
        {
            //string xxx = Decrypt("M2YzYmRi");

            string UserID = GetCookie((int)clsenumration.UserData.UserID);
            string GuardianID = GetCookie((int)clsenumration.UserData.GuardianID);
            int UserType = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserType));
            string CompanyID = GetCookie((int)clsenumration.UserData.CompanyID);
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string staffID = GetCookie((int)clsenumration.UserData.StaffID);
            string accessedUserDesgniation = "";
            if (string.IsNullOrEmpty(GuardianID))
            {
                model.Employees = false;
                model.Teachers = false;
                model.Students = false;
                model.RegisterRequests = false;
                model.TimeAttendees = false;
                model.Pay = false;
                model.Map = false;
                model.Calendar = false;
                model.TimeTable = false;
                model.TimeTableTeacher = false;
                model.TimeTableStudent = false;
                model.SystemManagment = false;
                model.Buses = false;
                model.SchoolDashboard = false;
                model.ExternalGuardianDashboard = true;
                model.QuestionsBank = false;
            }
            else if (GuardianID == "-1") // Login for Manager or teacher or user not parent
            {
                var StaffJobDetailsManager = factory.CreateStaffJobDetailsManager();
                int Desgniation = StaffJobDetailsManager.Find(a => a.SchoolID == SchoolID && a.StaffID == staffID).Select(a => a.Designation.Value).FirstOrDefault();
                if (Desgniation == 6)
                {
                    accessedUserDesgniation = "Teacher";
                    model.Buses = false;
                }
                else if (Desgniation == 12)
                {
                    accessedUserDesgniation = "F_Teacher";
                    model.Buses = false;
                }


                //Go to Privilegs
                if (UserType == 0) //Can see every thing(Admin -can see all company branches and edit, wirte, delete ...)
                {
                    if (accessedUserDesgniation == "Teacher" || accessedUserDesgniation == "F_Teacher")
                    {
                        model.TimeTableTeacher = true;
                        model.TimeTableStudent = false;

                    }
                    else
                    {
                        model.TimeTableTeacher = false;
                        model.TimeTableStudent = false;
                    }
                    if (CompanyID == "1000000" && SchoolID == 1000000 && staffID == "100000000") //Show all companies in system
                    {
                        var HeadQuartarManager = factory.CreateHeadquarterssManager();
                        var Companies = HeadQuartarManager.GetAll();
                        var CompaniesList = (from company in Companies
                                             select new LookupDTO
                                             {
                                                 Description = company.CompanyArabicName.ToString(),
                                                 DescriptionAR = company.CompanyEnglishName.ToString(),
                                                 ID = (company.CompanyID)
                                             }).ToList();
                        CompaniesList.Insert(0, new LookupDTO() { Description = CurrentLanguage == Languges.English ? "All" : "الكل", ID = 0 });
                        model.Employees = false;
                        model.Teachers = false;
                        model.Students = false;
                        model.RegisterRequests = false;

                        model.TimeAttendees = false;
                        model.Pay = false;
                        model.Map = false;
                        model.Calendar = false;
                        model.TimeTable = false;
                        model.TimeTableTeacher = false;
                        model.TimeTableStudent = false;
                        model.Buses = false;
                        model.SchoolDashboard = false;
                        model.QuestionsBank = false;

                    }
                    else
                    {
                        model.GenerateUsers = true;
                        model.SendSms = true;
                        model.SystemUsers = true;
                        model.StudentRegistration = true;

                        string query = string.Empty;
                        if (SchoolID == -1)
                        {
                            query = "SELECT Photo FROM Headquarters WHERE CompanyID = @CompanyID";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        byte[] imageData = null;
                                        if (!(reader[0] is DBNull))
                                        {
                                            // Get the image data from the reader
                                            imageData = (byte[])reader[0];
                                            model.Photo = imageData;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            query = "SELECT Photo FROM SchoolBranches WHERE SchoolID = @SchoolID";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand cmd = new SqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@SchoolID", SchoolID);
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        byte[] imageData = null;
                                        if (!(reader[0] is DBNull))
                                        {
                                            // Get the image data from the reader
                                            imageData = (byte[])reader[0];
                                            model.Photo = imageData;
                                        }
                                    }
                                }
                            }
                        }

                        var base64 = "";
                        var imgSrc = "../../AppContent/Images/avatar.jpeg";
                        if (model.Photo != null)
                        {
                            base64 = Convert.ToBase64String(model.Photo);
                            imgSrc = String.Format("data:image/*;base64,{0}", base64);
                        }
                        Session["UserImage"] = imgSrc;
                    }

                }
                if (UserType == 1) //Can see limited things (Global user - Just on school from branches (branch 1 or 2 or ...) and edit, write, delete ...)
                { //No thing to do
                    model.Employees = false;
                    model.Teachers = false;
                    model.Students = false;
                    model.RegisterRequests = false;

                    model.TimeAttendees = false;
                    model.Pay = false;
                    model.Map = false;
                    model.Calendar = true;
                    model.TimeTable = false;
                    if (accessedUserDesgniation == "Teacher" || accessedUserDesgniation == "F_Teacher")
                    {
                        model.TimeTableTeacher = true;
                    }
                    else
                    {
                        model.TimeTableTeacher = false;
                    }
                    model.TimeTableStudent = false;
                    model.SystemManagment = false;
                    model.Buses = false;
                    model.SchoolDashboard = false;
                    model.QuestionsBank = false;
                }
                if (UserType == 2) // Can see limited things (Local user)
                {
                    model.Employees = false;
                    model.Teachers = false;
                    model.Students = false;
                    model.RegisterRequests = false;

                    model.TimeAttendees = false;
                    model.Pay = false;
                    model.Map = false;
                    model.Calendar = true;
                    model.TimeTable = false;
                    model.TimeTableTeacher = true;
                    model.TimeTableStudent = false;
                    model.SystemManagment = false;
                    model.Buses = false;
                    model.VirtualClassRoom = true;
                    model.SchoolDashboard = false;
                    model.QuestionsBank = true;
                    model.Photo = factory.CreateStaffsManager().Find(s => s.StaffID == staffID).FirstOrDefault().Photo;

                    var base64 = "";
                    var imgSrc = "../../AppContent/Images/avatar.jpeg";
                    if (model.Photo != null)
                    {
                        base64 = Convert.ToBase64String(model.Photo);
                        imgSrc = String.Format("data:image/*;base64,{0}", base64);
                    }
                    Session["UserImage"] = imgSrc;
                }
            }
            else // login as a parent
            {
                model.Employees = false;
                model.Teachers = false;
                model.Students = false;
                model.RegisterRequests = false;

                model.TimeAttendees = false;
                model.Pay = false;
                model.Map = false;
                model.Calendar = false;
                model.TimeTable = false;
                model.TimeTableTeacher = false;
                model.TimeTableStudent = true;
                model.SystemManagment = false;
                model.Buses = false;
                model.ExternalGuardianDashboard = true;
                model.QuestionsBank = false;

            }
            if (UserType == 8)
                model.ExternalGuardianDashboard = false;
        }

        //'StaffID': $("#StaffID").val(), 'SchoolID': $("#SchoolID").val(), 'CompanyID': $("#CompanyID").val(),
        //                    'StaffType': $("#StaffType").val(), 'AcademicYear': $("#AcademicYear").val()
        [HttpGet]
        public JsonResult GetEncryptedUrl(string Params)
        {
            int url_timeH = DateTime.Now.Hour;
            int url_timeM = DateTime.Now.Minute;
            int url_timeS = DateTime.Now.Second;

            url_timeH = (url_timeH + 3) * 7;
            url_timeM = (url_timeM + 2) * 3;
            url_timeS = (url_timeS + 7) * 9;

            Params = Params + "*" + url_timeH.ToString() + "|" + url_timeM.ToString() + "|" + url_timeS.ToString();

            var result = EncryptUrl(Params);
            return Json(new { Success = true, result = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ChangeSchool(int SchoolID)
        {
            var UserManager = factory.CreateUsersManager();
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var User = UserManager.Find(a => a.SchoolID == SchoolID && a.UserType == 0 && a.StaffID == "10000" && a.CompanyID == CompanyID).FirstOrDefault();
            UserDTO userdto = new UserDTO();
            userdto.UserID = User.UserID;
            userdto.UserType = User.UserType.Value;
            userdto.Username = User.UserName;
            userdto.CompanyID = User.CompanyID.Value;
            userdto.SchoolID = User.SchoolID.Value;
            userdto.StaffID = User.StaffID;
            userdto.GuardianID = "-1";
            userdto.PreviousSchoolID = -1;
            Response.Cookies.Clear();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            setCookie(userdto);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}