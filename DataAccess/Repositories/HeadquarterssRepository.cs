using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using Objects.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Repositories
{
    internal class HeadquarterssRepository : RepositoryBase<Headquarters>, IHeadquarterssRepository
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        public HeadquarterssRepository(DbContext context)
        : base(context)
        {

        }

        public List<HeadQuarterDTO> GetHeadQuarters(string lang)
        {
            var result = from Headquarters in context.Headquarters
                         join Countries in context.Countries on
                         Headquarters.Country equals Countries.ID
                         orderby Headquarters.CompanyEnglishName ascending
                         select new HeadQuarterDTO
                         {
                             CompanyID = Headquarters.CompanyID,
                             CompanyArabicName = Headquarters.CompanyArabicName,
                             CompanyEnglishName = Headquarters.CompanyEnglishName,
                             Country = lang == "en" ? Countries.EnglishName : Countries.ArabicName,
                             ContactNo = Headquarters.ContactNo,
                             Email = Headquarters.Email,
                         };
            return result.OrderBy(a => a.CompanyEnglishName).ToList();
        }
    }

}
