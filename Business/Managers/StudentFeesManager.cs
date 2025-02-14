//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;


using DataAccess;
using DataAccess.Base;
using Business.Base;
using Business.Interfaces;
using Objects;


 
namespace Business.Managers
{
public class StudentFeesManager : BusinessComponentBase, IStudentFeesManager
{


    public StudentFeesManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(StudentFee StudentFee)
    {

DataAccessFactory.GetStudentFeesRepository.Add(StudentFee);
            DataAccessFactory.Commit();

}

public void Update(StudentFee StudentFee)
    {

DataAccessFactory.GetStudentFeesRepository.Update(StudentFee);
        DataAccessFactory.Commit();

}

public void Delete(StudentFee StudentFee)
    {

DataAccessFactory.GetStudentFeesRepository.Delete(StudentFee);
            DataAccessFactory.Commit();

}

public StudentFee GetById(short id)
    {
        StudentFee StudentFee = null;

        //  StudentFee =  DataAccessFactory.GetStudentFeesRepository.First(_StudentFee => _StudentFee.StudentFeeID == id);

        return StudentFee;
    }

    public IList<StudentFee> GetAll()
    {
        IEnumerable<StudentFee> StudentFees = null;

        StudentFees = DataAccessFactory.GetStudentFeesRepository.GetAll();

        IList<StudentFee> result = StudentFees.ToList<StudentFee>();

        return result;
    }
		
    public IEnumerable<StudentFee> Find(Expression<Func<StudentFee, bool>> @where, params Expression<Func<StudentFee, object>>[] includes)
    {
        return  DataAccessFactory.GetStudentFeesRepository.Find(@where, includes);
    }
		



        		

}

}
