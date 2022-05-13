using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ProductDiscountRepository : CommonRepository<ProductDiscount>, IProductDiscountRepository
    {
        public ProductDiscountRepository() : base(new FreshContext())
        {
        }
    }
}
