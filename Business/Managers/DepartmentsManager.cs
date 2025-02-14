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
public class DepartmentsManager : BusinessComponentBase, IDepartmentsManager
{


    public DepartmentsManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(Department Department)
    {

DataAccessFactory.GetDepartmentsRepository.Add(Department);
            DataAccessFactory.Commit();

}

public void Update(Department Department)
    {

DataAccessFactory.GetDepartmentsRepository.Update(Department);
        DataAccessFactory.Commit();

}

public void Delete(Department Department)
    {

DataAccessFactory.GetDepartmentsRepository.Delete(Department);
            DataAccessFactory.Commit();

}

public Department GetById(short id)
    {
        Department Department = null;

        //  Department =  DataAccessFactory.GetDepartmentsRepository.First(_Department => _Department.DepartmentID == id);

        return Department;
    }

    public IList<Department> GetAll()
    {
        IEnumerable<Department> Departments = null;

        Departments = DataAccessFactory.GetDepartmentsRepository.GetAll();

        IList<Department> result = Departments.ToList<Department>();

        return result;
    }
		
    public IEnumerable<Department> Find(Expression<Func<Department, bool>> @where, params Expression<Func<Department, object>>[] includes)
    {
        return  DataAccessFactory.GetDepartmentsRepository.Find(@where, includes);
    }
		



        		

}

}
