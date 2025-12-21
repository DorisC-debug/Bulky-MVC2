using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork 
    {
        public ICategoryRepository Category { get; }
        public IProductRepository Product { get; }

        void Save();
    }
}
