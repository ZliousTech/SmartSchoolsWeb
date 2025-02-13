using Business.Base;
using Common.Base;
using DataAccess;
using Objects;
using Objects.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Common.Helpers
{
    public class BaseController : Controller
    {

        private string _lang;
        BusinessComponentsFactory _Factory = new BusinessComponentsFactory();
        SmartSchoolsEntities _context = new SmartSchoolsEntities();

        //private string _DataBaseName;

        //public string DataBaseName
        //{
        //    get { return _DataBaseName; }
        //    set { _DataBaseName = value; }
        //}



        public BaseController(string lang = "")
        {

            _lang = lang;
        }

        public int SchoolID
        {
            get { return Convert.ToInt32(GetCookie((int)clsenumration.UserData.SchoolID));}
        }
        public int CompanyID
        {
            get { return Convert.ToInt32(GetCookie((int)clsenumration.UserData.CompanyID)); }
        }


        public Languges CurrentLanguage
        {
            get;
            set;

        }

        protected override void ExecuteCore()
        {
            string coockieName = "_culture";
            string cultureName = null;

            if (!string.IsNullOrEmpty(_lang))
            {
                CookieHelper.Set(coockieName, _lang);
            }
            else
            {
                // Attempt to read the culture cookie from Request
                if (CookieHelper.Exists(coockieName))
                {
                    cultureName = CookieHelper.Get(coockieName);
                }
                else
                {
                    //Create language coockie
                    CookieHelper.Set(coockieName, "ar");
                }
            }



            if (cultureName == "ar")
            {
                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-JO");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                CurrentLanguage = Languges.Arabic;
            }
            else
            {
                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                CurrentLanguage = Languges.English;
            }

            base.ExecuteCore();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string coockieName = "_culture";
            string cultureName = null;


            if (!string.IsNullOrEmpty(_lang))
            {
                CookieHelper.Set(coockieName, _lang);
                cultureName = _lang;
            }
            else
            {
                // Attempt to read the culture cookie from Request
                if (CookieHelper.Exists(coockieName))
                {
                    cultureName = CookieHelper.Get(coockieName);
                }
                else
                {
                    //Create language coockie
                    CookieHelper.Set(coockieName, "ar");
                }
            }



            if (cultureName == "ar")
            {
                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-JO");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                CurrentLanguage = Languges.Arabic;
            }
            else
            {
                // Modify current thread's cultures            
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                CurrentLanguage = Languges.English;
            }

            ViewBag.CurrentLanguage = CurrentLanguage;

            base.OnActionExecuting(filterContext);
        }

        protected override bool DisableAsyncSupport
        {
            get
            {
                return false;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            ErrorLog.LogException(filterContext.Exception);
            base.OnException(filterContext);
        }

        public static string Encrypt(string Password)
        {
            string sMessage = string.Empty;
            byte[] Encode = new byte[Password.Length - 1 + 1];
            Encode = Encoding.UTF8.GetBytes(Password);
            sMessage = Convert.ToBase64String(Encode);
            return (sMessage);
        }

        public static string Decrypt(string EncryptPassword)
        {
            string DecryptPassword = string.Empty;
            UTF8Encoding EncodePassword = new UTF8Encoding();
            Decoder Decode = EncodePassword.GetDecoder();
            byte[] ToDecode_Byte = Convert.FromBase64String(EncryptPassword);
            int CharCount = Decode.GetCharCount(ToDecode_Byte, 0, ToDecode_Byte.Length);
            char[] Decoded_Char = new char[CharCount - 1 + 1];
            Decode.GetChars(ToDecode_Byte, 0, ToDecode_Byte.Length, Decoded_Char, 0);
            DecryptPassword = new String(Decoded_Char);
            return (DecryptPassword);
        }

        public void setCookie(UserDTO User)
        {
          
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
                (
                1,
                User.Username.ToString(),
                DateTime.Now,
                DateTime.Now.AddHours(10),
                true,
                User.UserID + "$" + User.Username + "$" + User.SchoolID + "$" + User.UserType + "$" + User.GuardianID + "$" + User.StaffID + "$" + User.CompanyID+"$" + User.PreviousSchoolID + "$" + User.StudentID,
                FormsAuthentication.FormsCookiePath

                );
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public string GetCookie(int value)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[cookieName];
            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            string[] UserData = ticket.UserData.Split('$');
            switch (value)
            {
                case (int)clsenumration.UserData.UserID:
                    return UserData[0];
                case (int)clsenumration.UserData.Username:
                    return UserData[1];
                case (int)clsenumration.UserData.SchoolID:
                    return UserData[2];
                case (int)clsenumration.UserData.UserType:
                    return UserData[3];
                case (int)clsenumration.UserData.GuardianID:
                    return UserData[4];
                case (int)clsenumration.UserData.StaffID:
                    return UserData[5];
                case (int)clsenumration.UserData.CompanyID:
                    return UserData[6];
                case (int)clsenumration.UserData.PreviousSchoolID:
                    return UserData[7];
                case (int)clsenumration.UserData.StudentID:
                    return UserData[8];
                default:
                    return "";
            }

        }

        public static string EncryptUrl(string clearText)
        {
            string EncryptionKey = "MHOHD6548NWK015";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string DecryptURL(string cipherText)
        {
            string EncryptionKey = "MHOHD6548NWK015";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        //public HttpResponseMessage SendSMS(string Body, string MobileNumber)
        //{
        //    string mobilenumber = "962" + MobileNumber.TrimStart('0') + ",";
        //    const string baseUri = "http://josmsservice.com/smsonline/msgservicejo.cfm";
        //    const string SenderID = "SmartSchool";
        //    const string AccName = "masera";
        //    //const string AccPass = "O7!oH8!DxF9";
        //    const string AccPass = "ENmS7W94OR__U!0c";
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseUri);

        //        var content = new FormUrlEncodedContent(new[]
        //        {
        //           new KeyValuePair<string, string>("numbers", mobilenumber),
        //           new KeyValuePair<string, string>("senderid", SenderID),
        //           new KeyValuePair<string, string>("AccName", AccName),
        //           new KeyValuePair<string, string>("AccPass", AccPass),
        //           new KeyValuePair<string, string>("msg", Body),
        //           new KeyValuePair<string, string>("requesttimeout", "5000000"),
        //        });

        //        var response = client.PostAsync(baseUri, content);
        //        return response.Result;

        //    }
        //}
        //public HttpResponseMessage SendBulkSMS(string Body, List<string> MobileNumbers)
        //{

        //    string mobilenumber = "";
        //    foreach (var item in MobileNumbers)
        //    {
        //        mobilenumber += "962" + item.TrimStart('0') + ",";
        //    }
        //    const string baseUri = "http://josmsservice.com/smsonline/smppformwwl.cfm";
        //    const string SenderID = "SmartSchool";
        //    const string AccName = "masera";
        //    const string AccPass = "O7!oH8!DxF9";
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(baseUri);

        //        var content = new FormUrlEncodedContent(new[]
        //        {
        //           new KeyValuePair<string, string>("numbers", mobilenumber),
        //           new KeyValuePair<string, string>("senderid", SenderID),
        //           new KeyValuePair<string, string>("AccName", AccName),
        //           new KeyValuePair<string, string>("AccPass", AccPass),
        //           new KeyValuePair<string, string>("msg", Body),
        //           new KeyValuePair<string, string>("requesttimeout", "5000000"),


        //        });

        //        var response = client.PostAsync(baseUri, content);
        //        return response.Result;

        //    }

        //}

        // I set the default value for the countryCode to 962 so that other clients using this function are not affected by this modification.

        public HttpResponseMessage SendSMS(string Body, string MobileNumber, char lang = 'e', string countryCode = "962")
        {
            // mobile number format.
            string mobilenumber = countryCode + (MobileNumber.StartsWith("0") ? MobileNumber.TrimStart('0') : MobileNumber);
            
            // Egypt SMS.
            if (countryCode == "20")
            {
                // Create the SMSID whitch is Guid.
                Guid guid = Guid.NewGuid();
                // SMS API and its Parameters.
                const string baseUri = "https://smsvas.vlserv.com/VLSMSPlatformResellerAPI/NewSendingAPI/api/SMSSender/SendSMS";
                const string username = "Smartschool";
                const string password = "M^Q@5dq}B@";
                string smsText = "";
                try
                {
                    // this line because the format of sms go to ===> RegistrationController - NewParent function.  
                    smsText = Body.Replace("%0a", "\n");
                }
                catch (Exception)
                {
                    smsText = Body;
                }
                char smsLang = lang;
                const string smsSender = "Smartschool";
                string smsReceiver = mobilenumber;
                string smsID = guid.ToString();

                // Featch SMS API.
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUri);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("UserName", username),
                        new KeyValuePair<string, string>("Password", password),
                        new KeyValuePair<string, string>("SMSText", smsText),
                        new KeyValuePair<string, string>("SMSLang", smsLang.ToString()),
                        new KeyValuePair<string, string>("SMSSender", smsSender),
                        new KeyValuePair<string, string>("SMSReceiver", smsReceiver),
                        new KeyValuePair<string, string>("SMSID", smsID),
                    });

                    var response = client.PostAsync(baseUri, content);
                    return response.Result;
                }
            }
            // Jordan SMS.
            else if (countryCode == "962")
            {
                const string baseUri = "http://josmsservice.com/smsonline/msgservicejo.cfm";
                const string SenderID = "SmartSchool";
                const string AccName = "masera";
                //const string AccPass = "O7!oH8!DxF9";
                const string AccPass = "ENmS7W94OR__U!0c";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUri);

                    var content = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("numbers", mobilenumber),
                        new KeyValuePair<string, string>("senderid", SenderID),
                        new KeyValuePair<string, string>("AccName", AccName),
                        new KeyValuePair<string, string>("AccPass", AccPass),
                        new KeyValuePair<string, string>("msg", Body),
                        new KeyValuePair<string, string>("requesttimeout", "5000000"),
                    });

                    var response = client.PostAsync(baseUri, content);
                    return response.Result;
                }
            }
            else
                return null;
        }

        public HttpResponseMessage SendBulkSMS(string Body, List<string> MobileNumbers)
        {
            foreach (var mobilenumner in MobileNumbers)
            {
                string mobilenumber = "";
                foreach (var item in MobileNumbers)
                {
                    mobilenumber += "962" + item.TrimStart('0') + ",";
                }
                const string baseUri = "http://josmsservice.com/smsonline/smppformwwl.cfm";
                const string SenderID = "SmartSchool";
                const string AccName = "masera";
                const string AccPass = "O7!oH8!DxF9";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUri);

                    var content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("numbers", mobilenumber),
                    new KeyValuePair<string, string>("senderid", SenderID),
                    new KeyValuePair<string, string>("AccName", AccName),
                    new KeyValuePair<string, string>("AccPass", AccPass),
                    new KeyValuePair<string, string>("msg", Body),
                    new KeyValuePair<string, string>("requesttimeout", "5000000"),
                });

                    var response = client.PostAsync(baseUri, content);
                    return response.Result;
                }
                
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        public List<LookupDTO> GetQualificationsList()
        {
            var QualificationManager = _Factory.CreateQualificationsManager();
            var Qualifications = QualificationManager.GetAll();
            var QualificationList = (from qual in Qualifications
                                     select new LookupDTO
                                     {
                                         Description = qual.QualificationEnglishName.ToString(),
                                         DescriptionAR = qual.QualificationArabicName.ToString(),
                                         ID = (qual.QualificationID)
                                     }).ToList();
            return QualificationList;
        }

        public List<LookupDTO> GetDiscountTypes()
        {
            var DiscountTypeManager = _Factory.CreateDiscountTypesManager();
            var DiscountTypes = DiscountTypeManager.GetAll();
            var DiscountTypesList = (from Dis in DiscountTypes
                                     select new LookupDTO
                                     {
                                         Description = Dis.DiscountTypeEnglish.ToString(),
                                         DescriptionAR = Dis.DiscountTypeArabic.ToString(),
                                         ID = (Dis.DiscountTypeID)
                                     }).ToList();
            return DiscountTypesList;
        }

        public List<LookupDTO> GetCountriesList()
        {
            var CountriesManager = _Factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishName.ToString(),
                                     DescriptionAR = country.ArabicName.ToString(),
                                     ID = (country.ID)
                                 }).ToList();
            return CountriesList;
        }

        public List<LookupDTO> GetBusTours(int SchoolID,int TourDirection)
        {
            var BusTourManager = _Factory.CreateBusToursManager();
            var Tours = BusTourManager.Find(a=>a.SchoolID == SchoolID && a.TourDirection == TourDirection);
            var ToursList = (from tour in Tours
                                 select new LookupDTO
                                 {
                                     Description = tour.TourEnglishName.ToString(),
                                     DescriptionAR = tour.TourArabicName.ToString(),
                                     ID = (tour.TourID)
                                 }).ToList();
            return ToursList;
        }

        public List<LookupDTO> GetNationalitiesList()
        {
            var CountriesManager = _Factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishNationality.ToString(),
                                     DescriptionAR = country.ArabicNationality.ToString(),
                                     ID = (country.ID)
                                 }).ToList();
            return CountriesList;
        }

        public List<LookupDTO> GetCountryKeysList()
        {
            var CountriesManager = _Factory.CreateCountrysManager();
            var Countries = CountriesManager.GetAll();
            var CountriesList = (from country in Countries
                                 select new LookupDTO
                                 {
                                     Description = country.EnglishNationality.ToString() + " - " + country.CountryCode.ToString(),
                                     DescriptionAR = country.ArabicNationality.ToString() + " - " + country.CountryCode.ToString(),
                                     //ID = int.Parse(country.CountryCode)
                                     ID = (country.ID)
                                 }).ToList();
            return CountriesList;
        }

        public List<LookupDTO> GuardianTypesList()
        {
            var GurdianManager = _Factory.CreateGuardianTypesManager();
            var GurdianTypes = GurdianManager.GetAll();
            var GuardianTypeList = (from type in GurdianTypes
                                 select new LookupDTO
                                 {
                                     Description = type.GuardianTypeinEnglish.ToString(),
                                     DescriptionAR = type.GuardianTypeinArabic.ToString(),
                                     ID = (type.GuardianTypeID)
                                 }).ToList();
            return GuardianTypeList;
        }

        public List<LookupDTO> GetSpecializationsList()
        {
            var SpecializationsManager = _Factory.CreateSpecializationsManager();
            var Spelizations = SpecializationsManager.GetAll();
            var SpelizationsList = (from spel in Spelizations
                                    select new LookupDTO
                                    {
                                        Description = spel.SpecializationEnglishName.ToString(),
                                        DescriptionAR = spel.SpecializationArabicName.ToString(),
                                        ID = (spel.SpecializationID)
                                    }).ToList();

            return SpelizationsList;
        }

        public List<LookupDTO> GetLiveWithTypesList()
        {
            var LiveWithManager = _Factory.CreateLiveswithTypesManager();
            var livewith = LiveWithManager.GetAll();
            var liveList = (from live in livewith
                                    select new LookupDTO
                                    {
                                        Description = live.LivesWithEnglish.ToString(),
                                        DescriptionAR = live.LivesWithArabic.ToString(),
                                        ID = (live.LiveswithID)
                                    }).ToList();

            return liveList;
        }

        public List<LookupDTO> GetBloodTypes()
        {
            var BloodTypesManager = _Factory.CreateBloodTypesManager();
            var BloodTypes = BloodTypesManager.GetAll();
            var BloodTypesList = (from Blood in BloodTypes
                                 select new LookupDTO
                                 {
                                     Description = Blood.BloodType1.ToString(),
                                     DescriptionAR = Blood.BloodType1.ToString(),
                                     ID = (Blood.BloodTypeID)
                                 }).ToList();

            return BloodTypesList;
        }

        public List<LookupDTO> GetPhysicalStatus()
        {
            var PhysicalStatusManager = _Factory.CreateStudentPhysicalStatusManager();
            var PhysicalStatus = PhysicalStatusManager.GetAll();
            var PhysicalStatusList = (from physical in PhysicalStatus
                                  select new LookupDTO
                                  {
                                      Description = physical.PhysicalStatusEnglishText.ToString(),
                                      DescriptionAR = physical.PhysicalStatusArabicText.ToString(),
                                      ID = (physical.PhysicalStatusID)
                                  }).ToList();

            return PhysicalStatusList;
        }

        public List<LookupDTO> GetSpecialResidenceConditionTypes()
        {
            var ResidenceConditionManager = _Factory.CreateSpecialResidenceConditionTypesManager();
            var ResidenceConditionTypes = ResidenceConditionManager.GetAll();
            var ResidenceList = (from Residence in ResidenceConditionTypes
                            select new LookupDTO
                            {
                                Description = Residence.SpecialResidenceConditionEnglish.ToString(),
                                DescriptionAR = Residence.SpecialResidenceConditionArabic.ToString(),
                                ID = (Residence.SpecialResidenceConditionID)
                            }).ToList();

            return ResidenceList;
        }

        public List<LookupDTO> GetCountriesJoinHeadCountry()
        {
            var HeadQuarterManager = _Factory.CreateHeadquarterssManager();
            var HeadQuartersCountryIDs = HeadQuarterManager.GetAll().Select(a => a.Country).Distinct().ToList();
            var Countries = (from country in _context.Countries where HeadQuartersCountryIDs.Contains(country.ID) select country).ToList();
            var CountriesList = (from c in Countries
                               select new LookupDTO
                               {
                                   Description = c.EnglishName.ToString(),
                                   DescriptionAR = c.ArabicName.ToString(),
                                   ID = (c.ID)
                               }).ToList();
            return CountriesList;

        }

        public List<LookupDTO> GetSchoolsByCompanyID(int CompanyID)
        {
            var SchoolbranchesManager = _Factory.CreateSchoolBranchsManager();
            var schools = SchoolbranchesManager.Find(a => a.CompanyID == CompanyID).ToList();
            var schoolsList = (from school in schools
                               select new LookupDTO
                               {
                                   Description = school.SchoolEnglishName.ToString(),
                                   DescriptionAR = school.SchoolArabicName.ToString(),
                                   ID = (school.SchoolID)
                               }).ToList();
            return schoolsList;
        }

        public List<LookupDTO> GetSchoolClassesBySchoolID(int SchoolID)
        {
           var classesList = (from a in _context.SchoolClasses
                                  //join b in _context.Curriculums on a.CurriculumID equals b.CurriculumID
                                  //where b.SchoolID == SchoolID
                              where a.SchoolID == SchoolID
                              select new LookupDTO
                              {
                                  Description = a.SchoolClassEnglishName.ToString(), //+ " - " + b.CurriculumEnglishName.ToString(),
                                  DescriptionAR = a.SchoolClassArabicName.ToString(),// + " - " + b.CurriculumArabicName.ToString(),
                                  ID = (a.SchoolClassID)
                              }).ToList();
            return classesList;
        }

        public List<LookupDTO> GetSectionsBySchoolClassID(int SchoolClassID)
        {
            var SectionsManager = _Factory.CreateSectionsManager();
            var Sections = SectionsManager.Find(a => a.SchoolClassID == SchoolClassID).ToList();
            var sectionsList = (from c in Sections
                                select new LookupDTO
                                {
                                    Description = string.IsNullOrWhiteSpace(c.SectionEnglishName.ToString()) ?
                                    c.SectionArabicName.ToString() : c.SectionEnglishName.ToString(),
                                    DescriptionAR = string.IsNullOrWhiteSpace(c.SectionArabicName.ToString()) ?
                                    c.SectionEnglishName : c.SectionArabicName,
                                    ID = c.SectionID
                                }).ToList();
            return sectionsList;
        }

        public List<LookupDTO> GetSectionsBySchoolID(int SchoolID)
        {
            var SectionsManager = _Factory.CreateSectionsManager();
            var Sections = SectionsManager.Find(a => a.SchoolID == SchoolID).ToList();
            var sectionsList = (from c in Sections
                                select new LookupDTO
                                {
                                    Description = c.SectionEnglishName.ToString(),
                                    DescriptionAR = c.SectionArabicName.ToString(),
                                    ID = (c.SectionID)
                                }).ToList();
            return sectionsList;
        }

        public List<LookupDTO> GetStudentStatus()
        {
            var StudentStatusManager = _Factory.CreateStudentStatusManager();
            var StudentsStatus = StudentStatusManager.GetAll();
            var List = (from c in StudentsStatus
                                select new LookupDTO
                                {
                                    Description = c.StatusEnglishText.ToString(),
                                    DescriptionAR = c.StatusArabicText.ToString(),
                                    ID = (c.StatusID)
                                }).ToList();
            return List;
        }

        public List<LookupDTO> GetStudentResults()
        {
            var StudentResultManager = _Factory.CreateStudentResultsManager();
            var StudentsResults = StudentResultManager.GetAll();
            var List = (from c in StudentsResults
                        select new LookupDTO
                        {
                            Description = c.ResultEnglishText.ToString(),
                            DescriptionAR = c.ResultArabicText.ToString(),
                            ID = (c.ResultID)
                        }).ToList();
            return List;
        }

        public List<LookupDTO> GetHeadQuartersByCountryID(int CountryID)
        {
            var HeadQuartersManager = _Factory.CreateHeadquarterssManager();
            var quartes = HeadQuartersManager.Find(a => a.Country == CountryID).ToList();
            var QuartersList = (from q in quartes
                               select new LookupDTO
                               {
                                   Description = q.CompanyEnglishName.ToString(),
                                   DescriptionAR = q.CompanyArabicName.ToString(),
                                   ID = (q.CompanyID)
                               }).ToList();
            return QuartersList;
        }

        public List<LookupDTO> GetEducationalYears()
        {
            int[] EducationalYearsIDs = { 0, 0 };
            DataTable EducationalYearsInfo = 
                SystemBase.GetDataTble("SELECT TOP 2 EductionalYearID " +
                                       "FROM EducationalYear " +
                                       "ORDER BY EductionalYearID DESC");
            if (EducationalYearsInfo != null)
            {
                if (EducationalYearsInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < EducationalYearsInfo.Rows.Count; i++)
                    {
                        EducationalYearsIDs[i] = int.Parse(EducationalYearsInfo.Rows[i]["EductionalYearID"].ToString());
                    }
                }
            }

            //int EducationalYearsIDs = { 5, 6 };
            var Educationalyears = (from Educationalyear in _context.EducationalYears where EducationalYearsIDs.Contains(Educationalyear.EductionalYearID) select Educationalyear).ToList();
            var EducationalyearsList = (from q in Educationalyears
                                        select new LookupDTO
                                        {
                                            Description = q.EducationalYear1.ToString(),
                                            DescriptionAR = q.EducationalYear1.ToString(),
                                            ID = (q.EductionalYearID)
                                        }).ToList();
            return EducationalyearsList;
        }

        public List<LookupDTO> AcademicSubjects()
        {
            List<LookupDTO> AcademicSubjects = new List<LookupDTO>();
            AcademicSubjects.Add(new LookupDTO
            {
                ID = 1,
                DescriptionAR = "الرياضيات",
                Description = "Math"
            });
            AcademicSubjects.Add(new LookupDTO
            {
                ID = 2,
                DescriptionAR = "اللغة العربية",
                Description = "Arabic Language"
            });
            AcademicSubjects.Add(new LookupDTO
            {
                ID = 3,
                DescriptionAR = "اللغة الأنجليزية",
                Description = "English Language"
            });
            return AcademicSubjects;
        }

        public List<LookupDTO> SessionNameList()
        {
            List<LookupDTO> SessionNameList = new List<LookupDTO>();
            SessionNameList.Add(new LookupDTO
            {
                ID = 1,//SessionDayOrder
                DescriptionAR = "الحصة الأولى",
                Description = "First class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 2,
                DescriptionAR = "الحصة الثانية",
                Description = "Second class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 3,
                DescriptionAR = "الحصة الثالثة",
                Description = "Third class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 4,
                DescriptionAR = "الحصة الرابعة",
                Description = "Fourth class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 5,
                DescriptionAR = "الحصة الخامسة",
                Description = "Fifth  class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 6,
                DescriptionAR = "الحصة السادسة",
                Description = "Sixth  class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 7,
                DescriptionAR = "الحصة السابعة",
                Description = "Seventh class"
            });
            SessionNameList.Add(new LookupDTO
            {
                ID = 8,
                DescriptionAR = "الحصة الثامنة",
                Description = "Eighth class"
            });

            return SessionNameList;
        }

        public static bool AreAllSame(List<double> enumerable)
        {
            if (enumerable.Any(o => o != enumerable[0]))
            {
                return false;
            }
            return true;
        }

        public string getCurrentAcademicYear()
        {
            var currenetYear = _context.SystemSettings.Select(c => c.CurrentAcademicYear).FirstOrDefault();
            return currenetYear;
        }

        public List<LookupDTO> GetSubjectsBySchoolClassID(int SchoolClassID)
        {
            //var SubjectsManger = _Factory.CreateSubjectsManager();
            //var Subjects = SubjectsManger.Find(c => c.SchoolClassID == SchoolClassID && c.SectionID ==-1).ToList();
            //var SubjectsList = (from c in Subjects
            //                    select new LookupDTO
            //                    {
            //                        Description = c.SubjectEnglishName.ToString(),
            //                        DescriptionAR = c.SubjectArabicName.ToString(),
            //                        ID = (c.SubjectID)
            //                    }).ToList();
            //return SubjectsList;

            var SubjectsManger = _Factory.CreateSubjectsManager();
            var Subjects = SubjectsManger.Find(c => c.SchoolClassID == SchoolClassID).ToList();
            var SubjectsList = (from c in Subjects
                                select new LookupDTO
                                {
                                    Description = CurrentLanguage == Languges.English ?
                                    c.SubjectEnglishName.ToString() : c.SubjectArabicName.ToString(),
                                    DescriptionAR = c.SubjectArabicName.ToString(),
                                    ID = (c.SubjectID)
                                }).ToList();
            return SubjectsList;
        }

        public static TeacherTimeTable GetTeacherManualTimetable(string _TeacherID, int _WeekDay, int _SessionDayOrder,int _SchoolID)
        {
            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
            {
                var _SchoolYear = entities.SystemSettings.Select(c => c.CurrentAcademicYear).FirstOrDefault();
                var SchoolID = new SqlParameter("@SchoolID", _SchoolID);
                var SchoolYearID = new SqlParameter("@SchoolYearID", _SchoolYear);
                var TeacherID = new SqlParameter("@TeacherID", _TeacherID);
                var WeekDay = new SqlParameter("@WeekDay", _WeekDay);
                var SessionDayOrder = new SqlParameter("@SessionDayOrder", _SessionDayOrder);

                var results = entities.Database.SqlQuery<TeacherTimeTable>
                    ("exec [dbo].[TeacherManualTimetable] @SchoolID,@SchoolYearID,@TeacherID,@WeekDay,@SessionDayOrder",
                    new Object[] { SchoolID, SchoolYearID, TeacherID, WeekDay, SessionDayOrder });

                return results.FirstOrDefault();
            }

        }

        public static ClassTimeTableDTO GetClassManualTimetable(int _SchoolClassID,int _SectionID, int _WeekDay, int _SessionDayOrder, int _SchoolID)
        {
            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
            {
                var _SchoolYear = entities.SystemSettings.Select(c => c.CurrentAcademicYear).FirstOrDefault();
                var SchoolID = new SqlParameter("@SchoolID", _SchoolID);
                var SchoolYear = new SqlParameter("@SchoolYear", _SchoolYear);
                var SchoolClassID = new SqlParameter("@SchoolClassID", _SchoolClassID);
                var SectionID = new SqlParameter("@SectionID", _SectionID);
                var WeekDay = new SqlParameter("@WeekDay", _WeekDay);
                var SessionDayOrder = new SqlParameter("@SessionDayOrder", _SessionDayOrder);

                var results = entities.Database.SqlQuery<ClassTimeTableDTO>
                    ("exec [dbo].[GetClassManualTimetable] @SchoolID,@SchoolYear,@SchoolClassID,@SectionID,@WeekDay,@SessionDayOrder",
                    new Object[] { SchoolID, SchoolYear, SchoolClassID,SectionID, WeekDay, SessionDayOrder });

                return results.FirstOrDefault();

            }

        }

        public static TeacherTimeTable GetTeacherAutomaticTimetable(string _TeacherID, int _WeekDay, int _SessionDayOrder, int _SchoolID)
        {

            using (SmartSchoolsEntities entities = new SmartSchoolsEntities())
            {
                var _SchoolYear = entities.SystemSettings.Select(c => c.CurrentAcademicYear).FirstOrDefault();
                var SchoolID = new SqlParameter("@SchoolID", _SchoolID);
                var SchoolYearID = new SqlParameter("@SchoolYearID", _SchoolYear);
                var TeacherID = new SqlParameter("@TeacherID", _TeacherID);
                var WeekDay = new SqlParameter("@WeekDay", _WeekDay);
                var SessionDayOrder = new SqlParameter("@SessionDayOrder", _SessionDayOrder);

                var results = entities.Database.SqlQuery<TeacherTimeTable>
                    ("exec [dbo].[TeacherAutomaticTimetable] @SchoolID,@SchoolYearID,@TeacherID,@WeekDay,@SessionDayOrder",
                    new Object[] { SchoolID, SchoolYearID, TeacherID, WeekDay, SessionDayOrder });

                return results.FirstOrDefault();

            }

        }

        public string GenerateStudentID()
        {
            // update last student id in systemsettings table.
            var SystemSettingsManager = _Factory.CreateSystemSettingsManager();
            var SystemSettings = SystemSettingsManager.GetAll()[0];
            int? LastStudentID = SystemSettingsManager.GetAll()[0].LastStudentID;
            SystemSettings.LastStudentID = LastStudentID + 1;
            SystemSettingsManager.Update(SystemSettings);
            int? NextStudentID = LastStudentID + 1;

            return "S" + DateTime.Now.Year + this.SchoolID + NextStudentID?.ToString("0000000");
        }

        //// return the username and password to send them as sms.
        //public string CreateStudentAccount(string studentID, string username)
        //{
        //    try
        //    {
        //        // create the account.
        //        var StudentLoginManager = _Factory.CreateStudent_LoginsManager();
        //        Student_Login obj = new Student_Login();
        //        obj.StudentID = studentID;
        //        obj.Username = username;
        //        obj.Password = Encrypt(RandomString(6,true));
        //        obj.CreationDate = DateTime.Now;
        //        obj.IsBlocked = false;
        //        obj.IsDeleted = false;
        //        obj.SchoolID = this.SchoolID;
        //        StudentLoginManager.Add(obj);

        //        // get the username and password.
        //        StudentLoginManager = _Factory.CreateStudent_LoginsManager();
        //        var StudentLogin = StudentLoginManager.Find(s => s.StudentID == studentID).FirstOrDefault();
        //        return "Student Account:" + "\n" + "Username: " + StudentLogin.Username + "\n" + "Password: " + Decrypt(StudentLogin.Password);
        //        return "تم قبول الطالب" + " " + Request.StudentName + " " + "الرجاء تسجيل الدخول لإستكمال عملية التسجيل" + "%0a" + "Username:" + " " + Guardian.UserName + "%0a" + "Password:" + " " + Password;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}