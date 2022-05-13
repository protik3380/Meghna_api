using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductImageRepository: CommonRepository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository() : base(new FreshContext())
        {
        }
    }
}