using DataAccess.Base;
using System;

namespace Business.Base
{
    public abstract class BusinessComponentBase
    {
        #region Properties
        private IDataAccessFactory _dataAccessFactory;

        /// <summary>
        /// Gets the data access factory instance.
        /// </summary>
        protected IDataAccessFactory DataAccessFactory
        {
            get { return _dataAccessFactory; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessComponentBase"/> class.
        /// </summary>
        protected BusinessComponentBase()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessComponentBase"/> class.
        /// </summary>
        /// <param name="dataAccessFactory">The data access factory.</param>
        protected BusinessComponentBase(IDataAccessFactory dataAccessFactory)
        {
            if (dataAccessFactory == null)
            {
                throw new NullReferenceException();
            }
            _dataAccessFactory = dataAccessFactory;
        }




        #endregion
    }

}
