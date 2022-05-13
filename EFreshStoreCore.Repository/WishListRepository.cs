using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class WishListRepository : CommonRepository<WishList>, IWishListRepository
    {
        public WishListRepository() : base(new FreshContext())
        {
        }
    }
}
