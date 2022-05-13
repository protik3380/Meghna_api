using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class WishListManager : CommonManager<WishList>, IWishListManager
    {
        public WishListManager() : base(new WishListRepository())
        {
        }
        public ICollection<WishList> GetByUser(long id)
        {
            return Get(c => c.UserId == id,
                c => c.User,
                c => c.ProductUnit,
                c => c.ProductUnit.ProductImages,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.ProductDiscounts,
                c => c.ProductUnit.Product.Brand,
                c => c.ProductUnit.Product.Category);
        }

        public bool DeleteWishList(long id)
        {
            WishList wishList = GetFirstOrDefault(c => c.Id == id);
            return Delete(wishList);
        }

        public bool IsProductExist(long userId, long productUnitId)
        {
            WishList wishList = GetFirstOrDefault(c => c.ProductUnit.Id == productUnitId && c.UserId == userId);
            if(wishList != null)
                return true;
            else
                return false;
        }

        public WishList GetWishlistProduct(long userId, long productUnitId)
        {
            WishList wishList = GetFirstOrDefault(c => c.ProductUnit.Id == productUnitId && c.UserId == userId, c => c.User,
                c => c.ProductUnit,
                c => c.ProductUnit.ProductImages,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.ProductDiscounts,
                c => c.ProductUnit.Product.Brand,
                c => c.ProductUnit.Product.Category);

            return wishList;
        }
    }
}
