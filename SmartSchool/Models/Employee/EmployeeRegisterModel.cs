using Common.Helpers;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Employee
{
    public class EmployeeRegisterModel
    {
        //Staff

        public string StaffID { get; set; }
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
        public List<LookupDTO> NationalityList { get; set; }
        public string NationalNumber { get; set; }
        public string DateofBirth { get; set; }
        public Nullable<int> MaritalStatus { get; set; }
        public List<LookupDTO> MaritalStatusList { get; set; }
        //public HttpPostedFileBase fileuploader { get; set; }
        public string Successmsg { get; set; }
        public string Errormsg { get; set; }

        public string ResidencyNumber { get; set; }
        public string CivilNumber { get; set; }
        public string PassportNumber { get; set; }
        public Nullable<int> Active { get; set; }
        public byte[] Photo { get; set; }
        public string Photo_A { get; set; }
        public int SchoolNumber { get; set; }
        public string Attachment { set; get; }
        public string Ext { set; get; }

        //Staff Job Details
        public Nullable<int> SchoolID { get; set; }
        public Nullable<int> Department { get; set; }
        public List<LookupDTO> DepartmentsList { get; set; }
        public Nullable<int> Designation { get; set; }
        public List<LookupDTO> DesignationList { get; set; }
        public string DateOfJoining { get; set; }
        public Nullable<int> Qualification { get; set; }
        public List<LookupDTO> QualificationList { get; set; }
        public Nullable<int> Specialization { get; set; }
        public List<LookupDTO> SpecializationList { get; set; }

        public int YearOfExperience { get; set; }
        public int YearofGraduation { get; set; }
        public string Institution { get; set; }
        public Nullable<int> Status { get; set; }
        public int UseSchoolTransportation { get; set; }
        public int BusAttendance { get; set; }
        public Nullable<int> ComingTourID { get; set; }
        public List<LookupDTO> ComingTourList { get; set; }
        public Nullable<int> GoingTourID { get; set; }
        public List<LookupDTO> GoingTourList { get; set; }

        public int ComingTourOrder { get; set; }
        public int LeaveswithSchoolBus { get; set; }
        public int GoingTourOrder { get; set; }
        public string SchoolStaffNumber { get; set; }

        //Staff Contact Details
        public string PermanentAddress { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public Nullable<int> Country { get; set; }
        public List<LookupDTO> CountriesList { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<double> Latitude { get; set; }


        //Staff Salary Detials
        public Nullable<double> BasicSalary { get; set; }
        public Nullable<double> HousingAllowence { get; set; }
        public Nullable<double> TransportationAllowence { get; set; }
        public Nullable<double> OtherAllowence { get; set; }
        public string SocialInsuranceNumber { get; set; }
        public Nullable<float> SocialInsuranceValue { get; set; }
        public string IncomeTaxNumber { get; set; }
        public Nullable<float> IncomeTaxValue { get; set; }

        //Staff Bank Details
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Bank { get; set; }
        public List<LookupDTO> BanksList { get; set; }
        public string Branch { get; set; }
        public List<LookupDTO> BranchList { get; set; }
        public string IBANCode { get; set; }
        public byte[] Flag { get; set; }

    }
}