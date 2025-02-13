namespace DataAccess.Base
{
    public partial class DataAccessFactory : IDataAccessFactory
    {
        UnitOfWork unitOfWork;
        protected UnitOfWork UnitOfWork
        {
            get
            {
                if (unitOfWork == null)
                {

                    unitOfWork = new UnitOfWork(new SmartSchoolsEntities());
                }

                return unitOfWork;
            }
        }
        public int Commit()
        {
            return UnitOfWork.Save();
        }
    }

}
