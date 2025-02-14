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
using Objects.DTO;

namespace Business.Managers
{
public class HeadquarterssManager : BusinessComponentBase, IHeadquarterssManager
{


    public HeadquarterssManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(Headquarters Headquarters)
    {

DataAccessFactory.GetHeadquarterssRepository.Add(Headquarters);
            DataAccessFactory.Commit();

}

public void Update(Headquarters Headquarters)
    {

DataAccessFactory.GetHeadquarterssRepository.Update(Headquarters);
        DataAccessFactory.Commit();

}

public void Delete(Headquarters Headquarters)
    {

DataAccessFactory.GetHeadquarterssRepository.Delete(Headquarters);
            DataAccessFactory.Commit();

}

public Headquarters GetById(short id)
    {
        Headquarters Headquarters = null;

        //  Headquarters =  DataAccessFactory.GetHeadquarterssRepository.First(_Headquarters => _Headquarters.HeadquartersID == id);

        return Headquarters;
    }

    public IList<Headquarters> GetAll()
    {
        IEnumerable<Headquarters> Headquarterss = null;

        Headquarterss = DataAccessFactory.GetHeadquarterssRepository.GetAll();

        IList<Headquarters> result = Headquarterss.ToList<Headquarters>();

        return result;
    }
		
    public IEnumerable<Headquarters> Find(Expression<Func<Headquarters, bool>> @where, params Expression<Func<Headquarters, object>>[] includes)
    {
        return  DataAccessFactory.GetHeadquarterssRepository.Find(@where, includes);
    }

        public List<HeadQuarterDTO> GetHeadQuarters(string lang)
        {
            return DataAccessFactory.GetHeadquarterssRepository.GetHeadQuarters(lang);

        }




    }

}
