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
public class FamilysManager : BusinessComponentBase, IFamilysManager
{


    public FamilysManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(Family Family)
    {

DataAccessFactory.GetFamilysRepository.Add(Family);
            DataAccessFactory.Commit();

}

public void Update(Family Family)
    {

DataAccessFactory.GetFamilysRepository.Update(Family);
        DataAccessFactory.Commit();

}

public void Delete(Family Family)
    {

DataAccessFactory.GetFamilysRepository.Delete(Family);
            DataAccessFactory.Commit();

}

public Family GetById(short id)
    {
        Family Family = null;

        //  Family =  DataAccessFactory.GetFamilysRepository.First(_Family => _Family.FamilyID == id);

        return Family;
    }

    public IList<Family> GetAll()
    {
        IEnumerable<Family> Familys = null;

        Familys = DataAccessFactory.GetFamilysRepository.GetAll();

        IList<Family> result = Familys.ToList<Family>();

        return result;
    }
		
    public IEnumerable<Family> Find(Expression<Func<Family, bool>> @where, params Expression<Func<Family, object>>[] includes)
    {
        return  DataAccessFactory.GetFamilysRepository.Find(@where, includes);
    }
		



        		

}

}
