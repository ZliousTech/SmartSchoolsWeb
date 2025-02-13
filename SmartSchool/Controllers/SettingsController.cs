using Business.Base;
using Common;
using Common.Helpers;
using Objects;
using SmartSchool.Models.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Windows.Forms;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        // Dictionary for أبجد هوز.
        private Dictionary<int, string> ABJADHOS = new Dictionary<int, string>()
        {
            { 1, "أ" },
            { 2, "ب" },
            { 3, "ج" },
            { 4, "د" },
            { 5, "ه" },
            { 6, "و" },
            { 7, "ز" },
            { 8, "ح" },
            { 9, "ط" },
            { 10, "ي" },
            { 11, "ك" },
            { 12, "ل" },
            { 13, "م" },
            { 14, "ن" },
            { 15, "س" },
            { 16, "ع" },
            { 17, "ف" },
            { 18, "ص" },
            { 19, "ق" },
            { 20, "ر" },
            { 21, "ش" },
            { 22, "ت" },
            { 23, "ث" },
            { 24, "خ" },
            { 25, "ذ" },
            { 26, "ض" },
            { 27, "ظ" },
            { 28, "غ" },
        };

        // GET: Settings
        public ActionResult Index()
        {
            Session["CompanyID"] = CompanyID;
            return View();
        }

        #region HeadQuarters && SchoolBranches
        public ActionResult HeadQuarters()
        {
            return View();
        }

        public ActionResult GetAllHeadQuarters()
        {
            var HeadQuarterManager = factory.CreateHeadquarterssManager();
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            var AllHeadQuarters = HeadQuarterManager.GetHeadQuarters(lang);

            return View(AllHeadQuarters);
        }

        public ActionResult RegisterHeadQuarter()
        {
            string UserName = GetCookie((int)clsenumration.UserData.Username);
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));

            HeadQuarterRegisterModel model = new HeadQuarterRegisterModel();

            if (UserID == 1 && UserName == "Admin" && SchoolID == 1000000 && CompanyID == 1000000) //Admim for yazed
            {
                var CountriesManager = factory.CreateCountrysManager();
                var Countries = CountriesManager.GetAll();
                var CountriesList = (from country in Countries
                                     select new LookupDTO
                                     {
                                         Description = country.EnglishName.ToString(),
                                         DescriptionAR = country.ArabicName.ToString(),
                                         ID = (country.ID)
                                     }).ToList();
                model.CountryList = CountriesList;
            }
            else
            {
                return RedirectToAction("ViewHeadQuarters");
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult RegisterHeadQuarter(HeadQuarterRegisterModel model)
        {
            try
            {
                string query = string.Empty;
                byte[] imgByte = null;
                //var HeadquarterssManager = factory.CreateHeadquarterssManager();
                //Headquarters HeadQuarter = new Headquarters();

                model.ProductKey = "";
                //HeadQuarter.CompanyArabicName = model.CompanyArabicName;
                //HeadQuarter.CompanyEnglishName = model.CompanyEnglishName;
                //HeadQuarter.ContactNo = model.ContactNo;
                //HeadQuarter.Country = model.Country;
                //HeadQuarter.City = model.City == null ? "" : model.City;
                //HeadQuarter.Street = model.Street == null ? "" : model.Street;
                //HeadQuarter.Longitude = model.Longitude;
                //HeadQuarter.Latitude = model.Latitude;
                //HeadQuarter.Email = model.Email == null ? "" : model.Email;
                //HeadQuarter.ProductKey = model.ProductKey;
                //HeadquarterssManager.Add(HeadQuarter);

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

                try
                {
                    query = "INSERT INTO Headquarters " +
                        "(CompanyArabicName, CompanyEnglishName, Country, City, Street, ContactNo, Email, Longitude, Latitude, ProductKey, Photo) " +
                        "VALUES(@CompanyArabicName, @CompanyEnglishName, @Country, @City, @Street, @ContactNo, @Email, @Longitude, @Latitude, @ProductKey, @Photo)";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@CompanyArabicName", model.CompanyArabicName);
                            comm.Parameters.AddWithValue("@CompanyEnglishName", model.CompanyEnglishName);
                            comm.Parameters.AddWithValue("@Country", model.Country);
                            comm.Parameters.AddWithValue("@City", model.City == null ? "" : model.City);
                            comm.Parameters.AddWithValue("@Street", model.Street == null ? "" : model.Street);
                            comm.Parameters.AddWithValue("@ContactNo", model.ContactNo);
                            comm.Parameters.AddWithValue("@Email", model.Email == null ? "" : model.Email);
                            comm.Parameters.AddWithValue("@Longitude", model.Longitude);
                            comm.Parameters.AddWithValue("@Latitude", model.Latitude);
                            comm.Parameters.AddWithValue("@ProductKey", model.ProductKey);
                            comm.Parameters.AddWithValue("@Photo", imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "camera2.png")) : imgByte);
                            comm.ExecuteNonQuery();
                        }
                    }

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditHeadQuarter(int CompanyID)
        {
            HeadQuarterRegisterModel model = new HeadQuarterRegisterModel();
            var CountriesManager = factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishName.ToString(),
                                     DescriptionAR = country.ArabicName.ToString(),
                                     ID = (country.ID)
                                 }).ToList();
            model.CountryList = CountriesList;

            var HeadquarterManager = factory.CreateHeadquarterssManager();
            var HeadquarterResult = HeadquarterManager.Find(a => a.CompanyID == CompanyID).FirstOrDefault();

            model.CompanyID = HeadquarterResult.CompanyID;
            model.CompanyArabicName = HeadquarterResult.CompanyArabicName;
            model.CompanyEnglishName = HeadquarterResult.CompanyEnglishName;
            model.ContactNo = HeadquarterResult.ContactNo;
            model.Country = HeadquarterResult.Country;
            model.City = HeadquarterResult.City;
            model.Street = HeadquarterResult.Street;
            model.Email = HeadquarterResult.Email;
            model.Longitude = Convert.ToDouble(HeadquarterResult.Longitude);
            model.Latitude = Convert.ToDouble(HeadquarterResult.Latitude);

            // reading this attribute using ADO.NET becaues there is a problem in .edmx file, so its not accessible.
            if (HeadquarterResult != null)
            {
                string query = "SELECT Photo FROM HeadQuarters " +
                    "WHERE CompanyID = @CompanyID";
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

            return View(model);
        }

        [HttpPost]
        public JsonResult EditHeadQuarter(HeadQuarterRegisterModel model)
        {
            try
            {
                var HeadquarterManager = factory.CreateHeadquarterssManager();
                var Headquarterresult = HeadquarterManager.Find(a => a.CompanyID == model.CompanyID).FirstOrDefault();
                Headquarterresult.CompanyArabicName = model.CompanyArabicName;
                Headquarterresult.CompanyEnglishName = model.CompanyEnglishName;
                Headquarterresult.ContactNo = model.ContactNo;
                Headquarterresult.Country = model.Country;
                Headquarterresult.City = model.City == null ? "" : model.City;
                Headquarterresult.Street = model.Street == null ? "" : model.Street;
                Headquarterresult.Longitude = model.Longitude;
                Headquarterresult.Latitude = model.Latitude;
                Headquarterresult.Email = model.Email == null ? "" : model.Email;
                Headquarterresult.ProductKey = model.ProductKey;
                HeadquarterManager.Update(Headquarterresult);

                #region Update the Photo using ADO.NET.
                string query = string.Empty;
                byte[] imgByte, Photo = null;
                imgByte = Photo = null;
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
                try
                {
                    query = "SELECT Photo FROM HeadQuarters " +
                        "WHERE CompanyID = @CompanyID";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();

                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@CompanyID", model.CompanyID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (!(reader[0] is DBNull))
                                    {
                                        // Get the image data from the reader
                                        Photo = (byte[])reader[0];
                                    }
                                }
                            }
                        }

                        query = "UPDATE HeadQuarters SET Photo = @Photo WHERE CompanyID = @CompanyID";
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@Photo", imgByte == null ? Photo : imgByte);
                            comm.Parameters.AddWithValue("@CompanyID", model.CompanyID);
                            comm.ExecuteNonQuery();
                        }
                    }

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                #endregion

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RegisterUserHeadQuarter(int CompanyID)
        {
            HeadQuarterRegisterModel model = new HeadQuarterRegisterModel();

            var HeadquarterManager = factory.CreateHeadquarterssManager();
            var UsersManagers = factory.CreateUsersManager();
            var HeadquarterResult = HeadquarterManager.Find(a => a.CompanyID == CompanyID).FirstOrDefault();
            var UsersManagersResult = UsersManagers.Find(p => p.CompanyID == CompanyID && p.SchoolID == -1).FirstOrDefault();

            model.CompanyArabicName = HeadquarterResult.CompanyArabicName;
            model.CompanyEnglishName = HeadquarterResult.CompanyEnglishName;

            if (UsersManagersResult == null)
            {
                if (ViewBag.CurrentLanguage == Languges.English) { model.NewOrEdit = "New: "; }
                else { model.NewOrEdit = "جديد: "; }
            }
            else
            {
                if (ViewBag.CurrentLanguage == Languges.English) { model.NewOrEdit = "Edit: "; }
                else { model.NewOrEdit = "تعديل: "; }
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult RegisterUserHeadQuarter(HeadQuarterRegisterModel model)
        {
            try
            {
                var UsersManagers = factory.CreateUsersManager();
                //Check if user already exist.
                var UsersManagersUN_Found = UsersManagers.Find(a => a.UserName == model.UserName).FirstOrDefault();
                if (UsersManagersUN_Found != null)
                {

                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }

                var UsersManagersResult = UsersManagers.Find(a => a.CompanyID == model.CompanyID && a.SchoolID == -1).FirstOrDefault();
                if (UsersManagersResult == null)
                {
                    UsersManagersResult = UsersManagers.Find(a => a.CompanyID == model.CompanyID).FirstOrDefault();
                    Objects.User NewUser = new Objects.User();
                    NewUser.StaffID = "10000";
                    NewUser.UserName = model.UserName;
                    NewUser.UserType = 0;
                    NewUser.Password = Encrypt(model.Password);
                    NewUser.SchoolID = -1;
                    NewUser.CompanyID = model.CompanyID;
                    UsersManagers.Add(NewUser);
                }
                else
                {
                    //Update the UserName and Password.
                    UsersManagersResult = UsersManagers.Find(a => a.CompanyID == model.CompanyID && a.SchoolID == -1).FirstOrDefault();
                    UsersManagersResult.UserName = model.UserName;
                    UsersManagersResult.Password = Encrypt(model.Password);
                    UsersManagers.Update(UsersManagersResult);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DisplayUserInfoHeadQuarter(int CompanyID)
        {
            HeadQuarterRegisterModel model = new HeadQuarterRegisterModel();
            var HeadquarterManager = factory.CreateHeadquarterssManager();
            var UsersManagers = factory.CreateUsersManager();

            var HeadquarterResult = HeadquarterManager.Find(a => a.CompanyID == CompanyID).FirstOrDefault();
            model.CompanyArabicName = HeadquarterResult.CompanyArabicName;
            model.CompanyEnglishName = HeadquarterResult.CompanyEnglishName;

            var UsersManagersResult = UsersManagers.Find(p => p.CompanyID == CompanyID && p.SchoolID == -1).FirstOrDefault();
            if (UsersManagersResult == null)
            {
                if (ViewBag.CurrentLanguage == Languges.English) { model.UserHeadQuarterDecoreText = "The Username and password not set yet!"; }
                else { model.UserHeadQuarterDecoreText = "لم يتم تسجيل إسم المستخدم وكلمة المرور!"; }
            }
            else
            {
                model.UserName = UsersManagersResult.UserName;
                model.Password = Decrypt(UsersManagersResult.Password);

                if (ViewBag.CurrentLanguage == Languges.English) { model.UserHeadQuarterDecoreText = "UserName and Password"; }
                else { model.UserHeadQuarterDecoreText = "إسم المستخدم وكلمة المرور"; }
            }

            return View(model);
        }

        public ActionResult ViewHeadQuarters()
        {
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }

            string UserName = GetCookie((int)clsenumration.UserData.Username);
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));
            int PrevUserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.PreviousSchoolID));

            string CMDstr;

            if (UserID == 1 && UserName == "Admin" && SchoolID == 1000000 && CompanyID == 1000000) //Admim for yazed
            {
                CMDstr = "SELECT Headquarters.CompanyID, Headquarters.CompanyArabicName, Headquarters.CompanyEnglishName, " +
                         "       Headquarters.ContactNo, Headquarters.Email, Headquarters.City, Countries.ArabicName, Countries.EnglishName " +
                         "FROM Headquarters Headquarters " +
                         "INNER JOIN Countries Countries ON " +
                         "(Countries.ID = Headquarters.Country)";
            }
            else if (PrevUserID == -1) //Schools Manager (Company User = Headquarter User)
            {
                CMDstr = "SELECT Headquarters.CompanyID, Headquarters.CompanyArabicName, Headquarters.CompanyEnglishName, " +
                         "       Headquarters.ContactNo, Headquarters.Email, Headquarters.City, Countries.ArabicName, Countries.EnglishName " +
                         "FROM Headquarters Headquarters " +
                         "INNER JOIN Countries Countries ON " +
                         "(Countries.ID = Headquarters.Country) " +
                         "WHERE Headquarters.CompanyID = " + CompanyID;
            }
            else
            {
                //School school low level user
                return RedirectToAction("Index");
            }

            var model = new List<Viewheadquarters>();
            using (SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString))
            {
                if (CONN.State == ConnectionState.Closed) CONN.Open();
                SqlCommand CMD = new SqlCommand(CMDstr, CONN);
                SqlDataReader DR = CMD.ExecuteReader();
                while (DR.Read())
                {
                    var headquarter = new Viewheadquarters();
                    headquarter.CompanyID = int.Parse(DR["CompanyID"].ToString());
                    headquarter.CompanyArabicName = DR["CompanyArabicName"].ToString();
                    headquarter.CompanyEnglishName = DR["CompanyEnglishName"].ToString();
                    headquarter.ContactNo = DR["ContactNo"].ToString();
                    headquarter.Email = DR["Email"].ToString();
                    headquarter.Country = DR["EnglishName"].ToString();
                    if (lang == "ar") headquarter.Country = DR["ArabicName"].ToString();
                    headquarter.City = DR["City"].ToString();

                    model.Add(headquarter);
                }

                if (!DR.IsClosed) DR.Close();
                if (CONN.State == ConnectionState.Open)
                {
                    CONN.Close();
                    CONN.Dispose();
                }
            }

            return View(model);
        }

        public ActionResult ViewSchoolBranches(int CompanyID)
        {
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }

            string CMDstr;

            CMDstr = "SELECT SchoolBranches.SchoolID, SchoolBranches.SchoolArabicName, SchoolBranches.SchoolEnglishName, " +
                     "       SchoolBranches.SchoolContactNumber, SchoolBranches.Email, SchoolBranches.City, Countries.ArabicName, Countries.EnglishName " +
                     "FROM SchoolBranches SchoolBranches " +
                     "INNER JOIN Countries Countries ON " +
                     "(Countries.ID = SchoolBranches.Country) " +
                     "WHERE SchoolBranches.CompanyID = " + CompanyID;

            var model = new List<Viewschoolbranches>();
            using (SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString))
            {
                if (CONN.State == ConnectionState.Closed) CONN.Open();
                SqlCommand CMD = new SqlCommand(CMDstr, CONN);
                SqlDataReader DR = CMD.ExecuteReader();
                while (DR.Read())
                {
                    var schoolbranch = new Viewschoolbranches();
                    schoolbranch.SchoolID = int.Parse(DR["SchoolID"].ToString());
                    schoolbranch.SchoolArabicName = DR["SchoolArabicName"].ToString();
                    schoolbranch.SchoolEnglishName = DR["SchoolEnglishName"].ToString();
                    schoolbranch.SchoolContactNumber = DR["SchoolContactNumber"].ToString();
                    schoolbranch.Email = DR["Email"].ToString();
                    schoolbranch.Country = DR["EnglishName"].ToString();
                    if (lang == "ar") schoolbranch.Country = DR["ArabicName"].ToString();
                    schoolbranch.City = DR["City"].ToString();

                    model.Add(schoolbranch);
                }

                if (!DR.IsClosed) DR.Close();
                if (CONN.State == ConnectionState.Open)
                {
                    CONN.Close();
                    CONN.Dispose();
                }
            }

            return View(model);
        }

        public ActionResult EditSchoolBranch(int SchoolID)
        {
            string CMDstr;

            CMDstr = "UPDATE SchoolBranches " +
                     "SET IsNew = 1 " +
                     "WHERE SchoolID = " + SchoolID;

            var model = new List<Viewschoolbranches>();
            using (SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString))
            {
                if (CONN.State == ConnectionState.Closed) CONN.Open();
                SqlCommand CMD = new SqlCommand(CMDstr, CONN);
                CMD.ExecuteNonQuery();

                if (CONN.State == ConnectionState.Open)
                {
                    CONN.Close();
                    CONN.Dispose();
                }
            }
            return RedirectToAction("RegistrationStepTwo", "SchoolRegistration");
        }
        #endregion

        #region Departments

        //Hamza01
        public ActionResult GetAllDepartments()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT * FROM Departments WHERE SchoolID = " + SchoolID;

            var departments = new List<Departments>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new Departments()
                    {
                        DepartmentID = int.Parse(reader["DepartmentID"].ToString()),
                        DepartmentArabicName = reader["DepartmentArabicName"].ToString(),
                        DepartmentEnglishName = reader["DepartmentEnglishName"].ToString(),
                        DepartmentType = (bool)reader["IsAcademic"] ? "أكاديمي" : "إداري"
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(departments);
        }

        public ActionResult AddNewDepartment()
        {
            return View();
        }

        //Hamza01
        [HttpPost]
        public JsonResult AddNewDepartment(string DepartmentArabicName, string DepartmentEnglishName, string DepartmentType)
        {
            try
            {
                var DepartmentsManager = factory.CreateDepartmentsManager();
                Department dept = new Department();
                dept.SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)); ;
                dept.DepartmentArabicName = DepartmentArabicName;
                dept.DepartmentEnglishName = DepartmentEnglishName;
                dept.IsAcademic = DepartmentType == "Academic" ? true : false;
                DepartmentsManager.Add(dept);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Hamza01
        public ActionResult EditDepartment(int DepartmentID)
        {
            string query = "SELECT * FROM Departments WHERE DepartmentID = " + DepartmentID;

            var model = new Departments();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    model.DepartmentID = int.Parse(reader["DepartmentID"].ToString());
                    model.DepartmentArabicName = reader["DepartmentArabicName"].ToString();
                    model.DepartmentEnglishName = reader["DepartmentEnglishName"].ToString();
                    model.DepartmentType = reader["IsAcademic"].ToString();
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(model);
        }

        //Hamza01
        public ActionResult SaveEditDepartment(int DepartmentID, string DepartmentArabicName, string DepartmentEnglishName, string DepartmentType)
        {
            try
            {
                var DepartmentsManager = factory.CreateDepartmentsManager();
                var result = DepartmentsManager.Find(d => d.DepartmentID == DepartmentID).FirstOrDefault();
                if (result != null)
                {
                    result.SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)); ;
                    result.DepartmentArabicName = DepartmentArabicName;
                    result.DepartmentEnglishName = DepartmentEnglishName;
                    result.IsAcademic = DepartmentType == "Academic" ? true : false;
                    DepartmentsManager.Update(result);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Curriculums

        //Hamza01
        public ActionResult GetAllCurriculums()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT CUR.CurriculumID, CUR.CurriculumArabicName, CUR.CurriculumEnglishName, DEPT.DepartmentArabicName, DEPT.DepartmentEnglishName " +
                     "FROM Curriculums CUR " +
                     "INNER JOIN CurriculumDepartment CURDEPT ON " +
                     "(CUR.CurriculumID = CURDEPT.CurriculumID) " +
                     "INNER JOIN Departments DEPT ON " +
                     "(DEPT.DepartmentID = CURDEPT.DepartmentID) " +
                     "WHERE CUR.SchoolID = " + SchoolID + " AND DEPT.IsAcademic = 1";

            var model = new List<Curriculums>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    var Curriculum = new Curriculums();
                    Curriculum.CurriculumID = int.Parse(reader["CurriculumID"].ToString());
                    Curriculum.CurriculumArabicName = reader["CurriculumArabicName"].ToString();
                    Curriculum.CurriculumEnglishName = reader["CurriculumEnglishName"].ToString();
                    Curriculum.DepartmentArabicName = reader["DepartmentArabicName"].ToString();
                    Curriculum.DepartmentEnglishName = reader["DepartmentEnglishName"].ToString();

                    model.Add(Curriculum);
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(model);
        }

        public ActionResult EditCurriculum(int CurriculumID)
        {
            string query = "SELECT * FROM Curriculums WHERE CurriculumID = " + CurriculumID;

            var model = new Curriculums();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.Read())
                {
                    model.CurriculumID = int.Parse(reader["CurriculumID"].ToString());
                    model.CurriculumArabicName = reader["CurriculumArabicName"].ToString();
                    model.CurriculumEnglishName = reader["CurriculumEnglishName"].ToString();
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(model);
        }

        //Hamza01
        [HttpPost]
        public JsonResult SaveEditCurriculum(int CurriculumID, string CurriculumArabicName, string CurriculumEnglishName)
        {
            try
            {
                var CurriculumsManager = factory.CreateCurriculumsManager();
                var result = CurriculumsManager.Find(c => c.CurriculumID == CurriculumID).FirstOrDefault();
                if (result != null)
                {
                    result.CurriculumArabicName = CurriculumArabicName;
                    result.CurriculumEnglishName = CurriculumEnglishName;
                    CurriculumsManager.Update(result);
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddNewCurriculum()
        {
            Curriculums model = new Curriculums();
            model.DepartmentTextName = PopulateDepaertments();
            return View(model);
        }

        //Hamza01
        private List<SelectListItem> PopulateDepaertments()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT DepartmentID, DepartmentArabicName, DepartmentEnglishName FROM Departments " +
                     "WHERE SchoolID = " + SchoolID + " AND IsAcademic = 1";

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "ar" ? reader["DepartmentArabicName"].ToString() : reader["DepartmentEnglishName"].ToString(),
                        Value = reader["DepartmentID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        //Hamza01
        [HttpPost]
        public JsonResult AddNewCurriculum(string CurriculumArabicName, string CurriculumEnglishName, string Department)
        {
            try
            {
                int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                int CurriculumID = 0;
                string query;

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // 1:
                    //----- Insert Curriculum information into table Curriculums & Get last CurriculumID inserted.
                    query = "INSERT INTO Curriculums " +
                             "(CurriculumArabicName, CurriculumEnglishName, SchoolID)" +
                             "VALUES" +
                             "(N'" + CurriculumArabicName + "', N'" + CurriculumEnglishName + "', " + SchoolID + "); " +
                             "SELECT SCOPE_IDENTITY() AS result";

                    SqlCommand comm = new SqlCommand(query, conn);
                    CurriculumID = int.Parse(comm.ExecuteScalar().ToString());

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                    //----- End Insert Curriculum information into table Curriculums.


                    // 2:
                    //----- Insert the DepatmentID and CurriculumID into table CurriculumDepartment.
                    var CurriculumDepratmentManager = factory.CreateCurriculumDepartmentsManager();
                    CurriculumDepartment currdep = new CurriculumDepartment();
                    currdep.SchoolID = SchoolID;
                    currdep.CurriculumID = CurriculumID;
                    currdep.DepartmentID = int.Parse(Department);
                    CurriculumDepratmentManager.Add(currdep);
                    //----- End Insert the DepatmentID and CurriculumID into table CurriculumDepartment.
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Hamza01
        #region ClassRooms
        public ActionResult GetAllClassRooms()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT * FROM Classrooms WHERE SchoolID = " + SchoolID;

            var classrooms = new List<ClassRooms>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    classrooms.Add(new ClassRooms()
                    {
                        ClassRoomID = int.Parse(reader["ClassRoomID"].ToString()),
                        CLassRoomNumber = int.Parse(reader["CLassRoomNumber"].ToString()),
                        SchoolID = int.Parse(reader["SchoolID"].ToString()),
                        Building = int.Parse(reader["Building"].ToString()),
                        Floor = int.Parse(reader["Floor"].ToString()),
                        NumberofChairs = int.Parse(reader["NumberofChairs"].ToString()),
                        Available = int.Parse(reader["Available"].ToString()),
                        RoomArabicName = reader["RoomArabicName"].ToString(),
                        RoomEnglishName = reader["RoomEnglishName"].ToString(),
                        BuildingArabicName = reader["BuildingArabicName"].ToString(),
                        BuildingEnglishName = reader["BuildingEnglishName"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(classrooms);
        }

        public ActionResult AddNewClassRoom()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddNewClassRoom(ClassRooms model)
        {
            try
            {
                int _schoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                var schoolSettings = factory.CreateSchoolSettingsManager().Find(s => s.SchoolID == _schoolID).FirstOrDefault();
                var schoolClassRooms = factory.CreateClassroomsManager().Find(cr => cr.SchoolID == _schoolID);


                if (schoolSettings.NumberofClassRooms > schoolClassRooms.Count())
                {
                    string query = "INSERT INTO Classrooms (CLassRoomNumber, SchoolID, Building, Floor, NumberofChairs, Available, " +
                                "RoomArabicName, RoomEnglishName, BuildingArabicName, BuildingEnglishName) " +
                                "VALUES (@CLassRoomNumber, @SchoolID, @Building, @Floor, @NumberofChairs, @Available, " +
                                " @RoomArabicName, @RoomEnglishName, @BuildingArabicName, @BuildingEnglishName)";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@CLassRoomNumber", model.CLassRoomNumber);
                            comm.Parameters.AddWithValue("@SchoolID", _schoolID);
                            comm.Parameters.AddWithValue("@Building", model.Building);
                            comm.Parameters.AddWithValue("@Floor", model.Floor);
                            comm.Parameters.AddWithValue("@NumberofChairs", model.NumberofChairs);
                            comm.Parameters.AddWithValue("@Available", model.Available);
                            comm.Parameters.AddWithValue("@RoomArabicName", model.RoomArabicName);
                            comm.Parameters.AddWithValue("@RoomEnglishName", model.RoomEnglishName);
                            comm.Parameters.AddWithValue("@BuildingArabicName", model.BuildingArabicName);
                            comm.Parameters.AddWithValue("@BuildingEnglishName", model.BuildingEnglishName);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = false, Message = "The max number for Class Rooms is: " + schoolSettings.NumberofClassRooms }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditClassRoom(int ClassRoomID)
        {
            try
            {
                string query = "SELECT * FROM Classrooms WHERE ClassRoomID = " + ClassRoomID;
                var model = new ClassRooms();
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand comm = new SqlCommand(query, conn);
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        model.ClassRoomID = int.Parse(reader["ClassRoomID"].ToString());
                        model.CLassRoomNumber = int.Parse(reader["CLassRoomNumber"].ToString());
                        model.SchoolID = int.Parse(reader["SchoolID"].ToString());
                        model.Building = int.Parse(reader["Building"].ToString());
                        model.Floor = int.Parse(reader["Floor"].ToString());
                        model.NumberofChairs = int.Parse(reader["NumberofChairs"].ToString());
                        model.Available = int.Parse(reader["Available"].ToString());
                        model.RoomArabicName = reader["RoomArabicName"].ToString();
                        model.RoomEnglishName = reader["RoomEnglishName"].ToString();
                        model.BuildingArabicName = reader["BuildingArabicName"].ToString();
                        model.BuildingEnglishName = reader["BuildingEnglishName"].ToString();
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditClassRoom(ClassRooms model)
        {
            try
            {
                string query = "UPDATE Classrooms SET CLassRoomNumber = @CLassRoomNumber, SchoolID = @SchoolID, Building = @Building, " +
                    "Floor = @Floor, NumberofChairs = @NumberofChairs, Available = @Available, " +
                    "RoomArabicName = @RoomArabicName, RoomEnglishName = @RoomEnglishName, " +
                    "BuildingArabicName = @BuildingArabicName, BuildingEnglishName = @BuildingEnglishName " +
                    "WHERE ClassRoomID = @ClassRoomID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@CLassRoomNumber", model.CLassRoomNumber);
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@Building", model.Building);
                        comm.Parameters.AddWithValue("@Floor", model.Floor);
                        comm.Parameters.AddWithValue("@NumberofChairs", model.NumberofChairs);
                        comm.Parameters.AddWithValue("@Available", model.Available);
                        comm.Parameters.AddWithValue("@RoomArabicName", model.RoomArabicName);
                        comm.Parameters.AddWithValue("@RoomEnglishName", model.RoomEnglishName);
                        comm.Parameters.AddWithValue("@BuildingArabicName", model.BuildingArabicName);
                        comm.Parameters.AddWithValue("@BuildingEnglishName", model.BuildingEnglishName);
                        comm.Parameters.AddWithValue("@ClassRoomID", model.ClassRoomID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Hamza01
        #region Classes
        public ActionResult GetAllClasses()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT * FROM SchoolClasses WHERE SchoolID = " + SchoolID;

            var classes = new List<Classes>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    classes.Add(new Classes()
                    {
                        SchoolClassID = int.Parse(reader["SchoolClassID"].ToString()),
                        ClassID = int.Parse(reader["ClassID"].ToString()),
                        SchoolID = int.Parse(reader["SchoolID"].ToString()),
                        CurriculumID = int.Parse(reader["CurriculumID"].ToString()),
                        SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString(),
                        SchoolClassArabicName = reader["SchoolClassArabicName"].ToString(),
                        Coeducation = (bool)reader["Coeducation"],
                        Male = (bool)reader["Male"],
                        Female = (bool)reader["Female"]
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return View(classes);
        }

        private List<SelectListItem> PopulateClasses()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            string query = "SELECT ClassID, ClassArabicName, ClassEnglishName FROM Class";

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "ar" ? reader["ClassArabicName"].ToString() : reader["ClassEnglishName"].ToString(),
                        Value = reader["ClassID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        private List<SelectListItem> PopulateCurriculum()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT CurriculumID, CurriculumArabicName, CurriculumEnglishName FROM Curriculums " +
                "WHERE SchoolID = " + SchoolID;

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "ar" ? reader["CurriculumArabicName"].ToString() : reader["CurriculumEnglishName"].ToString(),
                        Value = reader["CurriculumID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        public ActionResult AddNewClass()
        {
            Classes model = new Classes();
            model.ClassTextName = PopulateClasses();
            model.CurriculumTextName = PopulateCurriculum();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetClassData(int ClassID, int CurriculumID)
        {
            try
            {
                string query, ClassArabicName, ClassEnglishName, CurriculumArabicName, CurriculumEnglishName;
                query = ClassArabicName = ClassEnglishName = CurriculumArabicName = CurriculumEnglishName = "";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    query = "SELECT ClassArabicName, ClassEnglishName From Class WHERE ClassID = " + ClassID;
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            ClassArabicName = reader["ClassArabicName"].ToString();
                            ClassEnglishName = reader["ClassEnglishName"].ToString();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    query = "SELECT CurriculumArabicName, CurriculumEnglishName FROM Curriculums WHERE CurriculumID = " + CurriculumID;
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            CurriculumArabicName = reader["CurriculumArabicName"].ToString();
                            CurriculumEnglishName = reader["CurriculumEnglishName"].ToString();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                string SchoolClassArabicName = ClassArabicName + " " + CurriculumArabicName;
                string SchoolClassEnglishName = ClassEnglishName + " " + CurriculumEnglishName;

                return Json(new { Success = true, SchoolClassArabicName = SchoolClassArabicName, SchoolClassEnglishName = SchoolClassEnglishName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNewClass(Classes model)
        {
            try
            {
                var SchoolSettingsManager = factory.CreateSchoolSettingsManager();
                var schoolMaxNoOfClasses = SchoolSettingsManager.Find(s => s.SchoolID == SchoolID)
                    .FirstOrDefault().NumberofClassRooms;
                var SchoolClassesManager = factory.CreateSchoolClasssManager();
                var schoolClassesNo = SchoolClassesManager.Find(c => c.SchoolID == SchoolID).ToList()
                    .Count();
                if (schoolClassesNo < schoolMaxNoOfClasses)
                {
                    int insertedID = 0;
                    string query = "";

                    query = "INSERT INTO SchoolClasses (ClassID, SchoolID, CurriculumID, SchoolClassEnglishName, " +
                        "SchoolClassArabicName, Coeducation, Male, Female) " +
                        "VALUES (@ClassID, @SchoolID, @CurriculumID, @SchoolClassEnglishName, " +
                        "@SchoolClassArabicName, @Coeducation, @Male, @Female); " +
                        "SELECT SCOPE_IDENTITY() AS result;";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@ClassID", model.ClassID);
                            comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                            comm.Parameters.AddWithValue("@CurriculumID", model.CurriculumID);
                            comm.Parameters.AddWithValue("@SchoolClassEnglishName", model.SchoolClassEnglishName);
                            comm.Parameters.AddWithValue("@SchoolClassArabicName", model.SchoolClassArabicName);
                            comm.Parameters.AddWithValue("@Coeducation", 1);
                            comm.Parameters.AddWithValue("@Male", 0);
                            comm.Parameters.AddWithValue("@Female", 0);
                            insertedID = Convert.ToInt32(comm.ExecuteScalar());
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    #region Adding one Section by default.
                    var classSectionsCount = factory.CreateSectionsManager()
                        .Find(c => c.SchoolClassID == model.SchoolClassID).ToList().Count();

                    if (classSectionsCount == 0)
                    {
                        query = "INSERT INTO Sections " +
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
                                comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                                comm.Parameters.AddWithValue("@SchoolClassID", insertedID);
                                comm.Parameters.AddWithValue("@ClassroomID", 0);
                                comm.Parameters.AddWithValue("@SectionCode", "أ");
                                comm.Parameters.AddWithValue("@MaxNumberofStudents",
                                    SchoolSettingsManager.Find(s => s.SchoolID == SchoolID)
                                    .FirstOrDefault().NumberofChairsPerClass);
                                comm.Parameters.AddWithValue("@NumberofStudents", 0);
                                comm.Parameters.AddWithValue("@IsFull", 0);
                                comm.Parameters.AddWithValue("@SectionArabicName", "شعبة أ");
                                comm.Parameters.AddWithValue("@SectionEnglishName", "Section A");
                                comm.ExecuteNonQuery();
                            }
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    #endregion

                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "",
                        MaxNoofClasses = "الحد الأقصى لعدد الغرف " + schoolMaxNoOfClasses + " غرفة برجاء الذهاب إلى الإعدادات وزيادة عدد الغرف لإتمام عملية الإضافة",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditClass(int SchoolClassID)
        {
            try
            {
                string query = "SELECT * FROM SchoolClasses WHERE SchoolClassID = " + SchoolClassID;
                var model = new Classes();
                model.ClassTextName = PopulateClasses();
                model.CurriculumTextName = PopulateCurriculum();
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand comm = new SqlCommand(query, conn);
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        model.SchoolClassID = int.Parse(reader["SchoolClassID"].ToString());
                        model.ClassID = int.Parse(reader["ClassID"].ToString());
                        model.SchoolID = int.Parse(reader["SchoolID"].ToString());
                        model.CurriculumID = int.Parse(reader["CurriculumID"].ToString());
                        model.SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString();
                        model.SchoolClassArabicName = reader["SchoolClassArabicName"].ToString();
                        model.Coeducation = (bool)reader["Coeducation"];
                        model.Male = (bool)reader["Male"];
                        model.Female = (bool)reader["Female"];
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditClass(Classes model)
        {
            try
            {
                string query = "UPDATE SchoolClasses SET ClassID = @ClassID, SchoolID = @SchoolID, CurriculumID = @CurriculumID, " +
                    "SchoolClassEnglishName = @SchoolClassEnglishName, SchoolClassArabicName = @SchoolClassArabicName, " +
                    "Coeducation = @Coeducation, Male = @Male, Female = @Female " +
                    "WHERE SchoolClassID = @SchoolClassID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@ClassID", model.ClassID);
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@CurriculumID", model.CurriculumID);
                        comm.Parameters.AddWithValue("@SchoolClassEnglishName", model.SchoolClassEnglishName);
                        comm.Parameters.AddWithValue("@SchoolClassArabicName", model.SchoolClassArabicName);
                        comm.Parameters.AddWithValue("@Coeducation", 0);
                        comm.Parameters.AddWithValue("@Male", 0);
                        comm.Parameters.AddWithValue("@Female", 0);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Hamza01
        #region Sections
        public ActionResult GetAllSections()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string query = "SELECT s.*, c.Building, c.CLassRoomNumber, sc.SchoolClassArabicName, sc.SchoolClassEnglishName " +
                "FROM Sections s " +
                "LEFT JOIN Classrooms c ON c.ClassroomID = s.ClassroomID " +
                "INNER JOIN SchoolClasses sc ON sc.SchoolClassID = s.SchoolClassID " +
                "WHERE s.SchoolID = @SchoolID";

            var sections = new List<Sections>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sections.Add(new Sections
                            {
                                SectionID = int.Parse(reader["SectionID"].ToString()),
                                SchoolID = int.Parse(reader["SchoolID"].ToString()),
                                SchoolClassID = int.Parse(reader["SchoolClassID"].ToString()),
                                ClassroomID = int.Parse(reader["ClassroomID"].ToString()),
                                SectionCode = reader["SectionCode"].ToString(),
                                MaxNumberofStudents = int.Parse(reader["MaxNumberofStudents"].ToString()),
                                NumberofStudents = int.Parse(reader["NumberofStudents"].ToString()),
                                IsFull = int.Parse(reader["IsFull"].ToString()),
                                SectionArabicName = reader["SectionArabicName"].ToString(),
                                SectionEnglishName = reader["SectionEnglishName"].ToString(),
                                ClassroomDescription = CurrentLanguage == Languges.English
                                    ? "Building: " + reader["Building"].ToString() + " , Room: " + reader["CLassRoomNumber"].ToString()
                                    : "المبنى: " + reader["Building"].ToString() + " , الغرفة: " + reader["CLassRoomNumber"].ToString(),
                                SchoolClassArabicName = reader["SchoolClassArabicName"].ToString(),
                                SchoolClassEnglishName = reader["SchoolClassEnglishName"].ToString()
                            });
                        }
                    }
                }
            }
            return View(sections);
        }

        public List<SelectListItem> PopulateSchoolClasses()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            string query = "SELECT SchoolClassID, SchoolClassArabicName, SchoolClassEnglishName FROM SchoolClasses " +
                "WHERE SchoolID = " + SchoolID;

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "ar" ? reader["SchoolClassArabicName"].ToString() : reader["SchoolClassEnglishName"].ToString(),
                        Value = reader["SchoolClassID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        private List<SelectListItem> PopulateRooms()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));

            string query = "SELECT ClassRoomID, RoomArabicName, RoomEnglishName FROM Classrooms " +
                "WHERE SchoolID = " + SchoolID;

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "en" ?
                        (string.IsNullOrWhiteSpace(reader["RoomEnglishName"].ToString()) ?
                        reader["RoomArabicName"].ToString() : reader["RoomEnglishName"].ToString()) :
                        (string.IsNullOrWhiteSpace(reader["RoomArabicName"].ToString()) ?
                        reader["RoomEnglishName"].ToString() : reader["RoomArabicName"].ToString()),
                        Value = reader["ClassRoomID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        public ActionResult AddNewSection()
        {
            Sections model = new Sections();
            model.SchoolClassTextName = PopulateSchoolClasses();
            model.RoomTextName = PopulateRooms();
            model.MaxNumberofStudents = (int)factory.CreateSchoolSettingsManager()
                .Find(s => s.SchoolID == SchoolID).FirstOrDefault().NumberofChairsPerClass;
            return View(model);
        }

        public JsonResult GetClassRoomMaxStdNumber(int ClassRoomID)
        {
            try
            {
                int NumberofChairs = 0;
                string query = "SELECT NumberofChairs From Classrooms WHERE ClassRoomID = " + ClassRoomID;

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            NumberofChairs = int.Parse(reader["NumberofChairs"].ToString());
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true, MaxNumberofStudents = NumberofChairs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSectionCode(int SchoolClassID)
        {
            try
            {
                int classSectionsCount = -1;
                string query = "SELECT COUNT(SectionID) FROM Sections WHERE SchoolClassID = @SchoolClassID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolClassID", SchoolClassID);
                        classSectionsCount = int.Parse(comm.ExecuteScalar().ToString());
                    }
                    conn.Close();
                    conn.Dispose();
                }

                string nextCode = ABJADHOS.ContainsKey(classSectionsCount + 1) ?
                    ABJADHOS[classSectionsCount + 1] : "-1";

                return Json(new { Success = true, SectionCode = nextCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message, SectionCode = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNewSection(Sections model)
        {
            try
            {
                string query = "INSERT INTO Sections " +
                    "(SchoolID, SchoolClassID, ClassroomID, SectionCode, MaxNumberofStudents, NumberofStudents, IsFull," +
                    " SectionArabicName, SectionEnglishName) " +
                    "VALUES(@SchoolID, @SchoolClassID, @ClassroomID, @SectionCode, @MaxNumberofStudents, @NumberofStudents, " +
                    "@IsFull, @SectionArabicName, @SectionEnglishName)";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@ClassroomID", model.ClassroomID);
                        comm.Parameters.AddWithValue("@SectionCode", model.SectionCode);
                        comm.Parameters.AddWithValue("@MaxNumberofStudents", model.MaxNumberofStudents);
                        comm.Parameters.AddWithValue("@NumberofStudents", 0);
                        comm.Parameters.AddWithValue("@IsFull", 0);
                        comm.Parameters.AddWithValue("@SectionArabicName", model.SectionArabicName);
                        comm.Parameters.AddWithValue("@SectionEnglishName", model.SectionEnglishName);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditSection(int SectionID)
        {
            try
            {
                string query = "SELECT * FROM Sections WHERE SectionID = " + SectionID;
                var model = new Sections();
                model.SchoolClassTextName = PopulateSchoolClasses();
                model.RoomTextName = PopulateRooms();
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand comm = new SqlCommand(query, conn);
                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.Read())
                    {
                        model.SectionID = int.Parse(reader["SectionID"].ToString());
                        model.SchoolID = int.Parse(reader["SchoolID"].ToString());
                        model.SchoolClassID = int.Parse(reader["SchoolClassID"].ToString());
                        model.ClassroomID = int.Parse(reader["ClassroomID"].ToString());
                        model.SectionCode = reader["SectionCode"].ToString();
                        model.MaxNumberofStudents = int.Parse(reader["MaxNumberofStudents"].ToString());
                        model.NumberofStudents = int.Parse(reader["NumberofStudents"].ToString());
                        model.IsFull = int.Parse(reader["IsFull"].ToString());
                        model.SectionArabicName = reader["SectionArabicName"].ToString();
                        model.SectionEnglishName = reader["SectionEnglishName"].ToString();
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditSection(Sections model)
        {
            try
            {
                string query = "UPDATE Sections SET " +
                    "SchoolID = @SchoolID, SchoolClassID = @SchoolClassID, ClassroomID = @ClassroomID, SectionCode = @SectionCode, " +
                    "MaxNumberofStudents = @MaxNumberofStudents, NumberofStudents = @NumberofStudents, IsFull = @IsFull, " +
                    "SectionArabicName = @SectionArabicName, SectionEnglishName = @SectionEnglishName " +
                    "WHERE SectionID = @SectionID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SectionID", model.SectionID);
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@ClassroomID", model.ClassroomID);
                        comm.Parameters.AddWithValue("@SectionCode", model.SectionCode);
                        comm.Parameters.AddWithValue("@MaxNumberofStudents", model.MaxNumberofStudents);
                        comm.Parameters.AddWithValue("@NumberofStudents", 0);
                        comm.Parameters.AddWithValue("@IsFull", 0);
                        comm.Parameters.AddWithValue("@SectionArabicName", model.SectionArabicName);
                        comm.Parameters.AddWithValue("@SectionEnglishName", model.SectionEnglishName);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        //Hamza01
        #region Subjects
        public ActionResult GetAllSubjects()
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string query = "SELECT s.SubjectID, s.SubjectArabicName, s.SubjectEnglishName, " +
                "s.NumberOfSessionsPerWeek, s.IsOptional, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName " +
                "FROM Subjects s " +
                "INNER JOIN SchoolClasses c ON c.SchoolClassID = s.SchoolClassID " +
                "WHERE s.SchoolID = @SchoolID";

            var subjects = new List<Subjects>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subjects
                            {
                                SubjectID = int.Parse(reader["SubjectID"].ToString()),
                                SubjectArabicName = reader["SubjectArabicName"].ToString(),
                                SubjectEnglishName = reader["SubjectEnglishName"].ToString(),
                                NumberOfSessionsPerWeek = int.Parse(reader["NumberOfSessionsPerWeek"].ToString()),
                                IsOptional = reader["IsOptional"].ToString() == "True" ? true : false,
                                SchoolClassName = CurrentLanguage == Languges.English
                                    ? reader["SchoolClassEnglishName"].ToString() : reader["SchoolClassArabicName"].ToString()
                            });
                        }
                    }
                }
            }
            return View(subjects);
        }

        private List<SelectListItem> PopulateSchedulingCondition()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            string query = "SELECT * FROM TimetableConditions";

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(new SelectListItem
                    {
                        Text = lang == "ar" ?
                            reader["ScheduleConditionArabicText"].ToString() : reader["ScheduleConditionEnglishText"].ToString(),
                        Value = reader["ScheduleConditionID"].ToString()
                    });
                }

                if (!reader.IsClosed) reader.Close();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return items;
        }

        private List<SelectListItem> PopulateIsOptional()
        {
            string lang = ViewBag.CurrentLanguage == Languges.English ? "en" : "ar";

            List<SelectListItem> items = new List<SelectListItem>();
            List<string> Types = lang == "ar" ? new List<string>() { "إجباري", "إختياري" } : new List<string>() { "Compulsory", "Optional" };

            for (int i = 0; i < Types.Count; i++)
            {
                items.Add(new SelectListItem()
                {
                    Text = Types[i],
                    Value = i.ToString()
                });
            }

            return items;
        }

        public ActionResult AddNewSubject()
        {
            Subjects model = new Subjects();
            model.SchoolClassTextName = PopulateSchoolClasses();
            model.SchedulingConditionTextName = PopulateSchedulingCondition();
            model.IsOptionalTextName = PopulateIsOptional();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddNewSubject(Subjects model)
        {
            try
            {
                string query = "INSERT INTO Subjects " +
                    "(SchoolID, SchoolClassID, SubjectArabicName, SubjectEnglishName, MaxMark, FailMark, NumberOfSessionsPerWeek, " +
                    "SchedulingCondition, SectionID, IsOptional) " +
                    "VALUES (@SchoolID, @SchoolClassID, @SubjectArabicName, @SubjectEnglishName, @MaxMark, @FailMark, " +
                    "@NumberOfSessionsPerWeek, @SchedulingCondition, @SectionID, @IsOptional)";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SubjectArabicName", model.SubjectArabicName);
                        comm.Parameters.AddWithValue("@SubjectEnglishName", model.SubjectEnglishName);
                        comm.Parameters.AddWithValue("@MaxMark", model.MaxMark);
                        comm.Parameters.AddWithValue("@FailMark", model.FailMark);
                        comm.Parameters.AddWithValue("@NumberOfSessionsPerWeek", model.NumberOfSessionsPerWeek);
                        comm.Parameters.AddWithValue("@SchedulingCondition", model.SchedulingCondition);
                        comm.Parameters.AddWithValue("@SectionID", -1);
                        comm.Parameters.AddWithValue("@IsOptional", model.IsOptional);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditSubject(int SubjectID)
        {
            string query = "SELECT * FROM Subjects " +
                "WHERE SubjectID = @SubjectID";

            var model = new Subjects();
            model.SchoolClassTextName = PopulateSchoolClasses();
            model.SchedulingConditionTextName = PopulateSchedulingCondition();
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SubjectID", SubjectID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.SubjectID = int.Parse(reader["SubjectID"].ToString());
                            model.SchoolID = int.Parse(reader["SchoolID"].ToString());
                            model.SchoolClassID = int.Parse(reader["SchoolClassID"].ToString());
                            model.SubjectArabicName = reader["SubjectArabicName"].ToString();
                            model.SubjectEnglishName = reader["SubjectEnglishName"].ToString();
                            model.MaxMark = int.Parse(reader["MaxMark"].ToString());
                            model.FailMark = int.Parse(reader["FailMark"].ToString());
                            model.NumberOfSessionsPerWeek = int.Parse(reader["NumberOfSessionsPerWeek"].ToString());
                            model.SchedulingCondition = int.Parse(reader["SchedulingCondition"].ToString());
                            model.IsOptional = reader["IsOptional"].ToString() == "True" ? true : false;
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult EditSubject(Subjects model)
        {
            try
            {
                string query = "UPDATE Subjects SET " +
                    "SchoolID = @SchoolID, SchoolClassID = @SchoolClassID, SubjectArabicName = @SubjectArabicName, " +
                    "SubjectEnglishName = @SubjectEnglishName, MaxMark = @MaxMark, FailMark = @FailMark, " +
                    "NumberOfSessionsPerWeek = @NumberOfSessionsPerWeek, SchedulingCondition = @SchedulingCondition, " +
                    "SectionID = @SectionID, IsOptional = @IsOptional " +
                    "WHERE SubjectID = @SubjectID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID)));
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SubjectArabicName", model.SubjectArabicName);
                        comm.Parameters.AddWithValue("@SubjectEnglishName", model.SubjectEnglishName);
                        comm.Parameters.AddWithValue("@MaxMark", model.MaxMark);
                        comm.Parameters.AddWithValue("@FailMark", model.FailMark);
                        comm.Parameters.AddWithValue("@NumberOfSessionsPerWeek", model.NumberOfSessionsPerWeek);
                        comm.Parameters.AddWithValue("@SchedulingCondition", model.SchedulingCondition);
                        comm.Parameters.AddWithValue("@SectionID", -1);
                        comm.Parameters.AddWithValue("@IsOptional", model.IsOptional);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Semester
        public ActionResult GetAllSemesters()
        {
            List<Semester> semesters = new List<Semester>();
            string query = "SELECT ID, SemesterArabicName, SemesterEnglishName FROM Semesters " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semesters.Add(new Semester()
                            {
                                ID = (int)reader["ID"],
                                SemesterName = CurrentLanguage == Languges.English ?
                                reader["SemesterEnglishName"].ToString() : reader["SemesterArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return View(semesters);
        }

        [HttpPost]
        public JsonResult InsertSemester()
        {
            List<string> semestersEngllishNames = new List<string>() { "First Semester", "Second Semester", "Third Semester", "Fourth Semester", "Fifth Semester" };
            List<string> semestersArabicNames = new List<string>() { "الفصل الدراسي الأول", "الفصل الدراسي الثاني", "الفصل الدراسي الثالث", "الفصل الدراسي الرابع", "الفصل الدراسي الخامس" };
            try
            {
                string query = "";
                int semestersCount = -1;

                query = "SELECT COUNT(ID) FROM Semesters WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                        semestersCount = (int)comm.ExecuteScalar();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                if (semestersCount < semestersEngllishNames.Count)
                {
                    query = "INSERT INTO Semesters " +
                                "(SemesterArabicName, SemesterEnglishName, SchoolID) " +
                                "VALUES (@SemesterArabicName, @SemesterEnglishName, @SchoolID);";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SemesterArabicName", semestersArabicNames[semestersCount]);
                            comm.Parameters.AddWithValue("@SemesterEnglishName", semestersEngllishNames[semestersCount]);
                            comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                            comm.ExecuteNonQuery();
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = false, Message = "Maximum number for Semester per year is 5" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteSemester(int ID)
        {
            try
            {
                string query = "DELETE FROM Semesters WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", ID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Exam

        public List<SelectListItem> PopulateSemesters()
        {
            List<SelectListItem> semesters = new List<SelectListItem>();
            string query = "SELECT ID, SemesterArabicName, SemesterEnglishName FROM Semesters " +
                "WHERE SchoolID = @SchoolID AND SchoolYear = @SchoolYear";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            semesters.Add(new SelectListItem()
                            {
                                Value = reader["ID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["SemesterEnglishName"].ToString() : reader["SemesterArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return semesters;
        }

        public List<SelectListItem> PopulateExamTypes()
        {
            List<SelectListItem> examTypes = new List<SelectListItem>();
            string query = "SELECT * FROM ExamTypes";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            examTypes.Add(new SelectListItem()
                            {
                                Value = reader["ExamTypeID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["TypeEnglishName"].ToString() : reader["TypeArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return examTypes;
        }

        public List<SelectListItem> PopulateExamTitles(bool isTeacherExam = false)
        {
            List<SelectListItem> examTypes = new List<SelectListItem>();

            string query = !isTeacherExam ? "SELECT * FROM ExamTitles " +
                "WHERE ID != (SELECT TOP 1 ID FROM ExamTitles ORDER BY ID DESC)"
                :
                "SELECT * FROM ExamTitles " +
                "WHERE ID = (SELECT TOP 1 ID FROM ExamTitles ORDER BY ID DESC)";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            examTypes.Add(new SelectListItem()
                            {
                                Value = reader["ID"].ToString(),
                                Text = CurrentLanguage == Languges.English ?
                                reader["ExamTitleEnglishName"].ToString() : reader["ExamTitleArabicName"].ToString()
                            });

                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            return examTypes;
        }

        public ActionResult PrepareExams(PrepareExam model)
        {
            model.SchoolClassesList = PopulateSchoolClasses();
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();

            List<Exam> exams = new List<Exam>();
            string query = "SELECT e.ID, e.TotalGrades, " +
                "c.SchoolClassArabicName, c.SchoolClassEnglishName, " +
                "m.SemesterArabicName, m.SemesterEnglishName, " +
                "j.SubjectArabicName, j.SubjectEnglishName, " +
                "t.TypeArabicName, t.TypeEnglishName, " +
                "l.ExamTitleArabicName, l.ExamTitleEnglishName " +
                "FROM (SELECT e.SchoolClassID, l.ID AS ExamTitleID, e.SubjectID, MIN(e.ID) AS ID " +
                "FROM Exams e " +
                "INNER JOIN SchoolClasses c ON e.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN ExamTitles l ON e.ExamTitleID = l.ID " +
                "WHERE e.SchoolID = @SchoolID AND e.SchoolYear = @SchoolYear " +
                "GROUP BY e.SemesterID, e.SchoolClassID, l.ID, e.SubjectID) AS distinctRecords " +
                "INNER JOIN Exams e ON e.SchoolClassID = distinctRecords.SchoolClassID " +
                "AND e.ExamTitleID = distinctRecords.ExamTitleID " +
                "AND e.ID = distinctRecords.ID " +
                "INNER JOIN SchoolClasses c ON e.SchoolClassID = c.SchoolClassID " +
                "INNER JOIN Semesters m ON e.SemesterID = m.ID " +
                "INNER JOIN Subjects j ON e.SubjectID = j.SubjectID " +
                "INNER JOIN ExamTypes t ON e.ExamTypeID = t.ExamTypeID " +
                "INNER JOIN ExamTitles l ON e.ExamTitleID = l.ID ";

            query += model.SchoolClassID != null && model.SchoolClassID != 0 ? " AND e.SchoolClassID = " + model.SchoolClassID : "";
            query += model.SubjectID != null && model.SubjectID != 0 ? " AND e.SubjectID = " + model.SubjectID : "";
            query += model.SemesterID != null && model.SemesterID != 0 ? " AND e.SemesterID = " + model.SemesterID : "";
            query += model.ExamTypeID != null && model.ExamTypeID != 0 ? " AND e.ExamTypeID = " + model.ExamTypeID : "";

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                    comm.Parameters.AddWithValue("@SchoolYear", DateTime.Now.Year.ToString());
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            exams.Add(new Exam()
                            {
                                ID = (int)reader["ID"],
                                ExamTitleName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["ExamTitleEnglishName"].ToString()) ?
                                reader["ExamTitleEnglishName"].ToString()
                                : reader["ExamTitleArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["ExamTitleArabicName"].ToString()) ?
                                reader["ExamTitleArabicName"].ToString()
                                : reader["ExamTitleEnglishName"].ToString(),
                                SchoolClassName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SchoolClassEnglishName"].ToString()) ?
                                reader["SchoolClassEnglishName"].ToString()
                                : reader["SchoolClassArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SchoolClassArabicName"].ToString()) ?
                                reader["SchoolClassArabicName"].ToString()
                                : reader["SchoolClassEnglishName"].ToString(),
                                SubjectName = CurrentLanguage == Languges.English ?
                                !string.IsNullOrWhiteSpace(reader["SubjectEnglishName"].ToString()) ?
                                reader["SubjectEnglishName"].ToString()
                                : reader["SubjectArabicName"].ToString() :
                                !string.IsNullOrWhiteSpace(reader["SubjectArabicName"].ToString()) ?
                                reader["SubjectArabicName"].ToString()
                                : reader["SubjectEnglishName"].ToString(),
                                SemesterName = CurrentLanguage == Languges.English ?
                                reader["SemesterEnglishName"].ToString() :
                                reader["SemesterEnglishName"].ToString(),
                                ExamTypeName = CurrentLanguage == Languges.English ?
                                reader["TypeEnglishName"].ToString() :
                                reader["TypeArabicName"].ToString(),
                                TotalGrades = (decimal)reader["TotalGrades"]
                            });
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            model.Exams = exams;
            return View(model);
        }

        [HttpPost]
        public JsonResult GetSections(int schoolClassID)
        {
            var sectionsManager = factory.CreateSectionsManager();
            var sections = sectionsManager.Find(s => s.SchoolClassID == schoolClassID).ToList();

            var sectionsList = (from s in sections
                                select new SelectListItem
                                {
                                    Value = s.SectionID.ToString(),
                                    Text = CurrentLanguage == Languges.English ? !string.IsNullOrWhiteSpace(s.SectionEnglishName) ?
                                    s.SectionEnglishName : s.SectionArabicName :
                                    !string.IsNullOrWhiteSpace(s.SectionArabicName) ?
                                    s.SectionArabicName : s.SectionEnglishName
                                });
            return Json(sectionsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSubjects(int schoolClassID)
        {
            var subjectsManager = factory.CreateSubjectsManager();
            var subjects = subjectsManager.Find(s => s.SchoolClassID == schoolClassID).ToList();

            var subjectsList = (from s in subjects
                                select new SelectListItem
                                {
                                    Value = s.SubjectID.ToString(),
                                    Text = CurrentLanguage == Languges.English ? !string.IsNullOrWhiteSpace(s.SubjectEnglishName) ?
                                    s.SubjectEnglishName : s.SubjectArabicName :
                                    !string.IsNullOrWhiteSpace(s.SubjectArabicName) ?
                                    s.SubjectArabicName : s.SubjectEnglishName
                                });
            return Json(subjectsList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddExam(Exam model)
        {
            model.SchoolClassesList = PopulateSchoolClasses();
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();
            model.ExamTitelsList = PopulateExamTitles();

            return View(model);
        }

        [HttpPost]
        public JsonResult NewExam(Exam model)
        {
            try
            {
                decimal subjectTotalGrades = -1;
                bool isExist = false;
                string query = "";

                query = "SELECT COUNT(e.ID) " +
                            "FROM Exams e " +
                            "WHERE EXISTS (SELECT 1 FROM Exams ex " +
                            "INNER JOIN SchoolClasses c ON ex.SchoolClassID = c.SchoolClassID " +
                            "INNER JOIN ExamTitles l ON ex.ExamTitleID = l.ID " +
                            "WHERE ex.SchoolID = @SchoolID AND ex.SchoolYear = @SchooYear " +
                            "AND ex.SchoolClassID = @SchoolClassID " +
                            "AND ex.SemesterID = @SemesterID AND ex.SubjectID = @SubjectID AND ex.ExamTitleID = @ExamTitleID " +
                            "GROUP BY ex.SchoolClassID, l.ID " +
                            "HAVING MIN(ex.ID) = e.ID)";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchooYear", DateTime.Now.Year.ToString());
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                        isExist = comm.ExecuteScalar() != DBNull.Value ?
                            (int)comm.ExecuteScalar() > 0 ? true : false : false;
                    }

                    conn.Close();
                    conn.Dispose();
                }
                if (isExist)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = R.GetResource("thissubjectalreadyhavethisexam"),
                        JsonRequestBehavior.AllowGet
                    });
                }

                if (model.IsCounted)
                {
                    var subjectsManager = factory.CreateSubjectsManager();
                    var subjectMaxMark = subjectsManager.Find(s => s.SubjectID == model.SubjectID).FirstOrDefault().MaxMark;

                    if (model.TotalGrades > subjectMaxMark)
                        return Json(new { Success = false, Message = R.GetResource("Themaxmarkforthissubjectis") + " = " + subjectMaxMark }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        query = "SELECT SUM(TotalGrades) " +
                            "FROM Exams e " +
                            "WHERE EXISTS (SELECT 1 FROM Exams ex " +
                            "INNER JOIN SchoolClasses c ON ex.SchoolClassID = c.SchoolClassID " +
                            "INNER JOIN ExamTitles l ON ex.ExamTitleID = l.ID " +
                            "WHERE ex.SchoolID = @SchoolID AND ex.SchoolYear = @SchooYear " +
                            "AND ex.SchoolClassID = @SchoolClassID " +
                            "AND ex.SemesterID = @SemesterID AND ex.SubjectID = @SubjectID AND IsCounted = @IsCounted " +
                            "GROUP BY ex.SchoolClassID, l.ID " +
                            "HAVING MIN(ex.ID) = e.ID)";

                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                                comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                                comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                                comm.Parameters.AddWithValue("@SchooYear", DateTime.Now.Year.ToString());
                                comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                                comm.Parameters.AddWithValue("@IsCounted", model.IsCounted);
                                subjectTotalGrades = comm.ExecuteScalar() != DBNull.Value ?
                                    (decimal)comm.ExecuteScalar() : 0;
                            }

                            conn.Close();
                            conn.Dispose();
                        }

                        if (subjectTotalGrades == subjectMaxMark)
                            return Json(new { Success = false, Message = R.GetResource("Thetotalexamsgradesforthissubjectisalreadyequalthemaxmark") }, JsonRequestBehavior.AllowGet);

                        if (subjectTotalGrades + model.TotalGrades > subjectMaxMark)
                            return Json(new { Success = false, Message = R.GetResource("Theremainingexamsgradesforthissubjectis") + " = " + (subjectMaxMark - subjectTotalGrades) }, JsonRequestBehavior.AllowGet);
                    }
                }

                var sectionsManager = factory.CreateSectionsManager();
                var schoolClassSections = sectionsManager.Find(s => s.SchoolClassID == model.SchoolClassID).ToList();

                int insertedExamID = -1;
                foreach (var section in schoolClassSections)
                {
                    query = "";
                    query = "INSERT INTO Exams " +
                      "(SchoolID, SchoolClassID, SectionID, SubjectID, SemesterID, ExamTypeID, " +
                      "ExamTitleID, TotalGrades, IsCounted) " +
                      "VALUES(@SchoolID, @SchoolClassID, " + section.SectionID + ", @SubjectID, " +
                      "@SemesterID, @ExamTypeID, @ExamTitleID, @TotalGrades, @IsCounted) " +
                      "SELECT SCOPE_IDENTITY() AS RESULT ";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                            comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                            comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                            comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                            comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                            comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                            comm.Parameters.AddWithValue("@TotalGrades", model.TotalGrades);
                            comm.Parameters.AddWithValue("@IsCounted", model.IsCounted);
                            insertedExamID = Convert.ToInt32(comm.ExecuteScalar());
                        }

                        conn.Close();
                        conn.Dispose();
                    }

                    #region
                    // insert the Grades for each student in exam section => zero by default.
                    List<string> studentsIDs = new List<string>();
                    query = "SELECT s.StudentID FROM Student s " +
                        "INNER JOIN StudentSchoolDetails d ON d.StudentID = s.StudentID " +
                        "WHERE d.SectionID = @SectionID";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@SectionID", section.SectionID);
                            using (SqlDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    studentsIDs.Add(reader["StudentID"].ToString());
                                }
                            }
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    foreach (var studentID in studentsIDs)
                    {
                        query = "INSERT INTO Grades " +
                          "(SchoolID, ExamID, StudentID) " +
                          "VALUES(@SchoolID, @ExamID, '" + studentID + "'); ";

                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                                comm.Parameters.AddWithValue("@ExamID", insertedExamID);
                                comm.ExecuteNonQuery();
                            }

                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    #endregion
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditExam(int ID)
        {
            Exam model = new Exam();
            model.SchoolClassesList = PopulateSchoolClasses();
            model.SemestersList = PopulateSemesters();
            model.ExamTypesList = PopulateExamTypes();

            string query = "SELECT * FROM Exams WHERE ID = @ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.ID = (int)reader["ID"];
                            model.SchoolClassID = (int)reader["SchoolClassID"];
                            model.SubjectID = (int)reader["SubjectID"];
                            model.SemesterID = (int)reader["SemesterID"];
                            model.ExamTypeID = (int)reader["ExamTypeID"];
                            model.ExamTitleID = (int)reader["ExamTitleID"];
                            model.TotalGrades = (decimal)reader["TotalGrades"];
                            model.IsCounted = (bool)reader["IsCounted"];
                            model.SchoolYear = reader["SchoolYear"].ToString();
                            model.TeacherID = reader["TeacherID"].ToString();
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            model.ExamTitelsList = model.TeacherID == "-1" ? PopulateExamTitles() : PopulateExamTitles(true);

            return View(model);
        }

        [HttpPost]
        public JsonResult EditExam(Exam model)
        {
            try
            {
                decimal subjectTotalGrades = -1;
                string query = "";

                if (model.IsCounted)
                {
                    var subjectsManager = factory.CreateSubjectsManager();
                    var subjectMaxMark = subjectsManager.Find(s => s.SubjectID == model.SubjectID).FirstOrDefault().MaxMark;

                    if (model.TotalGrades > subjectMaxMark)
                        return Json(new { Success = false, Message = R.GetResource("Themaxmarkforthissubjectis") + " = " + subjectMaxMark }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        query = "SELECT SUM(TotalGrades) " +
                            "FROM Exams e " +
                            "WHERE EXISTS (SELECT 1 FROM Exams ex " +
                            "INNER JOIN SchoolClasses c ON ex.SchoolClassID = c.SchoolClassID " +
                            "INNER JOIN ExamTitles l ON ex.ExamTitleID = l.ID " +
                            "WHERE ex.SchoolID = @SchoolID AND ex.SchoolYear = @SchooYear " +
                            "AND ex.SchoolClassID = @SchoolClassID " +
                            "AND ex.SubjectID = @SubjectID AND IsCounted = @IsCounted AND ex.ID != @ID " +
                            "GROUP BY ex.SchoolClassID, l.ID " +
                            "HAVING MIN(ex.ID) = e.ID)";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                                comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                                comm.Parameters.AddWithValue("@SchooYear", DateTime.Now.Year.ToString());
                                comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                                comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                                comm.Parameters.AddWithValue("@IsCounted", model.IsCounted);
                                comm.Parameters.AddWithValue("@ID", model.ID);
                                subjectTotalGrades = comm.ExecuteScalar() != DBNull.Value ?
                                    (decimal)comm.ExecuteScalar() : 0;
                            }

                            conn.Close();
                            conn.Dispose();
                        }

                        if (subjectTotalGrades == subjectMaxMark)
                            return Json(new { Success = false, Message = R.GetResource("Thetotalexamsgradesforthissubjectisalreadyequalthemaxmark") }, JsonRequestBehavior.AllowGet);

                        if (subjectTotalGrades + model.TotalGrades > subjectMaxMark)
                            return Json(new { Success = false, Message = R.GetResource("Theremainingexamsgradesforthissubjectis") + " = " + (subjectMaxMark - subjectTotalGrades) }, JsonRequestBehavior.AllowGet);
                    }
                }

                Exam oldmodel = new Exam();

                query = "SELECT * FROM Exams WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", model.ID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldmodel.ID = (int)reader["ID"];
                                oldmodel.SchoolClassID = (int)reader["SchoolClassID"];
                                oldmodel.SubjectID = (int)reader["SubjectID"];
                                oldmodel.SemesterID = (int)reader["SemesterID"];
                                oldmodel.ExamTypeID = (int)reader["ExamTypeID"];
                                oldmodel.ExamTitleID = (int)reader["ExamTitleID"];
                                oldmodel.SchoolYear = reader["SchoolYear"].ToString();
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                query = "UPDATE Exams SET " +
                  "SchoolID = @SchoolID, SchoolClassID = @SchoolClassID, " +
                  "SubjectID = @SubjectID, SemesterID = @SemesterID, ExamTypeID = @ExamTypeID, " +
                  "ExamTitleID = @ExamTitleID, TotalGrades = @TotalGrades, IsCounted = @IsCounted " +
                  "WHERE SchoolClassID = @OldSchoolClassID AND SubjectID = @OldSubjectID AND " +
                  "SemesterID = @OldSemesterID AND ExamTypeID = @OldExamTypeID AND " +
                  "ExamTitleID = @OldExamTitleID AND SchoolID = @SchoolID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                        comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                        comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                        comm.Parameters.AddWithValue("@TotalGrades", model.TotalGrades);
                        comm.Parameters.AddWithValue("@IsCounted", model.IsCounted);

                        comm.Parameters.AddWithValue("@OldSchoolClassID", oldmodel.SchoolClassID);
                        comm.Parameters.AddWithValue("@OldSubjectID", oldmodel.SubjectID);
                        comm.Parameters.AddWithValue("@OldSemesterID", oldmodel.SemesterID);
                        comm.Parameters.AddWithValue("@OldExamTypeID", oldmodel.ExamTypeID);
                        comm.Parameters.AddWithValue("@OldExamTitleID", oldmodel.ExamTitleID);
                        comm.ExecuteNonQuery();
                    }

                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteExam(int ID)
        {
            try
            {
                string query = "";
                Exam model = new Exam();

                query = "SELECT * FROM Exams WHERE ID = @ID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                model.ID = (int)reader["ID"];
                                model.SchoolClassID = (int)reader["SchoolClassID"];
                                model.SubjectID = (int)reader["SubjectID"];
                                model.SemesterID = (int)reader["SemesterID"];
                                model.ExamTypeID = (int)reader["ExamTypeID"];
                                model.ExamTitleID = (int)reader["ExamTitleID"];
                                model.SchoolYear = reader["SchoolYear"].ToString();
                            }
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }

                query = "DELETE FROM Exams WHERE SchoolClassID = @SchoolClassID AND " +
                    "SubjectID = @SubjectID AND SemesterID = @SemesterID AND " +
                    "ExamTypeID = @ExamTypeID AND ExamTitleID = @ExamTitleID AND " +
                    "SchoolID = @SchoolID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@SemesterID", model.SemesterID);
                        comm.Parameters.AddWithValue("@SchoolClassID", model.SchoolClassID);
                        comm.Parameters.AddWithValue("@SubjectID", model.SubjectID);
                        comm.Parameters.AddWithValue("@ExamTypeID", model.ExamTypeID);
                        comm.Parameters.AddWithValue("@ExamTitleID", model.ExamTitleID);
                        comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                        comm.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Dispose();
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Bank and Branches
        public ActionResult BanksIndex()
        {
            return View();
        }

        public ActionResult Banks()
        {
            var banks = new List<BankViewModel>();

            string query = "SELECT b.*, c.ArabicName, c.EnglishName FROM Banks b " +
                "INNER JOIN Countries c ON b.CountryId = c.ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            banks.Add(new BankViewModel()
                            {
                                BankCode = (int)reader["BankCode"],
                                BankArabicName = reader["BankArabicName"].ToString(),
                                BankEnglishName = reader["BankEnglishName"].ToString(),
                                CountryName = CurrentLanguage == Languges.English? 
                                    reader["EnglishName"].ToString() : reader["ArabicName"].ToString()
                            });

                        }
                    }
                }
            }
            return View(banks);
        }

        public ActionResult AddNewBank(BankViewModel model)
        {
            var countries = factory.CreateCountrysManager().GetAll().Select(c => new { c.ID, c.ArabicName, c.EnglishName });

            model.Countries = (from country in countries
                               select new SelectListItem
                               {
                                   Value = country.ID.ToString(),
                                   Text = CurrentLanguage == Languges.English? country.EnglishName : country.ArabicName
                               }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddBank(BankViewModel model)
        {
            try
            {
                string query = "INSERT INTO Banks " +
                    "(BankArabicName, BankEnglishName, CountryId) " +
                    "VALUES (@BankArabicName, @BankEnglishName, @CountryId);";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@BankArabicName", model.BankArabicName);
                        comm.Parameters.AddWithValue("@BankEnglishName", model.BankEnglishName);
                        comm.Parameters.AddWithValue("@CountryId", model.CountryId);
                        comm.ExecuteNonQuery();
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditBank(int bankCode)
        {
            var model = new BankViewModel();

            string query = "SELECT * FROM Banks WHERE BankCode = @BankCode";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@BankCode", bankCode);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.BankCode = Convert.ToInt32(reader["BankCode"].ToString());
                            model.BankArabicName = reader["BankArabicName"].ToString();
                            model.BankEnglishName = reader["BankEnglishName"].ToString();
                            model.CountryId = Convert.ToInt32(reader["CountryId"].ToString());
                        }
                    }
                }
            }

            var countries = factory.CreateCountrysManager().GetAll().Select(c => new { c.ID, c.ArabicName, c.EnglishName });

            model.Countries = (from country in countries
                               select new SelectListItem
                               {
                                   Value = country.ID.ToString(),
                                   Text = CurrentLanguage == Languges.English ? country.EnglishName : country.ArabicName
                               }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateBank(BankViewModel model)
        {
            try
            {
                string query = "Update Banks SET " +
                    "BankArabicName = @BankArabicName, BankEnglishName = @BankEnglishName, CountryId = @CountryId " +
                    "WHERE BankCode = @BankCode";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@BankCode", model.BankCode);
                        comm.Parameters.AddWithValue("@BankArabicName", model.BankArabicName);
                        comm.Parameters.AddWithValue("@BankEnglishName", model.BankEnglishName);
                        comm.Parameters.AddWithValue("@CountryId", model.CountryId);
                        comm.ExecuteNonQuery();
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Branches()
        {
            //TODO: Filter by Country.
            var bankbranches = new List<BankBranchViewModel>();

            string query = "SELECT bb.*, b.BankArabicName, b.BankEnglishName, c.ArabicName, c.EnglishName " +
                "FROM BankBranches bb " +
                "INNER JOIN Banks b ON bb.BankCode = b.BankCode " +
                "INNER JOIN Countries c ON b.CountryId = c.ID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bankbranches.Add(new BankBranchViewModel()
                            {
                                BranchCode = (int)reader["BranchCode"],
                                BankCode = (int)reader["BankCode"],
                                BranchArabicName = reader["BranchArabicName"].ToString(),
                                BranchEnglishName = reader["BranchEnglishName"].ToString(),
                                BranchAddress = reader["BranchAddress"].ToString(),
                                BranchContactNumber = reader["BranchContactNumber"].ToString(),
                                BankName = CurrentLanguage == Languges.English ?
                                    reader["BankEnglishName"].ToString() : reader["BankArabicName"].ToString(),
                                CountryName = CurrentLanguage == Languges.English ?
                                    reader["EnglishName"].ToString() : reader["ArabicName"].ToString()
                            });

                        }
                    }
                }
            }
            return View(bankbranches);
        }

        public ActionResult AddNewBranch(BankBranchViewModel model)
        {
            var banks = new List<BankViewModel>();
            string query = "SELECT BankCode, BankArabicName, BankEnglishName FROM Banks";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            banks.Add(new BankViewModel()
                            {
                                BankCode = (int)reader["BankCode"],
                                BankArabicName = reader["BankArabicName"].ToString(),
                                BankEnglishName = reader["BankEnglishName"].ToString()
                            });

                        }
                    }
                }
            }

            model.Banks = (from bank in banks
                           select new SelectListItem
                               {
                                   Value = bank.BankCode.ToString(),
                                   Text = CurrentLanguage == Languges.English ? bank.BankEnglishName : bank.BankArabicName
                               }).ToList();
            return View(model);
        }

        [HttpPost]
        public JsonResult AddBranch(BankBranch model)
        {
            try
            {
                string query = "INSERT INTO BankBranches " +
                    "(BankCode, BranchArabicName, BranchEnglishName, BranchAddress, BranchContactNumber) " +
                    "VALUES (@BankCode, @BranchArabicName, @BranchEnglishName, @BranchAddress, @BranchContactNumber);";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@BranchArabicName", model.BranchArabicName);
                        comm.Parameters.AddWithValue("@BranchEnglishName", model.BranchEnglishName);
                        comm.Parameters.AddWithValue("@BankCode", model.BankCode);
                        comm.Parameters.AddWithValue("@BranchAddress", string.IsNullOrEmpty(model.BranchAddress)? "" : model.BranchAddress);
                        comm.Parameters.AddWithValue("@BranchContactNumber", string.IsNullOrEmpty(model.BranchContactNumber) ? "" : model.BranchContactNumber);
                        comm.ExecuteNonQuery();
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditBranch(int branchCode)
        {
            var model = new BankBranchViewModel();
            var banks = new List<BankViewModel>();

            string query = "SELECT * FROM BankBranches WHERE BranchCode = @BranchCode;" +
                "SELECT BankCode, BankArabicName, BankEnglishName FROM Banks";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@branchCode", branchCode);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.BranchCode = (int)reader["BranchCode"];
                            model.BankCode = (int)reader["BankCode"];
                            model.BranchArabicName = reader["BranchArabicName"].ToString();
                            model.BranchEnglishName = reader["BranchEnglishName"].ToString();
                            model.BranchAddress = reader["BranchAddress"].ToString();
                            model.BranchContactNumber = reader["BranchContactNumber"].ToString();
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            banks.Add(new BankViewModel()
                            {
                                BankCode = (int)reader["BankCode"],
                                BankArabicName = reader["BankArabicName"].ToString(),
                                BankEnglishName = reader["BankEnglishName"].ToString()
                            });

                        }
                    }
                }
            }

            model.Banks = (from bank in banks
                           select new SelectListItem
                           {
                               Value = bank.BankCode.ToString(),
                               Text = CurrentLanguage == Languges.English ? bank.BankEnglishName : bank.BankArabicName
                           }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateBranch(BankBranch model)
        {
            try
            {
                string query = "UPDATE BankBranches SET " +
                    "BankCode = @BankCode, BranchArabicName = @BranchArabicName, BranchEnglishName = @BranchEnglishName, BranchAddress = @BranchAddress, BranchContactNumber = @BranchContactNumber " +
                    "WHERE BranchCode = @BranchCode";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@BranchCode", model.BranchCode);
                        comm.Parameters.AddWithValue("@BranchArabicName", model.BranchArabicName);
                        comm.Parameters.AddWithValue("@BranchEnglishName", model.BranchEnglishName);
                        comm.Parameters.AddWithValue("@BankCode", model.BankCode);
                        comm.Parameters.AddWithValue("@BranchAddress", string.IsNullOrEmpty(model.BranchAddress) ? "" : model.BranchAddress);
                        comm.Parameters.AddWithValue("@BranchContactNumber", string.IsNullOrEmpty(model.BranchContactNumber) ? "" : model.BranchContactNumber);
                        comm.ExecuteNonQuery();
                    }
                }

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}