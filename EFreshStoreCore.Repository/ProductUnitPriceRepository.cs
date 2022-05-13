using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductUnitPriceRepository : CommonRepository<ProductUnitPrice>, IProductUnitPriceRepository
    {
        public ProductUnitPriceRepository() : base(new FreshContext())
        {
        }

       
    }
}