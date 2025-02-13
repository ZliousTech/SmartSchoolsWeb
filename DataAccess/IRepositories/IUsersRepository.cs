using DataAccess.Base;
using Objects;
using Objects.DTO;

namespace DataAccess.IRepositories
{
    public interface IUsersRepository : IRepository<User>
    {
        UserDTO GetUserLogin(string username, string password, bool asparent, bool asStudent, string lang);

    }

}
