using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductUnitRepository: CommonRepository<ProductUnit>, IProductUnitRepository
    {
        public ProductUnitRepository() : base(new FreshContext())
        {
        }
    }
}