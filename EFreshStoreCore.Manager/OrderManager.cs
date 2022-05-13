using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Helpers;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class OrderManager : CommonManager<Order>, IOrderManager
    {
        public OrderManager() : base(new OrderRepository())
        {
        }

        public ICollection<Order> GetAll()
        {
            return base.GetAll(t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, o => o.OrderHistories);
        }


        public int CountOrder(string todaysDate)
        {
            ICollection<Order> orders = Get(o => o.OrderNo.StartsWith(todaysDate));
            int count = orders.Count;
            return count;
        }

        public Order GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id, c => c.OrderDetails, c => c.OrderHistories);
        }

        public List<Order> GetByUserId(long id)
        {
            return Get(c => c.UserId == id
                                          && c.OrderStateId.HasValue
                , t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories, c => c.OrderState, c => c.OrderReject).OrderByDescending(c => c.OrderDate).ToList();
        }

        public List<Order> GetByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
                c.OrderStateId == (long)OrderStatusEnum.Pending,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories).OrderByDescending(o => o.OrderDate).ToList();
            return (List<Order>)orders;
        }

        public List<Order> GetSavedOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
            c.OrderStateId != (long)OrderStatusEnum.Pending && !c.OrderRejectId.HasValue
            && c.OrderStateId != (long)OrderStatusEnum.Received,
            u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails);
            return (List<Order>)orders;
        }

        public List<Order> GetReceivedOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
            c.OrderStateId == (long)OrderStatusEnum.Received && !c.OrderRejectId.HasValue,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, o => o.OrderState);
            return (List<Order>)orders;
        }

        public List<Order> GetRejectedOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id && (c.OrderRejectId.HasValue || c.OrderStateId == (long)OrderStatusEnum.Rejected)
            , u => u.User, t => t.Thana,
            m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories, o => o.OrderReject);
            return (List<Order>)orders;
        }

        public List<Order> GetOnProcessingOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
                                                 c.OrderStateId == (long)OrderStatusEnum.OnProcessing,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories,c=>c.AssignOrders);
            return (List<Order>)orders;
        }

        public List<Order> GetShippedOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
                                                 c.OrderStateId == (long)OrderStatusEnum.Shipped,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories);
            return (List<Order>)orders;
        }
        public List<Order> GetConfirmedOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
                                                    c.OrderStateId == (long)OrderStatusEnum.Confirm,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories);
            return (List<Order>)orders;
        }

        public List<Order> GetDeliveredOrdersByMasterDepot(long id)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id &&
                                                 c.OrderStateId == (long)OrderStatusEnum.Delivered,
                u => u.User, t => t.Thana, m => m.MasterDepot, o => o.OrderDetails, c => c.OrderHistories);
            return (List<Order>)orders;
        }

        public int CountTotalOrderByOrderStatus(long id, long orderStatusId)
        {
            ICollection<Order> orders = Get(c => c.MasterDepotId == id && c.OrderStateId == orderStatusId);
            int count = orders.Count();
            return count;
        }

        public Order GetByOrderNo(string orderNo)
        {
            return GetFirstOrDefault(o => o.OrderNo.ToLower() == orderNo.ToLower(), o => o.OrderDetails, o => o.OrderHistories);
        }

        public Order GetOnlinePaymentOrderByOrderNo(string orderNo)
        {
            return GetFirstOrDefault(o => o.OrderNo.ToLower() == orderNo.ToLower() && o.PaymentMethod == "ONLINE");
        }

        public ICollection<Order> GetAllDistictUserOrders()
        {
            var orderLlist = Get(c => c.UserId != null && c.User.IsActive.HasValue && c.User.IsActive.Value && c.User.IsDeleted.HasValue && !c.User.IsDeleted.Value).ToList();
            return orderLlist;
        }

        public ICollection<Order> GetOrderByTime(SalesOverTimeVm salesOverTime)
        {
            var orderList = new List<Order>();
            if (salesOverTime.ReportType == 1)
            {
                orderList = Get(c => c.OrderDate.Value.Year >= salesOverTime.FromYear && c.OrderDate.Value.Year <= salesOverTime.ToYear).ToList();
            }
            else
            {
                orderList = Get(c => c.OrderDate.Value.Month >= salesOverTime.FromMonth && c.OrderDate.Value.Month <= salesOverTime.ToMonth).ToList();
            }
            return orderList;

        }

        public int CountDailyOrders(long depotId)
        {
            ICollection<Order> orders = GetByMasterDepot(depotId);
            int count = orders.Count(o => o.OrderDate != null &&
            (o.OrderDate.Value.Date == DateTime.Today && o.MasterDepotId.Equals(depotId)));
            return count;
        }

        //public override bool Update(Order entity)
        //{
        //    OrderHistory history = new OrderHistory();
        //    history.OrderId = entity.Id;
        //    history.OrderStateId = entity.OrderStateId.Value;
        //    history.OrderStateChangedOn = DateTime.UtcNow.AddHours(6);
        //    history.OrderStateChangedBy = entity.OrderStatusChangedBy;
        //    return base.Update(entity);
        //}

        public double GetOrderGrowthRate(OrderGrowthRateParams orderGrowthRateParams)
        {
            var priorToDate = orderGrowthRateParams.PriorToDate.AddDays(1);
            var currentToDate = orderGrowthRateParams.CurrentToDate.AddDays(1);

            var priorOrderList = Get(d => d.OrderDate >= orderGrowthRateParams.PriorFromDate 
                                          && d.OrderDate < priorToDate).ToList();
            var currentOrderList = Get(d => d.OrderDate >= orderGrowthRateParams.CurrentFromDate 
                                            && d.OrderDate <= currentToDate).ToList();
            var totalPriorOrder = Convert.ToDouble(priorOrderList.Count());
            var totalCurrentOrder = Convert.ToDouble(currentOrderList.Count());
            var orderGrowthRate =
                Convert.ToDouble((totalCurrentOrder - totalPriorOrder) * 100/ totalPriorOrder) ;
            return Math.Round(orderGrowthRate, 2); ;
        }

        public int GetTotalOrdersByUserIdAndCouponCode(CouponParams couponParams)
        {
            return Get(o => o.UserId == couponParams.UserId && o.CouponCode == couponParams.CouponCode)
                .Count();
        }

        public bool DeleteOrder(string orderNo)
        {
            var order = GetFirstOrDefault(c => c.OrderNo.ToLower() == orderNo.ToLower());
            return Delete(order);
        }

    }
}