namespace SmartSchool.Models.CrslRpts
{
    public class RptGuardianContractModel
    {
        public int Ext_GuardianID { get; set; }
        public int SchoolID { get; set; }
        public int DepaetmentID { get; set; }
        public int CurriculumID { get; set; }
        public int ClassID { get; set; }
        public int TransportID { get; set; }

        public string Ext_GuardianArabicName { get; set; }
        public string Ext_GuardainEnglishName { get; set; }
        public string Ext_GuardainGender { get; set; }
    }
}