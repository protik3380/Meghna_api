using EFreshStoreCore.Model.Context;
using System.Collections.Generic;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Helpers;


namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IOrderManager : ICommonManager<Order>
    {
        ICollection<Order> GetAll();
        List<Order> GetByUserId(long id);
        List<Order> GetByMasterDepot(long id);
        int CountOrder(string todaysDate);
        Order GetById(long id);
        List<Order> GetSavedOrdersByMasterDepot(long id);
        List<Order> GetReceivedOrdersByMasterDepot(long id);
        List<Order> GetRejectedOrdersByMasterDepot(long id);
        List<Order> GetOnProcessingOrdersByMasterDepot(long id);
        List<Order> GetShippedOrdersByMasterDepot(long id);
        List<Order> GetConfirmedOrdersByMasterDepot(long id);
        List<Order> GetDeliveredOrdersByMasterDepot(long id);
        int CountTotalOrderByOrderStatus(long id, long orderStatusId);
        Order GetByOrderNo(string orderNo);
        Order GetOnlinePaymentOrderByOrderNo(string orderNo);
        ICollection<Order> GetAllDistictUserOrders();
        ICollection<Order> GetOrderByTime(SalesOverTimeVm salesOverTime);
        double GetOrderGrowthRate(OrderGrowthRateParams orderGrowthRateParams);
        int GetTotalOrdersByUserIdAndCouponCode(CouponParams couponParams);
        bool DeleteOrder(string orderNo);
    }
}