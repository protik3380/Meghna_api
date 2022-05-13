using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IWishListManager : ICommonManager<WishList>
    {
        ICollection<WishList> GetByUser(long id);
        bool DeleteWishList(long id);
        bool IsProductExist(long userId, long productUnitId);
        WishList GetWishlistProduct(long userId, long productUnitId);
    }
}
