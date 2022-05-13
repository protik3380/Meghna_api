using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductLineDetailManager : ICommonManager<ProductLineDetail>
    {
        List<ProductLineDetail> GetByProduct(long productId);
        ProductLineDetail IsExistProductLine(ProductLineDetail productLineDetail);
        ICollection<ProductLineDetail> GetAll();
        ICollection<ProductLineDetail> GetProductsByLineId(long id);
        ProductLineDetail GetById(long id);

    }
}