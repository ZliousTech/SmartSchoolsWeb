using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class BankViewModel
    {
        public int BankCode { get; set; }
        public string BankArabicName { get; set; }
        public string BankEnglishName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}