using Template.Data.Core;
using System;

namespace Template.Services
{
    public abstract class BaseService : IService
    {
        protected IUnitOfWork UnitOfWork { get; private set; }
        public String CurrentAccountId { get; set; }
        private Boolean Disposed { get; set; }

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(Boolean disposing)
        {
            if (Disposed) return;

            UnitOfWork.Dispose();

            Disposed = true;
        }
    }
}
