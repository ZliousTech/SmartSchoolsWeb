using Common.Helpers;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Registration
{
    public class NewParentModel
    {
        public string FirstArabicName { get; set; }
        public string SecondArabicName { get; set; }
        public string ThirdArabicName { get; set; }
        public string FourthArabicName { get; set; }
        public string FirstEnglishName { get; set; }
        public string SecondEnglishName { get; set; }
        public string ThirdEnglishName { get; set; }
        public string FourthEnglishName { get; set; }
        public string StaffArabicName { get; set; }
        public string StaffEnglishName { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> Nationality { get; set; }
        public Nullable<int> CountryKey { get; set; }
        public List<LookupDTO> NationalityList { get; set; }
        public List<LookupDTO> CountryKeysList { get; set; }
        public string NationalNumber { get; set; }
        public string ResidencyNumber { get; set; }
        public string CivilNumber { get; set; }
        public string MobileNo { get; set; }
        public string Successmsg { get; set; }
        public string Errormsg { get; set; }
    }
}