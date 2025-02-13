using Common.Helpers;
using Objects.DTO;
using System.Collections.Generic;

namespace SmartSchool.Models.SystemUser
{
    public class SystemUserModel
    {
        public int GroupID { get; set; }
        public List<SystemUsersDTO> Users { get; set; }
        public List<LookupDTO> Groups { get; set; }
    }
}