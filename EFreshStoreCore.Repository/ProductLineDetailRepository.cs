using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductLineDetailRepository : CommonRepository<ProductLineDetail>, IProductLineDetailRepository
    {
        public ProductLineDetailRepository() : base(new FreshContext())
        {
        }
    }
}