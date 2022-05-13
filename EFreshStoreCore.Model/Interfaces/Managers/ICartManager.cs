using System.Collections.Generic;
using System.Web.UI.WebControls;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICartManager : ICommonManager<Cart>
    {
        ICollection<Cart> GetByUser(long id);
        bool DeleteCart(long id);
        bool DeleteAllFromCart(long userId);
        Cart IsProductExist(long userId, long productUnitId);
    }
}
