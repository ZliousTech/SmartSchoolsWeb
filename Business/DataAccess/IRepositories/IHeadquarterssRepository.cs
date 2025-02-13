using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Base;
using DataAccess;
using Objects;
using Objects.DTO;
 
namespace DataAccess.IRepositories
{
public interface IHeadquarterssRepository : IRepository<Headquarters>
{
        List<HeadQuarterDTO> GetHeadQuarters(string lang);
}

}
