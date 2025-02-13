using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Objects.DTO;
using SmartSchool.Models.SystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class SystemUsersController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        // GET: SystemUsers
        public ActionResult Index()
        {
            SystemUserModel model = new SystemUserModel();

            model.Groups = GetGroups(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SystemUserModel model)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            if (model.GroupID == (int)clsenumration.UsersGroups.Teacher)
            {
                var users = SchoolID == 1000000? (from user in context.Users
                                                  join employee in context.Staffs on user.StaffID equals employee.StaffID
                                                  where user.UserType == 2
                                                  select new SystemUsersDTO()
                                                  {
                                                      ID = user.StaffID,
                                                      Name = CurrentLanguage == Languges.English ? employee.StaffEnglishName : employee.StaffArabicName,
                                                      UserName = user.UserName,
                                                      Password = user.Password
                                                  }).Distinct().ToList() 
                                                  
                                                  : 
                                                  
                                                  (from user in context.Users
                                                   join employee in context.Staffs on user.StaffID equals employee.StaffID
                                                   where user.UserType == 2 && user.SchoolID == SchoolID
                                                   select new SystemUsersDTO()
                                                   {
                                                       ID = user.StaffID,
                                                       Name = CurrentLanguage == Languges.English ? employee.StaffEnglishName : employee.StaffArabicName,
                                                       UserName = user.UserName,
                                                       Password = user.Password
                                                   }).Distinct().ToList();
                foreach (var item in users)
                {
                    item.Password = Decrypt(item.Password);
                }
                model.Users = users;
            }
            else if (model.GroupID == (int)clsenumration.UsersGroups.Student)
            {
                var users = SchoolID == 1000000 ? (from student in context.Students
                                                   join studentlogin in context.Student_Login on student.StudentID equals studentlogin.StudentID
                                                   select new SystemUsersDTO()
                                                   {
                                                       ID = studentlogin.StudentID,
                                                       Name = CurrentLanguage == Languges.English ? student.StudentEnglishName : student.StudentArabicName,
                                                       UserName = studentlogin.Username,
                                                       Password = studentlogin.Password
                                                   }).Distinct().ToList() 
                                                   
                                                   :
                                                   
                                                   (from student in context.Students
                                                    join studentlogin in context.Student_Login on student.StudentID equals studentlogin.StudentID
                                                    where studentlogin.SchoolID == SchoolID
                                                    select new SystemUsersDTO()
                                                    {
                                                        ID = studentlogin.StudentID,
                                                        Name = CurrentLanguage == Languges.English ? student.StudentEnglishName : student.StudentArabicName,
                                                        UserName = studentlogin.Username,
                                                        Password = studentlogin.Password
                                                    }).Distinct().ToList();
                foreach (var item in users)
                {
                    item.Password = Decrypt(item.Password);
                }
                model.Users = users;
            }
            else if (model.GroupID == (int)clsenumration.UsersGroups.Guardian)
            {
                var users = SchoolID == 1000000 ? (from externalGuardian in context.ExternalGuardians
                                                   select new SystemUsersDTO()
                                                   {
                                                       id = externalGuardian.UserID,
                                                       Name = CurrentLanguage == Languges.English ? externalGuardian.EnglishName : externalGuardian.ArabicName,
                                                       UserName = externalGuardian.UserName,
                                                       Password = externalGuardian.Password
                                                   }).Distinct().ToList() 
                                                   
                                                   :

                                                   (from guardian in context.Guardians
                                                    join guardianschool in context.GuardianSchools on guardian.GuardianID equals guardianschool.GuardianID
                                                    join webuser in context.WebUsers on guardian.GuardianID.ToString() equals webuser.UserSystemID
                                                    where guardianschool.SchoolID == SchoolID
                                                    select new SystemUsersDTO()
                                                    {
                                                       id = guardian.GuardianID,
                                                       Name = CurrentLanguage == Languges.English ? guardian.GuardianEnglishName : guardian.GuardianArabicName,
                                                       UserName = webuser.UserName,
                                                       Password = webuser.Password
                                                    }).Distinct().ToList();
                foreach (var item in users)
                {
                    item.Password = Decrypt(item.Password);
                }
                model.Users = users;
            }
            model.Groups = GetGroups(model);

            return View(model);
        }
        public List<LookupDTO> GetGroups(SystemUserModel model)
        {
            List<LookupDTO> groups = new List<LookupDTO>();

            var teacher = new LookupDTO()
            {
                Description = "Teachers",
                DescriptionAR = "المعلمين",
                ID = 1

            };
            var student = new LookupDTO()
            {
                Description = "Students",
                DescriptionAR = "الطلاب",
                ID = 2

            };
            var guardian = new LookupDTO()
            {
                Description = "Guardians",
                DescriptionAR = "أولياء الأمور",
                ID = 3

            };
            groups.Add(teacher);
            groups.Add(student);
            groups.Add(guardian);
            return groups;
        }
    }
}