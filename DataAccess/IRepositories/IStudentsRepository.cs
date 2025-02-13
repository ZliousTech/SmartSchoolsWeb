using DataAccess.Base;
using Objects;
using Objects.DTO;
using System.Collections.Generic;

namespace DataAccess.IRepositories
{
    public interface IStudentsRepository : IRepository<Student>
    {
        List<StudentClassficationDTO> GetStudentStatistics(int SchoolID);
    }

}
