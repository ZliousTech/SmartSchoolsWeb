using Business.Base;
using Common;
using Common.Base;
using Common.Helpers;
using Microsoft.Graph;
using Objects;
using SmartSchool.Models.Employee;
using SmartSchool.Models.transportation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class EmployeeController : EmployeeBaseController
    {
        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterEmployee()
        {
            EmployeeRegisterModel model = new EmployeeRegisterModel();
            PrepareRegisterModel(model);
            return View(model);
        }

        public ActionResult AllEmployees()
        {
            var StaffManager = factory.CreateStaffsManager();
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            string lang = "";
            if (ViewBag.CurrentLanguage == Languges.English)
            {
                lang = "en";
            }
            else
            {
                lang = "ar";
            }
            var AllStafs = StaffManager.GetEmployees(SchoolID, lang);
            return View(AllStafs);
        }

        public ActionResult EditEmployee(string StaffID)
        {
            EmployeeRegisterModel model = new EmployeeRegisterModel();
            string query = string.Empty;

            var StaffManager = factory.CreateStaffsManager();
            var StaffDetailsManager = factory.CreateStaffJobDetailsManager();
            var StaffContactManager = factory.CreateStaffContactDetailsManager();
            var StaffSalaryManager = factory.CreateStaffSalaryDetailsManager();
            var StaffBankManager = factory.CreateStaffBankDetailsManager();
            var CountryManager = factory.CreateCountrysManager();

            var staffresult = StaffManager.Find(a => a.StaffID == StaffID).FirstOrDefault();
            var staffdetailsresult = StaffDetailsManager.Find(a => a.StaffID == StaffID).FirstOrDefault();
            var staffcontactresult = StaffContactManager.Find(a => a.StaffID == StaffID).FirstOrDefault();
            var staffsalaryresult = StaffSalaryManager.Find(a => a.StaffID == StaffID).FirstOrDefault();
            //var staffbankresult = StaffBankManager.Find(a => a.StaffID == StaffID).FirstOrDefault();

            var staffbankresult = new StaffBankDetail();
            query = "SELECT * FROM StaffBankDetails WHERE StaffID = @StaffID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StaffID", StaffID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            staffbankresult.StaffID = reader["StaffID"].ToString();
                            staffbankresult.AccountName = reader["AccountName"].ToString();
                            staffbankresult.AccountNumber = reader["AccountNumber"].ToString();
                            staffbankresult.Bank = reader["Bank"].ToString();
                            staffbankresult.Branch = reader["Branch"].ToString();
                            staffbankresult.IBANCode = reader["IBANCode"].ToString();
                        }
                    }
                }
            }

            string EnglishFullName = staffresult.StaffEnglishName;
            string ArabicFullName = staffresult.StaffArabicName;
            var arabicname = ArabicFullName.Split(' ');
            var englishname = EnglishFullName.Split(' ');
            PrepareRegisterModel(model);
            var flag = CountryManager.Find(a => a.ID == staffresult.Nationality).Select(a => a.Flag).FirstOrDefault();
            model.FirstArabicName = Regex.Replace(arabicname[0], "[-]+", " ", RegexOptions.Compiled);
            model.SecondArabicName = Regex.Replace(arabicname[1], "[-]+", " ", RegexOptions.Compiled);
            model.ThirdArabicName = Regex.Replace(arabicname[2], "[-]+", " ", RegexOptions.Compiled);
            model.FourthArabicName = Regex.Replace(arabicname[3], "[-]+", " ", RegexOptions.Compiled);
            model.FirstEnglishName = Regex.Replace(englishname[0], "[-]+", " ", RegexOptions.Compiled);
            model.SecondEnglishName = Regex.Replace(englishname[1], "[-]+", " ", RegexOptions.Compiled);
            model.ThirdEnglishName = Regex.Replace(englishname[2], "[-]+", " ", RegexOptions.Compiled);
            model.FourthEnglishName = Regex.Replace(englishname[3], "[-]+", " ", RegexOptions.Compiled);
            model.Gender = staffresult.Gender;
            model.Nationality = staffresult.Nationality;
            model.NationalNumber = staffresult.NationalNumber;
            model.DateofBirth = staffresult.DateofBirth.Value.ToShortDateString();
            //model.MaritalStatus = staffresult.MaritalStatus + 1;
            model.MaritalStatus = staffresult.MaritalStatus;

            byte[] fileBytes = null;
            string fileExtesion = "";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                query = "SELECT Attachment, Ext FROM Staff " +
                    "WHERE StaffID = @StaffID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@StaffID", StaffID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["Attachment"] != DBNull.Value && reader["Ext"] != DBNull.Value)
                            {
                                fileBytes = Convert.FromBase64String(reader["Attachment"].ToString());
                                fileExtesion = reader["Ext"].ToString();
                            }
                        }
                    }
                }
                conn.Close();
                conn.Dispose();
            }
            model.Attachment = fileBytes?.ToString() ?? null;
            model.Ext = fileExtesion;
            model.ResidencyNumber = staffresult.ResidencyNumber;
            model.CivilNumber = staffresult.CivilNumber;
            model.PassportNumber = staffresult.PassportNumber;
            model.Photo = staffresult.Photo;
            model.StaffID = staffresult.StaffID;
            model.Department = staffdetailsresult.Department;
            model.Designation = staffdetailsresult.Designation;
            model.DateOfJoining = staffdetailsresult.DateOfJoining.Value.ToShortDateString();
            model.Qualification = staffdetailsresult.Qualification + 1;
            model.Specialization = staffdetailsresult.Specialization;
            model.YearOfExperience = staffdetailsresult.YearOfExperience == null? 0 : staffdetailsresult.YearOfExperience.Value;
            model.YearofGraduation = staffdetailsresult.YearofGraduation.Value;
            model.Institution = staffdetailsresult.Institution;
            model.UseSchoolTransportation = staffdetailsresult.UseSchoolTransportation.Value;
            model.BusAttendance = staffdetailsresult.BusAttendance.Value;
            model.ComingTourID = staffdetailsresult.ComingTourID;
            model.GoingTourID = staffdetailsresult.GoingTourID;
            model.ComingTourOrder = staffdetailsresult.ComingTourOrder.Value;
            model.GoingTourID = staffdetailsresult.GoingTourID.Value;
            model.LeaveswithSchoolBus = staffdetailsresult.LeaveswithSchoolBus.Value;
            model.SchoolStaffNumber = staffdetailsresult.SchoolStaffNumber;
            model.PermanentAddress = staffcontactresult.PermanentAddress;
            model.MobileNo = staffcontactresult.MobileNo;
            model.Email = staffcontactresult.Email;
            model.Country = staffcontactresult.Country;
            model.City = staffcontactresult.City;
            model.Street = staffcontactresult.Street;
            model.Longitude = staffcontactresult.Longitude;
            model.Latitude = staffcontactresult.Latitude;
            model.BasicSalary = staffsalaryresult.BasicSalary;
            model.HousingAllowence = staffsalaryresult.HousingAllowence;
            model.TransportationAllowence = staffsalaryresult.TransportationAllowence;
            model.OtherAllowence = staffsalaryresult.OtherAllowence;
            model.SocialInsuranceNumber = staffsalaryresult.SocialInsuranceNumber;
            model.SocialInsuranceValue = staffsalaryresult.SocialInsuranceValue;
            model.IncomeTaxNumber = staffsalaryresult.IncomeTaxNumber;
            model.IncomeTaxValue = staffsalaryresult.IncomeTaxValue;
            model.AccountName = staffbankresult.AccountName;
            model.AccountNumber = staffbankresult.AccountNumber;

            var BankManager = factory.CreateBanksManager();
            var BranchManager = factory.CreateBankBranchsManager();

            var banks = new List<Bank>();
            var bankbranches = new List<BankBranch>();

            query = "SELECT * FROM Banks WHERE CountryID = @CountryID; " +
                "SELECT bb.* FROM BankBranches bb " +
                "INNER JOIN Banks b ON b.BankCode = bb.BankCode " +
                "WHERE b.CountryID = @CountryID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@CountryID", staffcontactresult.Country);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            banks.Add(new Bank
                            {
                                BankCode = reader["BankCode"].ToString(),
                                BankArabicName = reader["BankArabicName"].ToString(),
                                BanKEnglishName = reader["BanKEnglishName"].ToString(),
                                CountryId = Convert.ToInt32(reader["CountryId"].ToString())
                            });
                        }

                        reader.NextResult();

                        while (reader.Read())
                        {
                            bankbranches.Add(new BankBranch
                            {
                                BranchCode = reader["BranchCode"].ToString(),
                                BankCode = reader["BankCode"].ToString(),
                                BranchArabicName = reader["BranchArabicName"].ToString(),
                                BranchEnglishName = reader["BranchEnglishName"].ToString(),
                                BranchAddress = reader["BranchAddress"].ToString(),
                                BranchContactNumber = reader["BranchContactNumber"].ToString()
                            });
                        }
                    }
                }
            }

            ViewBag.Banks = banks;
            var Branchs = bankbranches;

            if (staffbankresult.Branch != "-1" || String.IsNullOrEmpty(staffbankresult.Branch))
            {

                var BranchList = (from branch in Branchs
                                  select new LookupDTO
                                  {
                                      Description = branch.BranchEnglishName.ToString() + " - " + branch.BranchAddress,
                                      DescriptionAR = branch.BranchArabicName.ToString() + " - " + branch.BranchAddress,
                                      id = (branch.BranchCode)
                                  }).ToList();
                BranchList.Insert(0, new LookupDTO() { Description = CurrentLanguage == Languges.English ? BranchList.Where(a => a.id == staffbankresult.Branch).Select(a => a.Description).FirstOrDefault() : BranchList.Where(a => a.id == staffbankresult.Branch).Select(a => a.DescriptionAR).FirstOrDefault(), id = BranchList.Where(a => a.id == staffbankresult.Branch).Select(a => a.id).FirstOrDefault() });
                model.BranchList = BranchList;
            }

            model.IBANCode = staffbankresult.IBANCode;
            model.Flag = flag;
            return View(model);
        }
        private void PrepareRegisterModel(EmployeeRegisterModel model)
        {
            var CountriesManager = factory.CreateCountrysManager();
            var MaritialStatusManager = factory.CreateMaritalStatussManager();
            var DepartmentManager = factory.CreateDepartmentsManager();
            var DesgniatioonManager = factory.CreateDesignationsManager();
            var QualificationsManager = factory.CreateQualificationsManager();
            var SpecializationsManager = factory.CreateSpecializationsManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishNationality.ToString(),
                                     DescriptionAR = country.ArabicNationality.ToString(),
                                     ID = (country.ID)
                                 }).ToList();
            model.NationalityList = CountriesList;
            var Martitialstatus = MaritialStatusManager.GetAll();
            var MartitialList = (from maritial in Martitialstatus
                                 select new LookupDTO
                                 {
                                     Description = maritial.EnglishMaritalStatus.ToString(),
                                     DescriptionAR = maritial.ArabicMaritalStatus.ToString(),
                                     ID = (maritial.MaritalStatusID)
                                 }).ToList();
            model.MaritalStatusList = MartitialList;
            int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
            int CompanyID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID));

            var Deparments = DepartmentManager.Find(a => a.SchoolID == SchoolID);
            var DepartmentList = (from dep in Deparments
                                  select new LookupDTO
                                  {
                                      Description = dep.DepartmentEnglishName.ToString(),
                                      DescriptionAR = dep.DepartmentArabicName.ToString(),
                                      ID = (dep.DepartmentID)
                                  }).ToList();
            model.DepartmentsList = DepartmentList;
            var Desginations = DesgniatioonManager.GetAll();
            var DesginationsList = (from des in Desginations
                                    select new LookupDTO
                                    {
                                        Description = des.DesignationEnglishText.ToString(),
                                        DescriptionAR = des.DesignationArabicText.ToString(),
                                        ID = (des.DesignationID)
                                    }).ToList();
            model.DesignationList = DesginationsList;
            var Qualifications = QualificationsManager.GetAll();
            var QualificationList = (from qual in Qualifications
                                     select new LookupDTO
                                     {
                                         Description = qual.QualificationEnglishName.ToString(),
                                         DescriptionAR = qual.QualificationArabicName.ToString(),
                                         ID = (qual.QualificationID)
                                     }).ToList();
            model.QualificationList = QualificationList;
            var Spelizations = SpecializationsManager.GetAll();
            var SpelizationsList = (from spel in Spelizations
                                    select new LookupDTO
                                    {
                                        Description = spel.SpecializationEnglishName.ToString(),
                                        DescriptionAR = spel.SpecializationArabicName.ToString(),
                                        ID = (spel.SpecializationID)
                                    }).ToList();
            model.SpecializationList = SpelizationsList;
            var HeadQuartarManager = factory.CreateHeadquarterssManager();
            var Companies = HeadQuartarManager.GetAll();
            var CompaniesList = (from company in Companies
                                 select new LookupDTO
                                 {
                                     Description = company.CompanyArabicName.ToString(),
                                     DescriptionAR = company.CompanyEnglishName.ToString(),
                                     ID = (company.CompanyID)
                                 }).ToList();
            CompaniesList.Insert(0, new LookupDTO() { Description = CurrentLanguage == Languges.English ? CompaniesList.Where(a => a.ID == CompanyID).Select(a => a.Description).FirstOrDefault() : CompaniesList.Where(a => a.ID == CompanyID).Select(a => a.DescriptionAR).FirstOrDefault(), ID = CompaniesList.Where(a => a.ID == CompanyID).Select(a => a.ID).FirstOrDefault() });

            var BusToursManager = factory.CreateBusToursManager();
            var ComingBusTours = BusToursManager.Find(a => a.SchoolID == SchoolID && a.TourDirection == 0).ToList();
            var GoingBusTours = BusToursManager.Find(a => a.SchoolID == SchoolID && a.TourDirection == 1).ToList();

            var ComingBusToursList = (from coming in ComingBusTours
                                      select new LookupDTO
                                      {
                                          Description = coming.TourEnglishName.ToString(),
                                          DescriptionAR = coming.TourArabicName.ToString(),
                                          ID = (coming.TourID)
                                      }).ToList();
            model.ComingTourList = ComingBusToursList;
            var GoingBusToursList = (from going in GoingBusTours
                                     select new LookupDTO
                                     {
                                         Description = going.TourEnglishName.ToString(),
                                         DescriptionAR = going.TourArabicName.ToString(),
                                         ID = (going.TourID)
                                     }).ToList();
            model.GoingTourList = GoingBusToursList;
            var CountryList = (from country in Countries
                               select new LookupDTO
                               {
                                   Description = country.EnglishName.ToString(),
                                   DescriptionAR = country.ArabicName.ToString(),
                                   ID = (country.ID)
                               }).ToList();
            model.CountriesList = CountryList;
       

            //var banks = new List<Bank>();

            //string query = "SELECT * FROM Banks";
            //using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            //{
            //    conn.Open();
            //    using (SqlCommand comm = new SqlCommand(query, conn))
            //    {
            //        using (SqlDataReader reader = comm.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                banks.Add(new Bank
            //                {
            //                    BankCode = reader["BankCode"].ToString(),
            //                    BankArabicName = reader["BankArabicName"].ToString(),
            //                    BanKEnglishName = reader["BanKEnglishName"].ToString(),
            //                    CountryId = Convert.ToInt32(reader["CountryId"].ToString())
            //                });
            //            }
            //        }
            //    }
            //}
            //var BanksList = (from bank in banks
            //                 select new LookupDTO
            //                 {
            //                     Description = bank.BanKEnglishName.ToString(),
            //                     DescriptionAR = bank.BankArabicName.ToString(),
            //                     id = (bank.BankCode.ToString())
            //                 }).ToList();

            //model.BanksList = BanksList;
            model.CountriesList = CountryList;
            model.UseSchoolTransportation = 0;
            model.BusAttendance = 0;
            model.LeaveswithSchoolBus = 0;
            model.DateOfJoining = DateTime.Now.Date.ToShortDateString();
            model.DateofBirth = DateTime.Now.Date.ToShortDateString();
        }
        [HttpGet]
        public JsonResult GetCountryFlagByCountryID(int CountryID)
        {
            var CountryManager = factory.CreateCountrysManager();
            var flag = CountryManager.Find(a => a.ID == CountryID).Select(a => a.Flag).FirstOrDefault();
            var base64 = Convert.ToBase64String(flag);
            var imgSrc = String.Format("data:image/*;base64,{0}", base64);
            return Json(new { Success = true, src = imgSrc }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetBankBranchesByBankCode(string BankCode)
        {
            //var BanksBranchesManager = factory.CreateBankBranchsManager();
            //var BanksBranchs = BanksBranchesManager.Find(a => a.BankCode == BankCode).ToList();
            try
            {
                var BanksBranchs = new List<BankBranch>();

                string query = "SELECT * FROM BankBranches WHERE BankCode = @BankCode";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@BankCode", BankCode);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BanksBranchs.Add(new BankBranch
                                {
                                    BranchCode = reader["BranchCode"].ToString(),
                                    BankCode = reader["BankCode"].ToString(),
                                    BranchArabicName = reader["BranchArabicName"].ToString(),
                                    BranchEnglishName = reader["BranchEnglishName"].ToString(),
                                    BranchAddress = reader["BranchAddress"].ToString(),
                                    BranchContactNumber = reader["BranchContactNumber"].ToString()
                                });
                            }
                        }
                    }
                }

                return Json(new { Success = true, result = BanksBranchs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = true, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetBankByCountryId(string countryId)
        {
            try
            {
                var banks = new List<Bank>();

                string query = "SELECT * FROM Banks WHERE CountryId = @CountryId";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@CountryId", countryId);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                banks.Add(new Bank
                                {
                                    BankCode = reader["BankCode"].ToString(),
                                    BankArabicName = reader["BankArabicName"].ToString(),
                                    BanKEnglishName = reader["BanKEnglishName"].ToString(),
                                    CountryId = Convert.ToInt32(reader["CountryId"].ToString())
                                });
                            }
                        }
                    }
                }

                return Json(new { Success = true, Banks = banks }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult EditEmployee(EmployeeRegisterModel model)
        {
            try
            {
                int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                string query = string.Empty;
                byte[] imgByte = null;

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader.ContentLength > 0)
                    {
                        imgByte = new Byte[fileuploader.ContentLength];
                        //force the control to load data in array
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }

                }
                var StaffManager = factory.CreateStaffsManager();
                var StaffDetailsManager = factory.CreateStaffJobDetailsManager();
                var StaffContactManager = factory.CreateStaffContactDetailsManager();
                var StaffSalaryManager = factory.CreateStaffSalaryDetailsManager();
                var StaffBankManager = factory.CreateStaffBankDetailsManager();
                var CountryManager = factory.CreateCountrysManager();

                var staffresult = StaffManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();
                if (model.ThirdArabicName == null)
                    model.ThirdArabicName = "";
                if (model.ThirdEnglishName == null)
                    model.ThirdEnglishName = "";

                staffresult.StaffArabicName = model.FirstArabicName.Trim().Replace(" ", "-") + " " + model.SecondArabicName.Trim().Replace(" ", "-") + " " + model.ThirdArabicName.Trim().Replace(" ", "-") + " " + model.FourthArabicName.Trim().Replace(" ", "-");
                staffresult.StaffEnglishName = model.FirstEnglishName.Trim().Replace(" ", "-") + " " + model.SecondEnglishName.Trim().Replace(" ", "-") + " " + model.ThirdEnglishName.Trim().Replace(" ", "-") + " " + model.FourthEnglishName.Trim().Replace(" ", "-");
                staffresult.Gender = model.Gender;
                staffresult.Nationality = model.Nationality;
                staffresult.NationalNumber = model.NationalNumber == null ? "" : model.NationalNumber;
                staffresult.DateofBirth = Convert.ToDateTime(model.DateofBirth);
                //staffresult.MaritalStatus = model.MaritalStatus - 1;
                staffresult.MaritalStatus = model.MaritalStatus;
                staffresult.ResidencyNumber = model.ResidencyNumber == null ? "" : model.ResidencyNumber;
                staffresult.CivilNumber = model.CivilNumber == null ? "" : model.CivilNumber;
                staffresult.PassportNumber = model.PassportNumber;
                if (staffresult.Photo != null && imgByte == null)
                {
                    staffresult.Photo = staffresult.Photo;
                }
                else
                {
                    staffresult.Photo = imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "DesktopCamera.PNG")) : imgByte;
                }
                staffresult.Photo_A = "NOPHOTO.jpeg";
                StaffManager.Update(staffresult);

                if (!string.IsNullOrEmpty(model.Attachment) && model.Attachment != "null")
                {
                    query = "UPDATE Staff SET " +
                                "Attachment = CONVERT(VARBINARY(MAX), @Attachment), Ext = @Ext " +
                                "WHERE StaffID = @StaffID";

                    using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                    {
                        conn.Open();
                        using (SqlCommand comm = new SqlCommand(query, conn))
                        {
                            byte[] fileData = null;
                            if (!string.IsNullOrEmpty(model.Attachment) && model.Attachment != "null")
                            {
                                string[] attachmentParts = model.Attachment.Split(',');

                                if (attachmentParts.Length > 1)
                                {
                                    string base64String = attachmentParts[1];
                                    fileData = Convert.FromBase64String(base64String);

                                    if (fileData.Length > maxFileSizeInBytes)
                                        return Json(new { Success = false, Message = "File size exceeds the maximum allowed size." }, JsonRequestBehavior.AllowGet);

                                    comm.Parameters.AddWithValue("@Attachment", base64String);

                                    // The model.Ext now is holding the file path.
                                    string filePath = @"" + model.Ext + "";
                                    string fileName = Path.GetFileName(filePath);
                                    string fileExtension = Path.GetExtension(fileName);
                                    comm.Parameters.AddWithValue("@Ext", fileExtension);
                                }
                                else
                                {
                                    comm.Parameters.Add("@Attachment", SqlDbType.VarBinary).Value = DBNull.Value;
                                }
                            }

                            comm.Parameters.AddWithValue("@StaffID", model.StaffID);
                            comm.ExecuteNonQuery();
                        }

                        conn.Close();
                        conn.Dispose();
                    }
                }

                var staffdetailsresult = StaffDetailsManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();
                staffdetailsresult.Department = model.Department;
                staffdetailsresult.Designation = model.Designation;
                staffdetailsresult.DateOfJoining = Convert.ToDateTime(model.DateOfJoining);
                staffdetailsresult.Qualification = model.Qualification - 1;
                staffdetailsresult.Specialization = model.Specialization;
                staffdetailsresult.YearOfExperience = model.YearOfExperience;
                staffdetailsresult.YearofGraduation = model.YearofGraduation;
                staffdetailsresult.Institution = model.Institution;
                staffdetailsresult.UseSchoolTransportation = model.UseSchoolTransportation;
                staffdetailsresult.BusAttendance = model.BusAttendance;
                staffdetailsresult.ComingTourID = model.ComingTourID;
                staffdetailsresult.GoingTourID = model.GoingTourID;
                staffdetailsresult.ComingTourOrder = model.ComingTourOrder;
                staffdetailsresult.GoingTourID = model.GoingTourID.Value;
                staffdetailsresult.LeaveswithSchoolBus = model.LeaveswithSchoolBus;
                staffdetailsresult.SchoolStaffNumber = model.SchoolStaffNumber == null ? "" : model.SchoolStaffNumber;
                StaffDetailsManager.Update(staffdetailsresult);

                var staffcontactresult = StaffContactManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();
                staffcontactresult.PermanentAddress = model.PermanentAddress == null ? "" : model.PermanentAddress;
                staffcontactresult.MobileNo = model.MobileNo;
                staffcontactresult.Email = model.Email == null ? "" : model.Email;
                staffcontactresult.Country = model.Country;
                staffcontactresult.City = model.City;
                staffcontactresult.Street = model.Street;
                staffcontactresult.Longitude = model.Longitude;
                staffcontactresult.Latitude = model.Latitude;
                StaffContactManager.Update(staffcontactresult);

                var staffsalaryresult = StaffSalaryManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();
                staffsalaryresult.BasicSalary = model.BasicSalary;
                staffsalaryresult.HousingAllowence = model.HousingAllowence;
                staffsalaryresult.TransportationAllowence = model.TransportationAllowence;
                staffsalaryresult.OtherAllowence = model.OtherAllowence;
                staffsalaryresult.SocialInsuranceNumber = model.SocialInsuranceNumber;
                staffsalaryresult.SocialInsuranceValue = model.SocialInsuranceValue;
                staffsalaryresult.IncomeTaxNumber = model.IncomeTaxNumber;
                StaffSalaryManager.Update(staffsalaryresult);

                //var staffbankresult = StaffBankManager.Find(a => a.StaffID == model.StaffID).FirstOrDefault();

                var staffbankresult = new StaffBankDetail();
                query = "SELECT * FROM StaffBankDetails WHERE StaffID = @StaffID";
                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@StaffID", model.StaffID);
                        using (SqlDataReader reader = comm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                staffbankresult.StaffID = reader["StaffID"].ToString();
                                staffbankresult.AccountName = reader["AccountName"].ToString();
                                staffbankresult.AccountNumber = reader["AccountNumber"].ToString();
                                staffbankresult.Bank = reader["Bank"].ToString();
                                staffbankresult.Branch = reader["Branch"].ToString();
                                staffbankresult.IBANCode = reader["IBANCode"].ToString();
                            }
                        }
                    }
                }
                staffbankresult.AccountName = model.AccountName == null ? "" : model.AccountName;
                staffbankresult.AccountNumber = model.AccountNumber == null ? "" : model.AccountNumber;
                staffbankresult.Bank = model.Bank == null || model.Bank == "null" || model.Bank == "0" ? "-1" : model.Bank;
                staffbankresult.Branch = model.Branch == "" || model.Branch == "null" || model.Branch == "0" ? "-1" : model.Branch;
                staffbankresult.IBANCode = model.IBANCode == null ? "" : model.IBANCode;
                StaffBankManager.Update(staffbankresult);
                //return Json(response, JsonRequestBehavior.AllowGet);
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RegisterEmployee(EmployeeRegisterModel model)
        {
            try
            {
                int SchoolID = Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));
                byte[] imgByte = null;

                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileuploader = System.Web.HttpContext.Current.Request.Files["file"];
                    if (fileuploader.ContentLength > 0)
                    {
                        imgByte = new Byte[fileuploader.ContentLength];
                        //force the control to load data in array
                        fileuploader.InputStream.Read(imgByte, 0, fileuploader.ContentLength);
                    }

                }

                #region Add to Staff 

                var SystemSettingsManager = factory.CreateSystemSettingsManager();

                var StaffManager = factory.CreateStaffsManager();
                var Joiningyear = DateTime.Now.Year.ToString();
                var StaffSchoolID = SchoolID.ToString() + "000";
                var settings = SystemSettingsManager.GetAll().FirstOrDefault();
                var laststaffID = settings.LastStaffID + 1;
                string StaffID = Joiningyear + StaffSchoolID + "0" + laststaffID.ToString();

                Staff staff = new Staff();
                staff.StaffID = StaffID;
                if (model.ThirdArabicName == null)
                    model.ThirdArabicName = "";
                if (model.ThirdEnglishName == null)
                    model.ThirdEnglishName = "";
                staff.StaffArabicName = model.FirstArabicName.Trim().Replace(" ", "-") + " " + model.SecondArabicName.Trim().Replace(" ", "-") + " " + model.ThirdArabicName.Trim().Replace(" ", "-") + " " + model.FourthArabicName.Trim().Replace(" ", "-");
                staff.StaffEnglishName = model.FirstEnglishName.Trim().Replace(" ", "-") + " " + model.SecondEnglishName.Trim().Replace(" ", "-") + " " + model.ThirdEnglishName.Trim().Replace(" ", "-") + " " + model.FourthEnglishName.Trim().Replace(" ", "-");
                staff.Gender = model.Gender;
                staff.Nationality = model.Nationality;
                staff.NationalNumber = model.NationalNumber == null ? "" : model.NationalNumber;
                staff.DateofBirth = Convert.ToDateTime(model.DateofBirth);
                //staff.MaritalStatus = model.MaritalStatus - 1;
                staff.MaritalStatus = model.MaritalStatus;
                staff.ResidencyNumber = model.ResidencyNumber == null ? "" : model.ResidencyNumber;
                staff.CivilNumber = model.CivilNumber == null ? "" : model.CivilNumber;
                staff.PassportNumber = model.PassportNumber;
                staff.Active = -1;

                staff.Photo = imgByte == null ? System.IO.File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "DesktopCamera.PNG")) : imgByte;
                staff.Photo_A = "NOPHOTO.jpeg";
                StaffManager.Add(staff);

                string query = "UPDATE Staff SET " +
                    "Attachment = @Attachment, Ext = @Ext " +
                    "WHERE StaffID = @StaffID";

                using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
                {
                    conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        byte[] fileData = null;

                        if (!string.IsNullOrEmpty(model.Attachment))
                        {
                            string[] attachmentParts = model.Attachment.Split(',');

                            if (attachmentParts.Length > 1)
                            {
                                string base64String = attachmentParts[1];
                                fileData = Convert.FromBase64String(base64String);

                                if (fileData.Length > maxFileSizeInBytes)
                                    return Json(new { Success = false, Message = "File size exceeds the maximum allowed size." }, JsonRequestBehavior.AllowGet);

                                comm.Parameters.AddWithValue("@Attachment", base64String);

                                // The model.Ext now is holding the file path.
                                string filePath = @"" + model.Ext + "";
                                string fileName = Path.GetFileName(filePath);
                                string fileExtension = Path.GetExtension(fileName);
                                comm.Parameters.AddWithValue("@Ext", fileExtension);
                            }
                            else
                            {
                                comm.Parameters.Add("@Attachment", SqlDbType.VarBinary).Value = DBNull.Value;
                                comm.Parameters.Add("@Ext", SqlDbType.VarChar).Value = DBNull.Value;
                            }
                        }
                        else
                        {
                            comm.Parameters.Add("@Attachment", SqlDbType.VarBinary).Value = DBNull.Value;
                            comm.Parameters.Add("@Ext", SqlDbType.VarChar).Value = DBNull.Value;
                        }
                        comm.Parameters.AddWithValue("@StaffID", staff.StaffID);
                        comm.ExecuteNonQuery();
                    }

                    conn.Close();
                    conn.Dispose();
                }

                settings.LastStaffID = laststaffID;
                SystemSettingsManager.Update(settings);
                #endregion

                #region Add to StaffJobDetails 
                var StaffDetailsManager = factory.CreateStaffJobDetailsManager();

                StaffJobDetail staffjobdetail = new StaffJobDetail();
                staffjobdetail.StaffID = StaffID.ToString();
                staffjobdetail.SchoolID = SchoolID;
                staffjobdetail.Department = model.Department;
                staffjobdetail.Designation = model.Designation;
                staffjobdetail.DateOfJoining = Convert.ToDateTime(model.DateOfJoining);
                staffjobdetail.Qualification = model.Qualification - 1;
                staffjobdetail.Specialization = model.Specialization;
                staffjobdetail.YearOfExperience = model.YearOfExperience;
                staffjobdetail.YearofGraduation = model.YearofGraduation;
                staffjobdetail.Institution = model.Institution == null ? "" : model.Institution;
                staffjobdetail.Status = model.Status;
                staffjobdetail.UseSchoolTransportation = model.UseSchoolTransportation;
                staffjobdetail.BusAttendance = model.BusAttendance;
                staffjobdetail.ComingTourID = model.ComingTourID;
                staffjobdetail.GoingTourID = model.GoingTourID;
                staffjobdetail.ComingTourOrder = model.ComingTourOrder;
                staffjobdetail.LeaveswithSchoolBus = model.LeaveswithSchoolBus;
                staffjobdetail.GoingTourOrder = model.GoingTourOrder;
                staffjobdetail.SchoolStaffNumber = model.SchoolStaffNumber == null ? "" : model.SchoolStaffNumber;
                StaffDetailsManager.Add(staffjobdetail);
                #endregion

                #region Add to StaffContactDetails
                var StaffContactManager = factory.CreateStaffContactDetailsManager();

                StaffContactDetail contactdetail = new StaffContactDetail();
                contactdetail.StaffID = StaffID.ToString();
                contactdetail.PermanentAddress = model.PermanentAddress == null ? "" : model.PermanentAddress;
                contactdetail.MobileNo = model.MobileNo == null ? "" : model.MobileNo;
                contactdetail.Email = model.Email == null ? "" : model.Email;
                contactdetail.Country = model.Country;
                contactdetail.City = model.City;
                contactdetail.Street = model.Street == null ? "" : model.Street;
                contactdetail.Longitude = model.Longitude == null ? 0 : model.Longitude;
                contactdetail.Latitude = model.Latitude == null ? 0 : model.Latitude;
                StaffContactManager.Add(contactdetail);
                #endregion

                #region add StaffSalaryDetails
                var StaffSalaryManager = factory.CreateStaffSalaryDetailsManager();
                StaffSalaryDetail staffsalary = new StaffSalaryDetail();
                staffsalary.StaffID = StaffID.ToString();
                staffsalary.BasicSalary = model.BasicSalary;
                staffsalary.HousingAllowence = model.HousingAllowence;
                staffsalary.TransportationAllowence = model.TransportationAllowence;
                staffsalary.OtherAllowence = model.OtherAllowence;
                staffsalary.SocialInsuranceNumber = model.SocialInsuranceNumber;
                staffsalary.SocialInsuranceValue = model.SocialInsuranceValue;
                staffsalary.IncomeTaxNumber = model.IncomeTaxNumber;
                staffsalary.IncomeTaxValue = model.IncomeTaxValue;
                StaffSalaryManager.Add(staffsalary);
                #endregion

                #region add StaffBankDetails
                var StaffBankManager = factory.CreateStaffBankDetailsManager();
                StaffBankDetail bank = new StaffBankDetail();
                bank.StaffID = StaffID.ToString();
                bank.AccountName = model.AccountName == null ? "" : model.AccountName;
                bank.AccountNumber = model.AccountNumber == null ? "" : model.AccountNumber;
                bank.Bank = model.Bank == null || model.Bank == "null" || model.Bank == "0" ? "-1" : model.Bank;
                bank.Branch = model.Branch == " " || model.Branch == "null" || model.Branch == "0" ? "-1" : model.Branch;
                bank.IBANCode = model.IBANCode == null ? "" : model.IBANCode;
                StaffBankManager.Add(bank);

                #endregion

                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBankCodebyStaffID(string StaffID)
        {
            //var BankManager = factory.CreateStaffBankDetailsManager();
            //string BankCode = BankManager.Find(a => a.StaffID == StaffID).Select(a => a.Bank).FirstOrDefault();
            string BankCode = string.Empty;

            string query = "SELECT Bank FROM StaffBankDetails WHERE StaffID = @StaffID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StaffID", StaffID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if(reader.Read())
                            BankCode = reader["Bank"].ToString();
                    }
                }
            }

            return Json(new { Success = true, BankCode = BankCode }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchCodebyStaffID(string StaffID)
        {
            //var BankManager = factory.CreateStaffBankDetailsManager();
            //string Branch = BankManager.Find(a => a.StaffID == StaffID).Select(a => a.Branch).FirstOrDefault();

            string Branch = string.Empty;

            string query = "SELECT Branch FROM StaffBankDetails WHERE StaffID = @StaffID";
            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@StaffID", StaffID);
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        if (reader.Read())
                            Branch = reader["Branch"].ToString();
                    }
                }
            }

            return Json(new { Success = true, BranchCode = Branch }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmployee(string NationalNumber, string ResidencyNumber)
        {
            var StaffManager = factory.CreateStaffsManager();
            string result = "";
            if (!string.IsNullOrEmpty(NationalNumber))
            {
                result = StaffManager.Find(a => a.NationalNumber == NationalNumber).Select(a => a.NationalNumber).FirstOrDefault();
            }
            else
            {
                result = StaffManager.Find(a => a.ResidencyNumber == ResidencyNumber).Select(a => a.ResidencyNumber).FirstOrDefault();
            }
            return Json(new { Success = true, result = result }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string staffID)
        {
            byte[] fileBytes;
            string staffName, fileExtesion;

            using (SqlConnection conn = new SqlConnection(ConnectionString.ConnStr()))
            {
                conn.Open();

                string query = "SELECT StaffArabicName, StaffEnglishName, Attachment, Ext FROM Staff " +
                    "WHERE StaffID = @StaffID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@StaffID", staffID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (reader["Attachment"] != DBNull.Value && reader["Ext"] != DBNull.Value)
                            {
                                staffName = CurrentLanguage == Languges.English ?
                                                        reader["StaffEnglishName"].ToString() : reader["StaffArabicName"].ToString();
                                fileBytes = Convert.FromBase64String(reader["Attachment"].ToString());
                                fileExtesion = reader["Ext"].ToString();
                            }
                            else
                                return HttpNotFound();
                        }
                        else
                            return HttpNotFound();
                    }
                }
                conn.Close();
                conn.Dispose();
            }

            string contentType = "application/pdf";
            return File(fileBytes, contentType, staffName.Trim().Replace(" ", "-") + "-Attachment" + fileExtesion);

        }
    }
}