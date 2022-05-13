using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CartManager : CommonManager<Cart>, ICartManager
    {
        public CartManager() : base(new CartRepository())
        {
        }

        public ICollection<Cart> GetByUser(long id)
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

        public bool DeleteCart(long id)
        {
            Cart cart = GetFirstOrDefault(c => c.Id == id);
            return Delete(cart);
        }

        public bool DeleteAllFromCart(long userId)
        {
            ICollection<Cart> products = Get(c => c.UserId == userId);
            return Delete(products);
        }

        public Cart IsProductExist(long userId, long productUnitId)
        {
            Cart cart = GetFirstOrDefault(c => c.ProductUnit.Id == productUnitId && c.UserId == userId, c => c.User,
                 c => c.ProductUnit,
                 c => c.ProductUnit.ProductImages,
                 c => c.ProductUnit.Product,
                 c => c.ProductUnit.ProductDiscounts,
                 c => c.ProductUnit.Product.Brand,
                 c => c.ProductUnit.Product.Category);
            return cart;
        }
    }
}
