using Business.Base;
using Common;
using Common.Helpers;
using Objects;
using SmartSchool.Models.Registration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{

    public class RegistrationController : BaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewParent()
        {
            NewParentModel model = new NewParentModel();
            PrepareRegisterModel(model);
            return View(model);
        }

        //[Authorize]
        //public ActionResult ParentRegistrationRequests()
        //{

        //    ParentRegistrationModel model = new ParentRegistrationModel();
        //    var ExternalGuardianManager = factory.CreateExternalGuardiansManager();
        //    int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
        //    var user = ExternalGuardianManager.Find(a => a.UserID == UserID).FirstOrDefault();
        //    var arabicname = user.ArabicName.Split(' ');
        //    string FirstArabicName = Regex.Replace(arabicname[0], "[-]+", " ", RegexOptions.Compiled);
        //    string SecondArabicName = Regex.Replace(arabicname[1], "[-]+", " ", RegexOptions.Compiled);
        //    string FourthArabicName = Regex.Replace(arabicname[3], "[-]+", " ", RegexOptions.Compiled);
        //    model.ParentName = FirstArabicName + " " + SecondArabicName + " " + FourthArabicName;
        //    var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
        //    var results = ExternalStudentsReqManager.Find(a => a.GuardianID == UserID).ToList();
        //    List<ExternalGuardStudentRequestsModel> studentsrequests = new List<ExternalGuardStudentRequestsModel>();
        //    foreach (var item in results)
        //    {
        //        ExternalGuardStudentRequestsModel Requests = new ExternalGuardStudentRequestsModel();
        //        Requests.StudentID = EncryptUrl(item.StudentID.ToString());
        //        Requests.GuardianID = item.GuardianID;
        //        Requests.StudentName = item.StudentName;
        //        Requests.RequestStatus = item.RequestStatus;
        //        studentsrequests.Add(Requests);

        //    }
        //    model.GuardianID = UserID;
        //    model.StudentsRequests = studentsrequests;
        //    return View(model);
        //}

        //[Authorize]
        //public ActionResult ParentRegistrationDashboard()
        //{
        //    ParentRegistrationModel model = new ParentRegistrationModel();
        //    var ExternalGuardianManager = factory.CreateExternalGuardiansManager();
        //    int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
        //    var user = ExternalGuardianManager.Find(a => a.UserID == UserID).FirstOrDefault();
        //    var arabicname = user.ArabicName.Split(' ');
        //    string FirstArabicName = Regex.Replace(arabicname[0], "[-]+", " ", RegexOptions.Compiled);
        //    string SecondArabicName = Regex.Replace(arabicname[1], "[-]+", " ", RegexOptions.Compiled);
        //    string FourthArabicName = Regex.Replace(arabicname[3], "[-]+", " ", RegexOptions.Compiled);
        //    model.ParentName = FirstArabicName + " " + SecondArabicName + " " + FourthArabicName;

        //    return View(model);
        //}

        [Authorize]
        public ActionResult ParentRegistrationRequests()
        {

            ParentRegistrationModel model = new ParentRegistrationModel();
            var ExternalGuardianManager = factory.CreateExternalGuardiansManager();
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            var user = ExternalGuardianManager.Find(a => a.UserID == UserID).FirstOrDefault();

            var arabicNameParts = user.ArabicName.Split(' ');
            model.ParentName = string.Empty;

            foreach (var namePart in arabicNameParts)
            {
                model.ParentName += Regex.Replace(namePart, "[-]+", " ", RegexOptions.Compiled) + " ";
            }

            model.ParentName = model.ParentName.Trim();

            var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
            var results = ExternalStudentsReqManager.Find(a => a.GuardianID == UserID).ToList();
            List<ExternalGuardStudentRequestsModel> studentsrequests = new List<ExternalGuardStudentRequestsModel>();
            foreach (var item in results)
            {
                ExternalGuardStudentRequestsModel Requests = new ExternalGuardStudentRequestsModel();
                Requests.StudentID = EncryptUrl(item.StudentID.ToString());
                Requests.GuardianID = item.GuardianID;
                Requests.StudentName = item.StudentName;
                Requests.RequestStatus = item.RequestStatus;
                studentsrequests.Add(Requests);

            }
            model.GuardianID = UserID;
            model.StudentsRequests = studentsrequests;
            return View(model);
        }

        [Authorize]
        public ActionResult ParentRegistrationDashboard()
        {
            ParentRegistrationModel model = new ParentRegistrationModel();
            var ExternalGuardianManager = factory.CreateExternalGuardiansManager();
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            var user = ExternalGuardianManager.Find(a => a.UserID == UserID).FirstOrDefault();
            var arabicNameParts = user.ArabicName.Split(' ');
            model.ParentName = string.Empty;

            foreach (var namePart in arabicNameParts)
            {
                model.ParentName += Regex.Replace(namePart, "[-]+", " ", RegexOptions.Compiled) + " ";
            }

            model.ParentName = model.ParentName.Trim();
            return View(model);
        }

        [Authorize]
        public ActionResult StudentRegistrationWizard(string StudentID)
        {
            int studentid = Convert.ToInt32(DecryptURL(StudentID));
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
            string query = "";

            StudentRegistrationViewModel model = new StudentRegistrationViewModel();
            var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();
            var ExternalGuardStudentsRequestsManager = factory.CreateExternalGuardStudentsRequestsManager();
            var ExternalStudentsManager = factory.CreateExternalStudentsManager();
            var ExternalStudentsOtherDetailsManager = factory.CreateExternalOtherStudentDetailsManager();
            var ExternalStudentAddressManager = factory.CreateExternalStudentAdresssManager();
            var StudentDiseasManager = factory.CreateStudentDiseassManager();
            var StudentGuardDetailsManager = factory.CreateExternalStudentGuardDetailsManager();
            var SystemSettingManager = factory.CreateSystemSettingsManager();
            var ExternalStudentSchoolDetailsManager = factory.CreateExternalStudentSchoolDetailsManager();
            var student = ExternalStudentsManager.Find(a => a.StudentID == studentid).FirstOrDefault();

            var OtherStudentDetails = new ExternalOtherStudentDetail();
            OtherStudentDetails = ExternalStudentsOtherDetailsManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            if (OtherStudentDetails == null)
            {
                OtherStudentDetails = ExternalStudentsOtherDetailsManager.Find(a => a.GuardianID == UserID).FirstOrDefault();

            }
            //var studentAddress = ExternalStudentAddressManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            ExternalStudentAdress externalStudentAddress = null;
            query = "SELECT * FROM ExternalStudentAdresses WHERE StudentID = @StudentID AND GuardianID = @GuardianID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", studentid);
                    comm.Parameters.AddWithValue("@GuardianID", UserID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            externalStudentAddress = new ExternalStudentAdress();
                            externalStudentAddress.AddressID = Convert.ToInt32(reader["AddressID"].ToString());
                            externalStudentAddress.StudentID = reader["StudentID"].ToString();
                            externalStudentAddress.GuardianID = Convert.ToInt32(reader["GuardianID"].ToString());
                            externalStudentAddress.CountryID = Convert.ToInt32(reader["CountryID"].ToString());
                            externalStudentAddress.City = reader["City"].ToString();
                            externalStudentAddress.Street = reader["Street"].ToString();
                            externalStudentAddress.Building = Convert.ToInt32(reader["Building"].ToString());
                            externalStudentAddress.Flat = Convert.ToInt32(reader["Flat"].ToString());
                            externalStudentAddress.MobileNumber = reader["MobileNumber"].ToString();
                            externalStudentAddress.Email = reader["Email"].ToString();
                            externalStudentAddress.Longitude = Convert.ToDouble(reader["Longitude"].ToString());
                            externalStudentAddress.Latitude = Convert.ToDouble(reader["Latitude"].ToString());
                            externalStudentAddress.TransportDirectionID = Convert.ToInt32(reader["TransportDirectionID"].ToString());
                        }
                    }

                }
            }
            var StudentDiseas = StudentDiseasManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            var studentguardetails = new ExternalStudentGuardDetail();
            studentguardetails = StudentGuardDetailsManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            if (studentguardetails == null)
            {
                studentguardetails = StudentGuardDetailsManager.Find(a => a.GuardianID == UserID).FirstOrDefault();

            }
            var studentresult = ExternalStudentsManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            var studentschoolDetails = ExternalStudentSchoolDetailsManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            string studentname = "";
            studentname = studentresult == null ? "" : studentresult.StudentArabicName;
            if (string.IsNullOrEmpty(studentname))
            {
                studentname = ExternalGuardStudentsRequestsManager.Find(a => a.StudentID == studentid).Select(a => a.StudentName).FirstOrDefault();

            }
            var name = studentname.Split(' ');
            if (name.Length == 3) // means the student name is until third name, like (Hamza Ashry Hamza).
            {
                model.FirstArabicName = Regex.Replace(name[0], "[-]+", " ", RegexOptions.Compiled);
                model.SecondArabicName = Regex.Replace(name[1], "[-]+", " ", RegexOptions.Compiled);
                model.FourthArabicName = Regex.Replace(name[2], "[-]+", " ", RegexOptions.Compiled);
            }
            else // other means the student name is until fourth name, like (Hamza Ashry Hamza abdelhafeez).
            {
                model.FirstArabicName = Regex.Replace(name[0], "[-]+", " ", RegexOptions.Compiled);
                model.SecondArabicName = Regex.Replace(name[1], "[-]+", " ", RegexOptions.Compiled);
                model.ThirdArabicName = Regex.Replace(name[2], "[-]+", " ", RegexOptions.Compiled);
                model.FourthArabicName = Regex.Replace(name[3], "[-]+", " ", RegexOptions.Compiled);
            }

            model.student = studentresult;
            // reading those three attributes using ADO.NET becaues there is a problem in .edmx file, so they not accessible.
            if (studentresult != null)
            {
                //studentresult.DateofBirth = Convert.ToDateTime(studentresult.DateofBirth.Value.ToShortDateString());
                query = "SELECT BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate FROM ExternalStudent " +
                    "WHERE StudentID = @StudentID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", studentid);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            byte[] imageData = null;
                            if (!(reader[0] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[0];
                                model.student.BirthCertificatePhoto = imageData;
                            }
                            if (!(reader[1] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[1];
                                model.student.FamilyBookPhoto = imageData;
                            }
                            if (!(reader[2] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[2];
                                model.student.LastYearCertificate = imageData;
                            }
                        }
                    }
                }
            }
            model.OtherStudentDetail = OtherStudentDetails;
            //model.StudentAddress = studentAddress;
            if (externalStudentAddress == null)
            {
                query = "SELECT TOP(1) * FROM ExternalStudentAdresses WHERE GuardianID = @GuardianID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@GuardianID", UserID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                externalStudentAddress = new ExternalStudentAdress();
                                externalStudentAddress.AddressID = Convert.ToInt32(reader["AddressID"].ToString());
                                externalStudentAddress.GuardianID = Convert.ToInt32(reader["GuardianID"].ToString());
                                externalStudentAddress.CountryID = Convert.ToInt32(reader["CountryID"].ToString());
                                externalStudentAddress.City = reader["City"].ToString();
                                externalStudentAddress.Street = reader["Street"].ToString();
                                externalStudentAddress.Building = Convert.ToInt32(reader["Building"].ToString());
                                externalStudentAddress.Flat = Convert.ToInt32(reader["Flat"].ToString());
                                externalStudentAddress.MobileNumber = reader["MobileNumber"].ToString();
                                externalStudentAddress.Email = reader["Email"].ToString();
                                externalStudentAddress.Longitude = Convert.ToDouble(reader["Longitude"].ToString());
                                externalStudentAddress.Latitude = Convert.ToDouble(reader["Latitude"].ToString());
                                externalStudentAddress.TransportDirectionID = Convert.ToInt32(reader["TransportDirectionID"].ToString());
                            }
                        }
                    }
                }
            }
            model.StudentAddress = externalStudentAddress;
            model.StudentDiseas = StudentDiseas;
            model.student = student;

            model.StudentIDNumber = studentid;
            model.GurdianID = UserID;
            model.SchoolDetail = studentschoolDetails;

            var externalGurdianresult = ExternalGuardiansManager.Find(a => a.UserID == UserID).FirstOrDefault();
            model.GuardianName = externalGurdianresult.ArabicName;
            if (studentguardetails != null)
            {
                model.FatherName = studentguardetails.FatherName == null ? "" : studentguardetails.FatherName;
                model.MotherName = studentguardetails.MotherName == null ? "" : studentguardetails.MotherName;
                model.FatherQualification = studentguardetails.FatherQualification.Value;
                model.MotherQualification = studentguardetails.MotherQualification.Value;
                model.FatherSpecialization = studentguardetails.FatherSpecialization.Value;
                model.MotherSpecialization = studentguardetails.MotherSpecialization.Value;
                model.FatherJob = studentguardetails.FatherJob == null ? "" : studentguardetails.FatherJob;
                model.MotherJob = studentguardetails.MotherJob == null ? "" : studentguardetails.MotherJob;
                model.FatherWorkPhone = studentguardetails.FatherWorkPhone == null ? "" : studentguardetails.FatherWorkPhone;
                model.MotherWorkPhone = studentguardetails.MotherWorkPhone == null ? "" : studentguardetails.MotherWorkPhone;
                model.FatherEmail = studentguardetails.FatherEmail == null ? "" : studentguardetails.FatherEmail;
                model.MotherEmail = studentguardetails.MotherEmail == null ? "" : studentguardetails.MotherEmail;
                model.mailBox = studentguardetails.MailBox == null ? "" : studentguardetails.MailBox;
                model.PostalCode = studentguardetails.PostalCode == null ? "" : studentguardetails.PostalCode;
                model.smsNumber = studentguardetails.SmsNumber == null ? "" : studentguardetails.SmsNumber;
                model.MotherMobile = studentguardetails.MotherMobile == null ? "" : studentguardetails.MotherMobile;
                model.FatherMobile = studentguardetails.FatherMobile == null ? "" : studentguardetails.FatherMobile;

            }

            if (student != null)
            {
                if (student.DateofBirth != null)
                {
                    model.DateofBirth = student.DateofBirth.Value.ToShortDateString();
                }
                model.BirthPlace = student.BirthPlace == null ? "" : student.BirthPlace;
            }

            model.GuardianMobileNumber = externalGurdianresult.MobileNo;
            PrepareStudentModel(model);
            return View(model);
        }

        [HttpPost]
        public JsonResult NewParent(NewParentModel model)
        {
            try
            {
                //----------------------------------------------------------------------------------------
                int CountryID = int.Parse(model.CountryKey.ToString());
                string CountryKey = "0";
                var CountrysManager = factory.CreateCountrysManager();
                var CountrysResult = CountrysManager.Find(a => a.ID == CountryID).FirstOrDefault();
                if (CountrysResult != null)
                {
                    CountryKey = CountrysResult.CountryCode;
                }
                //----------------------------------------------------------------------------------------

                var ExternalGuardianManager = factory.CreateExternalGuardiansManager();
                ExternalGuardian obj = new ExternalGuardian();
                obj.ArabicName = model.FirstArabicName.Trim().Replace(" ", "-") + " " + model.SecondArabicName.Trim().Replace(" ", "-") + " " + model.ThirdArabicName.Trim().Replace(" ", "-") + " " + model.FourthArabicName.Trim().Replace(" ", "-");
                obj.EnglishName = model.FirstArabicName.Trim().Replace(" ", "-") + " " + model.SecondArabicName.Trim().Replace(" ", "-") + " " + model.ThirdArabicName.Trim().Replace(" ", "-") + " " + model.FourthArabicName.Trim().Replace(" ", "-");
                obj.Gender = model.Gender;
                obj.Nationality = model.Nationality;
                obj.NationalNumber = model.NationalNumber == null ? "" : model.NationalNumber;
                obj.ResidencyNumber = model.ResidencyNumber == null ? "" : model.ResidencyNumber;
                obj.CivilNumber = model.CivilNumber == null ? "" : model.CivilNumber;
                obj.MobileNo = CountryKey + (model.MobileNo.StartsWith("0") ? model.MobileNo.TrimStart('0') : model.MobileNo);
                obj.IsAccepted = false;

                string usrname = "";
                bool SelectedFieldDone = false;
                if (!string.IsNullOrEmpty(model.NationalNumber))
                {
                    usrname = model.NationalNumber;
                    SelectedFieldDone = true;
                }
                if (!string.IsNullOrEmpty(model.ResidencyNumber) && !SelectedFieldDone)
                {
                    usrname = model.ResidencyNumber;
                    SelectedFieldDone = true;
                }
                if (!string.IsNullOrEmpty(model.CivilNumber) && !SelectedFieldDone)
                {
                    usrname = model.CivilNumber;
                }
                obj.UserName = usrname;

                var Password = RandomString(6, true);
                obj.Password = Encrypt(Password);
                ExternalGuardianManager.Add(obj);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string sms = "Username:" + " " + usrname + "%0a" + "Password:" + " " + Password;

                var result = SendSMS(sms, model.MobileNo, 'e', CountryKey);
                if (result.IsSuccessStatusCode)
                {
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddStudentsToRegister(List<string> StudentsArray)
        {
            var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));

            foreach (var item in StudentsArray)
            {
                ExternalGuardStudentsRequest obj = new ExternalGuardStudentsRequest();
                obj.StudentName = item;
                obj.GuardianID = UserID;
                obj.RequestStatus = (int)clsenumration.RequestStatus.NotSent;
                ExternalStudentsReqManager.Add(obj);
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult AddAdditionalStudentToRegister(string StudentName)
        {
            var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
            int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));

            ExternalGuardStudentsRequest obj = new ExternalGuardStudentsRequest();
            obj.StudentName = StudentName;
            obj.GuardianID = UserID;
            obj.RequestStatus = (int)clsenumration.RequestStatus.NotSent;
            ExternalStudentsReqManager.Add(obj);

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult DeleteStudent(string StudentID)
        {
            int studentid = Convert.ToInt32(DecryptURL(StudentID));
            var ExternalStudentsReqManager = factory.CreateExternalGuardStudentsRequestsManager();
            var student = ExternalStudentsReqManager.Find(a => a.StudentID == studentid).FirstOrDefault();
            ExternalStudentsReqManager.Delete(student);

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        private void PrepareRegisterModel(NewParentModel model)
        {
            model.NationalityList = GetNationalitiesList();
            model.CountryKeysList = GetCountryKeysList();
        }

        // Hamza01
        public JsonResult CheckParent(string NationalNumber, string ResidencyNumber, string CountryCode, string MobileNo)
        {
            // -------------------------------------------------------------
            // Get countryKey to concatenate it with the MobileNo.
            int CountryID = int.Parse(CountryCode);
            string CountryKey = "0";
            var CountrysManager = factory.CreateCountrysManager();
            var CountrysResult = CountrysManager.Find(c => c.ID == CountryID).FirstOrDefault();
            if (CountrysResult != null)
            {
                CountryKey = CountrysResult.CountryCode;
                MobileNo = CountryKey + MobileNo.TrimStart('0');
            }
            // -------------------------------------------------------------
            var GuardiansManager = factory.CreateGuardiansManager();
            var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();
            string old = "", oldPhone = "";
            string External = "", externalPhone = "";
            if (!string.IsNullOrEmpty(NationalNumber))
            {
                External = ExternalGuardiansManager.Find(a => a.NationalNumber == NationalNumber).Select(a => a.NationalNumber).FirstOrDefault();
                old = GuardiansManager.Find(a => a.NationalNumber == NationalNumber).Select(a => a.NationalNumber).FirstOrDefault();
            }
            if (!string.IsNullOrEmpty(ResidencyNumber))
            {
                External = ExternalGuardiansManager.Find(a => a.ResidencyNumber == ResidencyNumber).Select(a => a.ResidencyNumber).FirstOrDefault();
                old = GuardiansManager.Find(a => a.ResidencyNumber == ResidencyNumber).Select(a => a.ResidencyNumber).FirstOrDefault();

            }
            if (!string.IsNullOrEmpty(MobileNo))
            {
                externalPhone = ExternalGuardiansManager.Find(e => e.MobileNo == MobileNo).Select(e => e.MobileNo).FirstOrDefault();
                oldPhone = GuardiansManager.Find(g => g.MobileNumber == MobileNo).Select(g => g.MobileNumber).FirstOrDefault();
            }
            return Json(new { Success = true, old = old, External = External, oldPhone = oldPhone, externalPhone = externalPhone }, JsonRequestBehavior.AllowGet);
        }
        private void PrepareStudentModel(StudentRegistrationViewModel model)
        {
            model.NationalityList = GetNationalitiesList();
            model.GuardianRelationshipList = GuardianTypesList();
            model.SpecializationList = GetSpecializationsList();
            model.QualificationList = GetQualificationsList();
            model.Countries = GetCountriesList();
            model.AcademicSubjectList = AcademicSubjects();
            model.LiveWithTypesList = GetLiveWithTypesList();
            model.ResidenceConditionsList = GetSpecialResidenceConditionTypes();
            model.BloodTypesList = GetBloodTypes();
            model.physicalStatusList = GetPhysicalStatus();
            model.CountriesHeadQuarter = GetCountriesJoinHeadCountry();
            model.EducationalYears = GetEducationalYears();
        }

        #region Submit Student Register Steps

        // Hamza001
        [HttpPost]
        public JsonResult StudentInfoStepOne(StudentRegistrationViewModel model)
        {
            try
            {
                byte[] imgByte, BCPimgByte, FBPimgByte, LYCimgByte;
                imgByte = BCPimgByte = FBPimgByte = LYCimgByte = null;
                string query = "";

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader != null && fileuploader.ContentLength > 0)
                    {
                        imgByte = new byte[fileuploader.ContentLength];
                        //force the control to load data in array
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }
                    var BCFfileuploader = System.Web.HttpContext.Current.Request.Files["BCPfileuploader"];
                    if (BCFfileuploader != null && BCFfileuploader.ContentLength > 0)
                    {
                        BCPimgByte = new byte[BCFfileuploader.ContentLength];
                        //force the control to load data in array
                        BCFfileuploader.InputStream.Read(BCPimgByte, 0, BCFfileuploader.ContentLength);
                    }
                    var FBPfileuploader = System.Web.HttpContext.Current.Request.Files["FBPfileuploader"];
                    if (FBPfileuploader != null && FBPfileuploader.ContentLength > 0)
                    {
                        FBPimgByte = new byte[FBPfileuploader.ContentLength];
                        //force the control to load data in array
                        FBPfileuploader.InputStream.Read(FBPimgByte, 0, FBPfileuploader.ContentLength);
                    }
                    var LYCfileuploader = System.Web.HttpContext.Current.Request.Files["LYCfileuploader"];
                    if (LYCfileuploader != null && LYCfileuploader.ContentLength > 0)
                    {
                        LYCimgByte = new byte[LYCfileuploader.ContentLength];
                        //force the control to load data in array
                        LYCfileuploader.InputStream.Read(LYCimgByte, 0, LYCfileuploader.ContentLength);
                    }
                }
                var StudentManager = factory.CreateExternalStudentsManager();
                var result = StudentManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (model.ThirdArabicName == null)
                    model.ThirdArabicName = "";
                var StudentName = model.FirstArabicName.Trim().Replace(" ", "-") + " " + model.SecondArabicName.Trim().Replace(" ", "-") + " " + model.ThirdArabicName.Trim().Replace(" ", "-") + " " + model.FourthArabicName.Trim().Replace(" ", "-");
                var Gender = model.student.Gender;
                var Nationality = model.student.Nationality;
                var NationalNumber = model.student.NationalNumber == null ? "" : model.student.NationalNumber;
                var DateOfBirth = model.DateofBirth;
                var ResidencyNumber = model.student.ResidencyNumber == null ? "" : model.student.ResidencyNumber;

                if (result != null)
                {
                    result.StudentArabicName = StudentName;
                    result.StudentEnglishName = StudentName;
                    result.NationalNumber = NationalNumber;
                    result.Gender = Gender;
                    result.Nationality = Nationality;
                    result.DateofBirth = Convert.ToDateTime(Convert.ToDateTime(DateOfBirth).ToShortDateString());
                    result.AccountNumber = null;
                    result.FamilyID = -1;
                    result.MotherID = -1;
                    result.ResidencyNumber = ResidencyNumber;
                    result.PassportNumber = "";
                    result.Active = -1;
                    result.BirthPlace = model.BirthPlace;
                    result.Photo = imgByte == null ? result.Photo : imgByte;
                    result.Photo_A = "NOPHOTO.jpeg";
                    StudentManager.Update(result);
                    #region adding the Student Files using ADO.NET.
                    byte[] Photo, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate;
                    Photo = BirthCertificatePhoto = FamilyBookPhoto = LastYearCertificate = null;
                    try
                    {
                        query = "SELECT Photo, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate FROM ExternalStudent " +
                            "WHERE StudentID = @StudentID";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
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
                                    if (!(reader[1] is DBNull))
                                    {
                                        // Get the image data from the reader
                                        imageData = (byte[])reader[1];
                                        BirthCertificatePhoto = imageData;
                                    }
                                    if (!(reader[2] is DBNull))
                                    {
                                        // Get the image data from the reader
                                        imageData = (byte[])reader[2];
                                        FamilyBookPhoto = imageData;
                                    }
                                    if (!(reader[3] is DBNull))
                                    {
                                        // Get the image data from the reader
                                        imageData = (byte[])reader[3];
                                        LastYearCertificate = imageData;
                                    }
                                }
                            }
                        }
                        query = "UPDATE ExternalStudent SET Photo = @Photo, BirthCertificatePhoto = @BCP, " +
                            "FamilyBookPhoto = @FBP, LastYearCertificate = @LYC " +
                            "WHERE StudentID = @StudentID";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@Photo", imgByte == null ? Photo : imgByte);
                                comm.Parameters.AddWithValue("@BCP", BCPimgByte == null ? BirthCertificatePhoto : BCPimgByte);
                                comm.Parameters.AddWithValue("@FBP", FBPimgByte == null ? FamilyBookPhoto : FBPimgByte);
                                comm.Parameters.AddWithValue("@LYC", LYCimgByte == null ? LastYearCertificate : LYCimgByte);
                                comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                                comm.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                }
                else
                {
                    try
                    {
                        query = "INSERT INTO ExternalStudent" +
                            " (StudentID, StudentArabicName, StudentEnglishName, NationalNumber, Gender," +
                            " Nationality, DateofBirth, GuardianID, AccountNumber, FamilyID, MotherID, ResidencyNumber, PassportNumber, Active," +
                            " Photo, Photo_A, BirthPlace, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate)" +
                            " VALUES (@StudentID, @StudentArabicName, @StudentEnglishName, @NationalNumber, @Gender, @Nationality, @DateofBirth," +
                            " @GuardianID, NULL, -1, -1,'', '', -1, @Photo, N'NOPHOTO.jpeg'," +
                            " @BirthPlace, @BirthCertificatePhoto, @FamilyBookPhoto, @LastYearCertificate);" +
                            " SELECT SCOPE_IDENTITY() AS result;";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
                                comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                                comm.Parameters.AddWithValue("@StudentArabicName", StudentName);
                                comm.Parameters.AddWithValue("@StudentEnglishName", StudentName);
                                comm.Parameters.AddWithValue("@NationalNumber", NationalNumber);
                                comm.Parameters.AddWithValue("@Gender", Gender);
                                comm.Parameters.AddWithValue("@Nationality", Nationality);
                                comm.Parameters.AddWithValue("@DateofBirth", DateOfBirth);
                                comm.Parameters.AddWithValue("@GuardianID", model.GurdianID);
                                comm.Parameters.AddWithValue("@Photo", imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "camera2.png")) : imgByte);
                                comm.Parameters.AddWithValue("@BirthPlace", model.BirthPlace);
                                comm.Parameters.AddWithValue("@BirthCertificatePhoto", BCPimgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "BrithCertificate.jpeg")) : BCPimgByte);
                                comm.Parameters.AddWithValue("@FamilyBookPhoto", FBPimgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "FamilyBook.jpeg")) : FBPimgByte);
                                comm.Parameters.AddWithValue("@LastYearCertificate", LYCimgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "LastYearCertificate.jpeg")) : LYCimgByte);
                                comm.ExecuteNonQuery(); ;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
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
        public JsonResult SchoolInfoStepTwo(StudentRegistrationViewModel model)
        {
            try
            {
                var studentSchoolDetailsManager = factory.CreateExternalStudentSchoolDetailsManager();
                var result = studentSchoolDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (result != null)
                {
                    result.EductionalYear = model.SchoolDetail.EductionalYear;
                    result.CountryID = model.SchoolDetail.CountryID;
                    result.CompanyID = model.SchoolDetail.CompanyID;
                    result.SchoolID = model.SchoolDetail.SchoolID;
                    result.ClassID = model.SchoolDetail.ClassID;
                    result.LastSchoolName = model.SchoolDetail.LastSchoolName;
                    studentSchoolDetailsManager.Update(result);
                }
                else
                {
                    ExternalStudentSchoolDetail obj = new ExternalStudentSchoolDetail();
                    obj.EductionalYear = model.SchoolDetail.EductionalYear;
                    obj.CountryID = model.SchoolDetail.CountryID;
                    obj.CompanyID = model.SchoolDetail.CompanyID;
                    obj.SchoolID = model.SchoolDetail.SchoolID;
                    obj.ClassID = model.SchoolDetail.ClassID;
                    obj.LastSchoolName = model.SchoolDetail.LastSchoolName;
                    obj.StudentID = model.StudentIDNumber;
                    studentSchoolDetailsManager.Add(obj);
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Hamza01
        [HttpPost]
        public JsonResult ParentsInfoStepThree(StudentRegistrationViewModel model)
        {
            try
            {
                int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));

                var ExternalstudentGuardsDetailsManager = factory.CreateExternalStudentGuardDetailsManager();
                var Externalstudentmanager = factory.CreateExternalStudentsManager();
                var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();

                var Externalstudent = Externalstudentmanager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var ExternalstudentGuardsDetails = ExternalstudentGuardsDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var ExternalGuardian = ExternalGuardiansManager.Find(a => a.UserID == UserID).FirstOrDefault();

                ExternalGuardian.MobileNo = model.GuardianMobileNumber;
                ExternalGuardiansManager.Update(ExternalGuardian);

                Externalstudent.GuardianRelationship = model.student.GuardianRelationship;
                Externalstudentmanager.Update(Externalstudent);

                if (ExternalstudentGuardsDetails != null)
                {
                    ExternalstudentGuardsDetails.FatherName = model.FatherName;
                    ExternalstudentGuardsDetails.MotherName = model.MotherName;
                    ExternalstudentGuardsDetails.FatherQualification = model.FatherQualification;
                    ExternalstudentGuardsDetails.MotherQualification = model.MotherQualification;
                    ExternalstudentGuardsDetails.FatherSpecialization = model.FatherSpecialization;
                    ExternalstudentGuardsDetails.MotherSpecialization = model.MotherSpecialization;
                    ExternalstudentGuardsDetails.FatherJob = model.FatherJob;
                    ExternalstudentGuardsDetails.MotherJob = model.MotherJob;
                    ExternalstudentGuardsDetails.FatherWorkPhone = model.FatherWorkPhone;
                    ExternalstudentGuardsDetails.MotherWorkPhone = model.MotherWorkPhone;
                    ExternalstudentGuardsDetails.FatherEmail = model.FatherEmail;
                    ExternalstudentGuardsDetails.MotherEmail = model.MotherEmail;
                    ExternalstudentGuardsDetails.MailBox = model.mailBox;
                    ExternalstudentGuardsDetails.PostalCode = model.PostalCode;
                    ExternalstudentGuardsDetails.SmsNumber = model.smsNumber;
                    ExternalstudentGuardsDetails.FatherMobile = model.FatherMobile;
                    ExternalstudentGuardsDetails.MotherMobile = model.MotherMobile;
                    ExternalstudentGuardsDetailsManager.Update(ExternalstudentGuardsDetails);
                }
                else
                {
                    ExternalStudentGuardDetail obj = new ExternalStudentGuardDetail();
                    obj.FatherName = model.FatherName;
                    obj.MotherName = model.MotherName;
                    obj.FatherQualification = model.FatherQualification;
                    obj.MotherQualification = model.MotherQualification;
                    obj.FatherSpecialization = model.FatherSpecialization;
                    obj.MotherSpecialization = model.MotherSpecialization;
                    obj.FatherJob = model.FatherJob;
                    obj.MotherJob = model.MotherJob;
                    obj.FatherWorkPhone = model.FatherWorkPhone;
                    obj.MotherWorkPhone = model.MotherWorkPhone;
                    obj.FatherEmail = model.FatherEmail;
                    obj.MotherEmail = model.MotherEmail;
                    obj.MailBox = model.mailBox;
                    obj.PostalCode = model.PostalCode;
                    obj.SmsNumber = model.smsNumber;
                    obj.StudentID = model.StudentIDNumber;
                    obj.GuardianID = UserID;
                    obj.FatherMobile = model.FatherMobile;
                    obj.MotherMobile = model.MotherMobile;
                    ExternalstudentGuardsDetailsManager.Add(obj);
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //Hamza01
        [HttpPost]
        public JsonResult AddressInfoStepFour(StudentRegistrationViewModel model)
        {
            try
            {
                string query = "";
                int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
                bool existedStdAddress = false;
                var ExternalAddressManager = factory.CreateExternalStudentAdresssManager();
                var ExternalstudentManger = factory.CreateExternalStudentsManager();
                //var result = ExternalAddressManager.Find(a => a.GuardianID == UserID).FirstOrDefault(); 
                //if(result != null)
                //{
                //    result.CountryID = model.StudentAddress.CountryID;
                //    result.City = model.StudentAddress.City;
                //    result.Building = model.StudentAddress.Building;
                //    result.Email = "";
                //    result.MobileNumber = "";
                //    result.Street = model.StudentAddress.Street;
                //    result.Flat = model.StudentAddress.Flat;
                //    result.Longitude = model.StudentAddress.Longitude;
                //    result.Latitude = model.StudentAddress.Latitude;
                //    ExternalAddressManager.Update(result);

                //    query = "UPDATE ExternalStudentAdresses SET TransportDirectionID = @TransportDirectionID WHERE AddressID = @AddressID;";
                //    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                //    {
                //        conn.Open();
                //        using (SqlCommand comm = new SqlCommand(query, conn))
                //        {
                //            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                //            comm.Parameters.AddWithValue("@AddressID", result.AddressID);
                //            comm.ExecuteNonQuery();
                //        }
                //    }

                //    // I used the ADO.NET here becuase the AddressID is not shown in the .edmx file.
                //    query = "UPDATE ExternalStudent SET TransportDirectionID = @TransportDirectionID, AddressID = @AddressID WHERE StudentID = @StudentID;";
                //    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                //    {
                //        conn.Open();
                //        using (SqlCommand comm = new SqlCommand(query, conn))
                //        {
                //            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                //            comm.Parameters.AddWithValue("@AddressID", result.AddressID);
                //            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                //            comm.ExecuteScalar();
                //        }
                //    }
                //}

                query = "SELECT AddressID FROM ExternalStudentAdresses WHERE StudentID = @StudentID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                existedStdAddress = true;
                            }
                        }
                    }
                }

                if (existedStdAddress)
                {
                    query = "Update ExternalStudentAdresses SET CountryID = @CountryID, City = @City, Street = @Street, Building = @Building, Flat = @Flat, " +
                        "MobileNumber = @MobileNumber, Email = @Email, Longitude = @Longitude, Latitude = @Latitude, TransportDirectionID = @TransportDirectionID " +
                        "WHERE StudentID = @StudentID AND GuardianID = @GuardianID;";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                            comm.Parameters.AddWithValue("@GuardianID", UserID);
                            comm.Parameters.AddWithValue("@CountryID", model.StudentAddress.CountryID);
                            comm.Parameters.AddWithValue("@City", model.StudentAddress.City);
                            comm.Parameters.AddWithValue("@Street", model.StudentAddress.Street);
                            comm.Parameters.AddWithValue("@Building", model.StudentAddress.Building);
                            comm.Parameters.AddWithValue("@Flat", -1);
                            comm.Parameters.AddWithValue("@MobileNumber", "");
                            comm.Parameters.AddWithValue("@Email", "");
                            comm.Parameters.AddWithValue("@Longitude", model.StudentAddress.Longitude);
                            comm.Parameters.AddWithValue("@Latitude", model.StudentAddress.Latitude);
                            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                            comm.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    int insertedAddressID = 0;
                    query = "INSERT INTO ExternalStudentAdresses (StudentID, GuardianID, CountryID, City, Street, Building, Flat, " +
                        "MobileNumber, Email, Longitude, Latitude, TransportDirectionID) " +
                        "VALUES (@StudentID, @GuardianID, @CountryID, @City, @Street, @Building, @Flat, @MobileNumber, @Email, @Longitude, @Latitude, @TransportDirectionID); " +
                        "SELECT SCOPE_IDENTITY() AS result";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                            comm.Parameters.AddWithValue("@GuardianID", UserID);
                            comm.Parameters.AddWithValue("@CountryID", model.StudentAddress.CountryID);
                            comm.Parameters.AddWithValue("@City", model.StudentAddress.City);
                            comm.Parameters.AddWithValue("@Street", model.StudentAddress.Street);
                            comm.Parameters.AddWithValue("@Building", model.StudentAddress.Building);
                            comm.Parameters.AddWithValue("@Flat", -1);
                            comm.Parameters.AddWithValue("@MobileNumber", "");
                            comm.Parameters.AddWithValue("@Email", "");
                            comm.Parameters.AddWithValue("@Longitude", model.StudentAddress.Longitude);
                            comm.Parameters.AddWithValue("@Latitude", model.StudentAddress.Latitude);
                            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                            insertedAddressID = Convert.ToInt32(comm.ExecuteScalar());
                        }
                    }

                    // I used the ADO.NET here becuase the AddressID is not shown in the .edmx file.
                    query = "UPDATE ExternalStudent SET TransportDirectionID = @TransportDirectionID, AddressID = @AddressID WHERE StudentID = @StudentID;";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                            comm.Parameters.AddWithValue("@AddressID", insertedAddressID);
                            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                            comm.ExecuteScalar();
                        }
                    }
                }
                var StudentSchoolManager = factory.CreateExternalStudentSchoolDetailsManager();
                int SchoolID = StudentSchoolManager.Find(a => a.StudentID == model.StudentIDNumber).Select(a => a.SchoolID.Value).FirstOrDefault();
                return Json(new { Success = true, SchoolID = SchoolID, StudentID = model.StudentIDNumber }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetDiscountsStep(int SchoolID, int StudentID)
        {
            DiscountsModel model = new DiscountsModel();
            var DiscountsManager = factory.CreateDiscountsManager();
            var StudentDiscountManager = factory.CreateDiscountStudentsManager();
            model.SchoolDiscounts = DiscountsManager.Find(a => a.SchoolID == SchoolID).ToList();
            model.StudentDiscounts = StudentDiscountManager.Find(a => a.StudentID == StudentID).ToList();
            model.StudentID = StudentID;
            model.SchoolID = SchoolID;
            return PartialView("_DiscountsStep", model);
        }

        [HttpPost]
        public JsonResult AddUpdateStudentDiscounts(List<string> CheckedDiscounts, int StudentID)
        {
            var StudentDiscountManager = factory.CreateDiscountStudentsManager();
            if (CheckedDiscounts != null)
            {
                foreach (var item in CheckedDiscounts)
                {
                    bool Isyes = false;
                    var Discount = item.Split('$');
                    string key = Discount[0];
                    int DiscountID = Convert.ToInt32(Discount[1]);
                    if (key == "yes")
                        Isyes = true;
                    else
                        Isyes = false;
                    var studentDiscount = StudentDiscountManager.Find(a => a.StudentID == StudentID && a.DiscountID == DiscountID).FirstOrDefault();
                    if (studentDiscount != null)
                    {
                        studentDiscount.IsYes = Isyes;
                        StudentDiscountManager.Update(studentDiscount);
                    }
                    else
                    {
                        DiscountStudent obj = new DiscountStudent();
                        obj.DiscountID = DiscountID;
                        obj.StudentID = StudentID;
                        obj.IsYes = Isyes;
                        StudentDiscountManager.Add(obj);
                    }
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SocialInfoStepSex(StudentRegistrationViewModel model)
        {
            try
            {
                int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
                var OtherStudentDetailsManager = factory.CreateExternalOtherStudentDetailsManager();
                var result = OtherStudentDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (result != null)
                {
                    result.Behavior = -1;
                    result.BehaviorNotes = "";
                    result.LivesWith = model.OtherStudentDetail.LivesWith;
                    result.NumberofBrothers = model.OtherStudentDetail.NumberofBrothers;
                    result.NumberofSisters = model.OtherStudentDetail.NumberofSisters;
                    result.FamilyOrder = model.OtherStudentDetail.FamilyOrder;
                    result.FamilyTotalMonthlyIncome = model.OtherStudentDetail.FamilyTotalMonthlyIncome;
                    result.SpecialResidenceConditions = model.OtherStudentDetail.SpecialResidenceConditions;
                    OtherStudentDetailsManager.Update(result);
                }
                else
                {
                    ExternalOtherStudentDetail obj = new ExternalOtherStudentDetail();
                    obj.Behavior = -1;
                    obj.BehaviorNotes = "";
                    obj.LivesWith = model.OtherStudentDetail.LivesWith;
                    obj.NumberofBrothers = model.OtherStudentDetail.NumberofBrothers;
                    obj.NumberofSisters = model.OtherStudentDetail.NumberofSisters;
                    obj.FamilyOrder = model.OtherStudentDetail.FamilyOrder;
                    obj.FamilyTotalMonthlyIncome = model.OtherStudentDetail.FamilyTotalMonthlyIncome;
                    obj.SpecialResidenceConditions = model.OtherStudentDetail.SpecialResidenceConditions;
                    obj.StudentID = model.StudentIDNumber;
                    obj.GuardianID = UserID;
                    OtherStudentDetailsManager.Add(obj);
                }
                //No add only update new values because we had add record in Academic step :)
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult HealthInfoStepSeven(StudentRegistrationViewModel model)
        {
            try
            {
                int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
                var OtherStudentDetailsManager = factory.CreateExternalOtherStudentDetailsManager();
                var StudentDiseasManager = factory.CreateStudentDiseassManager();
                var result = OtherStudentDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var StudentDiseasResult = StudentDiseasManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (result != null)
                {
                    result.BloodType = model.OtherStudentDetail.BloodType;
                    result.PhysicalStatus = model.OtherStudentDetail.PhysicalStatus;
                    result.SpecialCare = model.OtherStudentDetail.SpecialCare;
                    result.HasChronicDisease = model.OtherStudentDetail.HasChronicDisease;
                    result.DiseaseType = model.OtherStudentDetail.DiseaseType;
                    OtherStudentDetailsManager.Update(result);
                }
                if (StudentDiseasResult != null)
                {
                    StudentDiseasResult.Mumps = model.StudentDiseas.Mumps;
                    StudentDiseasResult.Chickenpox = model.StudentDiseas.Chickenpox;
                    StudentDiseasResult.rubella = model.StudentDiseas.rubella;
                    StudentDiseasResult.Rheumaticfever = model.StudentDiseas.Rheumaticfever;
                    StudentDiseasResult.Diabetes = model.StudentDiseas.Diabetes;
                    StudentDiseasResult.Heartdiseases = model.StudentDiseas.Heartdiseases;
                    StudentDiseasResult.Pissingoff = model.StudentDiseas.Pissingoff;
                    StudentDiseasResult.Jointbonediseases = model.StudentDiseas.Jointbonediseases;
                    StudentDiseasResult.sprayer = model.StudentDiseas.sprayer;
                    StudentDiseasResult.Hearingimpairment = model.StudentDiseas.Visualimpairment;
                    StudentDiseasResult.Speechimpairment = model.StudentDiseas.Bladderdiseases;
                    StudentDiseasResult.Epilepsy = model.StudentDiseas.Epilepsy;
                    StudentDiseasResult.Hepatitis = model.StudentDiseas.Hepatitis;
                    StudentDiseasResult.Shakika = model.StudentDiseas.Shakika;
                    StudentDiseasResult.Fainting = model.StudentDiseas.Fainting;
                    StudentDiseasResult.Kidneydisease = model.StudentDiseas.Kidneydisease;
                    StudentDiseasResult.Surgery = model.StudentDiseas.Surgery;
                    StudentDiseasResult.Urinarysystemdiseases = model.StudentDiseas.Urinarysystemdiseases;
                    StudentDiseasResult.Visualimpairment = model.StudentDiseas.Visualimpairment;
                    StudentDiseasResult.Bladderdiseases = model.StudentDiseas.Bladderdiseases;
                    StudentDiseasManager.Update(StudentDiseasResult);


                }
                else
                {
                    StudentDiseas obj = new StudentDiseas();
                    obj.Mumps = model.StudentDiseas.Mumps;
                    obj.Chickenpox = model.StudentDiseas.Chickenpox;
                    obj.rubella = model.StudentDiseas.rubella;
                    obj.Rheumaticfever = model.StudentDiseas.Rheumaticfever;
                    obj.Diabetes = model.StudentDiseas.Diabetes;
                    obj.Heartdiseases = model.StudentDiseas.Heartdiseases;
                    obj.Pissingoff = model.StudentDiseas.Pissingoff;
                    obj.Jointbonediseases = model.StudentDiseas.Jointbonediseases;
                    obj.sprayer = model.StudentDiseas.sprayer;
                    obj.Hearingimpairment = model.StudentDiseas.Hearingimpairment;
                    obj.Speechimpairment = model.StudentDiseas.Speechimpairment;
                    obj.Epilepsy = model.StudentDiseas.Epilepsy;
                    obj.Hepatitis = model.StudentDiseas.Hepatitis;
                    obj.Shakika = model.StudentDiseas.Shakika;
                    obj.Fainting = model.StudentDiseas.Fainting;
                    obj.Kidneydisease = model.StudentDiseas.Kidneydisease;
                    obj.Surgery = model.StudentDiseas.Surgery;
                    obj.Urinarysystemdiseases = model.StudentDiseas.Urinarysystemdiseases;
                    obj.Visualimpairment = model.StudentDiseas.Visualimpairment;
                    obj.Bladderdiseases = model.StudentDiseas.Bladderdiseases;
                    obj.StudentID = model.StudentIDNumber;
                    StudentDiseasManager.Add(obj);
                    var StudentRequestManager = factory.CreateExternalGuardStudentsRequestsManager();
                    var request = StudentRequestManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                    request.RequestStatus = (int)clsenumration.RequestStatus.pending;
                    StudentRequestManager.Update(request);
                }

                //No add only update new values because we had add record in Academic step :)
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public JsonResult GetCompaniesByCountryID(int CountryID)
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
            var result = GetHeadQuartersByCountryID(CountryID);
            return Json(new { Success = true, result = result, lang = lang }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSchoolBranchsByCompanyID(int CompanyID)
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
            var result = GetSchoolsByCompanyID(CompanyID);
            return Json(new { Success = true, result = result, lang = lang }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassesBySchoolID(int SchoolID)
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
            var result = GetSchoolClassesBySchoolID(SchoolID);
            return Json(new { Success = true, result = result, lang = lang }, JsonRequestBehavior.AllowGet);
        }
    }
}
