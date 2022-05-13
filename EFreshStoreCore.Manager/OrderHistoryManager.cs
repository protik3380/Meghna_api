using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class OrderHistoryManager : CommonManager<OrderHistory>, IOrderHistoryManager
    {
        public OrderHistoryManager() : base(new OrderHistoryRepository())
        {
        }

        public bool DeleteByOrderNo(string orderNo)
        {
            var orderHistories = Get(c => c.Order.OrderNo.ToLower() == orderNo.ToLower());
            return Delete(orderHistories);
        }
    }
}
