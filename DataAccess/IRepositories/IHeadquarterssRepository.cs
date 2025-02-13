using DataAccess.Base;
using Objects;
using Objects.DTO;
using System.Collections.Generic;

namespace DataAccess.IRepositories
{
    public interface IHeadquarterssRepository : IRepository<Headquarters>
    {
        List<HeadQuarterDTO> GetHeadQuarters(string lang);
    }

}
