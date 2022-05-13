using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductLineManager : ICommonManager<ProductLine>
    {
        List<ProductLine> GetByProduct(long productId);
        bool IsExistByName(string productLineName);
        ProductLine GetById(long id);
    }
}