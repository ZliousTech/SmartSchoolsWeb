using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base
{
    public partial interface IDataAccessFactory
    {
        int Commit();
    }
}
