using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.DTO
{
    public class EmployeeDTO
    {
        public string StaffID { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string Destigniation { get; set; }
        public string mobilenumber { get; set; }
        public string Email { get; set; }
        public DateTime dateofjoining { get; set; }
        public byte[] image { get; set; }


    }
}
