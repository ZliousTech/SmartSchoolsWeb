using Business.Base;
using Common;
using Common.Helpers;
using SmartSchool.Models.Common;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{

    public class CommonController : BaseController
    {
        BusinessComponentsFactory _factory = new BusinessComponentsFactory();
        // GET: Common
        public ActionResult SwitchLanguage()
        {
            LanguageHelper.SwitchLanguage();
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        //[HttpPost]
        //public ActionResult switchlang()
        //{
        //    LanguageHelper.SwitchLanguage();
        //    return Redirect(Request.UrlReferrer.AbsoluteUri);
        //}

        public ActionResult Menu()
        {
            MenuModel model = new MenuModel();
            string UserID = GetCookie((int)clsenumration.UserData.UserID);
            string GuardianID = GetCookie((int)clsenumration.UserData.GuardianID);
            int UserType = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserType));
            string CompanyID = GetCookie((int)clsenumration.UserData.CompanyID);
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string staffID = GetCookie((int)clsenumration.UserData.StaffID);
            string accessedUserDesgniation = "";

            if (GuardianID == "-1") // Login for Manager or teacher or user not parent
            {
                var StaffJobDetailsManager = _factory.CreateStaffJobDetailsManager();
                int Desgniation = StaffJobDetailsManager.Find(a => a.SchoolID == SchoolID && a.StaffID == staffID).Select(a => a.Designation.Value).FirstOrDefault();
                if (Desgniation == 6)
                {
                    accessedUserDesgniation = "Teacher";

                }
                else if (Desgniation == 12)
                {
                    accessedUserDesgniation = "F_Teacher";

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
                        var HeadQuartarManager = _factory.CreateHeadquarterssManager();
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
                        model.TimeAttendees = false;
                        model.Pay = false;
                        model.Map = false;
                        model.Calendar = false;
                        model.TimeTable = false;
                        model.TimeTableTeacher = false;
                        model.TimeTableStudent = false;

                    }

                }
                if (UserType == 1) //Can see limited things (Global user - Just on school from branches (branch 1 or 2 or ...) and edit, write, delete ...)
                { //No thing to do
                    model.Employees = false;
                    model.Teachers = false;
                    model.Students = false;
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

                }
                if (UserType == 2) // Can see limited things (Local user)
                {
                    model.Employees = false;
                    model.Teachers = false;
                    model.Students = false;
                    model.TimeAttendees = false;
                    model.Pay = false;
                    model.Map = false;
                    model.Calendar = true;
                    model.TimeTable = false;
                    model.TimeTableTeacher = false;
                    model.TimeTableStudent = false;
                    model.SystemManagment = false;

                }
            }
            else // login as a parent
            {
                model.Employees = false;
                model.Teachers = false;
                model.Students = false;
                model.TimeAttendees = false;
                model.Pay = false;
                model.Map = false;
                model.Calendar = false;
                model.TimeTable = false;
                model.TimeTableTeacher = false;
                model.TimeTableStudent = true;
                model.SystemManagment = false;

            }
            return View(model);
        }
    }
}