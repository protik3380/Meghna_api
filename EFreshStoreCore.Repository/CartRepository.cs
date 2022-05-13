using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CartRepository : CommonRepository<Cart>, ICartRepository
    {
        public CartRepository() : base(new FreshContext())
        {
        }
    }
}
