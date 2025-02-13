using Common.Helpers;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Account
{
    public class AccountLoginModel
    {
        public AccountLoginModel()
        {
            Errormsg = null;
            ErrormsgGrdn = null;
            ErrormsgStd = null;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Errormsg { get; set; }
        public string ErrormsgGrdn { get; set; }
        public string ErrormsgStd { get; set; }
        public bool loginasparent { get; set; }
        public bool loginasStudent { get; set; }
        public List<LookupDTO> CountryKeysList { get; set; }
        public string NationalNoForget { get; set; }
        public string MobileNoForget { get; set; }
        public Nullable<int> CountryKeyID { get; set; }
    }

    public class Encryption
    {
        public int value { get; set; }
        public string input { get; set; }
        public string result { get; set; }
        public string key { get; set; }
    }
}