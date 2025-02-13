 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using Objects.DTO;

namespace DataAccess.Repositories
{
    internal class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();
        public UsersRepository(DbContext context)
            : base(context)
        {

        }


        public UserDTO GetUserLogin(string username, string password, bool asparent, bool asStudent, string lang)
        {
            IQueryable<UserDTO> result;
            if (!asparent && !asStudent)
            {
                result = from User in context.Users
                         where User.UserName == username && User.Password == password
                         select new UserDTO
                         {
                             UserID = User.UserID,
                             UserType = User.UserType.Value,
                             Username = User.UserName,
                             CompanyID = User.CompanyID.Value,
                             SchoolID = User.SchoolID.Value,
                             StaffID = User.StaffID,
                             GuardianID = "-1",
                             PreviousSchoolID = 0
                         };
                return result.FirstOrDefault();
            }

            else if (asStudent)
            {
                result = from StudentLogin in context.Student_Login
                         where StudentLogin.Username == username && StudentLogin.Password == password
                         select new UserDTO
                         {
                             UserId = StudentLogin.StudentID,
                             UserType = 8,
                             Username = (from s in context.Students
                                         where s.StudentID == StudentLogin.StudentID
                                         select lang == "ar" ? s.StudentArabicName : s.StudentEnglishName).FirstOrDefault()
                             ,
                             StudentID = StudentLogin.StudentID,
                             SchoolID = StudentLogin.SchoolID.Value

                         };
                return result.FirstOrDefault();
            }
            else
            {
                result = from ExternalWebUser in context.ExternalGuardians
                         where ExternalWebUser.UserName == username && ExternalWebUser.Password == password
                         select new UserDTO
                         {
                             UserID = ExternalWebUser.UserID,
                             UserType = -1,
                             Username = ExternalWebUser.UserName,
                             CompanyID = -1,
                             SchoolID = -1,
                             StaffID = "-1",
                             GuardianID = null,
                             PreviousSchoolID = 0

                         };
                if (result == null)
                {
                    result = from WebUser in context.WebUsers
                             where WebUser.UserName == username && WebUser.Password == password
                             select new UserDTO
                             {
                                 UserID = -1,
                                 UserType = -1,
                                 Username = WebUser.UserName,
                                 CompanyID = -1,
                                 SchoolID = -1,
                                 StaffID = "-1",
                                 GuardianID = WebUser.UserSystemID,
                                 PreviousSchoolID = 0

                             };
                }

                return result.FirstOrDefault();
            }

        }

    }

}
