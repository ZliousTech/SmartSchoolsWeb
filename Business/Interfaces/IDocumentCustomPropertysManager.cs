//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.Linq.Expressions;
using Objects;
namespace Business.Interfaces
{
public interface IDocumentCustomPropertysManager
	{

    void Add(DocumentCustomProperty DocumentCustomProperty);

    void Update(DocumentCustomProperty DocumentCustomProperty);

    void Delete(DocumentCustomProperty DocumentCustomProperty);

    DocumentCustomProperty GetById(short id);
      
    IList<DocumentCustomProperty> GetAll();

    IEnumerable<DocumentCustomProperty> Find(Expression<Func<DocumentCustomProperty, bool>> where, params Expression<Func<DocumentCustomProperty, object>>[] includes);
		
    }

}
