using Common.Helpers;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Settings
{
    public class HeadQuarterRegisterModel
    {
        //Headquarter
        public int CompanyID { get; set; }
        public string CompanyArabicName { get; set; }
        public string CompanyEnglishName { get; set; }
        public Nullable<int> Country { get; set; }
        public List<LookupDTO> CountryList { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ContactNo { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
        public string ProductKey { get; set; }

        //Users
        public int UserID { get; set; }
        public string StaffID { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }
        public int SchoolID { get; set; }
        public string NewOrEdit { get; set; }
        public string UserHeadQuarterDecoreText { get; set; }
    }
}