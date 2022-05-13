using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductUnitPriceManager : CommonManager<ProductUnitPrice>, IProductUnitPriceManager
    {
        public ProductUnitPriceManager() : base(new ProductUnitPriceRepository())
        {
        }
    }
}