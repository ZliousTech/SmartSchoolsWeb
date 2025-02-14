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
public class ExternalStudentAdresssManager : BusinessComponentBase, IExternalStudentAdresssManager
{


    public ExternalStudentAdresssManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(ExternalStudentAdress ExternalStudentAdress)
    {

DataAccessFactory.GetExternalStudentAdresssRepository.Add(ExternalStudentAdress);
            DataAccessFactory.Commit();

}

public void Update(ExternalStudentAdress ExternalStudentAdress)
    {

DataAccessFactory.GetExternalStudentAdresssRepository.Update(ExternalStudentAdress);
        DataAccessFactory.Commit();

}

public void Delete(ExternalStudentAdress ExternalStudentAdress)
    {

DataAccessFactory.GetExternalStudentAdresssRepository.Delete(ExternalStudentAdress);
            DataAccessFactory.Commit();

}

public ExternalStudentAdress GetById(short id)
    {
        ExternalStudentAdress ExternalStudentAdress = null;

        //  ExternalStudentAdress =  DataAccessFactory.GetExternalStudentAdresssRepository.First(_ExternalStudentAdress => _ExternalStudentAdress.ExternalStudentAdressID == id);

        return ExternalStudentAdress;
    }

    public IList<ExternalStudentAdress> GetAll()
    {
        IEnumerable<ExternalStudentAdress> ExternalStudentAdresss = null;

        ExternalStudentAdresss = DataAccessFactory.GetExternalStudentAdresssRepository.GetAll();

        IList<ExternalStudentAdress> result = ExternalStudentAdresss.ToList<ExternalStudentAdress>();

        return result;
    }
		
    public IEnumerable<ExternalStudentAdress> Find(Expression<Func<ExternalStudentAdress, bool>> @where, params Expression<Func<ExternalStudentAdress, object>>[] includes)
    {
        return  DataAccessFactory.GetExternalStudentAdresssRepository.Find(@where, includes);
    }
		



        		

}

}

