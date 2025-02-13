using Business.Base;
using Common;
using Common.Helpers;
using DataAccess;
using Objects;
using SmartSchool.Models.SchoolRegistration;
using SmartSchool.Models.TransportMAPs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class SchoolRegistrationController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        // GET: SchoolRegistration
        public ActionResult RegistrationStepOne()
        {
            string UserName = GetCookie((int)clsenumration.UserData.Username);
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));

            if (UserID == 1 && UserName == "Admin" && SchoolID == 1000000 && CompanyID == 1000000) //Admim for yazed
            {
                return RedirectToAction("ViewHeadQuarters", "Settings");
            }

            SchoolRegistrationModel model = new SchoolRegistrationModel();
            //int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var HeadQuarterManager = factory.CreateHeadquarterssManager();
            model.CompanyName = HeadQuarterManager.Find(a => a.CompanyID == CompanyID).Select(a => CurrentLanguage == Languges.English ? a.CompanyEnglishName : a.CompanyArabicName).FirstOrDefault();
            return View(model);
        }
        public ActionResult RegistrationStepTwo()
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            if (CompanyID == 1000000)
            {
                return RedirectToAction("ViewHeadQuarters", "Settings");
            }
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var schoolBranchs = SchoolBranchsManager.Find(a => a.CompanyID == CompanyID && a.IsNew == true).ToList();
            var IsRegisteredList = schoolBranchs.Select(a => a.IsRegistered).ToList();

            string query = "SELECT Photo FROM Headquarters WHERE CompanyID = @CompanyID";
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

            var base64 = "";
            var imgSrc = "../../AppContent/Images/avatar.jpeg";
            if (model.Photo != null)
            {
                base64 = Convert.ToBase64String(model.Photo);
                imgSrc = String.Format("data:image/*;base64,{0}", base64);
            }
            Session["UserImage"] = imgSrc;

            // In case you need to make all schools required.
            if (IsRegisteredList.Contains(false))
            {
                model.AllSchoolsRegistered = false;
            }
            else
            {
                model.AllSchoolsRegistered = true;
            }

            // In case you need to make one school at least required.
            if (IsRegisteredList.Contains(true))
            {
                model.OneSchoolsRegistered = true;
            }
            else
            {
                model.OneSchoolsRegistered = false;
            }
            model.SchoolBranchList = schoolBranchs;
            return View(model);
        }
        public ActionResult SchoolRegistrationWizard(int SchoolID)
        {

            SchoolRegistrationModel model = new SchoolRegistrationModel();
            var UserManager = factory.CreateUsersManager();
            PrepareSchoolRegistrationModel(model, SchoolID);
            var TransportCategoryTypesManager = factory.CreateTransportCategoryTypesManager();
            model.TransportCategoryTypes = (from TranCatTypes in context.TransportCategoryTypes select TranCatTypes).ToList();
            model.DiscountTypes = GetDiscountTypes();
            var user = UserManager.Find(a => a.SchoolID == SchoolID && a.UserType == 0 && a.StaffID == "10000").FirstOrDefault();
            if (user != null)
            {
                model.UserName = user.UserName;
                model.Password = Decrypt(user.Password);
                model.Confirm = Decrypt(user.Password);
            }

            string query = "SELECT Photo FROM SchoolBranches " +
                    "WHERE SchoolID = @SchoolID";
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
                            model.schoolBranch.Photo = imageData;
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult GetCurriculumStep(int SchoolID)
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var DepartmentsManager = factory.CreateDepartmentsManager();
            var CurriculumManager = factory.CreateCurriculumsManager();
            model.schoolBranch = SchoolBranchsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            model.AcademicDepartments = (from Department in context.Departments where Department.SchoolID == SchoolID && Department.IsAcademic == true select Department).ToList();
            var Curriculums = CurriculumManager.Find(a => a.SchoolID == SchoolID).ToList();
            model.Curriculum = Curriculums;
            return PartialView("_CurriculumStep", model);
        }

        public ActionResult GetDepartments(int SchoolID)
        {
            var DepartmentManager = factory.CreateDepartmentsManager();
            var Departments = DepartmentManager.Find(a => a.SchoolID == SchoolID).ToList();
            return Json(new { data = Departments }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SaveDepartment(int id)
        {
            var DepartmentManager = factory.CreateDepartmentsManager();
            var d = DepartmentManager.Find(a => a.DepartmentID == id).FirstOrDefault();

            return View(d);
        }
        [HttpPost]
        public ActionResult SaveDepartment(Department dep)
        {
            var DepartmentManager = factory.CreateDepartmentsManager();
            var d = DepartmentManager.Find(a => a.DepartmentID == dep.DepartmentID).FirstOrDefault();
            if (d != null)
            {
                d.DepartmentArabicName = dep.DepartmentArabicName;
                d.DepartmentEnglishName = dep.DepartmentEnglishName;
                DepartmentManager.Update(d);
            }
            else
            {
                Department obj = new Department();
                obj.DepartmentArabicName = dep.DepartmentArabicName;
                obj.DepartmentEnglishName = dep.DepartmentEnglishName;
                obj.SchoolID = dep.SchoolID;
                if (dep.IsAcademic == true)
                    obj.IsAcademic = true;
                else
                    obj.IsAcademic = false;
                DepartmentManager.Add(obj);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult DeleteDepartment(int id)
        {

            var DepartmentManager = factory.CreateDepartmentsManager();
            var d = DepartmentManager.Find(a => a.DepartmentID == id).FirstOrDefault();
            if (d != null)
            {
                return View(d);
            }
            else
            {
                return HttpNotFound();
            }

        }

        [HttpPost]
        [ActionName("DeleteDepartment")]
        public ActionResult DeleteDepartment(Department dep)
        {

            var DepartmentManager = factory.CreateDepartmentsManager();
            var d = DepartmentManager.Find(a => a.DepartmentID == dep.DepartmentID).FirstOrDefault();
            if (d != null)
            {
                DepartmentManager.Delete(d);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetClassesStep(int SchoolID)
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            PrepareSchoolRegistrationModel(model, SchoolID);
            return PartialView("_ClassesStep", model);
        }
        [HttpGet]
        public ActionResult GetPricesStep(int SchoolID)
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            var FeesManager = factory.CreateFeesManager();
            PrepareSchoolRegistrationModel(model, SchoolID);
            var FeesBook = FeesManager.Find(a => a.SchoolID == SchoolID && a.FeeType == (int)clsenumration.FeeType.Books).Select(a => a.FeeAmount.Value).ToList();
            var FeesUniform = FeesManager.Find(a => a.SchoolID == SchoolID && a.FeeType == (int)clsenumration.FeeType.Uniform).Select(a => a.FeeAmount.Value).ToList();
            if (FeesBook != null)
            {
                if (AreAllSame(FeesBook))
                    model.BookYes = true;
                else
                {
                    model.BookYes = false;
                }
            }
            if (FeesUniform != null)
            {
                if (AreAllSame(FeesUniform))
                    model.UniformYes = true;
                else
                {
                    model.UniformYes = false;
                }
            }
            model.BookFees = FeesBook;
            model.UniformFees = FeesUniform;

            return PartialView("_PricesStep", model);
        }
        [HttpGet]
        public ActionResult GetFeesStep(int SchoolID)
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            var FeesManager = factory.CreateFeesManager();
            PrepareSchoolRegistrationModel(model, SchoolID);
            var FeesSchool = FeesManager.Find(a => a.SchoolID == SchoolID && a.FeeType == (int)clsenumration.FeeType.RegistrationFee).Select(a => a.FeeAmount.Value).ToList();
            if (FeesSchool != null)
            {
                if (AreAllSame(FeesSchool))
                {
                    model.FeeYes = true;
                }
                else
                {
                    model.FeeYes = false;
                }
            }

            model.SchoolFees = FeesSchool;
            return PartialView("_FeesStep", model);
        }

        [HttpGet]
        public ActionResult GetDiscountsStep(int SchoolID)
        {
            SchoolRegistrationModel model = new SchoolRegistrationModel();
            var DiscountManager = factory.CreateDiscountsManager();
            var Discounts = DiscountManager.Find(a => a.SchoolID == SchoolID).ToList();
            model.Discounts = Discounts;
            PrepareSchoolRegistrationModel(model, SchoolID);
            return PartialView("_DiscountsStep", model);
        }
        public void PrepareSchoolRegistrationModel(SchoolRegistrationModel model, int SchoolID)
        {
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            var DepartmentsManager = factory.CreateDepartmentsManager();
            var SchoolClassManager = factory.CreateSchoolClasssManager();
            var CurriculumManager = factory.CreateCurriculumsManager();
            var schoolresult = SchoolBranchsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            var SchoolSettings = SchoolSettingsManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            model.schoolBranch = schoolresult;
            model.SchoolEmail = schoolresult.Email;
            model.SchoolSettings = SchoolSettings;
            model.Departments = (from Department in context.Departments where Department.SchoolID == SchoolID select Department).ToList();
            model.AcademicDepartments = (from Department in context.Departments where Department.SchoolID == SchoolID && Department.IsAcademic == true select Department).ToList();
            var Curriculums = CurriculumManager.Find(a => a.SchoolID == SchoolID).ToList();
            model.Curriculum = Curriculums;
            model.Classes = (from Class in context.Classes select Class).ToList();
            model.Countries = GetCountriesList();
        }

        [HttpGet]
        public JsonResult GetNoOfClassRooms(int SchoolID)
        {
            try
            {
                var SchoolSettigsManager = factory.CreateSchoolSettingsManager();
                var SchoolSettigs = SchoolSettigsManager.Find(s => s.SchoolID == SchoolID).FirstOrDefault();
                return Json(new { Success = true, NoOfClassRooms = SchoolSettigs.NumberofClassRooms }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult AddSchoolsToRegister(List<string> SchoolsArray)
        {
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var CurManager = factory.CreateCurriculumsManager();
            string[] AdminDepartments = new string[] { "الادارة", "الحركة", "المحاسبة", "شؤون الطلاب", "التسجيل" };
            string[] AdminDepartmentsEnglish = new string[] { "Administration", "Transportation", "Accounting", "Students Affairs", "Registration" };

            foreach (var item in SchoolsArray)
            {
                SchoolBranch obj = new SchoolBranch();
                obj.CompanyID = CompanyID;
                obj.SchoolArabicName = item;
                obj.SchoolEnglishName = item;
                obj.IsNew = true;
                obj.IsRegistered = false;
                SchoolBranchsManager.Add(obj);
                for (int i = 0; i < AdminDepartments.Length; i++)
                {
                    Department depobj = new Department();
                    depobj.SchoolID = obj.SchoolID;
                    depobj.DepartmentArabicName = AdminDepartments[i];
                    depobj.DepartmentEnglishName = AdminDepartmentsEnglish[i];
                    depobj.IsAcademic = false;

                    //Abu Hammad ------------------------------
                    //if (i == AdminDepartments.Length - 1)
                    //{
                    //    depobj.IsAcademic = true;
                    //}
                    //else depobj.IsAcademic = false;
                    //-----------------------------------------

                    DepartmentManager.Add(depobj);
                }

                Curriculum Curobj = new Curriculum();
                Curobj.CurriculumArabicName = "الوطني";
                Curobj.CurriculumEnglishName = "National";
                Curobj.SchoolID = obj.SchoolID;
                CurManager.Add(Curobj);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult AddAdditionalSchoolToRegister(string SchoolName)
        {
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var CurManager = factory.CreateCurriculumsManager();
            string[] AdminDepartments = new string[] { "الادارة", "الحركة", "المحاسبة", "شؤون الطلاب", "التسجيل" };
            string[] AdminDepartmentsEnglish = new string[] { "Administration", "Transportation", "Accounting", "Students Affairs", "Registration" };

            SchoolBranch obj = new SchoolBranch();
            obj.CompanyID = CompanyID;
            obj.SchoolArabicName = SchoolName;
            obj.SchoolEnglishName = SchoolName;
            obj.IsNew = true;
            obj.IsRegistered = false;
            SchoolBranchsManager.Add(obj);
            for (int i = 0; i < AdminDepartments.Length; i++)
            {
                Department depobj = new Department();
                depobj.SchoolID = obj.SchoolID;
                depobj.DepartmentArabicName = AdminDepartments[i];
                depobj.DepartmentEnglishName = AdminDepartmentsEnglish[i];
                depobj.IsAcademic = false;
                DepartmentManager.Add(depobj);
            }
            Curriculum Curobj = new Curriculum();
            Curobj.CurriculumArabicName = "الوطني";
            Curobj.CurriculumEnglishName = "National";
            Curobj.SchoolID = obj.SchoolID;
            CurManager.Add(Curobj);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult FinishSchoolsRegistration()
        {
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
            var schools = SchoolBranchsManager.Find(a => a.CompanyID == CompanyID).ToList();
            if (schools.Count > 0)
            {
                foreach (var item in schools)
                {
                    item.IsNew = false;
                    SchoolBranchsManager.Update(item);
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SchoolInfoStepOne(SchoolRegistrationModel model)
        {
            try
            {
                byte[] imgByte = null;
                string query = "";

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader != null && fileuploader.ContentLength > 0)
                    {
                        imgByte = new Byte[fileuploader.ContentLength];
                        //force the control to load data in array
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }
                }

                var SchoolBranchsManager = factory.CreateSchoolBranchsManager();
                var SchoolSettingManager = factory.CreateSchoolSettingsManager();
                var SystemSettingsperSchoolManager = factory.CreateSystemSettingsperSchoolsManager();
                var result = SchoolBranchsManager.Find(a => a.SchoolID == model.schoolBranch.SchoolID).FirstOrDefault();
                if (result != null)
                {
                    result.SchoolArabicName = model.schoolBranch.SchoolArabicName;
                    result.SchoolEnglishName = model.schoolBranch.SchoolArabicName;
                    result.SchoolContactNumber = model.schoolBranch.SchoolContactNumber;
                    result.Longitude = model.schoolBranch.Longitude;
                    result.Latitude = model.schoolBranch.Latitude;
                    result.Email = model.SchoolEmail;
                    result.City = "";
                    result.ClassroomArea = 0;
                    result.Country = model.schoolBranch.Country;
                    result.CurrentNumberofStudents = 0;
                    result.YardArea = 0;
                    SchoolBranchsManager.Update(result);

                    #region adding the Photo using ADO.NET.
                    byte[] Photo = null;
                    try
                    {
                        query = "SELECT Photo FROM SchoolBranches " +
                            "WHERE SchoolID = @SchoolID";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@SchoolID", model.schoolBranch.SchoolID);
                                SqlDataReader reader = comm.ExecuteReader();
                                if (reader.Read())
                                {
                                    byte[] imageData = null;
                                    if (!(reader[0] is DBNull))
                                    {
                                        // Get the image data from the reader
                                        imageData = (byte[])reader[0];
                                        Photo = imageData;
                                    }
                                }
                            }
                        }
                        if (imgByte != null || Photo != null)
                        {
                            query = "UPDATE SchoolBranches SET Photo = @Photo WHERE SchoolID = @SchoolID";
                            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                            {
                                conn.Open();
                                using (SqlCommand comm = new SqlCommand(query, conn))
                                {
                                    comm.Parameters.AddWithValue("@Photo", imgByte == null ? Photo : imgByte);
                                    comm.Parameters.AddWithValue("@SchoolID", model.schoolBranch.SchoolID);
                                    comm.ExecuteNonQuery();
                                }
                            } 
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }
                var settingresult = SchoolSettingManager.Find(a => a.SchoolID == model.schoolBranch.SchoolID).FirstOrDefault();
                if (settingresult != null)
                {
                    settingresult.NumberofClassRooms = model.SchoolSettings.NumberofClassRooms;
                    settingresult.NumberofChairsPerClass = model.SchoolSettings.NumberofChairsPerClass;
                    SchoolSettingManager.Update(settingresult);
                }
                else
                {
                    SchoolSetting obj = new SchoolSetting();
                    obj.NumberofClassRooms = model.SchoolSettings.NumberofClassRooms;
                    obj.NumberofChairsPerClass = model.SchoolSettings.NumberofChairsPerClass;
                    obj.SchoolID = model.schoolBranch.SchoolID;
                    obj.AllowedNumberofStudentsPerClassroomArea = 0;
                    obj.AllowedNumberofStudentsPerYardArea = 0;
                    obj.BreakBetweenSessionsDuration = 0;
                    obj.BreakDuration = 0;
                    obj.FirstClassStartingTime = DateTime.Now;
                    obj.MultiDependantsDiscount = 0;
                    obj.NumberofSemesters = 2;
                    obj.NumberofSessionsPerDay = 0;
                    obj.NumberofSessionsPerTeacher = 0;
                    obj.SchoolID = model.schoolBranch.SchoolID;
                    obj.SessionDuration = 0;
                    obj.WeekStartingDay = 1;
                    obj.StartingTime = DateTime.Now;
                    SchoolSettingManager.Add(obj);

                    var settingperschoolresult = SystemSettingsperSchoolManager.Find(a => a.SchoolID == model.schoolBranch.SchoolID).FirstOrDefault();
                    if (settingperschoolresult == null)
                    {
                        SystemSettingsperSchool obj1 = new SystemSettingsperSchool();
                        obj1.SchoolID = model.schoolBranch.SchoolID;
                        obj1.LastInvoiceNumber = 0;
                        obj1.OptionalArabicFields = 0;
                        obj1.OptionalEnglishFields = 0;
                        obj1.OptionalFamilyDetails = 0;
                        obj1.OptionalThirdName = 0;
                        obj1.BusScheduleDateType = 0;
                        obj1.BusScheduleType = 0;
                        obj1.NumberofBusTrips = 0;
                        obj1.TimetableType = 0;
                        obj1.SubjectDistributionMethod = 0;
                        obj1.SubjectforClassorSection = 0;
                        obj1.CurrencyOptions = 0;
                        obj1.BusAttendantAssigningMethod = 0;
                        SystemSettingsperSchoolManager.Add(obj1);
                    }
                }

                int semestersCount = -1;

                query = "SELECT COUNT(ID) FROM Semesters WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", model.schoolBranch.SchoolID);
                        comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                        semestersCount = (int)comm.ExecuteScalar();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                if (semestersCount == 0)
                {
                    List<string> semestersEngllishNames = new List<string>() { "First Semester", "Second Semester" };
                    List<string> semestersArabicNames = new List<string>() { "الفصل الدراسي الأول", "الفصل الدراسي الثاني" };

                    query = "";
                    for (int i = 0; i < semestersEngllishNames.Count; i++)
                    {
                        query += "INSERT INTO Semesters " +
                            "(SemesterArabicName, SemesterEnglishName, SchoolID) " +
                            "VALUES (N'" + semestersArabicNames[i] + "', N'" + semestersEngllishNames[i] + "', @SchoolID);";
                    }
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SchoolID", model.schoolBranch.SchoolID);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
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
        public JsonResult AddCurriculumDepartments(List<string> CheckedDepartments, List<string> UncheckedDepartments, int SchoolID)
        {
            var CurDepManager = factory.CreateCurriculumDepartmentsManager();
            string NoCheckAcdError = "";
            if (UncheckedDepartments != null)
            {
                foreach (var item in UncheckedDepartments)
                {
                    var CurDep = item.Split('$');
                    int CurriculumID = Convert.ToInt32(CurDep[0]);
                    int DepartmentID = Convert.ToInt32(CurDep[1]);
                    var result = CurDepManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.DepartmentID == DepartmentID).FirstOrDefault();
                    if (result != null)
                    {
                        CurDepManager.Delete(result);
                    }
                }
            }
            if (CheckedDepartments != null)
            {
                foreach (var item in CheckedDepartments)
                {
                    var CurDep = item.Split('$');
                    int CurriculumID = Convert.ToInt32(CurDep[0]);
                    int DepartmentID = Convert.ToInt32(CurDep[1]);
                    var result = CurDepManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.DepartmentID == DepartmentID).FirstOrDefault();
                    if (result == null)
                    {
                        CurriculumDepartment obj = new CurriculumDepartment();
                        obj.CurriculumID = CurriculumID;
                        obj.DepartmentID = DepartmentID;
                        obj.SchoolID = SchoolID;
                        CurDepManager.Add(obj);
                    }
                }
            }
            else
            {
                NoCheckAcdError = "الرجاء أختيار الأقسام الأكاديمية لكل منهاج";
            }
            return Json(new { Success = true, NoCheckAcdError = NoCheckAcdError }, JsonRequestBehavior.AllowGet);

        }

        //Hamza001
        [HttpPost]
        public JsonResult AddSchoolClasses(List<string> SchoolClasses, List<string> UnCheckedClasses, List<string> Mix, List<string> UnCheckMix, List<string> Male, List<string> UnCheckMale, List<string> Female, List<string> UnCheckFemale, int SchoolID)
        {
            try
            {
                var SchoolClassesManager = factory.CreateSchoolClasssManager();
                var ClassManager = factory.CreateClasssManager();
                var CurriculumsManager = factory.CreateCurriculumsManager();
                var SectionsManager = factory.CreateSectionsManager();
                var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
                var SchoolSettings = SchoolSettingsManager.Find(s => s.SchoolID == SchoolID).FirstOrDefault();
                List<int> ClassesIDs = new List<int>();
                List<int> CurriculumIDs = new List<int>();
                int Malecount = 0;
                int Femalecount = 0;
                int MixCount = 0;
                string NoClassError = "";
                string DepError = "";
                string CurError = "";
                if (UnCheckedClasses != null)
                {
                    foreach (var item in UnCheckedClasses)
                    {
                        var Class_Curriculum = item.Split('$');
                        int ClassID = Convert.ToInt32(Class_Curriculum[0]);
                        int CurriculumID = Convert.ToInt32(Class_Curriculum[1]);
                        var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                        if (result != null)
                        {
                            SchoolClassesManager.Delete(result);
                        }
                    }
                }
                if (SchoolClasses != null)
                {

                    foreach (var item in SchoolClasses)
                    {
                        var Class_Curriculum = item.Split('$');
                        int ClassID = Convert.ToInt32(Class_Curriculum[0]);
                        int CurriculumID = Convert.ToInt32(Class_Curriculum[1]);
                        var classresult = ClassManager.Find(a => a.ClassID == ClassID).FirstOrDefault();
                        var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                        var Curriculum = CurriculumsManager.Find(c => c.CurriculumID == CurriculumID).FirstOrDefault();
                        if (result == null)
                        {
                            SchoolClass obj = new SchoolClass();
                            obj.ClassID = ClassID;
                            obj.CurriculumID = CurriculumID;
                            obj.SchoolID = SchoolID;
                            obj.SchoolClassArabicName = classresult.ClassArabicName + " " + Curriculum.CurriculumArabicName;
                            obj.SchoolClassEnglishName = classresult.ClassEnglishName + " " + Curriculum.CurriculumEnglishName;
                            SchoolClassesManager.Add(obj);
                            ClassesIDs.Add(ClassID);

                        }
                        else
                        {
                            var sectionCount =
                                SectionsManager.Find(s => s.SchoolClassID == result.SchoolClassID).Count();
                            int schoolClassID = result.SchoolClassID;
                            bool isMix = result.Coeducation;
                            bool isMale = result.Male;
                            bool isFemale = result.Female;
                            HandlingDefualtSections(SchoolID, schoolClassID, sectionCount, isMix, isMale, isFemale);
                        }
                        CurriculumIDs.Add(CurriculumID);
                    }
                    //var SchoolCurIDs = (from c in context.Curriculums where c.SchoolID == SchoolID  group c by c.CurriculumArabicName ).ToList();
                    //var CurIDs = CurriculumIDs.Distinct().ToList();
                    //var Cur = (from c in context.Curriculums where CurIDs.Contains(c.CurriculumID) group c by c.CurriculumArabicName).ToList();
                    //if(SchoolCurIDs.Count() != Cur.Count())
                    //{
                    //    CurError = "CurriculumError";
                    //    return Json(new { Success = true, NoClassError = NoClassError, DepError = DepError, CurError = CurError }, JsonRequestBehavior.AllowGet);

                    //}
                }
                else
                {
                    NoClassError = "NoClasses";
                    return Json(new { Success = true, NoClassError = NoClassError, DepError = DepError, CurError = CurError }, JsonRequestBehavior.AllowGet);

                }
                if (Mix != null)
                {
                    if (Mix.Count > 0)
                    {
                        MixCount = Mix.Count();

                        foreach (var item in Mix)
                        {
                            var SchoolClassMix = item.Replace("Mix_", "").Split('$');
                            int ClassID = Convert.ToInt32(SchoolClassMix[0]);
                            int CurriculumID = Convert.ToInt32(SchoolClassMix[1]);
                            var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                            if (result != null)
                            {
                                result.Coeducation = true;
                                SchoolClassesManager.Update(result);

                                var sectionCount =
                                SectionsManager.Find(s => s.SchoolClassID == result.SchoolClassID).Count();
                                HandlingDefualtSections(SchoolID, result.SchoolClassID, sectionCount, true, false, false);
                            }
                        }
                    }
                }
                foreach (var item in UnCheckMix)
                {
                    var SchoolClassMix = item.Replace("Mix_", "").Split('$');
                    int ClassID = Convert.ToInt32(SchoolClassMix[0]);
                    int CurriculumID = Convert.ToInt32(SchoolClassMix[1]);
                    var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                    if (result != null)
                    {
                        result.Coeducation = false;
                        SchoolClassesManager.Update(result);
                    }
                }
                if (Male != null)
                {
                    if (Male.Count > 0)
                    {
                        Malecount = Male.Count();
                        foreach (var item in Male)
                        {
                            var SchoolClassMale = item.Replace("Male_", "").Split('$');
                            int ClassID = Convert.ToInt32(SchoolClassMale[0]);
                            int CurriculumID = Convert.ToInt32(SchoolClassMale[1]);
                            var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                            if (result != null)
                            {
                                result.Male = true;
                                SchoolClassesManager.Update(result);

                                var sectionCount =
                                SectionsManager.Find(s => s.SchoolClassID == result.SchoolClassID).Count();
                                HandlingDefualtSections(SchoolID, result.SchoolClassID, sectionCount, false, true, result.Female);
                            }
                        }
                    }
                }
                foreach (var item in UnCheckMale)
                {
                    var SchoolClassMale = item.Replace("Male_", "").Split('$');
                    int ClassID = Convert.ToInt32(SchoolClassMale[0]);
                    int CurriculumID = Convert.ToInt32(SchoolClassMale[1]);
                    var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                    if (result != null)
                    {
                        result.Male = false;
                        SchoolClassesManager.Update(result);
                    }
                }
                if (Female != null)
                {
                    if (Female.Count > 0)
                    {
                        Femalecount = Female.Count();
                        foreach (var item in Female)
                        {
                            var SchoolClassFemale = item.Replace("Female_", "").Split('$');
                            int ClassID = Convert.ToInt32(SchoolClassFemale[0]);
                            int CurriculumID = Convert.ToInt32(SchoolClassFemale[1]);
                            var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                            if (result != null)
                            {
                                result.Female = true;
                                SchoolClassesManager.Update(result);

                                var sectionCount =
                                SectionsManager.Find(s => s.SchoolClassID == result.SchoolClassID).Count();
                                HandlingDefualtSections(SchoolID, result.SchoolClassID, sectionCount, false, result.Male, true);
                            }
                        }
                    }
                }
                foreach (var item in UnCheckFemale)
                {
                    var SchoolClassFemale = item.Replace("Female_", "").Split('$');
                    int ClassID = Convert.ToInt32(SchoolClassFemale[0]);
                    int CurriculumID = Convert.ToInt32(SchoolClassFemale[1]);
                    var result = SchoolClassesManager.Find(a => a.ClassID == ClassID && a.CurriculumID == CurriculumID && a.SchoolID == SchoolID).FirstOrDefault();
                    if (result != null)
                    {
                        result.Female = false;
                        SchoolClassesManager.Update(result);
                    }
                }
                int DepCount = Malecount + Femalecount + MixCount;
                if (DepCount < SchoolClasses.Count())
                {
                    DepError = "DepError";

                }

                return Json(new { Success = true, NoClassError = NoClassError, DepError = DepError, CurError = CurError }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Hamza01
        [HttpPost]
        public JsonResult AddUpdateBookUniformPrices(List<string> BooksPrice, string UniformsPrice, List<string> BookPriceList, List<string> UniformPriceList, int RadioBook, int RadioUniform, int SchoolID)
        {
            var FeesManager = factory.CreateFeesManager();
            var ClassManager = factory.CreateClasssManager();
            var SchoolClassManager = factory.CreateSchoolClasssManager();
            if (RadioBook == 0) //No
            {
                if (BookPriceList.Count > 0)
                {
                    foreach (var item in BookPriceList)
                    {
                        var BooksPricearray = item.Split(',');
                        var ClassAndCurriculum = BooksPricearray[0].Replace("Book_", "").Split('$');
                        int ClassID = Convert.ToInt32(ClassAndCurriculum[0]);
                        int CurriculumID = Convert.ToInt32(ClassAndCurriculum[1]);
                        double price = Convert.ToDouble(BooksPricearray[1]);
                        var Class = ClassManager.Find(a => a.ClassID == ClassID).FirstOrDefault();
                        var FeeResult = FeesManager.Find(a => a.ClassID == ClassID && a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.FeeType == (int)clsenumration.FeeType.Books).FirstOrDefault();
                        if (FeeResult != null)
                        {
                            FeeResult.FeeAmount = price;
                            FeesManager.Update(FeeResult);
                        }
                        else
                        {
                            Fee obj = new Fee();
                            obj.FeeArabicName = "رسوم الكتب" + " " + Class.ClassArabicName;
                            obj.FeeEnglishName = "Books Fees for" + " " + Class.ClassEnglishName;
                            obj.FeeAmount = price;
                            obj.ClassID = ClassID;
                            obj.SchoolID = SchoolID;
                            obj.CurriculumID = CurriculumID;
                            obj.FeeType = (int)clsenumration.FeeType.Books;
                            FeesManager.Add(obj);
                        }
                    }
                }
            }
            else if (RadioBook == 1) //yes
            {
                int CurriculumID = 0;
                double FeeAmount = 0;
                foreach (var item in BooksPrice)
                {
                    var booksarray = item.Replace("booksPrice_", "").Split(',');
                    CurriculumID = Convert.ToInt32(booksarray[0]);
                    FeeAmount = Convert.ToInt32(booksarray[1]);
                    var schoolClasses = SchoolClassManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID).ToList();
                    var FeeResult = FeesManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.FeeType == (int)clsenumration.FeeType.Books).ToList();
                    if (FeeResult.Count > 0)
                    {
                        foreach (var f in FeeResult)
                        {
                            FeesManager.Delete(f);
                        }
                    }
                    if (schoolClasses != null)
                    {
                        foreach (var x in schoolClasses)
                        {
                            var classs = ClassManager.Find(a => a.ClassID == x.ClassID).FirstOrDefault();
                            Fee obj = new Fee();
                            obj.FeeArabicName = "رسوم الكتب" + " " + classs.ClassArabicName;
                            obj.FeeEnglishName = "Books Fees for" + " " + classs.ClassEnglishName;
                            obj.FeeAmount = FeeAmount;
                            obj.ClassID = x.ClassID;
                            obj.SchoolID = SchoolID;
                            obj.FeeType = (int)clsenumration.FeeType.Books;
                            obj.CurriculumID = CurriculumID;
                            FeesManager.Add(obj);
                        }

                    }
                }
            }
            if (RadioUniform == 0) //No
            {
                if (UniformPriceList.Count > 0)
                {
                    foreach (var item in UniformPriceList)
                    {
                        var UniformPricearray = item.Split(',');
                        int ClassID = Convert.ToInt32(UniformPricearray[0].Replace("Uniform_", ""));
                        double price = Convert.ToDouble(UniformPricearray[1]);
                        var Class = ClassManager.Find(a => a.ClassID == ClassID).FirstOrDefault();
                        var FeeResults = FeesManager.Find(a => a.ClassID == ClassID && a.SchoolID == SchoolID && a.FeeType == (int)clsenumration.FeeType.Uniform).ToList();
                        var Curriculums = factory.CreateCurriculumsManager().Find(c => c.SchoolID == SchoolID).ToList();
                    EqualLists:
                        if (FeeResults.Count == Curriculums.Count)
                        {
                            for (int i = 0; i < FeeResults.Count; i++)
                            {
                                if (FeeResults[i] != null)
                                {
                                    FeeResults[i].FeeAmount = price;
                                    FeeResults[i].CurriculumID = Curriculums[i].CurriculumID;
                                    FeesManager.Update(FeeResults[i]);
                                }
                                else
                                {
                                    Fee obj = new Fee();
                                    obj.FeeArabicName = "رسوم الزي المدرسي" + " " + Class.ClassArabicName;
                                    obj.FeeEnglishName = "School Costume Fees for" + " " + Class.ClassEnglishName;
                                    obj.FeeAmount = price;
                                    obj.ClassID = ClassID;
                                    obj.SchoolID = SchoolID;
                                    obj.FeeType = (int)clsenumration.FeeType.Uniform;
                                    FeeResults[i].CurriculumID = Curriculums[i].CurriculumID;
                                    FeesManager.Add(obj);
                                }
                            }
                        }
                        else
                        {
                            int numOfElementstoDelete = Curriculums.Count - FeeResults.Count;
                            for (int i = 0; i < numOfElementstoDelete; i++)
                                Curriculums.RemoveAt(Curriculums.Count - 1);
                            goto EqualLists;
                        }
                    }
                }
            }
            else if (RadioUniform == 1) //yes
            {
                var FeeResult = FeesManager.Find(a => a.SchoolID == SchoolID && a.FeeType == (int)clsenumration.FeeType.Uniform).ToList();
                if (FeeResult.Count > 0)
                {
                    foreach (var item in FeeResult)
                    {
                        FeesManager.Delete(item);
                    }
                }
                var schoolClasses = SchoolClassManager.Find(a => a.SchoolID == SchoolID).ToList();
                if (schoolClasses != null)
                {
                    foreach (var item in schoolClasses)
                    {
                        var classs = ClassManager.Find(a => a.ClassID == item.ClassID).FirstOrDefault();
                        Fee obj = new Fee();
                        obj.FeeArabicName = "رسوم الزي المدرسي" + " " + classs.ClassArabicName;
                        obj.FeeEnglishName = "School Costume Fees for" + " " + classs.ClassEnglishName;
                        obj.FeeAmount = Convert.ToDouble(UniformsPrice);
                        obj.ClassID = item.ClassID;
                        obj.SchoolID = SchoolID;
                        obj.FeeType = (int)clsenumration.FeeType.Uniform;
                        obj.CurriculumID = Convert.ToInt32(item.CurriculumID);
                        FeesManager.Add(obj);
                    }

                }
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult AddUpdateSchoolFees(List<string> FeesPrice, List<string> FeePriceList, int RadioFee, int SchoolID)
        {
            var FeesManager = factory.CreateFeesManager();
            var ClassManager = factory.CreateClasssManager();
            var SchoolClassManager = factory.CreateSchoolClasssManager();
            if (RadioFee == 0) //No
            {
                if (FeePriceList.Count > 0)
                {
                    foreach (var item in FeePriceList)
                    {
                        var FeesPricearray = item.Split(',');
                        var ClassAndCurriculum = FeesPricearray[0].Replace("Fee_", "").Split('$');
                        int ClassID = Convert.ToInt32(ClassAndCurriculum[0]);
                        int CurriculumID = Convert.ToInt32(ClassAndCurriculum[1]);
                        double price = Convert.ToDouble(FeesPricearray[1]);
                        var Class = ClassManager.Find(a => a.ClassID == ClassID).FirstOrDefault();
                        var FeeResult = FeesManager.Find(a => a.ClassID == ClassID && a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.FeeType == (int)clsenumration.FeeType.RegistrationFee).FirstOrDefault();
                        if (FeeResult != null)
                        {
                            FeeResult.FeeAmount = price;
                            FeesManager.Update(FeeResult);
                        }
                        else
                        {
                            Fee obj = new Fee();
                            obj.FeeArabicName = "الرسوم المدرسية" + " " + Class.ClassArabicName;
                            obj.FeeEnglishName = "School Fees for" + " " + Class.ClassEnglishName;
                            obj.FeeAmount = price;
                            obj.ClassID = ClassID;
                            obj.SchoolID = SchoolID;
                            obj.CurriculumID = CurriculumID;
                            obj.FeeType = (int)clsenumration.FeeType.RegistrationFee;
                            FeesManager.Add(obj);
                        }
                    }
                }
            }
            else if (RadioFee == 1) //yes
            {
                int CurriculumID = 0;
                double FeeAmount = 0;
                foreach (var item in FeesPrice)
                {
                    var feesarray = item.Replace("feesPrice_", "").Split(',');
                    CurriculumID = Convert.ToInt32(feesarray[0]);
                    FeeAmount = Convert.ToInt32(feesarray[1]);
                    var schoolClasses = SchoolClassManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID).ToList();

                    var FeeResult = FeesManager.Find(a => a.SchoolID == SchoolID && a.CurriculumID == CurriculumID && a.FeeType == (int)clsenumration.FeeType.RegistrationFee).ToList();
                    if (FeeResult.Count > 0)
                    {
                        foreach (var f in FeeResult)
                        {
                            FeesManager.Delete(f);
                        }
                    }
                    if (schoolClasses != null)
                    {
                        foreach (var x in schoolClasses)
                        {
                            var Class = ClassManager.Find(a => a.ClassID == x.ClassID).FirstOrDefault();
                            Fee obj = new Fee();
                            obj.FeeArabicName = "الرسوم المدرسية" + " " + Class.ClassArabicName;
                            obj.FeeEnglishName = "School Fees for" + " " + Class.ClassEnglishName;
                            obj.FeeAmount = FeeAmount;
                            obj.ClassID = x.ClassID;
                            obj.SchoolID = SchoolID;
                            obj.FeeType = (int)clsenumration.FeeType.RegistrationFee;
                            obj.CurriculumID = CurriculumID;
                            FeesManager.Add(obj);
                        }

                    }
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult AddUpdateTransportCategories(TransportMAPs model)
        {
            var TransportCategoriesManager = factory.CreateTransportCategorysManager();
            var TransportCategoryTypes = (from TranCatTypes in context.TransportCategoryTypes select TranCatTypes).ToList();
            var schoolManager = factory.CreateSchoolBranchsManager();
            var schoolresult = schoolManager.Find(a => a.SchoolID == model.SchoolID).FirstOrDefault();
            double Distanceinmeters = 0;
            double GoCost = 0;
            double ReturnCost = 0;
            double TwoWayCost = 0;
            foreach (var item in TransportCategoryTypes)
            {
                var TransportRes = (from TransportCategory in context.TransportCategories where TransportCategory.SchoolID == model.SchoolID && TransportCategory.TransportCategoryTypeID == item.TransportCategoryTypeID && TransportCategory.TransportTypeID == 1 select TransportCategory).FirstOrDefault();

                if (item.TransportCategoryTypeID == 1)
                {
                    Distanceinmeters = model.Distanceinmeters1;
                    GoCost = model.GoCost_1;
                    ReturnCost = model.ReturnCost_1;
                    TwoWayCost = model.TwoWay_1;
                }
                else if (item.TransportCategoryTypeID == 2)
                {
                    Distanceinmeters = model.Distanceinmeters2;
                    GoCost = model.GoCost_2;
                    ReturnCost = model.ReturnCost_2;
                    TwoWayCost = model.TwoWay_2;
                }
                else if (item.TransportCategoryTypeID == 3)
                {
                    Distanceinmeters = model.Distanceinmeters2;
                    GoCost = model.GoCost_3;
                    ReturnCost = model.ReturnCost_3;
                    TwoWayCost = model.TwoWay_3;
                }
                else if (item.TransportCategoryTypeID == 4)
                {
                    Distanceinmeters = model.Distanceinmeters4;
                    GoCost = model.GoCost_4;
                    ReturnCost = model.ReturnCost_4;
                    TwoWayCost = model.TwoWay_4;
                }
                else if (item.TransportCategoryTypeID == 5)
                {
                    Distanceinmeters = model.Distanceinmeters5;
                    GoCost = model.GoCost_5;
                    ReturnCost = model.ReturnCost_5;
                    TwoWayCost = model.TwoWay_5;
                }
                if (TransportRes != null)
                {
                    TransportRes.DistanceInMeters = Distanceinmeters;
                    TransportRes.TransportCategoryCostGo = GoCost;
                    TransportRes.TransportCategoryCostReturn = ReturnCost;
                    TransportRes.TransportCategoryCostTwoWay = TwoWayCost;
                    TransportCategoriesManager.Update(TransportRes);
                }
                else
                {
                    TransportCategory obj = new TransportCategory();
                    obj.DistanceInMeters = Distanceinmeters;
                    obj.TransportCategoryCostGo = GoCost;
                    obj.TransportCategoryCostReturn = ReturnCost;
                    obj.TransportCategoryCostTwoWay = TwoWayCost;
                    obj.SchoolID = model.SchoolID;
                    obj.TransportCategoryTypeID = item.TransportCategoryTypeID;
                    obj.TransportTypeID = 1;
                    TransportCategoriesManager.Add(obj);
                }
            }
            schoolresult.IsRegistered = true;
            schoolManager.Update(schoolresult);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCurriculums(int SchoolID)
        {
            var CurriculumManager = factory.CreateCurriculumsManager();
            var Curriculums = CurriculumManager.Find(a => a.SchoolID == SchoolID).ToList();
            return Json(new { data = Curriculums }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult SaveCurriculum(int id)
        {
            var CurriculumManager = factory.CreateCurriculumsManager();
            var d = CurriculumManager.Find(a => a.CurriculumID == id).FirstOrDefault();

            return View(d);
        }
        [HttpPost]
        public ActionResult SaveCurriculum(Curriculum cur)
        {
            var CurriculumManager = factory.CreateCurriculumsManager();
            var d = CurriculumManager.Find(a => a.CurriculumID == cur.CurriculumID).FirstOrDefault();
            if (d != null)
            {
                d.CurriculumArabicName = cur.CurriculumArabicName;
                d.CurriculumEnglishName = cur.CurriculumEnglishName;
                CurriculumManager.Update(d);
            }
            else
            {
                Curriculum obj = new Curriculum();
                obj.CurriculumArabicName = cur.CurriculumArabicName;
                obj.CurriculumEnglishName = cur.CurriculumEnglishName;
                obj.SchoolID = cur.SchoolID;

                CurriculumManager.Add(obj);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteCurriculum(int id)
        {

            var CurriculumManager = factory.CreateCurriculumsManager();
            var d = CurriculumManager.Find(a => a.CurriculumID == id).FirstOrDefault();
            if (d != null)
            {
                return View(d);
            }
            else
            {
                return HttpNotFound();
            }

        }
        [HttpPost]
        [ActionName("DeleteCurriculum")]
        public ActionResult DeleteCurriculum(Curriculum cur)
        {

            var CurriculumManager = factory.CreateCurriculumsManager();
            var d = CurriculumManager.Find(a => a.CurriculumID == cur.CurriculumID).FirstOrDefault();
            if (d != null)
            {
                CurriculumManager.Delete(d);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddDiscount(string DiscountDecription, string DiscountQuestion, double DiscountPercentege, int DiscountTypeID, int SchoolID)
        {
            var DiscountManager = factory.CreateDiscountsManager();
            Discount obj = new Discount();
            obj.DiscountDescription = DiscountDecription;
            obj.DiscountQuestion = DiscountQuestion;
            obj.DiscountPercentage = DiscountPercentege;
            obj.DiscountTypeID = DiscountTypeID;
            obj.SchoolID = SchoolID;
            DiscountManager.Add(obj);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeleteDiscount(int DiscountID)
        {
            var DiscountManager = factory.CreateDiscountsManager();
            var result = DiscountManager.Find(a => a.DiscountID == DiscountID).FirstOrDefault();
            DiscountManager.Delete(result);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult UpdateDiscount(int DiscountID, string DiscountDecription, string DiscountQuestion, double DiscountPercentege, int DiscountTypeID)
        {
            var DiscountManager = factory.CreateDiscountsManager();
            var result = DiscountManager.Find(a => a.DiscountID == DiscountID).FirstOrDefault();
            result.DiscountDescription = DiscountDecription;
            result.DiscountQuestion = DiscountQuestion;
            result.DiscountPercentage = DiscountPercentege;
            result.DiscountTypeID = DiscountTypeID;
            DiscountManager.Update(result);
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult GetDiscountByID(int DiscountID)
        {
            var DiscountManager = factory.CreateDiscountsManager();
            var DiscountTypesManager = factory.CreateDiscountTypesManager();
            var result = DiscountManager.Find(a => a.DiscountID == DiscountID).FirstOrDefault();
            var DiscountTypes = DiscountTypesManager.GetAll();
            return Json(new { Success = true, DiscountDecription = result.DiscountDescription, DiscountQuestion = result.DiscountQuestion, DiscountPercentege = result.DiscountPercentage, DiscountTypeID = result.DiscountTypeID, DiscountTypes = DiscountTypes }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult LoginInfoStep(string UserName, string Password, int SchoolID)
        {
            string yy = UserName;
            string xx = Password;
            int zz = SchoolID;

            var UsersManager = factory.CreateUsersManager();
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));

            string EncryPassword = Encrypt(Password);
            var user = UsersManager.Find(a => a.UserName == UserName && a.Password == EncryPassword && a.SchoolID == SchoolID && a.CompanyID == CompanyID).SingleOrDefault();
            if (user != null)
            {
                user.UserName = UserName;

                user.Password = EncryPassword;

                UsersManager.Update(user);
            }
            else
            {
                User User = new User();
                User.StaffID = "10000";
                User.UserName = UserName;
                User.UserType = 0;
                User.Password = EncryPassword;
                User.SchoolID = SchoolID;
                User.CompanyID = CompanyID;
                UsersManager.Add(User);
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        private void AddDefaultSection(int SchoolID, int SchoolClassID,
            string ArabicCode, string EnglishCode)
        {
            var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
            string query = "INSERT INTO Sections " +
            "(SchoolID, SchoolClassID, ClassroomID, SectionCode, MaxNumberofStudents, " +
            "NumberofStudents, IsFull, SectionArabicName, SectionEnglishName) " +
            "VALUES(@SchoolID, @SchoolClassID, @ClassroomID, @SectionCode, " +
            "@MaxNumberofStudents, @NumberofStudents, @IsFull, " +
            "@SectionArabicName, @SectionEnglishName)";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolClassID", SchoolClassID);
                    comm.Parameters.AddWithValue("@ClassroomID", 0);
                    comm.Parameters.AddWithValue("@SectionCode", ArabicCode);
                    comm.Parameters.AddWithValue("@MaxNumberofStudents", SchoolSettingsManager
                                .Find(s => s.SchoolID == SchoolID).FirstOrDefault().NumberofChairsPerClass);
                    comm.Parameters.AddWithValue("@NumberofStudents", 0);
                    comm.Parameters.AddWithValue("@IsFull", 0);
                    comm.Parameters.AddWithValue("@SectionArabicName", "شعبة " + ArabicCode);
                    comm.Parameters.AddWithValue("@SectionEnglishName", "Section " + EnglishCode);
                    comm.ExecuteNonQuery();
                }
                conn.Close();
                conn.Dispose();
            }
        }

        private void HandlingDefualtSections(int schoolID, int schoolClassID, int sectionCount, bool isMix, bool isMale, bool isFemale)
        {
            if (sectionCount == 0)
            {
                if (isMix || (isMale && isFemale))
                {
                    AddDefaultSection(schoolID, schoolClassID, "أ", "A");
                    if (isMale && isFemale)
                    {
                        AddDefaultSection(schoolID, schoolClassID, "ب", "B");
                    }
                }
                else if (isMale || isFemale)
                {
                    AddDefaultSection(schoolID, schoolClassID, "أ", "A");
                }
            }
            else if (isMale && isFemale && sectionCount == 1)
            {
                AddDefaultSection(schoolID, schoolClassID, "ب", "B");
            }
        }
    }
}