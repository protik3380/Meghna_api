using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductDiscountManager : ICommonManager<ProductDiscount>
    {
        List<ProductDiscount> GetByProductId();
        ProductDiscount GetByProductUnitId(long? id);
        ProductDiscount GetByProductDiscountId(long? id);
    }
}
