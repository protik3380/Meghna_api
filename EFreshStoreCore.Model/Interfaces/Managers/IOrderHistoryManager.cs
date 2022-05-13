using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IOrderHistoryManager : ICommonManager<OrderHistory>
    {
        bool DeleteByOrderNo(string orderNo);
    }
}
