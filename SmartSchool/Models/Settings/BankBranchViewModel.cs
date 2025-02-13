using Microsoft.Data.OData.Query.SemanticAst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class BankBranchViewModel
    {
        public int BranchCode { get; set; }
        public int BankCode { get; set; }
        public string BankName { get; set; }
        public List<SelectListItem> Banks { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public string BranchArabicName { get; set; }
        public string BranchEnglishName { get; set; }
        public string BranchAddress { get; set; }
        public string BranchContactNumber { get; set; }
    }
}