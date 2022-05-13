using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductImageManager: CommonManager<ProductImage>, IProductImageManager
    {
        public ProductImageManager() : base(new ProductImageRepository())
        {
        }
    }
}