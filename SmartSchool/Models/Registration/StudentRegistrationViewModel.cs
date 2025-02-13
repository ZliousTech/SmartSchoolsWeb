using Common.Helpers;
using Objects;
using System.Collections.Generic;

namespace SmartSchool.Models.Registration
{
    public class StudentRegistrationViewModel
    {
        // Student Info
        public int StudentIDNumber { get; set; }
        public int GurdianID { get; set; }
        public int SectionID { get; set; }
        public string FirstArabicName { get; set; }
        public string SecondArabicName { get; set; }
        public string ThirdArabicName { get; set; }
        public string FourthArabicName { get; set; }
        public string DateofBirth { get; set; }
        public string BirthPlace { get; set; }
        public string StudentID { get; set; }
        public int TransportDirectionID { get; set; }
        //Parents Info
        public string GuardianName { get; set; }
        public string GuardianMobileNumber { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int FatherQualification { get; set; }
        public int FatherSpecialization { get; set; }
        public int MotherQualification { get; set; }
        public int MotherSpecialization { get; set; }
        public string FatherJob { get; set; }
        public string MotherJob { get; set; }
        public string FatherWorkPhone { get; set; }
        public string MotherWorkPhone { get; set; }
        public string FatherMobile { get; set; }
        public string smsNumber { get; set; }
        public string FatherEmail { get; set; }
        public string MotherMobile { get; set; }
        public string MotherEmail { get; set; }
        public string mailBox { get; set; }
        public string PostalCode { get; set; }

        public List<LookupDTO> Countries { get; set; }
        public List<LookupDTO> CountriesHeadQuarter { get; set; }
        public List<LookupDTO> EducationalYears { get; set; }
        public List<LookupDTO> NationalityList { get; set; }
        public List<LookupDTO> GuardianRelationshipList { get; set; }
        public List<LookupDTO> QualificationList { get; set; }
        public List<LookupDTO> SpecializationList { get; set; }
        public List<LookupDTO> AcademicSubjectList { get; set; }
        public List<LookupDTO> LiveWithTypesList { get; set; }
        public List<LookupDTO> ResidenceConditionsList { get; set; }
        public List<LookupDTO> BloodTypesList { get; set; }
        public List<LookupDTO> physicalStatusList { get; set; }
        public List<DiscountStudent> DiscountStudentList { get; set; }
        public decimal? MaxDiscountNumber { get; set; }
        public decimal? DiscountValue { get; set; }


        public ExternalStudent student { get; set; }
        public ExternalOtherStudentDetail OtherStudentDetail { get; set; }
        public ExternalStudentAdress StudentAddress { get; set; }
        public DiscountsModel DiscountModel { get; set; }

        public StudentDiseas StudentDiseas { get; set; }
        public ExternalStudentSchoolDetail SchoolDetail { get; set; }

        //
        public Guardian Guardian { get; set; }


    }
}