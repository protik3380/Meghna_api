using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Model.Interfaces.Repositories;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductTypeManager : CommonManager<ProductType>, IProductTypeManager
    {
        public ProductTypeManager() : base(new ProductTypeRepository())
        {
        }

        public ICollection<ProductType> GetAll()
        {
            return Get(pt => !pt.IsDeleted);
        }

        public ICollection<ProductType> GetActiveProductTypes()
        {
            return Get(pt => !pt.IsDeleted && pt.IsActive);
        }

        public ProductType GetById(long id)
        {
            return GetFirstOrDefault(pt => pt.Id == id
                                          && !pt.IsDeleted);
        }
    }
}
