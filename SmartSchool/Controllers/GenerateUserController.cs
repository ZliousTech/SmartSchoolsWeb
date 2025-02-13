using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using SmartSchool.Models.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SmartSchool.Controllers
{
    public class GenerateUserController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: GenerateUser
        public ActionResult Index()
        {

            return View();
        }



        [HttpPost]
        public JsonResult GenrateStudentUser(bool SendSms)
        {
            var Student_LoginsManager = factory.CreateStudent_LoginsManager();
            var StudentsManager = factory.CreateStudentsManager();
            var GuardiansManager = factory.CreateGuardiansManager();
            var StudentSmsModel = new List<StudentSmsModel>();
            var entities = new SmartSchoolsEntities();
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var Student_LoginsIDs = Student_LoginsManager.GetAll().Where(c => c.IsDeleted == false && c.IsBlocked == false && c.SchoolID == this.SchoolID).Select(c => c.StudentID).ToList();
            string body = lang == "en" ? "Dear,{0} your Username:{1} & Password:{2}" : "{عزيزي {0} اسم المستخدم : {1} كلمة المرور: {2";
            if (Student_LoginsIDs != null && Student_LoginsIDs.Count > 0)
            {
                StudentSmsModel = (from st in entities.Students
                                   join sc in entities.StudentSchoolDetails
                                   on st.StudentID equals sc.StudentID
                                   join Gu in entities.Guardians
                                   on st.GuardianID equals Gu.GuardianID
                                   where sc.SchoolID == this.SchoolID && !Student_LoginsIDs.Contains(st.StudentID)
                                   select new StudentSmsModel
                                   {
                                       StudentID = st.StudentID,
                                       NationalNumber = Gu.NationalNumber,
                                       ResidencyNumber = Gu.ResidencyNumber,
                                       MobileNumber = Gu.MobileNumber,
                                       GuardianID = st.GuardianID,
                                       StudentName = lang == "en" ? st.StudentEnglishName : st.StudentArabicName
                                   }).ToList();

            }
            else
            {
                StudentSmsModel = (from st in entities.Students
                                   join sc in entities.StudentSchoolDetails
                                   on st.StudentID equals sc.StudentID
                                   join Gu in entities.Guardians
                                   on st.GuardianID equals Gu.GuardianID
                                   where sc.SchoolID == this.SchoolID
                                   select new StudentSmsModel
                                   {
                                       StudentID = st.StudentID,
                                       NationalNumber = Gu.NationalNumber,
                                       ResidencyNumber = Gu.ResidencyNumber,
                                       MobileNumber = Gu.MobileNumber,
                                       GuardianID = st.GuardianID,
                                       StudentName = lang == "en" ? st.StudentEnglishName : st.StudentArabicName
                                   }).ToList();

            }
            try
            {
                foreach (var item in StudentSmsModel)
                {
                    var Student_Login = new Objects.Student_Login
                    {
                        StudentID = item.StudentID,
                        Username = !string.IsNullOrEmpty(item.NationalNumber) ? item.NationalNumber : item.ResidencyNumber,
                        Password = GenratePassword(),
                        CreationDate = DateTime.Now,
                        IsBlocked = false,
                        IsDeleted = false,
                        SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID))
                    };

                    Student_LoginsManager.Add(Student_Login);
                    //if (SendSms)
                    //{
                    //    string _body = string.Format(body, item.StudentName, Student_Login.Username, Decrypt(Student_Login.Password));
                    //    var response = SendSMS(_body, item.MobileNumber);
                    //}

                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GenrateTeacherUser(bool SendSms)
        {
            var UsersManager = factory.CreateUsersManager();
            var StaffsManager = factory.CreateStaffsManager();
            var GuardiansManager = factory.CreateGuardiansManager();
            var TeachersSmsModel = new List<TeacherUserDto>();
            var entities = new SmartSchoolsEntities();
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var ExStaffIDs = UsersManager.GetAll().Where(c => c.SchoolID == this.SchoolID).Select(c => c.StaffID).ToList();
            string body = lang == "en" ? "Dear,{0} your Username:{1} & Password:{2}" : "{عزيزي {0} اسم المستخدم : {1} كلمة المرور: {2";
            if (ExStaffIDs != null && ExStaffIDs.Count > 0)
            {
                TeachersSmsModel = (from st in entities.Teachers
                                    join f in entities.Staffs
                                    on st.StaffID equals f.StaffID
                                    join J in entities.StaffJobDetails
                                    on st.StaffID equals J.StaffID
                                    where !ExStaffIDs.Contains(st.StaffID)
                                    && J.SchoolID == this.SchoolID
                                    select new TeacherUserDto
                                    {
                                        StaffID = f.StaffID,
                                        UserName = f.StaffEnglishName,

                                    }).ToList();

            }
            else
            {
                TeachersSmsModel = (from st in entities.Teachers
                                    join f in entities.Staffs
                                    on st.StaffID equals f.StaffID
                                    join J in entities.StaffJobDetails
                                    on st.StaffID equals J.StaffID
                                    where J.SchoolID == this.SchoolID
                                    select new TeacherUserDto
                                    {
                                        StaffID = f.StaffID,
                                        UserName = f.StaffEnglishName
                                    }).ToList();

            }
            
            try
            {
                int i_indx = 1;
                foreach (var item in TeachersSmsModel)
                {
                    var _SplitUserName = item.UserName.Split(' ');
                    string _username = Regex.Replace(_SplitUserName[0], "[-]+", " ", RegexOptions.Compiled) + " " +
                                       Regex.Replace(_SplitUserName[1], "[-]+", " ", RegexOptions.Compiled);

                    var User = new Objects.User();

                    User.SchoolID = this.SchoolID;
                    User.UserType = 2;
                    User.StaffID = item.StaffID;
                    User.UserName = _username.TrimStart() + i_indx.ToString();
                    User.Password = GenratePassword();
                    User.CompanyID = this.CompanyID;

                    UsersManager.Add(User);

                    i_indx++;

                    //if (SendSms)
                    //{
                    //    string _body = string.Format(body, item.StudentName, Student_Login.Username, Decrypt(Student_Login.Password));
                    //    var response = SendSMS(_body, item.MobileNumber);
                    //}
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public string GenratePassword()
        {
            Guid guid = Guid.NewGuid();
            var str = guid.ToString().Substring(0, 6);
            var password = Encrypt(str);
            return password;
        }

        public class TeacherUserDto
        {
            public string StaffID { get; set; }
            public string UserName { get; set; }
        }
    }
}