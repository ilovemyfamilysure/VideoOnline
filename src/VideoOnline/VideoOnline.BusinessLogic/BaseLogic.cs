using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOnline.Data.EntityFrameWork.Data.Core;
using VideoOnline.Data.UnitOfWork.Data.Core;

namespace VideoOnline.BusinessLogic
{
    public abstract class BaseLogic
    {
        protected IUnitOfWork IUnitOfWork { get; private set; }

        public BaseLogic() : this(null) { }

        public BaseLogic(IUnitOfWork iUnitOfWork)
        {
            IUnitOfWork = IUnitOfWork;
        }

        protected ICompleteRepository<T> LoadRepository<T>() where T : class
        {
            return IUnitOfWork == null ? new DbContextRepository<T>() : new DbContextRepository<T>(IUnitOfWork);
        }
    }
}
