using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductDiscountManager : CommonManager<ProductDiscount>, IProductDiscountManager
    {
        public ProductDiscountManager(): base(new ProductDiscountRepository())
        {
        }

        public List<ProductDiscount> GetByProductId()
        {
            return (List<ProductDiscount>) Get(c=> c.IsActive.HasValue
                                   && c.IsActive.Value
                                   && c.IsDeleted.HasValue
                                   && !c.IsDeleted.Value
                                    , c => c.ProductUnit.Product);
        }

        public ProductDiscount GetByProductUnitId(long? id)
        {
            var productDiscount = GetFirstOrDefault(c => c.ProductUnitId == id
                                  && c.IsActive.HasValue
                                  && c.IsActive.Value
                                  && c.IsDeleted.HasValue
                                  && !c.IsDeleted.Value,
                                  c => c.ProductUnit.Product);
            return productDiscount;
        }

        public ProductDiscount GetByProductDiscountId(long? id)
        {
            var productDiscount = GetFirstOrDefault(c => c.Id == id
                                                         && c.IsActive.HasValue
                                                         && c.IsActive.Value
                                                         && c.IsDeleted.HasValue
                                                         && !c.IsDeleted.Value,
                c => c.ProductUnit.Product);
            return productDiscount;
        }
    }
}
