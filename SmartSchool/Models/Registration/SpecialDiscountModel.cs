namespace SmartSchool.Models.Registration
{
    public class SpecialDiscountModel
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string InternalStudentID { get; set; }
        public decimal DiscountValue { get; set; }
        public int SchoolID { get; set; }
    }
}