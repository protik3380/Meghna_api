using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductLineRepository : CommonRepository<ProductLine>, IProductLineRepository
    {
        public ProductLineRepository() : base(new FreshContext())
        {
        }
    }
}