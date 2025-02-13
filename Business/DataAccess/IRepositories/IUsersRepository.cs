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
public interface IUsersRepository : IRepository<User>
{
        UserDTO GetUserLogin(string username, string password,bool asparent, bool asStudent ,string lang);

    }

}
