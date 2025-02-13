using Common.Helpers;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Student
{
    public class StudentInternalRegModel
    {
        //Gaurdian Info------------------------------
        public string FirstArabicName_Grdn { get; set; }
        public string SecondArabicName_Grdn { get; set; }
        public string ThirdArabicName_Grdn { get; set; }
        public string FourthArabicName_Grdn { get; set; }
        public string FirstEnglishName_Grdn { get; set; }
        public string SecondEnglishName_Grdn { get; set; }
        public string ThirdEnglishName_Grdn { get; set; }
        public string FourthEnglishName_Grdn { get; set; }
        public Nullable<int> Gender_Grdn_M { get; set; }
        public Nullable<int> Gender_Grdn_F { get; set; }
        public Nullable<int> Nationality_Grdn { get; set; }
        public string NationalNumber_Grdn { get; set; }
        public string Religion_Grdn { get; set; }
        public string ResidencyNumber_Grdn { get; set; }
        public string PaymentMethod_Grdn { get; set; }
        public string PassportNo_Grdn { get; set; }
        public string Country_Grdn { get; set; }
        public string City_Grdn { get; set; }
        public string Street_Grdn { get; set; }
        public string Building_Grdn { get; set; }
        public string Floor_Grdn { get; set; }
        public string Longitude_Grdn { get; set; }
        public string Latitude_Grdn { get; set; }
        public string MobNo_Grdn { get; set; }
        public string EMail_Grdn { get; set; }
        public string CreditCardType_Grdn { get; set; }
        public string CreditCardNo_Grdn { get; set; }
        public string NameOnCreditCard_Grdn { get; set; }
        public string CreditCardExpirationDateYear_Grdn { get; set; }
        public string CreditCardExpirationDateMonth_Grdn { get; set; }
        public string VerificationNo_Grdn { get; set; }
        public Nullable<int> Gender { get; set; }

        //Student Info------------------------------
        public string FirstArabicName { get; set; }
        public string SecondArabicName { get; set; }
        public string ThirdArabicName { get; set; }
        public string FourthArabicName { get; set; }
        public string FirstEnglishName { get; set; }
        public string SecondEnglishName { get; set; }
        public string ThirdEnglishName { get; set; }
        public string FourthEnglishName { get; set; }
        public string Nationality { get; set; }
        public string NationalNumber { get; set; }
        public string ResidencyNo { get; set; }
        public string PassportNo { get; set; }
        public string DateofBirth { get; set; }
        public string JoinDate { get; set; }
        public int ClassID { get; set; }
        public int SectionID { get; set; }
        public string StdStatus { get; set; }
        public string StdAcademicStatus { get; set; }
        public string ChekComeToSchool { get; set; }
        public string ChekGoToHome { get; set; }
        public string StdComeToSchoolTour { get; set; }
        public string StdGoToHomeTour { get; set; }
        public int GoingToSchoolOrder { get; set; }
        public int ComingToHomeOrder { get; set; }
        public string PreviousSchoolName { get; set; }
        public string StdGuardianID { get; set; }
        public string StdRelativeRelation { get; set; }
        public string GuardainStdNationalNo { get; set; }
        public string GuardainStdResidencyNo { get; set; }
        public string GuardainStdPassportNo { get; set; }
        public string GuardainStdNationality { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FatherNationalNo { get; set; }
        public string MotherNationalNo { get; set; }
        public string FatherResidencyNo { get; set; }
        public string MotherResidencyNo { get; set; }
        public string FatherPassportNo { get; set; }
        public string MotherPassportNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string MobileNo { get; set; }
        public string EMail { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string SuperiorInSubject { get; set; }
        public string WeakInSubject { get; set; }
        public string Talent { get; set; }
        public string LiveWith { get; set; }
        public string TypeOfResidence { get; set; }
        public string NumberOfBrother { get; set; }
        public string NumberOfSisters { get; set; }
        public string BrothersOrder { get; set; }
        public string FamilyTotIncome { get; set; }
        public string BloodType { get; set; }
        public string PhysicalCondition { get; set; }
        public string SpecialCare { get; set; }
        public string ChronicDisease { get; set; }
        public string DiseaseType { get; set; }
        public string DiseaseNotes { get; set; }
        public string StudentBehavior { get; set; }
        public string StudentBehaviorNotes { get; set; }
        public string DisciplinaryDesc { get; set; }
        public string DisciplinaryDate { get; set; }
        public string DisciplinaryEncourage { get; set; }
        public string AccountNo { get; set; }
        public string Installments { get; set; }
        //-------------------------------------------

        //School Info-------------------------------
        public string SchoolID { get; set; }
        public string SchoolLat { get; set; }
        public string SchoolLng { get; set; }
        //------------------------------------------

        //LookupDTO ---------------------------------
        public List<LookupDTO> NationalityList { get; set; }
        public List<LookupDTO> ReligionList { get; set; }
        public List<LookupDTO> PaymentMethodList { get; set; }
        public List<LookupDTO> Countries { get; set; }
        public List<LookupDTO> CreditCardTypeList { get; set; }
        public List<LookupDTO> CreditCardExpirationDateMonthList_Grdn { get; set; }
        public List<LookupDTO> CompanySchooslList { get; set; }
        public List<LookupDTO> SchoolClassesList { get; set; }
        public List<LookupDTO> ClassSectionsList { get; set; }
        public List<LookupDTO> StdStatusList { get; set; }
        public List<LookupDTO> StdAcademicStatuslist { get; set; }
        public List<LookupDTO> StdComeToSchoolTourList { get; set; }
        public List<LookupDTO> StdGoToHomeTourList { get; set; }
        public List<LookupDTO> StdGuardianIDList { get; set; }
        public List<LookupDTO> StdRelativeRelationList { get; set; }
        public List<LookupDTO> FatherNameList { get; set; }
        public List<LookupDTO> MotherNameList { get; set; }
        public List<LookupDTO> SuperiorInSubjectList { get; set; }
        public List<LookupDTO> WeakInSubjectList { get; set; }
        public List<LookupDTO> LiveWithList { get; set; }
        public List<LookupDTO> TypeOfResidenceList { get; set; }
        public List<LookupDTO> BloodTypeList { get; set; }
        public List<LookupDTO> PhysicalConditionList { get; set; }
        public List<LookupDTO> StudentBehaviorList { get; set; }
        //-------------------------------------------
    }
}