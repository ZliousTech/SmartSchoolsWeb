using Business.Base;
using Common;
using Common.Base;
using Common.Helpers;
using DataAccess;
using Newtonsoft.Json;
using Objects;
using Objects.DTO;
using SmartSchool.Models.Registration;
using SmartSchool.Models.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    public class StudentController : StudentBaseController
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: Student
        public ActionResult Index()
        {
            int __SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            StudentModel model = new StudentModel();
            var Requests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                            join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                            on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                            //where ExternalStudentSchoolDetail.SchoolID == SchoolID
                            where ExternalStudentSchoolDetail.SchoolID == __SchoolID
                            select ExternalGuardStudentsRequest).ToList();

            if (Requests != null)
            {
                model.PendingRequests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                                         join ExternalGuardian in context.ExternalGuardians
                                         on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                                         join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                                         on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                                         join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                                         SchoolClass.SchoolClassID
                                         join Class in context.Classes on SchoolClass.ClassID
                                         equals Class.ClassID
                                         where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.pending
                                         && ExternalStudentSchoolDetail.SchoolID == SchoolID
                                         select ExternalGuardStudentsRequest).Count();
            }
            else
            {
                model.PendingRequests = 0;
            }

            return View(model);
        }

        //AbuHammad 001
        public ActionResult StudentInternalReg()
        {
            StudentInternalRegModel model = new StudentInternalRegModel();
            var SchoolManager = factory.CreateSchoolBranchsManager();
            var SchoolBranchInf = SchoolManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();

            model.SchoolLat = SchoolBranchInf.Latitude.ToString();
            model.SchoolLng = SchoolBranchInf.Longitude.ToString();

            PrepareModelLookups(model);

            return View(model);
        }

        public string GuardianExistance(string GrdnID)
        {
            //StudentInternalRegModel model = new StudentInternalRegModel();

            if (GrdnID.Trim().Length <= 0)
            {
                return "";
            }

            var GuardianManager = factory.CreateGuardiansManager();
            var GrdnStdAddressManager = factory.CreateStudentAdresssManager();
            var GaurdainInfo = GuardianManager.Find(a => a.NationalNumber == GrdnID).FirstOrDefault();

            if (GaurdainInfo == null)
            {
                GaurdainInfo = GuardianManager.Find(a => a.ResidencyNumber == GrdnID).FirstOrDefault();

                if (GaurdainInfo == null)
                {
                    GaurdainInfo = GuardianManager.Find(a => a.PassportNumber == GrdnID).FirstOrDefault();
                    if (GaurdainInfo == null)
                    {
                        return "";
                    }
                }
            }

            var GrdnStdAddressInfo = GrdnStdAddressManager.Find(a => a.GuardianID == GaurdainInfo.GuardianID).FirstOrDefault();

            //Guardian Information -----------------------------
            string[] FullArabicName_Grdn;
            string G_ArabicName1 = "";
            string G_ArabicName2 = "";
            string G_ArabicName3 = "";
            string G_ArabicName4 = "";
            string[] FullEnglishName_Grdn;
            string G_EnglishName1 = "";
            string G_EnglishName2 = "";
            string G_EnglishName3 = "";
            string G_EnglishName4 = "";
            int G_Gender_M = 0;
            int G_Gender_F = 1;
            string DataRes = "";

            //model.NationalNumber_Grdn = GaurdainInfo.NationalNumber;
            //model.ResidencyNumber_Grdn = GaurdainInfo.ResidencyNumber;
            //model.PassportNo_Grdn = GaurdainInfo.PassportNumber;
            //model.MobNo_Grdn = GaurdainInfo.MobileNumber;
            FullArabicName_Grdn = GaurdainInfo.GuardianArabicName.Split(' ');
            FullEnglishName_Grdn = GaurdainInfo.GuardianEnglishName.Split(' ');
            if (FullArabicName_Grdn.Length > 3)
            {
                //model.FirstArabicName_Grdn = FullArabicName_Grdn[0];
                //model.SecondArabicName_Grdn = FullArabicName_Grdn[1];
                //model.ThirdArabicName_Grdn = FullArabicName_Grdn[2];
                //model.FourthArabicName_Grdn = FullArabicName_Grdn[3];
                G_ArabicName1 = FullArabicName_Grdn[0].Replace('-', ' ');
                G_ArabicName2 = FullArabicName_Grdn[1].Replace('-', ' ');
                G_ArabicName3 = FullArabicName_Grdn[2].Replace('-', ' ');
                G_ArabicName4 = FullArabicName_Grdn[3].Replace('-', ' ');
            }
            else
            {
                if (FullArabicName_Grdn.Length == 2)
                {
                    //model.FirstArabicName_Grdn = FullArabicName_Grdn[0];
                    //model.SecondArabicName_Grdn = "";
                    //model.ThirdArabicName_Grdn = "";
                    //model.FourthArabicName_Grdn = FullArabicName_Grdn[1];
                    G_ArabicName1 = FullArabicName_Grdn[0].Replace('-', ' ');
                    G_ArabicName2 = "";
                    G_ArabicName3 = "";
                    G_ArabicName4 = FullArabicName_Grdn[1].Replace('-', ' ');
                }
                else
                {
                    //model.FirstArabicName_Grdn = FullArabicName_Grdn[0];
                    //model.SecondArabicName_Grdn = FullArabicName_Grdn[1];
                    //model.ThirdArabicName_Grdn = "";
                    //model.FourthArabicName_Grdn = FullArabicName_Grdn[2];
                    G_ArabicName1 = FullArabicName_Grdn[0].Replace('-', ' ');
                    G_ArabicName2 = FullArabicName_Grdn[1].Replace('-', ' ');
                    G_ArabicName3 = "";
                    G_ArabicName4 = FullArabicName_Grdn[2].Replace('-', ' ');
                }
            }

            if (FullEnglishName_Grdn.Length > 3)
            {
                //model.FirstEnglishName_Grdn = FullEnglishName_Grdn[0];
                //model.SecondEnglishName_Grdn = FullEnglishName_Grdn[1];
                //model.ThirdEnglishName_Grdn = FullEnglishName_Grdn[2];
                //model.FourthEnglishName_Grdn = FullEnglishName_Grdn[3];
                G_EnglishName1 = FullEnglishName_Grdn[0].Replace('-', ' ');
                G_EnglishName2 = FullEnglishName_Grdn[1].Replace('-', ' ');
                G_EnglishName3 = FullEnglishName_Grdn[2].Replace('-', ' ');
                G_EnglishName4 = FullEnglishName_Grdn[3].Replace('-', ' ');
            }
            else
            {
                if (FullEnglishName_Grdn.Length == 2)
                {
                    //model.FirstEnglishName_Grdn = FullEnglishName_Grdn[0];
                    //model.SecondEnglishName_Grdn = "";
                    //model.ThirdEnglishName_Grdn = "";
                    //model.FourthEnglishName_Grdn = FullEnglishName_Grdn[1];
                    G_EnglishName1 = FullEnglishName_Grdn[0].Replace('-', ' ');
                    G_EnglishName2 = "";
                    G_EnglishName3 = "";
                    G_EnglishName4 = FullEnglishName_Grdn[1].Replace('-', ' ');
                }
                else
                {
                    //model.FirstEnglishName_Grdn = FullEnglishName_Grdn[0];
                    //model.SecondEnglishName_Grdn = FullEnglishName_Grdn[1];
                    //model.ThirdEnglishName_Grdn = "";
                    //model.FourthEnglishName_Grdn = FullEnglishName_Grdn[2];
                    G_EnglishName1 = FullEnglishName_Grdn[0].Replace('-', ' ');
                    G_EnglishName2 = FullEnglishName_Grdn[1].Replace('-', ' ');
                    G_EnglishName3 = "";
                    G_EnglishName4 = FullEnglishName_Grdn[2].Replace('-', ' ');
                }
            }

            if (GaurdainInfo.Gender == 0)
            {
                //model.Gender_Grdn_M = 0; model.Gender_Grdn_F = 1; //0 = Checked.
                G_Gender_M = 0; G_Gender_F = 1;
            }
            else
            {
                //model.Gender_Grdn_M = 1; model.Gender_Grdn_F = 0;
                G_Gender_M = 1; G_Gender_F = 0;
            }
            //----------------------------------------------

            //Guardian Address -----------------------------
            //model.City_Grdn = GrdnStdAddressInfo.City;
            //model.Street_Grdn = GrdnStdAddressInfo.Street;
            //model.Building_Grdn = GrdnStdAddressInfo.Building.ToString();
            //model.Floor_Grdn = GrdnStdAddressInfo.Flat.ToString();
            //model.Latitude_Grdn = GrdnStdAddressInfo.Latitude.ToString();
            //model.Longitude_Grdn = GrdnStdAddressInfo.Longitude.ToString();
            //model.EMail_Grdn = GrdnStdAddressInfo.Email;
            //----------------------------------------------

            //Lookups Vals ---------------------------------
            //model.Nationality_Grdn = GaurdainInfo.Nationality;
            //model.Religion_Grdn = (GaurdainInfo.Religion + 1).ToString();
            //model.PaymentMethod_Grdn = (GaurdainInfo.PaymentMethod + 1).ToString();
            //model.Country_Grdn = GrdnStdAddressInfo.CountryID.ToString();
            //----------------------------------------------

            DataRes = GaurdainInfo.NationalNumber + "|" + GaurdainInfo.ResidencyNumber + "|" + GaurdainInfo.PassportNumber + "|"
                    + GaurdainInfo.MobileNumber + "|" + G_ArabicName1 + "|" + G_ArabicName2 + "|" + G_ArabicName3 + "|" + G_ArabicName4
                    + "|" + G_EnglishName1 + "|" + G_EnglishName2 + "|" + G_EnglishName3 + "|" + G_EnglishName4
                    + "|" + G_Gender_M + "|" + G_Gender_F + "|" + GrdnStdAddressInfo.CountryID.ToString() + "|" + GrdnStdAddressInfo.City
                    + "|" + GrdnStdAddressInfo.Street + "|" + GrdnStdAddressInfo.Building.ToString()
                    + "|" + GrdnStdAddressInfo.Flat.ToString() + "|" + GrdnStdAddressInfo.Latitude.ToString() + "|" + GrdnStdAddressInfo.Longitude.ToString()
                    + "|" + GrdnStdAddressInfo.Email + "|" + GaurdainInfo.Nationality + "|" + (GaurdainInfo.Religion + 1).ToString()
                    + "|" + (GaurdainInfo.PaymentMethod + 1).ToString() + "|" + GaurdainInfo.GuardianID.ToString();

            return DataRes;
        }

        public string AddNewGuardian()
        {
            return "";
        }

        public void PrepareModelLookups(StudentInternalRegModel model)
        {
            var CountriesManager = factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishNationality.ToString(),
                                     DescriptionAR = country.ArabicNationality.ToString(),
                                     ID = country.ID
                                 }).ToList();
            model.NationalityList = CountriesList;

            var _CountriesList = (from country in Countries
                                  select new LookupDTO
                                  {
                                      Description = country.EnglishName.ToString(),
                                      DescriptionAR = country.ArabicName.ToString(),
                                      ID = country.ID
                                  }).ToList();
            model.Countries = _CountriesList;

            var ReligonsManager = factory.CreateReligionsManager();
            var Religons = ReligonsManager.GetAll();
            var ReligonsList = (from religon in Religons
                                select new LookupDTO
                                {
                                    Description = religon.ReligionEnglishName.ToString(),
                                    DescriptionAR = religon.ReligionArabicName.ToString(),
                                    ID = religon.ReligionID
                                }).ToList();
            model.ReligionList = ReligonsList;

            var PaymentMethodsManager = factory.CreatePaymentMethodsManager();
            var PaymentMethods = PaymentMethodsManager.GetAll();
            var PaymentMethodList = (from payment in PaymentMethods
                                     select new LookupDTO
                                     {
                                         Description = payment.PaymentMethodEnglishText.ToString(),
                                         DescriptionAR = payment.PaymentMethodArabicText.ToString(),
                                         ID = payment.PaymentMethodID
                                     }).ToList();
            model.PaymentMethodList = PaymentMethodList;

            //var _CountriesManager = factory.CreateCountrysManager();
            //var _Countries = _CountriesManager.GetAll();
            //var _CountriesList = (from country in _Countries
            //                      select new LookupDTO
            //                     {
            //                         Description = country.EnglishName.ToString(),
            //                         DescriptionAR = country.ArabicName.ToString(),
            //                         ID = (country.ID)
            //                     }).ToList();
            //model.Countries = _CountriesList;

        }

        //Hamza01
        public ActionResult SchoolStudents(StudentModel model)
        {
            return PrepareSchoolStudents(model);
        }


        [HttpPost]
        public JsonResult GetSections(int SchoolClassID)
        {
            var Sections = GetSectionsBySchoolClassID(SchoolClassID);
            return Json(Sections, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RegisterStudent()
        {
            EditStudentViewModel model = new EditStudentViewModel();
            PrepareStudentModel(model);
            return View(model);
        }

        [HttpGet]
        public JsonResult GetSectionsBySchoolClsID(int SchoolClassID)
        {
            var Sections = (from s in context.Sections
                            join c in context.SchoolClasses
                            on s.SchoolClassID equals c.SchoolClassID
                            where c.SchoolClassID == SchoolClassID
                            select new
                            {
                                sectionCode = s.SectionCode,
                                sectionNameEn = s.SectionEnglishName.ToString(),
                                sectionNameAr = s.SectionArabicName.ToString(),
                                sectionID = s.SectionID
                            }).ToList();
            return this.Json(JsonConvert.SerializeObject(Sections), JsonRequestBehavior.AllowGet);
        }

        //Hamza01
        [Authorize]
        public ActionResult EditStudent(string StudentID)
        {
            EditStudentViewModel model = new EditStudentViewModel();
            var GuardiansManager = factory.CreateGuardiansManager();
            var StudentsManager = factory.CreateStudentsManager();
            var StudentsOtherDetailsManager = factory.CreateOtherStudentDetailsManager();
            var StudentAddressManager = factory.CreateStudentAdresssManager();
            var StudentHealthManager = factory.CreateStudentHealthsManager();
            var StudentGuardDetailsManager = factory.CreateStudentGuardDetailsManager();
            var SystemSettingManager = factory.CreateSystemSettingsManager();
            var StudentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
            var student = StudentsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var BusToursManager = factory.CreateBusToursManager();
            model.ComingTourList = GetBusTours(SchoolID, 0);
            model.GoingTourList = GetBusTours(SchoolID, 1);
            string query = "";

            var OtherStudentDetails = new OtherStudentDetail();
            OtherStudentDetails = StudentsOtherDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            if (OtherStudentDetails == null)
            {
                OtherStudentDetails = StudentsOtherDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();

            }
            //var studentAddress = StudentAddressManager.Find(a => a.GuardianID == student.GuardianID).FirstOrDefault();
            StudentAdress studentAddress = null;
            query = "SELECT * FROM StudentAdresses WHERE StudentID = @StudentID AND GuardianID = @GuardianID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StudentID", StudentID);
                    comm.Parameters.AddWithValue("@GuardianID", student.GuardianID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            studentAddress = new StudentAdress();
                            studentAddress.AddressID = Convert.ToInt32(reader["AddressID"].ToString());
                            studentAddress.StudentID = reader["StudentID"].ToString();
                            studentAddress.GuardianID = Convert.ToInt32(reader["GuardianID"].ToString());
                            studentAddress.CountryID = Convert.ToInt32(reader["CountryID"].ToString());
                            studentAddress.City = reader["City"].ToString();
                            studentAddress.Street = reader["Street"].ToString();
                            studentAddress.Building = Convert.ToInt32(reader["Building"].ToString());
                            studentAddress.Flat = Convert.ToInt32(reader["Flat"].ToString());
                            studentAddress.MobileNumber = reader["MobileNumber"].ToString();
                            studentAddress.Email = reader["Email"].ToString();
                            studentAddress.Longitude = Convert.ToDouble(reader["Longitude"].ToString());
                            studentAddress.Latitude = Convert.ToDouble(reader["Latitude"].ToString());
                            studentAddress.TransportDirectionID = Convert.ToInt32(reader["TransportDirectionID"].ToString());
                        }
                    }

                }
            }
            var StudentHealth = StudentHealthManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            var studentguardetails = new StudentGuardDetail();
            studentguardetails = StudentGuardDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            if (studentguardetails == null)
            {
                studentguardetails = StudentGuardDetailsManager.Find(a => a.GuardianID == student.GuardianID).FirstOrDefault();

            }
            var studentschoolDetails = StudentSchoolDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            string studentname = "";
            studentname = StudentsManager.Find(a => a.StudentID == StudentID).Select(a => a.StudentArabicName).FirstOrDefault();

            var name = studentname.Split(' ');
            try
            {
                model.FirstArabicName = Regex.Replace(name[0], "[-]+", " ", RegexOptions.Compiled);
                model.SecondArabicName = Regex.Replace(name[1], "[-]+", " ", RegexOptions.Compiled);
                model.ThirdArabicName = Regex.Replace(name[2], "[-]+", " ", RegexOptions.Compiled);
                model.FourthArabicName = Regex.Replace(name[3], "[-]+", " ", RegexOptions.Compiled);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            model.BirthPlace = student.BirthPlace;
            model.student = student;
            // reading those three attributes using ADO.NET becaues there is a problem in .edmx file, so they not accessible.
            if (student != null)
            {
                query = "SELECT BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate FROM Student WHERE StudentID = @StudentID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", StudentID);
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
            if (studentAddress == null)
            {
                query = "SELECT TOP(1) * FROM StudentAdresses WHERE GuardianID = @GuardianID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@GuardianID", student.GuardianID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentAddress = new StudentAdress();
                                studentAddress.AddressID = Convert.ToInt32(reader["AddressID"].ToString());
                                studentAddress.StudentID = reader["StudentID"].ToString();
                                studentAddress.GuardianID = Convert.ToInt32(reader["GuardianID"].ToString());
                                studentAddress.CountryID = Convert.ToInt32(reader["CountryID"].ToString());
                                studentAddress.City = reader["City"].ToString();
                                studentAddress.Street = reader["Street"].ToString();
                                studentAddress.Building = Convert.ToInt32(reader["Building"].ToString());
                                studentAddress.Flat = Convert.ToInt32(reader["Flat"].ToString());
                                studentAddress.MobileNumber = reader["MobileNumber"].ToString();
                                studentAddress.Email = reader["Email"].ToString();
                                studentAddress.Longitude = Convert.ToDouble(reader["Longitude"].ToString());
                                studentAddress.Latitude = Convert.ToDouble(reader["Latitude"].ToString());
                                studentAddress.TransportDirectionID = Convert.ToInt32(reader["TransportDirectionID"].ToString());
                            }
                        }
                    }
                }
            }
            model.StudentAddress = studentAddress;
            model.StudentDiseas = StudentHealth;
            model.StudentIDNumber = StudentID;
            model.GurdianID = student.GuardianID.Value;
            model.SchoolDetail = studentschoolDetails;

            var Gurdianresult = GuardiansManager.Find(a => a.GuardianID == student.GuardianID).FirstOrDefault();
            model.GuardianName = CurrentLanguage == Languges.English ? Gurdianresult.GuardianEnglishName : Gurdianresult.GuardianArabicName;
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
                //model.BirthPlace = student.BirthPlace == null ? "" : student.BirthPlace;
            }

            model.GuardianMobileNumber = Gurdianresult.MobileNumber;
            PrepareStudentModel(model);
            return View(model);
        }

        private void PrepareStudentModel(EditStudentViewModel model)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var SectionsManager = factory.CreateSectionsManager();
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
            model.Sections = (from s in context.Sections
                              join c in context.SchoolClasses
                              on s.SchoolClassID equals c.SchoolClassID
                              where s.SchoolID == SchoolID && c.SchoolClassID == model.SchoolDetail.ClassID
                              select new LookupDTO
                              {
                                  Description = s.SectionCode.ToString(),
                                  DescriptionAR = s.SectionCode.ToString(),
                                  ID = s.SectionID
                              }).ToList();
            model.StudentStatusList = GetStudentStatus();
            model.StudentResultList = GetStudentResults();
            model.classes = GetSchoolClassesBySchoolID(SchoolID);
        }

        [HttpPost]
        public ActionResult PrepareSchoolStudents(StudentModel model)
        {
            if (model == null)
                model = new StudentModel();

            model.Students = (from a in context.Students
                              join b in context.StudentSchoolDetails on a.StudentID equals b.StudentID
                              where b.SchoolID == SchoolID &&
                                (model.SchoolClassID == 0 || b.ClassID == model.SchoolClassID) &&
                                (model.SectionID == 0 || b.SectionID == model.SectionID)
                              orderby a.StudentID descending
                              select a).ToList();

            model.Classes = GetSchoolClassesBySchoolID(SchoolID);
            model.SchoolClassID = model.SchoolClassID;
            model.SectionID = model.SectionID;
            return View("SchoolStudents", model);
        }

        [HttpPost]
        public ActionResult FilterSchoolStudents(StudentModel model)
        {
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            model.Students = (from a in context.Students
                              join b in context.StudentSchoolDetails on a.StudentID equals b.StudentID
                              orderby a.StudentID descending
                              where b.ClassID == model.SchoolClassID && b.SectionID == model.SectionID &&
                              b.SchoolID == SchoolID
                              select a).ToList();
            model.Classes = GetSchoolClassesBySchoolID(SchoolID);
            model.SchoolClassID = model.SchoolClassID;
            model.SectionID = model.SectionID;
            return View("PrepareSchoolStudents", model);
        }

        public ActionResult RegisterationRequests(bool isPending = true)
        {
            ViewBag.isPending = isPending.ToString();
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
                lang = "en";
            else
                lang = "ar";
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            if (isPending)
            {
                var result = from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                             join ExternalGuardian in context.ExternalGuardians
                             on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                             join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                             on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                             join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                             SchoolClass.SchoolClassID
                             join Class in context.Classes on SchoolClass.ClassID
                             equals Class.ClassID
                             where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.pending
                             && ExternalStudentSchoolDetail.SchoolID == SchoolID
                             select new RegistrationRequestsDTO()
                             {
                                 GudrianMobileNo = ExternalGuardian.MobileNo,
                                 studentID = ExternalGuardStudentsRequest.StudentID,
                                 StudentName = ExternalGuardStudentsRequest.StudentName,
                                 ClassName = lang == "en" ? Class.ClassEnglishName : Class.ClassArabicName
                             };
                var studentData = result.ToList();
                return View(studentData);
            }
            else
            {
                var result = from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                             join ExternalGuardian in context.ExternalGuardians
                             on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                             join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                             on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                             join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                             SchoolClass.SchoolClassID
                             join Class in context.Classes on SchoolClass.ClassID
                             equals Class.ClassID
                             where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.Rejected
                             && ExternalStudentSchoolDetail.SchoolID == SchoolID
                             select new RegistrationRequestsDTO()
                             {
                                 GudrianMobileNo = ExternalGuardian.MobileNo,
                                 studentID = ExternalGuardStudentsRequest.StudentID,
                                 StudentName = ExternalGuardStudentsRequest.StudentName,
                                 ClassName = lang == "en" ? Class.ClassEnglishName : Class.ClassArabicName
                             };
                var studentData = result.ToList();
                return View(studentData);
            }
        }

        //Hamza01
        public ActionResult StudentRegistrationDetails(int StudentID)
        {
            StudentRegistrationViewModel model = new StudentRegistrationViewModel();
            var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();
            var ExternalGuardStudentsRequestsManager = factory.CreateExternalGuardStudentsRequestsManager();
            var ExternalStudentsManager = factory.CreateExternalStudentsManager();
            var ExternalStudentsOtherDetailsManager = factory.CreateExternalOtherStudentDetailsManager();
            var ExternalStudentAddressManager = factory.CreateExternalStudentAdresssManager();
            var StudentDiseasManager = factory.CreateStudentDiseassManager();
            var StudentGuardDetailsManager = factory.CreateExternalStudentGuardDetailsManager();
            var DiscountManager = factory.CreateDiscountsManager();
            var DiscountStudentManager = factory.CreateDiscountStudentsManager();
            var SystemSettingManager = factory.CreateSystemSettingsManager();
            int GuardianID = ExternalGuardStudentsRequestsManager.Find(a => a.StudentID == StudentID).Select(a => a.GuardianID).FirstOrDefault();
            var ExternalStudentSchoolDetailsManager = factory.CreateExternalStudentSchoolDetailsManager();
            var student = ExternalStudentsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            var OtherStudentDetails = new ExternalOtherStudentDetail();
            OtherStudentDetails = ExternalStudentsOtherDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();

            var studentAddress = ExternalStudentAddressManager.Find(a => a.GuardianID == GuardianID).FirstOrDefault();
            var StudentDiseas = StudentDiseasManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            var studentguardetails = new ExternalStudentGuardDetail();
            studentguardetails = StudentGuardDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            if (studentguardetails == null)
            {
                studentguardetails = StudentGuardDetailsManager.Find(a => a.GuardianID == GuardianID).FirstOrDefault();

            }
            var studentresult = ExternalStudentsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            var studentschoolDetails = ExternalStudentSchoolDetailsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();

            model.student = studentresult;
            var SchoolDiscounts = DiscountManager.Find(s => s.SchoolID == SchoolID).ToList();
            var StudentDiscounts = DiscountStudentManager.Find(s => s.StudentID == StudentID).ToList();
            // reading those three attributes using ADO.NET becaues there is a problem in .edmx file, so they not accessible.
            if (student != null)
            {
                string query = "SELECT BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate FROM ExternalStudent " +
                    "WHERE StudentID = @StudentID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StudentID", StudentID);
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
            if (SchoolDiscounts != null && StudentDiscounts != null)
            {
                if (model.DiscountModel == null)
                {
                    model.DiscountModel = new DiscountsModel();
                }
                model.DiscountModel.SchoolDiscounts = SchoolDiscounts;
                model.DiscountModel.StudentDiscounts = StudentDiscounts;
            }
            model.OtherStudentDetail = OtherStudentDetails;
            model.StudentAddress = studentAddress;
            model.StudentDiseas = StudentDiseas;
            model.student = student;
            model.StudentIDNumber = StudentID;
            model.GurdianID = GuardianID;
            model.SchoolDetail = studentschoolDetails;

            var externalGurdianresult = ExternalGuardiansManager.Find(a => a.UserID == GuardianID).FirstOrDefault();
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

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand("SELECT MaxDiscountNumber FROM  SchoolSettings WHERE SchoolID = " + SchoolID, conn))
                {
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.MaxDiscountNumber = reader["MaxDiscountNumber"] != DBNull.Value
                            ? (int?)Convert.ToInt32(reader["MaxDiscountNumber"]) : null;
                        }
                    }
                }
            }

            model.GuardianMobileNumber = externalGurdianresult.MobileNo;
            model.GuardianRelationshipList = GuardianTypesList();
            return View(model);
        }


        // Hamza01
        [HttpPost]
        public JsonResult AcceptStudent(int StudentID, List<string> CheckedDiscounts, decimal SpecialDiscount)
        {
            try
            {
                string query = "";
                int insertedGuardianID = 0, insertedAddressID = 0;

                #region Step 1 adding the Guardian to our system.

                var ExternalRequestsManager = factory.CreateExternalGuardStudentsRequestsManager();
                var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();
                var ExternalRequest = ExternalRequestsManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
                var ExternalGuardian = ExternalGuardiansManager.Find(a => a.UserID == ExternalRequest.GuardianID).FirstOrDefault();

                // check if Guardian is already inside the Guardians Table.
                var GuardiansManager = factory.CreateGuardiansManager();
                var Guardian = GuardiansManager.Find(g => g.NationalNumber == ExternalGuardian.NationalNumber).FirstOrDefault();

                if (Guardian == null)
                {
                    query = "INSERT INTO Guardians (GuardianArabicName, GuardianEnglishName, NationalNumber, Gender, Nationality," +
                        " Religion, ResidencyNumber, PassportNumber, PaymentMethod, CreditCardType, CreditCardNumber, NameonCreditCard," +
                        " CreditCardExpiryDate, CreditCardCode, MobileNumber)" +
                        " VALUES (@Arabicname, @Englishname, @Nationalnumber, @Gender, @Nationality, 0, N'', N'', 0, -1, N'', N'', N'-1A', N'', @Mobilenumber);" +
                        " SELECT SCOPE_IDENTITY() as result";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@Arabicname", ExternalGuardian.ArabicName);
                            comm.Parameters.AddWithValue("@Englishname", ExternalGuardian.EnglishName);
                            comm.Parameters.AddWithValue("@Nationalnumber", ExternalGuardian.NationalNumber);
                            comm.Parameters.AddWithValue("@Gender", ExternalGuardian.Gender);
                            comm.Parameters.AddWithValue("@Nationality", ExternalGuardian.Nationality);
                            comm.Parameters.AddWithValue("@Mobilenumber", ExternalGuardian.MobileNo);
                            insertedGuardianID = int.Parse(comm.ExecuteScalar().ToString());
                        }
                    }
                }
                else
                    insertedGuardianID = Guardian.GuardianID;
                #endregion

                #region Insert Guardian SchoolID.
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    int GuardianSchoolID = 0;

                    conn.Open();
                    query = "SELECT ID FROM GuardianSchool WHERE GuardianID = @GuardianID AND SchoolID = @SchoolID";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@GuardianID", insertedGuardianID);
                        comm.Parameters.AddWithValue("@SchoolID", this.SchoolID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                GuardianSchoolID = int.Parse(reader["ID"].ToString());
                            }
                        }
                    }

                    if (GuardianSchoolID == 0)
                    {
                        query = "INSERT INTO GuardianSchool (GuardianID, SchoolID) VALUES (@GuardianID, @SchoolID)";
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@GuardianID", insertedGuardianID);
                            comm.Parameters.AddWithValue("@SchoolID", this.SchoolID);
                            comm.ExecuteNonQuery();
                        }
                    }
                    conn.Close();
                    conn.Dispose();
                }
                #endregion

                #region adding the Guardian to the WebUsers Table.
                var WebUsersManager = factory.CreateWebUsersManager();
                var WebUsers = WebUsersManager.Find(w => w.UserName == ExternalGuardian.NationalNumber).FirstOrDefault();
                if (WebUsers == null)
                {
                    WebUser obj = new WebUser();
                    obj.ApplicationID = "";
                    obj.Locked = 0;
                    obj.Password = ExternalGuardian.Password;
                    obj.SchoolID = this.SchoolID;
                    obj.UserName = ExternalGuardian.UserName;
                    obj.UserSystemID = insertedGuardianID.ToString();
                    obj.UserType = 0;
                    WebUsersManager.Add(obj);
                }
                #endregion

                #region Step 2 adding the Student to our system.

                var ExternalStudentsManager = factory.CreateExternalStudentsManager();
                var ExternalStudent = ExternalStudentsManager.Find(s => s.StudentID == StudentID).FirstOrDefault();
                int? SchoolID = factory.CreateExternalStudentSchoolDetailsManager().Find(s => s.StudentID == StudentID).FirstOrDefault().SchoolID;
                string generatedStudentID = GenerateStudentID();

                var StudentManager = factory.CreateStudentsManager();
                Student stdobj = new Student();
                stdobj.StudentID = generatedStudentID;
                stdobj.StudentArabicName = ExternalStudent.StudentArabicName;
                stdobj.StudentEnglishName = ExternalStudent.StudentEnglishName;
                stdobj.NationalNumber = ExternalStudent.NationalNumber;
                stdobj.Gender = ExternalStudent.Gender;
                stdobj.Nationality = ExternalStudent.Nationality;
                stdobj.DateofBirth = ExternalStudent.DateofBirth;
                stdobj.GuardianID = insertedGuardianID; // the variable that is comming from step one.
                stdobj.AccountNumber = ExternalStudent.AccountNumber;
                stdobj.FamilyID = ExternalStudent.FamilyID;
                stdobj.MotherID = ExternalStudent.MotherID;
                stdobj.ResidencyNumber = ExternalStudent.ResidencyNumber;
                stdobj.PassportNumber = ExternalStudent.PassportNumber;
                stdobj.Active = ExternalStudent.Active;
                stdobj.GuardianRelationship = ExternalStudent.GuardianRelationship;
                stdobj.Photo = ExternalStudent.Photo;
                stdobj.Photo_A = ExternalStudent.Photo_A;
                stdobj.AddressID = ExternalStudent.AddressID;
                stdobj.BirthPlace = ExternalStudent.BirthPlace;
                StudentManager.Add(stdobj);

                byte[] BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate;
                BirthCertificatePhoto = FamilyBookPhoto = LastYearCertificate = null;
                query = "SELECT BirthCertificatePhoto , FamilyBookPhoto , LastYearCertificate FROM ExternalStudent " +
                    "WHERE StudentID = @StudentID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StudentID", StudentID);
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            byte[] imageData = null;
                            if (!(reader[0] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[0];
                                BirthCertificatePhoto = imageData;
                            }
                            if (!(reader[1] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[1];
                                FamilyBookPhoto = imageData;
                            }
                            if (!(reader[2] is DBNull))
                            {
                                // Get the image data from the reader
                                imageData = (byte[])reader[2];
                                LastYearCertificate = imageData;
                            }
                        }
                    }
                }

                query = "UPDATE Student" +
                 " SET TransportDirectionID = @TransportDirectionID, BirthCertificatePhoto = @BirthCertificatePhoto, FamilyBookPhoto = @FamilyBookPhoto, LastYearCertificate = @LastYearCertificate " +
                 " WHERE StudentID = @StudentID;";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@TransportDirectionID", ExternalStudent.TransportDirectionID);
                        comm.Parameters.AddWithValue("@BirthCertificatePhoto", BirthCertificatePhoto == null ?
                             System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "BrithCertificate.jpeg")) : BirthCertificatePhoto);
                        comm.Parameters.AddWithValue("@FamilyBookPhoto", FamilyBookPhoto == null ?
                            System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "FamilyBook.jpeg")) : FamilyBookPhoto);
                        comm.Parameters.AddWithValue("@LastYearCertificate", LastYearCertificate == null ?
                            System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "LastYearCertificate.jpeg")) : LastYearCertificate);
                        comm.Parameters.AddWithValue("@StudentID", generatedStudentID);
                        comm.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Step 3 adding the Student School Details to the student.

                var ExternalStudentSchoolDetailsManager = factory.CreateExternalStudentSchoolDetailsManager();
                var ExternalStudentSchoolDetails = ExternalStudentSchoolDetailsManager.Find(d => d.StudentID == StudentID).FirstOrDefault();

                var StudentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
                StudentSchoolDetail ssdobj = new StudentSchoolDetail();
                ssdobj.StudentID = generatedStudentID;
                ssdobj.SchoolID = ExternalStudentSchoolDetails.SchoolID;
                ssdobj.DateofAdmission = ExternalStudentSchoolDetails.DateofAdmission;
                ssdobj.ClassID = ExternalStudentSchoolDetails.ClassID;
                ssdobj.SectionID = ExternalStudentSchoolDetails.SectionID;
                ssdobj.Status = ExternalStudentSchoolDetails.Status;
                ssdobj.Results = 0;
                ssdobj.ComingBusTourID = 0;
                ssdobj.GoingBusTourID = 0;
                ssdobj.PreviousSchool = ExternalStudentSchoolDetails.LastSchoolName;
                ssdobj.ComingTourOrder = 0;
                ssdobj.GoingTourOrder = 0;
                StudentSchoolDetailsManager.Add(ssdobj);
                #endregion

                #region Step 4 adding the Student Guardians Details to our system.

                var ExternalStudentGuardDetailsManager = factory.CreateExternalStudentGuardDetailsManager();
                var ExternalStudentGuardDetails = ExternalStudentGuardDetailsManager.Find(d => d.StudentID == StudentID).FirstOrDefault();

                var StudentGuardDetailsManager = factory.CreateStudentGuardDetailsManager();
                StudentGuardDetail sgdobj = new StudentGuardDetail();
                sgdobj.StudentID = generatedStudentID;
                sgdobj.GuardianID = insertedGuardianID;
                sgdobj.FatherName = ExternalStudentGuardDetails.FatherName;
                sgdobj.MotherName = ExternalStudentGuardDetails.MotherName;
                sgdobj.FatherQualification = ExternalStudentGuardDetails.FatherQualification;
                sgdobj.MotherQualification = ExternalStudentGuardDetails.MotherQualification;
                sgdobj.FatherSpecialization = ExternalStudentGuardDetails.FatherSpecialization;
                sgdobj.MotherSpecialization = ExternalStudentGuardDetails.MotherSpecialization;
                sgdobj.FatherJob = ExternalStudentGuardDetails.FatherJob;
                sgdobj.MotherJob = ExternalStudentGuardDetails.MotherJob;
                sgdobj.FatherWorkPhone = "-1";
                sgdobj.MotherWorkPhone = "-1";
                sgdobj.FatherMobile = "-1";
                sgdobj.MotherMobile = "-1";
                sgdobj.FatherEmail = "-1";
                sgdobj.MotherEmail = "-1";
                sgdobj.MailBox = "-1";
                sgdobj.PostalCode = "-1";
                sgdobj.SmsNumber = ExternalStudentGuardDetails.SmsNumber;
                StudentGuardDetailsManager.Add(sgdobj);
                #endregion

                #region Step 5 adding the Guardain Address (StudentAdresses table) to our system.

                var ExternalStudentAddressManager = factory.CreateExternalStudentAdresssManager();
                //var ExternalStudentAddress = ExternalStudentAddressManager.Find(a => a.GuardianID == ExternalStudent.GuardianID).FirstOrDefault();

                ExternalStudentAdress ExternalStudentAddress = null;
                query = "SELECT * FROM ExternalStudentAdresses WHERE StudentID = @StudentID AND GuardianID = @GuardianID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StudentID", StudentID);
                        comm.Parameters.AddWithValue("@GuardianID", ExternalStudent.GuardianID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ExternalStudentAddress = new ExternalStudentAdress();
                                ExternalStudentAddress.AddressID = Convert.ToInt32(reader["AddressID"].ToString());
                                ExternalStudentAddress.StudentID = reader["StudentID"].ToString();
                                ExternalStudentAddress.GuardianID = Convert.ToInt32(reader["GuardianID"].ToString());
                                ExternalStudentAddress.CountryID = Convert.ToInt32(reader["CountryID"].ToString());
                                ExternalStudentAddress.City = reader["City"].ToString();
                                ExternalStudentAddress.Street = reader["Street"].ToString();
                                ExternalStudentAddress.Building = Convert.ToInt32(reader["Building"].ToString());
                                ExternalStudentAddress.Flat = Convert.ToInt32(reader["Flat"].ToString());
                                ExternalStudentAddress.MobileNumber = reader["MobileNumber"].ToString();
                                ExternalStudentAddress.Email = reader["Email"].ToString();
                                ExternalStudentAddress.Longitude = Convert.ToDouble(reader["Longitude"].ToString());
                                ExternalStudentAddress.Latitude = Convert.ToDouble(reader["Latitude"].ToString());
                                ExternalStudentAddress.TransportDirectionID = Convert.ToInt32(reader["TransportDirectionID"].ToString());
                            }
                        }

                    }
                }

                query = "INSERT INTO StudentAdresses (StudentID, GuardianID, CountryID, City, Street, Building, Flat, MobileNumber," +
                    " Email, Longitude, Latitude, TransportDirectionID)" +
                    " VALUES (@StudentID, @GuardianID, @CountryID, @City, @Street, @Building, @Flat, @MobileNumber," +
                    " @Email, @Longitude, @Latitude, @TransportDirectionID);" +
                    " SELECT SCOPE_IDENTITY() AS result;";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StudentID", generatedStudentID);
                        comm.Parameters.AddWithValue("@GuardianID", insertedGuardianID); // insertedGuardianID is comming from Step two.
                        comm.Parameters.AddWithValue("@CountryID", ExternalStudentAddress.CountryID);
                        comm.Parameters.AddWithValue("@City", ExternalStudentAddress.City);
                        comm.Parameters.AddWithValue("@Street", ExternalStudentAddress.Street);
                        comm.Parameters.AddWithValue("@Building", ExternalStudentAddress.Building);
                        comm.Parameters.AddWithValue("@Flat", "-1");
                        comm.Parameters.AddWithValue("@MobileNumber", ExternalStudentAddress.MobileNumber);
                        comm.Parameters.AddWithValue("@Email", ExternalStudentAddress.Email);
                        comm.Parameters.AddWithValue("@Longitude", ExternalStudentAddress.Longitude);
                        comm.Parameters.AddWithValue("@Latitude", ExternalStudentAddress.Latitude);
                        comm.Parameters.AddWithValue("@TransportDirectionID", ExternalStudentAddress.TransportDirectionID);
                        insertedAddressID = int.Parse(comm.ExecuteScalar().ToString());
                    }
                }

                // I used the ADO.NET here becuase the AddressID is not shown in the .edmx file.
                query = "UPDATE Student SET TransportDirectionID = @TransportDirectionID, AddressID = @AddressID WHERE StudentID = @StudentID;";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@TransportDirectionID", ExternalStudentAddress.TransportDirectionID);
                        comm.Parameters.AddWithValue("@AddressID", insertedAddressID);
                        comm.Parameters.AddWithValue("@StudentID", generatedStudentID);
                        comm.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Checked Discounts
                AddUpdateStudentDiscounts(CheckedDiscounts, StudentID);
                var StudentDiscountManager = factory.CreateDiscountStudentsManager();
                if (CheckedDiscounts != null)
                {
                    foreach (var item in CheckedDiscounts)
                    {
                        bool Isyes = false;
                        var Discount = item.Split('$');
                        string key = Discount[0];
                        int DiscountID = Convert.ToInt32(Discount[1]);
                        Isyes = key == "yes" ? true : false;
                        var studentDiscount = StudentDiscountManager.Find(a => a.StudentID == StudentID && a.DiscountID == DiscountID).FirstOrDefault();
                        if (studentDiscount != null)
                        {
                            studentDiscount.IsYes = Isyes;
                            studentDiscount.InternalStudentID = generatedStudentID;
                            StudentDiscountManager.Update(studentDiscount);
                        }
                    }
                }
                #endregion

                #region Special Discount
                if (SpecialDiscount != 0)
                {
                    query = "INSERT INTO SpecialDiscount " +
                            "(StudentID, InternalStudentID, DiscountValue, SchoolID) " +
                            "VALUES(@StudentID, @InternalStudentID, @DiscountValue, @SchoolID)";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", StudentID);
                            comm.Parameters.AddWithValue("@InternalStudentID", generatedStudentID);
                            comm.Parameters.AddWithValue("@DiscountValue", SpecialDiscount);
                            comm.Parameters.AddWithValue("@SchoolID", SchoolID);
                            comm.ExecuteNonQuery();
                        }
                    }
                }
                #endregion

                #region Step 6 adding the Other Student Details to our system.

                var ExternalOtherStudentDetailsManager = factory.CreateExternalOtherStudentDetailsManager();
                var ExternalOtherStudentDetails = ExternalOtherStudentDetailsManager.Find(o => o.StudentID == StudentID).FirstOrDefault();

                var OtherStudentDetailsManager = factory.CreateOtherStudentDetailsManager();
                OtherStudentDetail osd = new OtherStudentDetail();
                osd.StudentID = generatedStudentID;
                osd.Behavior = ExternalOtherStudentDetails.Behavior;
                osd.BehaviorNotes = ExternalOtherStudentDetails.BehaviorNotes;
                osd.BloodType = ExternalOtherStudentDetails.BloodType;
                osd.DiseaseType = ExternalOtherStudentDetails.DiseaseType;
                osd.FamilyOrder = ExternalOtherStudentDetails.FamilyOrder;
                osd.FamilyTotalMonthlyIncome = ExternalOtherStudentDetails.FamilyTotalMonthlyIncome;
                osd.HasChronicDisease = ExternalOtherStudentDetails.HasChronicDisease;
                osd.HealthNotes = ExternalOtherStudentDetails.HealthNotes;
                osd.LivesWith = ExternalOtherStudentDetails.LivesWith;
                osd.NumberofBrothers = ExternalOtherStudentDetails.NumberofBrothers;
                osd.NumberofSisters = ExternalOtherStudentDetails.NumberofSisters;
                osd.SpecialCare = ExternalOtherStudentDetails.SpecialCare;
                osd.SpecialResidenceConditions = ExternalOtherStudentDetails.SpecialResidenceConditions;
                osd.Talents = ExternalOtherStudentDetails.Talents;
                osd.ValedictorianinSubject = ExternalOtherStudentDetails.ValedictorianinSubject;
                osd.WeakinSubject = ExternalOtherStudentDetails.WeakinSubject;
                OtherStudentDetailsManager.Add(osd);
                #endregion

                #region Step 7 adding the Student Diseases to our system.

                var StudentDiseasManager = factory.CreateStudentDiseassManager();
                var StudentDiseasResult = StudentDiseasManager.Find(a => a.StudentID == StudentID).FirstOrDefault();

                if (StudentDiseasResult != null)
                {
                    StudentDiseasResult.InternalStudentID = generatedStudentID;
                    StudentDiseasManager.Update(StudentDiseasResult);
                }
                #endregion

                #region Final step send username and password to the guardian mobile number.

                // update student requst state.
                ExternalRequest.RequestStatus = (int)clsenumration.RequestStatus.Accepted;
                ExternalRequestsManager.Update(ExternalRequest);

                string body = "تم قبول الطالب" + " " + ExternalRequest.StudentName + "\n" + "الرجاء تسجيل الدخول لإستكمال عملية التسجيل" + "%0a" +
                    "Username: " + ExternalGuardian.UserName + "%0a" + "Password: " + Decrypt(ExternalGuardian.Password);

                // sent sms with the student username and password to the guardian mobile number;
                string MobileNo = ExternalGuardian.MobileNo;
                if (MobileNo.StartsWith("20") || MobileNo.StartsWith("962"))
                {
                    string contryCode = MobileNo.StartsWith("20") ? "20" : "962";
                    // Remove(0,contryCode.Length) becouse SendSMS already add the contrycode to the number.
                    var resulte = SendSMS(body, Guardian != null
                        ?
                        Guardian.MobileNumber.Remove(0, contryCode.Length)
                        :
                        ExternalGuardian.MobileNo.Remove(0, contryCode.Length), 'e', contryCode);
                    if (resulte.IsSuccessStatusCode)
                        return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                    else
                    {
                        #region Delete the inserted rows.
                        // (we must use transaction but the database doesnt have relationships and there is a problem in .edmx file).
                        var Student = StudentManager.Find(s => s.StudentID == generatedStudentID).FirstOrDefault();
                        if (Student != null)
                            StudentManager.Delete(Student);

                        var StudentSchoolDetails = StudentSchoolDetailsManager.Find(s => s.StudentID == generatedStudentID).FirstOrDefault();
                        if (StudentSchoolDetails != null)
                            StudentSchoolDetailsManager.Delete(StudentSchoolDetails);

                        var StudentGuardDetails = StudentGuardDetailsManager.Find(s => s.StudentID == generatedStudentID).FirstOrDefault();
                        if (StudentGuardDetails != null)
                            StudentGuardDetailsManager.Delete(StudentGuardDetails);

                        var StudentAdressesManager = factory.CreateStudentAdresssManager();
                        var StudentAdresses = StudentAdressesManager.Find(s => s.AddressID == insertedAddressID).FirstOrDefault();
                        if (StudentAdresses != null)
                            StudentAdressesManager.Delete(StudentAdresses);

                        var StudentDiscount = StudentDiscountManager.Find(s => s.InternalStudentID == generatedStudentID).FirstOrDefault();
                        if (StudentDiscount != null)
                            StudentDiscountManager.Delete(StudentDiscount);

                        var OtherStudentDetails = OtherStudentDetailsManager.Find(s => s.StudentID == generatedStudentID).FirstOrDefault();
                        if (OtherStudentDetails != null)
                            OtherStudentDetailsManager.Delete(OtherStudentDetails);

                        ExternalRequest.RequestStatus = (int)clsenumration.RequestStatus.pending;
                        ExternalRequestsManager.Update(ExternalRequest);
                        #endregion

                        return Json(new { Success = false, Message = "Soming went wrong happens please try again." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                    return Json(new { Success = false, Message = "The number must be (Jordan or Egyption number)!" }, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Hamza01
        [HttpPost]
        public JsonResult RejectStudent(int StudentID, string RejectReason)
        {
            var RequestManager = factory.CreateExternalGuardStudentsRequestsManager();
            var ExternalGuardian = factory.CreateExternalGuardiansManager();

            var Request = RequestManager.Find(a => a.StudentID == StudentID).FirstOrDefault();
            var Guardian = ExternalGuardian.Find(a => a.UserID == Request.GuardianID).FirstOrDefault();

            Request.RequestStatus = (int)clsenumration.RequestStatus.Rejected;
            RequestManager.Update(Request);

            // sent sms with the RejectReason to guardian mobile number.
            string sms = "تم رفض طلب تسجيل الطالب" + " " + Request.StudentName + " " + "بسبب" + " " + RejectReason;
            string MobileNo = Guardian.MobileNo;
            if (MobileNo.StartsWith("20") || MobileNo.StartsWith("962"))
            {
                string contryCode = MobileNo.StartsWith("20") ? "20" : "962";
                // Remove(0,contryCode.Length) becouse SendSMS already add the contrycode to the number.
                var resulte = SendSMS(sms, Guardian.MobileNo.Remove(0, contryCode.Length), 'e', contryCode);
                if (resulte.IsSuccessStatusCode)
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Success = false, Message = "Soming went wrong happens please try again." }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Success = false, Message = "Soming went wrong please try again." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdmissionDashboard()
        {
            AdmissionDashboardModel model = new AdmissionDashboardModel();
            var RequestManager = factory.CreateExternalGuardStudentsRequestsManager();
            var ExternalStudentAddressManager = factory.CreateExternalStudentAdresssManager();
            var ExternalStudentManager = factory.CreateExternalStudentsManager();
            var SchoolBranchManager = factory.CreateSchoolBranchsManager();
            var TransportCategories = factory.CreateTransportCategorysManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            var CategoriesDistances = TransportCategories.Find(a => a.SchoolID == SchoolID).ToList();
            var Requests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                            join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                            on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                            where ExternalStudentSchoolDetail.SchoolID == SchoolID
                            select ExternalGuardStudentsRequest).ToList();


            model.PendingRequests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                                     join ExternalGuardian in context.ExternalGuardians
                                     on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                                     join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                                     on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                                     join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                                     SchoolClass.SchoolClassID
                                     join Class in context.Classes on SchoolClass.ClassID
                                     equals Class.ClassID
                                     where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.pending
                                     && ExternalStudentSchoolDetail.SchoolID == SchoolID
                                     select ExternalGuardStudentsRequest).Count();
            //model.PendingRequests = Requests.Where(a => a.RequestStatus == (int)clsenumration.RequestStatus.pending).ToList().Count();
            model.AcceptedRequests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                                      join ExternalGuardian in context.ExternalGuardians
                                      on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                                      join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                                      on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                                      join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                                      SchoolClass.SchoolClassID
                                      join Class in context.Classes on SchoolClass.ClassID
                                      equals Class.ClassID
                                      where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.Accepted
                                      && ExternalStudentSchoolDetail.SchoolID == SchoolID
                                      select ExternalGuardStudentsRequest).Count();
            model.RejectedRequests = (from ExternalGuardStudentsRequest in context.ExternalGuardStudentsRequests
                                      join ExternalGuardian in context.ExternalGuardians
                                      on ExternalGuardStudentsRequest.GuardianID equals ExternalGuardian.UserID
                                      join ExternalStudentSchoolDetail in context.ExternalStudentSchoolDetails
                                      on ExternalGuardStudentsRequest.StudentID equals ExternalStudentSchoolDetail.StudentID
                                      join SchoolClass in context.SchoolClasses on ExternalStudentSchoolDetail.ClassID equals
                                      SchoolClass.SchoolClassID
                                      join Class in context.Classes on SchoolClass.ClassID
                                      equals Class.ClassID
                                      where ExternalGuardStudentsRequest.RequestStatus == (int)clsenumration.RequestStatus.Rejected
                                      && ExternalStudentSchoolDetail.SchoolID == SchoolID
                                      select ExternalGuardStudentsRequest).Count();

            //////model.CategoryOne = 0;
            //////model.CategoryTwo = 0;
            //////model.CategoryThree = 0;
            //////model.CategoryFour = 0;
            //////model.CategoryFive = 0;
            //////model.OutsideCategories = 0;
            //////if (Requests.Count > 0)
            //////{
            //////    foreach (var item in Requests)
            //////    {
            //////        double DistanceM;
            //////        double DistanceKM;
            //////        int GuardianID = ExternalStudentManager.Find(a => a.StudentID == item.StudentID).Select(a => a.GuardianID.Value).FirstOrDefault();
            //////        var StudentAddress = ExternalStudentAddressManager.Find(a => a.GuardianID == GuardianID).FirstOrDefault();
            //////        var SchoolAddress = SchoolBranchManager.Find(a => a.SchoolID == SchoolID).FirstOrDefault();
            //////        double SChoolLatitude = SchoolAddress.Latitude.Value;
            //////        double SchoolLongitude = SchoolAddress.Longitude.Value;

            //////        double StudentLatitude = StudentAddress.Latitude.Value;

            //////        double StudentLongitude = StudentAddress.Longitude.Value;
            //////        string Origins = SChoolLatitude.ToString() + ',' + SchoolLongitude.ToString();
            //////        string Distination = StudentLatitude.ToString() + ',' + StudentLongitude.ToString();
            //////        string jsonData = DistanceMatrixRequest(Origins, Distination, "Driving", ConfigurationManager.AppSettings["GoogleAPIKey"]);
            //////        JObject o = JObject.Parse(jsonData);
            //////        //abuhammad
            //////        DistanceM = (double)o.SelectToken("rows[0].elements[0].distance.value");
            //////        DistanceKM = DistanceM / 1000;
            //////        DistanceKM = Math.Round(DistanceKM, 1);
            //////        if (CategoriesDistances.Count > 0)
            //////        {
            //////            foreach (var x in CategoriesDistances)
            //////            {
            //////                if (DistanceM <= x.DistanceInMeters)
            //////                {
            //////                    if (x.TransportCategoryTypeID == 1)
            //////                    {
            //////                        model.CategoryOne++;
            //////                        break;
            //////                    }
            //////                    else if (x.TransportCategoryTypeID == 2)
            //////                    {
            //////                        model.CategoryTwo++;
            //////                        break;
            //////                    }
            //////                    else if (x.TransportCategoryTypeID == 3)
            //////                    {
            //////                        model.CategoryThree++;
            //////                        break;
            //////                    }
            //////                    else if (x.TransportCategoryTypeID == 4)
            //////                    {
            //////                        model.CategoryFour++;
            //////                        break;
            //////                    }
            //////                    else if (x.TransportCategoryTypeID == 5)
            //////                    {
            //////                        model.CategoryFive++;
            //////                        break;
            //////                    }
            //////                    else
            //////                    {
            //////                        model.OutsideCategories++;
            //////                    }
            //////                }

            //////            }
            //////        }
            //////    }
            //////}


            return View(model);
        }

        #region Submit Student Register Steps
        public string DistanceMatrixRequest(string source, string Destination, string travelMode, string keyString)
        {
            try
            {
                string urlRequest = "";
                urlRequest = @"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + source + "&destinations=" + Destination + "&mode=" + travelMode + "&key=" + keyString + "&sensor=false";

                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Hamza01
        [HttpPost]
        public JsonResult StudentInfoStepOne(EditStudentViewModel model)
        {
            try
            {
                int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                byte[] imgByte, BCPimgByte, FBPimgByte, LYCimgByte;
                imgByte = BCPimgByte = FBPimgByte = LYCimgByte = null;

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
                var StudentManager = factory.CreateStudentsManager();
                var Joiningyear = DateTime.Now.Year.ToString();
                var result = StudentManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (model.ThirdArabicName == null)
                    model.ThirdArabicName = "";
                var StudentName = model.FirstArabicName.Replace(" ", "-") + " " + model.SecondArabicName.Replace(" ", "-") + " " + model.ThirdArabicName.Replace(" ", "-") + " " + model.FourthArabicName.Replace(" ", "-");
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
                    result.DateofBirth = Convert.ToDateTime(DateOfBirth);
                    result.AccountNumber = null;
                    result.FamilyID = -1;
                    result.MotherID = -1;
                    result.ResidencyNumber = ResidencyNumber;
                    result.PassportNumber = "";
                    result.Active = -1;
                    result.BirthPlace = model.BirthPlace;
                    result.Photo_A = "NOPHOTO.jpeg";
                    StudentManager.Update(result);
                    #region adding the Student Files using ADO.NET.
                    byte[] Photo, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate;
                    Photo = BirthCertificatePhoto = FamilyBookPhoto = LastYearCertificate = null;
                    string query = "";
                    try
                    {
                        query = "SELECT Photo, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate FROM Student " +
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
                        query = "UPDATE Student SET Photo = @Photo, BirthCertificatePhoto = @BCP, " +
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
                        string query = "INSERT INTO Student" +
                            " (StudentArabicName, StudentEnglishName, NationalNumber, Gender," +
                            " Nationality, DateofBirth, GuardianID, AccountNumber, FamilyID, MotherID, ResidencyNumber, PassportNumber, Active," +
                            " Photo, Photo_A, BirthPlace, BirthCertificatePhoto, FamilyBookPhoto, LastYearCertificate)" +
                            " VALUES (@StudentArabicName, @StudentEnglishName, @NationalNumber, @Gender, @Nationality, @DateofBirth," +
                            " @GuardianID, NULL, -1, -1,'', '', -1, @Photo, N'NOPHOTO.jpeg'," +
                            " @BirthPlace, @BirthCertificatePhoto, @FamilyBookPhoto, @LastYearCertificate);";
                        using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                        {
                            conn.Open();
                            using (SqlCommand comm = new SqlCommand(query, conn))
                            {
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
                                comm.ExecuteNonQuery();
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
        public JsonResult SchoolInfoStepTwo(EditStudentViewModel model)
        {
            try
            {
                var studentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();
                var result = studentSchoolDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (result != null)
                {
                    result.ClassID = model.SchoolDetail.ClassID;
                    result.SectionID = model.SchoolDetail.SectionID;
                    result.Status = model.SchoolDetail.Status;
                    result.Results = model.SchoolDetail.Results;
                    result.ComingBusTourID = model.SchoolDetail.ComingBusTourID;
                    result.ComingTourOrder = model.SchoolDetail.ComingTourOrder;
                    result.GoingBusTourID = model.SchoolDetail.GoingBusTourID;
                    result.GoingTourOrder = model.SchoolDetail.GoingTourOrder;
                    studentSchoolDetailsManager.Update(result);
                }
                else
                {
                    StudentSchoolDetail obj = new StudentSchoolDetail();
                    obj.SchoolID = model.SchoolDetail.SchoolID;
                    obj.ClassID = model.SchoolDetail.ClassID;
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

        [HttpPost]
        public JsonResult ParentsInfoStepThree(EditStudentViewModel model)
        {
            try
            {

                var studentGuardsDetailsManager = factory.CreateStudentGuardDetailsManager();
                var studentmanager = factory.CreateStudentsManager();
                var GuardiansManager = factory.CreateGuardiansManager();

                var stdresult = studentmanager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var result = studentGuardsDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                if (result != null)
                {
                    var Guardresult = GuardiansManager.Find(a => a.GuardianID == result.GuardianID).FirstOrDefault();
                    Guardresult.MobileNumber = model.GuardianMobileNumber;
                    GuardiansManager.Update(Guardresult);
                }
                stdresult.GuardianRelationship = model.student.GuardianRelationship;
                studentmanager.Update(stdresult);
                if (result != null)
                {
                    result.FatherName = model.FatherName;
                    result.MotherName = model.MotherName;
                    result.FatherQualification = model.FatherQualification;
                    result.MotherQualification = model.MotherQualification;
                    result.FatherSpecialization = model.FatherSpecialization;
                    result.MotherSpecialization = model.MotherSpecialization;
                    result.FatherJob = model.FatherJob;
                    result.MotherJob = model.MotherJob;
                    result.FatherWorkPhone = model.FatherWorkPhone;
                    result.MotherWorkPhone = model.MotherWorkPhone;
                    result.FatherEmail = model.FatherEmail;
                    result.MotherEmail = model.MotherEmail;
                    result.MailBox = model.mailBox;
                    result.PostalCode = model.PostalCode;
                    result.SmsNumber = model.smsNumber;
                    result.FatherMobile = model.FatherMobile;
                    result.MotherMobile = model.MotherMobile;
                    studentGuardsDetailsManager.Update(result);
                }
                else
                {
                    StudentGuardDetail obj = new StudentGuardDetail();
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
                    obj.GuardianID = stdresult.GuardianID.Value;
                    obj.FatherMobile = model.FatherMobile;
                    obj.MotherMobile = model.MotherMobile;
                    studentGuardsDetailsManager.Add(obj);
                }
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddressInfoStepFour(EditStudentViewModel model)
        {
            string query = "";
            try
            {
                var AddressManager = factory.CreateStudentAdresssManager();
                var studentManger = factory.CreateStudentsManager();
                var stdresult = studentManger.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var result = AddressManager.Find(a => a.GuardianID == stdresult.GuardianID).FirstOrDefault();
                bool existedStdAddress = false;
                //if (result != null)
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

                //    query = "UPDATE StudentAdresses SET TransportDirectionID = @TransportDirectionID WHERE AddressID = @AddressID;";
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
                //    query = "UPDATE Student SET TransportDirectionID = @TransportDirectionID, AddressID = @AddressID WHERE StudentID = @StudentID;";
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
                query = "SELECT AddressID FROM StudentAdresses WHERE StudentID = @StudentID";
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
                    query = "Update StudentAdresses SET CountryID = @CountryID, City = @City, Street = @Street, Building = @Building, Flat = @Flat, " +
                        "MobileNumber = @MobileNumber, Email = @Email, Longitude = @Longitude, Latitude = @Latitude, TransportDirectionID = @TransportDirectionID " +
                        "WHERE StudentID = @StudentID AND GuardianID = @GuardianID;";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                            comm.Parameters.AddWithValue("@GuardianID", stdresult.GuardianID);
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
                    query = "INSERT INTO StudentAdresses (StudentID, GuardianID, CountryID, City, Street, Building, Flat, " +
                        "MobileNumber, Email, Longitude, Latitude, TransportDirectionID) " +
                        "VALUES (@StudentID, @GuardianID, @CountryID, @City, @Street, @Building, @Flat, @MobileNumber, @Email, @Longitude, @Latitude, @TransportDirectionID); " +
                        "SELECT SCOPE_IDENTITY() AS result";
                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            comm.Parameters.AddWithValue("@StudentID", model.StudentIDNumber);
                            comm.Parameters.AddWithValue("@GuardianID", model.StudentAddress.GuardianID);
                            comm.Parameters.AddWithValue("@CountryID", model.StudentAddress.CountryID);
                            comm.Parameters.AddWithValue("@City", model.StudentAddress.City);
                            comm.Parameters.AddWithValue("@Street", model.StudentAddress.Street);
                            comm.Parameters.AddWithValue("@Building", model.StudentAddress.Building);
                            comm.Parameters.AddWithValue("@Flat", model.StudentAddress.Flat);
                            comm.Parameters.AddWithValue("@MobileNumber", model.StudentAddress.MobileNumber);
                            comm.Parameters.AddWithValue("@Email", model.StudentAddress.Email);
                            comm.Parameters.AddWithValue("@Longitude", model.StudentAddress.Longitude);
                            comm.Parameters.AddWithValue("@Latitude", model.StudentAddress.Latitude);
                            comm.Parameters.AddWithValue("@TransportDirectionID", model.StudentAddress.TransportDirectionID);
                            insertedAddressID = comm.ExecuteNonQuery();
                        }
                    }

                    // I used the ADO.NET here becuase the AddressID is not shown in the .edmx file.
                    query = "UPDATE Student SET TransportDirectionID = @TransportDirectionID, AddressID = @AddressID WHERE StudentID = @StudentID;";
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
                var StudentSchoolManager = factory.CreateStudentSchoolDetailsManager();
                int SchoolID = StudentSchoolManager.Find(a => a.StudentID == model.StudentIDNumber).Select(a => a.SchoolID.Value).FirstOrDefault();
                return Json(new { Success = true, SchoolID = SchoolID, StudentID = model.StudentIDNumber }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SocialInfoStepSex(EditStudentViewModel model)
        {
            try
            {
                var OtherStudentDetailsManager = factory.CreateOtherStudentDetailsManager();
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

                //No add only update new values because we had add record in Academic step :)
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult HealthInfoStepSeven(EditStudentViewModel model)
        {
            try
            {
                var OtherStudentDetailsManager = factory.CreateOtherStudentDetailsManager();
                var StudentHealthManager = factory.CreateStudentHealthsManager();
                var result = OtherStudentDetailsManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
                var StudentDiseasResult = StudentHealthManager.Find(a => a.StudentID == model.StudentIDNumber).FirstOrDefault();
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
                    StudentHealthManager.Update(StudentDiseasResult);


                }
                else
                {
                    StudentHealth obj = new StudentHealth();
                    obj.Mumps = model.StudentDiseas.Mumps;
                    obj.Chickenpox = model.StudentDiseas.Chickenpox;
                    obj.rubella = model.StudentDiseas.rubella;
                    obj.Rheumaticfever = model.StudentDiseas.Rheumaticfever;
                    obj.Diabetes = model.StudentDiseas.Diabetes;
                    obj.Heartdiseases = model.StudentDiseas.Heartdiseases;
                    obj.Pissingoff = model.StudentDiseas.Pissingoff;
                    obj.Jointbonediseases = model.StudentDiseas.Jointbonediseases;
                    obj.sprayer = model.StudentDiseas.sprayer;
                    obj.Hearingimpairment = model.StudentDiseas.Visualimpairment;
                    obj.Speechimpairment = model.StudentDiseas.Bladderdiseases;
                    obj.Epilepsy = model.StudentDiseas.Epilepsy;
                    obj.Hepatitis = model.StudentDiseas.Hepatitis;
                    obj.Shakika = model.StudentDiseas.Shakika;
                    obj.Fainting = model.StudentDiseas.Fainting;
                    obj.Kidneydisease = model.StudentDiseas.Kidneydisease;
                    obj.Surgery = model.StudentDiseas.Surgery;
                    obj.Urinarysystemdiseases = model.StudentDiseas.Urinarysystemdiseases;
                    obj.StudentID = model.StudentIDNumber;
                    StudentHealthManager.Add(obj);
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

        #region studentNewWizard
        public ActionResult StudentRegistrationWizard()
        {
            StudentRegistrationViewModel model = new StudentRegistrationViewModel();
            PrepareStudentModel(model);
            return View(model);
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

        [HttpPost]
        public JsonResult ParentsInfoStepOne(StudentRegistrationViewModel model)
        {
            try
            {
                var GuardiansManager = factory.CreateGuardiansManager();
                Guardian g = new Guardian()
                {
                    GuardianArabicName = model.Guardian.GuardianArabicName,
                    GuardianEnglishName = model.Guardian.GuardianEnglishName,
                    NationalNumber = model.Guardian.NationalNumber,
                    Gender = model.Guardian.Gender,
                    Nationality = model.Guardian.Nationality,
                    Religion = model.Guardian.Religion,
                    ResidencyNumber = model.Guardian.ResidencyNumber,
                    PassportNumber = model.Guardian.PassportNumber,
                    MobileNumber = model.Guardian.MobileNumber,
                    PaymentMethod = 0,
                    CreditCardType = -1,
                    CreditCardNumber = string.Empty,
                    NameonCreditCard = string.Empty,
                    CreditCardExpiryDate = "-1A",
                    CreditCardCode = string.Empty

                };
                GuardiansManager.Add(g);
                return Json(new { Success = true, GurdianID = g.GuardianID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ParentsInfoStepTwo(StudentRegistrationViewModel model)
        {
            try
            {
                var studentGuardsDetailsManager = factory.CreateStudentGuardDetailsManager();
                var SystemSettingsManager = factory.CreateSystemSettingsManager();
                var StaffManager = factory.CreateStaffsManager();
                var Joiningyear = DateTime.Now.Year.ToString();
                var _SchoolID = this.SchoolID.ToString() + "000";
                var settings = SystemSettingsManager.GetAll().FirstOrDefault();
                var lastStudentID = settings.LastStudentID + 1;
                settings.LastStudentID = lastStudentID;
                SystemSettingsManager.Update(settings);
                string StudentID = "S" + Joiningyear + _SchoolID + lastStudentID.ToString();

                StudentGuardDetail obj = new StudentGuardDetail()
                {
                    StudentID = StudentID,
                    FatherName = model.FatherName,
                    MotherName = model.MotherName,
                    FatherQualification = model.FatherQualification,
                    MotherQualification = model.MotherQualification,
                    FatherSpecialization = model.FatherSpecialization,
                    MotherSpecialization = model.MotherSpecialization,
                    FatherJob = model.FatherJob,
                    MotherJob = model.MotherJob,
                    FatherWorkPhone = model.FatherWorkPhone,
                    MotherWorkPhone = model.MotherWorkPhone,
                    FatherEmail = model.FatherEmail,
                    MotherEmail = model.MotherEmail,
                    MailBox = model.mailBox,
                    PostalCode = model.PostalCode,
                    SmsNumber = model.smsNumber,
                    GuardianID = model.GurdianID,
                    FatherMobile = model.FatherMobile,
                    MotherMobile = model.MotherMobile,

                };
                studentGuardsDetailsManager.Add(obj);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult StudentInfoStepThree(StudentRegistrationViewModel model)
        {
            try
            {
                byte[] imgByte = null;

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader.ContentLength > 0)
                    {
                        imgByte = new byte[fileuploader.ContentLength];
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }

                }
                var StudentManager = factory.CreateStudentsManager();
                if (model.ThirdArabicName == null)
                    model.ThirdArabicName = "";
                var StudentName = model.FirstArabicName.Replace(" ", "-") + " " + model.SecondArabicName.Replace(" ", "-") + " " + model.ThirdArabicName.Replace(" ", "-") + " " + model.FourthArabicName.Replace(" ", "-");
                var Gender = model.student.Gender;
                var Nationality = model.student.Nationality;
                var NationalNumber = model.student.NationalNumber == null ? "" : model.student.NationalNumber;
                var DateOfBirth = model.DateofBirth;
                var ResidencyNumber = model.student.ResidencyNumber == null ? "" : model.student.ResidencyNumber;

                var SystemSettingsManager = factory.CreateSystemSettingsManager();

                var StaffManager = factory.CreateStaffsManager();
                var Joiningyear = DateTime.Now.Year.ToString();
                var _SchoolID = this.SchoolID.ToString() + "000";
                var settings = SystemSettingsManager.GetAll().FirstOrDefault();
                var lastStudentID = settings.LastStudentID + 1;
                string StudentID = "S" + Joiningyear + _SchoolID + lastStudentID.ToString();

                Student obj = new Student()
                {
                    StudentID = StudentID,
                    StudentArabicName = StudentName,
                    StudentEnglishName = StudentName,
                    NationalNumber = NationalNumber,
                    Gender = Gender,
                    Nationality = Nationality,
                    DateofBirth = Convert.ToDateTime(DateOfBirth),
                    GuardianID = model.GurdianID,
                    AccountNumber = null,
                    FamilyID = -1,
                    MotherID = -1,
                    ResidencyNumber = ResidencyNumber,
                    PassportNumber = string.Empty,
                    Active = -1,
                    GuardianRelationship = 0,
                    Photo = imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "DesktopCamera.PNG")) : imgByte,
                    Photo_A = "NOPHOTO.jpeg",
                    BirthPlace = model.BirthPlace,
                    TransportDirectionID = 2
                };
                StudentManager.Add(obj);
                settings.LastStudentID = lastStudentID;
                SystemSettingsManager.Update(settings);
                return Json(new { Success = true, StudentID = obj.StudentID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SchoolInfoStepFour(StudentRegistrationViewModel model)
        {
            try
            {
                var studentSchoolDetailsManager = factory.CreateStudentSchoolDetailsManager();

                StudentSchoolDetail obj = new StudentSchoolDetail()
                {
                    StudentID = model.StudentID,
                    SchoolID = model.SchoolDetail.SchoolID,
                    DateofAdmission = DateTime.Now,
                    ClassID = model.SchoolDetail.ClassID,
                    SectionID = model.SectionID,
                    Status = 0,
                    Results = 0,
                    ComingBusTourID = 0,
                    GoingBusTourID = 0,
                    PreviousSchool = model.SchoolDetail.LastSchoolName,
                    ComingTourOrder = 0,
                    GoingTourOrder = 0
                };

                studentSchoolDetailsManager.Add(obj);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddressInfoStepFive(StudentRegistrationViewModel model)
        {
            try
            {
                var AddressManager = factory.CreateStudentAdresssManager();
                var studentManger = factory.CreateStudentsManager();

                StudentAdress obj = new StudentAdress()
                {
                    GuardianID = model.GurdianID,
                    CountryID = model.StudentAddress.CountryID,
                    City = model.StudentAddress.City,
                    Street = model.StudentAddress.Street,
                    Building = model.StudentAddress.Building,
                    Flat = model.StudentAddress.Flat,
                    MobileNumber = "",
                    Email = "",
                    Longitude = model.StudentAddress.Longitude,
                    Latitude = model.StudentAddress.Latitude,
                };

                AddressManager.Add(obj);
                //set address info for student 
                var resultstudent = studentManger.Find(a => a.GuardianID == model.GurdianID && a.StudentID == model.StudentID).FirstOrDefault();
                resultstudent.TransportDirectionID = model.StudentAddress.TransportDirectionID;
                resultstudent.AddressID = obj.AddressID;
                studentManger.Update(resultstudent);

                var StudentSchoolManager = factory.CreateStudentSchoolDetailsManager();
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SocialInfoStepSeven(StudentRegistrationViewModel model)
        {
            try
            {
                var OtherStudentDetailsManager = factory.CreateOtherStudentDetailsManager();
                OtherStudentDetail obj = new OtherStudentDetail();
                obj.Behavior = -1;
                obj.BehaviorNotes = "";
                obj.LivesWith = model.OtherStudentDetail.LivesWith;
                obj.NumberofBrothers = model.OtherStudentDetail.NumberofBrothers;
                obj.NumberofSisters = model.OtherStudentDetail.NumberofSisters;
                obj.FamilyOrder = model.OtherStudentDetail.FamilyOrder;
                obj.FamilyTotalMonthlyIncome = model.OtherStudentDetail.FamilyTotalMonthlyIncome;
                obj.SpecialResidenceConditions = model.OtherStudentDetail.SpecialResidenceConditions;
                obj.StudentID = model.StudentID;

                OtherStudentDetailsManager.Add(obj);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult HealthInfoStepEight(StudentRegistrationViewModel model)
        {
            try
            {
                int UserID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.UserID));
                var OtherStudentDetailsManager = factory.CreateOtherStudentDetailsManager();
                var StudentDiseasManager = factory.CreateStudentDiseassManager();

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
                obj.Hearingimpairment = model.StudentDiseas.Visualimpairment;
                obj.Speechimpairment = model.StudentDiseas.Bladderdiseases;
                obj.Epilepsy = model.StudentDiseas.Epilepsy;
                obj.Hepatitis = model.StudentDiseas.Hepatitis;
                obj.Shakika = model.StudentDiseas.Shakika;
                obj.Fainting = model.StudentDiseas.Fainting;
                obj.Kidneydisease = model.StudentDiseas.Kidneydisease;
                obj.Surgery = model.StudentDiseas.Surgery;
                obj.Urinarysystemdiseases = model.StudentDiseas.Urinarysystemdiseases;
                obj.StudentID = 0;
                obj.InternalStudentID = model.StudentID;
                StudentDiseasManager.Add(obj);

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, Messge = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult AddUpdateStudentDiscountsInternal(List<string> CheckedDiscounts, string StudentID)
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
                    var studentDiscount = StudentDiscountManager.Find(a => a.InternalStudentID == StudentID && a.DiscountID == DiscountID).FirstOrDefault();
                    if (studentDiscount != null)
                    {
                        studentDiscount.IsYes = Isyes;
                        StudentDiscountManager.Update(studentDiscount);
                    }
                    else
                    {
                        DiscountStudent obj = new DiscountStudent();
                        obj.DiscountID = DiscountID;
                        obj.StudentID = 0;
                        obj.InternalStudentID = StudentID;
                        obj.IsYes = Isyes;
                        StudentDiscountManager.Add(obj);
                    }
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDiscounts(string StudentID)
        {
            DiscountsModel model = new DiscountsModel();
            int SchoolID = this.SchoolID;
            var DiscountsManager = factory.CreateDiscountsManager();
            var StudentDiscountManager = factory.CreateDiscountStudentsManager();
            model.SchoolDiscounts = DiscountsManager.Find(a => a.SchoolID == SchoolID).ToList();
            model.StudentDiscounts = StudentDiscountManager.Find(a => a.InternalStudentID == StudentID).ToList();
            model.InternalStudentID = StudentID;
            model.SchoolID = this.SchoolID;
            return PartialView("_DiscountsStep", model);
        }

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
        #endregion
    }
}
