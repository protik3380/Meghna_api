using System;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;
using EFreshStoreCore.Model.Helpers;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IOrderDetailManager : ICommonManager<OrderDetail>
    {
        List<OrderDetail> GetById(long id);
        ICollection<OrderDetail> GetByOderId(long id);
        ICollection<OrderDetail> GetOrderDetailsForSalesByProduct(SalesByProductParams salesByProductParams);
        ICollection<OrderDetail> GetOrderDetailsForSalesByLocation(SalesByLocationParams salesByLocationParams);
        ICollection<OrderDetail> GetOrderDetailsForOrdersByStatus(OrdersByStatusParams ordersByStatusParams);
        ICollection<OrderDetail> GetOrderDetailsForTotalOrders(TotalOrdersParams ordersParams);
        OrderDetail GetByOrderDetailsId(long id);
        ICollection<OrderDetail> GetByOnProcessingOrderDetailsByOrderIds(long[] orderIds);
        ICollection<OrderDetail> GetReceivedOrderDetailsByOrderIds(long[] orderIds);
        ICollection<OrderDetail> GetPickedOrderDetailsByOrderIds(long[] orderIds);
        OrderDetail IsProductExistsInOrderDetails(OrderDetail orderDetail);
        bool DeleteByOrderNo(string orderNo);
    }
}