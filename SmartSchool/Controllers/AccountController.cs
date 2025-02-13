using Business.Base;
using Common.Helpers;
using SmartSchool.Models.Account;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartSchool.Controllers
{
    public class AccountController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        // GET: Account
        public ActionResult Login()
        {
            //string decrypt = Decrypt("MTIz");
            AccountLoginModel model = new AccountLoginModel();
            model.CountryKeysList = GetCountryKeysList();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(AccountLoginModel model, string command)
        {
            string lang = CurrentLanguage == Languges.English ? "en" : "ar";
            var UserManager = factory.CreateUsersManager();
            var User = UserManager.GetUserLogin(model.Username, Encrypt(model.Password), model.loginasparent, model.loginasStudent, lang);

            if (User != null)
            {
                if (User.SchoolID == -1)
                {
                    User.PreviousSchoolID = -1;
                }
                setCookie(User);

                if (string.IsNullOrEmpty(User.GuardianID) && model.loginasStudent == false)
                {
                    var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
                    var checkGuardStudents = ExternalStudentsReqManager.Find(a => a.GuardianID == User.UserID).FirstOrDefault();

                    if (checkGuardStudents != null)
                    {
                        return RedirectToAction("ParentRegistrationRequests", "Registration");
                    }
                    else
                    {
                        return RedirectToAction("ParentRegistrationDashboard", "Registration");
                    }
                }
                else
                {
                    if (User.UserID != -1 && User.StaffID == "10000" && User.UserType == 0 && User.SchoolID == -1)
                    {
                        var schoolBranchesManager = factory.CreateSchoolBranchsManager();
                        var result = schoolBranchesManager.Find(a => a.CompanyID == User.CompanyID).ToList();
                        if (result.Count == 0)
                        {
                            return RedirectToAction("RegistrationStepOne", "SchoolRegistration");
                        }
                        else
                        {
                            var NewSchools = schoolBranchesManager.Find(a => a.CompanyID == User.CompanyID && a.IsNew == true).ToList();

                            if (NewSchools.Count > 0)
                            {
                                return RedirectToAction("RegistrationStepTwo", "SchoolRegistration");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                model.Errormsg = null;
                model.ErrormsgGrdn = null;
                model.ErrormsgStd = null;

                if (model.loginasparent)
                {
                    model.ErrormsgGrdn = CurrentLanguage == Languges.English ? "Wrong username or password" : "إسم المستخدم او كلمة السر خاطئة";
                }
                else
                if (model.loginasStudent)
                {
                    model.ErrormsgStd = CurrentLanguage == Languges.English ? "Wrong username or password" : "إسم المستخدم او كلمة السر خاطئة";
                }
                else
                {
                    model.Errormsg = CurrentLanguage == Languges.English ? "Wrong username or password" : "إسم المستخدم او كلمة السر خاطئة";
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Response.Cookies.Clear();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            Response.Redirect(FormsAuthentication.LoginUrl);
            return RedirectToAction("Login");
        }

        public ActionResult Encryption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Encryption(Encryption model)
        {
            //zls21pfx
            if (Encrypt(model.key) == "emxzMjFwZng=")
            {
                if (model.value == 1)
                {
                    model.result = Decrypt(model.input);
                }
                else
                {
                    model.result = Encrypt(model.input);
                }
            }
            else
            {
                model.result = "Please enter key and input";
            }
            return View(model);
        }

        private string GetCountryCode(string CountryKeyID)
        {
            // Get Key of Country.
            int CountryID = int.Parse(CountryKeyID);
            string CountryKey = "0";
            var CountrysManager = factory.CreateCountrysManager();
            var CountrysResult = CountrysManager.Find(a => a.ID == CountryID).FirstOrDefault();
            if (CountrysResult != null)
            {
                CountryKey = CountrysResult.CountryCode;
            }
            return CountryKey;
        }

        [HttpPost]
        public JsonResult GerGrdInfo(string NationalNo, string CountryKeyID, string MobileNo)
        {
            string CountryKey = GetCountryCode(CountryKeyID);
            string message = GetGrdPass(NationalNo, MobileNo, CountryKey);
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            //return Json(new { Success = true });
        }

        private string GetGrdPass(string NationalNo, string MobileNo, string CountryKey)
        {
            string PassStatus = "error*" + "الرجاء التأكد من الرقم الوطني ورقم الهاتف";
            string MsgBody = "";
            string Pass = "";
            string UserName = "";
            MobileNo = CountryKey + (MobileNo.StartsWith("0") ? MobileNo.TrimStart('0') : MobileNo);

            //Check the database to send sms or not.
            SqlCommand cmd;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            cmd = new SqlCommand("SELECT UserName, Password " +
                                 "FROM ExternalGuardians " +
                                 "WHERE UserName = @UserName AND MobileNo = @MobileNo", con);
            cmd.Parameters.AddWithValue("@UserName", NationalNo);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.HasRows)
                {
                    UserName = dr["UserName"].ToString();
                    Pass = Decrypt(dr["Password"].ToString());
                }
            }
            if (!dr.IsClosed) dr.Close();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            if (Pass != "")
            {
                MsgBody = "SmartSchool" + "\n";
                MsgBody = MsgBody + "UserName: " + UserName + "\n";
                MsgBody = MsgBody + "Password: " + Pass;

                var response = SendSMS(MsgBody, MobileNo.Remove(0, CountryKey.Length), 'e', CountryKey);
                PassStatus = "success*" + "أرسلت رسالة نصيه تحمل إسم المستخدم وكلمة المرور الخاصة بك";
            }

            return PassStatus;
        }

        [HttpPost]
        public JsonResult GetStaffInfo(string UserName, string CountryKeyID, string MobileNo)
        {
            // Get Key of Country.
            string CountryKey = GetCountryCode(CountryKeyID);
            string message = GetStaffPass(UserName, MobileNo, CountryKey);
            return Json(new { Message = message, JsonRequestBehavior.AllowGet });
            //return Json(new { Success = true });
        }

        private string GetStaffPass(string UserName, string MobileNo, string CountryKey)
        {
            string PassStatus = "error*" + "الرجاء التأكد من اسم المستخدم ورقم الهاتف";
            string MsgBody = "";
            string Pass = "";

            //Check the database to send sms or not.
            SqlCommand cmd;
            SqlDataReader dr;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (con.State == System.Data.ConnectionState.Closed) con.Open();
            cmd = new SqlCommand("SELECT Users.Password " +
                                 "FROM Users Users " +
                                 "INNER JOIN StaffContactDetails StaffContactDetails ON " +
                                 "(StaffContactDetails.StaffID = Users.StaffID) " +
                                 "WHERE Users.UserName = @UserName AND StaffContactDetails.MobileNo = @MobileNo", con);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (dr.HasRows)
                {
                    Pass = Decrypt(dr["Password"].ToString());
                }
            }
            if (!dr.IsClosed) dr.Close();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
            }

            if (Pass != "")
            {
                MsgBody += "SmartSchool" + "\n";
                MsgBody += "Your Password: " + Pass;
                //var response = SendSMS(Pass, MobileNo);

                var response = SendSMS(MsgBody, MobileNo, 'e', CountryKey);
                PassStatus = "success*" + "أرسلت رسالة نصيه تحمل كلمة المرور الخاصة بك";
            }

            return PassStatus;
        }

        [HttpPost]
        public JsonResult switchlang()
        {
            LanguageHelper.SwitchLanguage();
            return Json(new { Success = true });

        }
    }
}