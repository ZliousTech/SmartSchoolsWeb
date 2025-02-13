using Objects;
using System.Collections.Generic;

namespace SmartSchool.Models.Registration
{
    public class DiscountsModel
    {
        public int StudentID { get; set; }
        public int SchoolID { get; set; }
        public List<Discount> SchoolDiscounts { get; set; }
        public List<DiscountStudent> StudentDiscounts { get; set; }
        public string InternalStudentID { get; set; }


    }
}