 

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
internal class StudentsRepository : RepositoryBase<Student> ,IStudentsRepository
	{
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        public StudentsRepository(DbContext context)
		: base(context)
    {
           
    }
       public List<StudentClassficationDTO> GetStudentStatistics(int SchoolID)
        {
            var result = from Student in context.Students
                         join StudentSchoolDetail in context.StudentSchoolDetails on Student.StudentID
                          equals StudentSchoolDetail.StudentID
                         join SchoolClass in context.SchoolClasses on StudentSchoolDetail.ClassID
                         equals SchoolClass.SchoolClassID
                         join Class in context.Classes on SchoolClass.ClassID equals Class.ClassID
                         join AgeClassification in context.AgeClassifications on Class.AgeClassification
                         equals AgeClassification.AgeClassificationID
                         where Student.Active == -1 && StudentSchoolDetail.SchoolID == SchoolID
                         select new StudentClassficationDTO
                         {
                             StudentID=Student.StudentID,
                             AgeClassificationID=AgeClassification.AgeClassificationID,
                             AgeClassificationArabicText=AgeClassification.AgeClassificationArabicText,
                             AgeClassificationEnglishText=AgeClassification.AgeClassificationEnglishText
                         };
            return result.ToList();
        }

    }

}
