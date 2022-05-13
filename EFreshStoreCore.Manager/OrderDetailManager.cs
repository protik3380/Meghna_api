using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Helpers;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class OrderDetailManager : CommonManager<OrderDetail>, IOrderDetailManager
    {
        public OrderDetailManager() : base(new OrderDetailRepository())
        {
        }
        public OrderDetail GetByOrderDetailsId(long id)
        {
            OrderDetail orderDetails = GetFirstOrDefault(o => o.Id.Equals(id));
            return orderDetails;
        }

        public ICollection<OrderDetail> GetByOnProcessingOrderDetailsByOrderIds(long[] orderIds)
        {
            ICollection<OrderDetail> orderDetails = Get(c => orderIds.Contains((long) c.OrderId) && c.Order.OrderStateId == (long) OrderStatusEnum.OnProcessing, c=>c.Order,c=>c.Order.OrderState, c => c.Order.OrderHistories);
            return orderDetails;
        }
        public ICollection<OrderDetail> GetReceivedOrderDetailsByOrderIds(long[] orderIds)
        {
            ICollection<OrderDetail> orderDetails = Get(c => orderIds.Contains((long)c.OrderId) && c.Order.OrderStateId == (long)OrderStatusEnum.Received, c => c.Order, c => c.Order.OrderState, c => c.Order.OrderHistories);
            return orderDetails;
        }
        public ICollection<OrderDetail> GetPickedOrderDetailsByOrderIds(long[] orderIds)
        {
            ICollection<OrderDetail> orderDetails = Get(c => orderIds.Contains((long)c.OrderId) && c.Order.OrderStateId == (long)OrderStatusEnum.Shipped, c => c.Order, c => c.Order.OrderState, c => c.Order.OrderHistories);
            return orderDetails;
        }

        public OrderDetail IsProductExistsInOrderDetails(OrderDetail orderDetail)
        {
            return GetFirstOrDefault(o =>
                o.Id == orderDetail.Id && o.OrderId == orderDetail.OrderId &&
                o.ProductUnitId == orderDetail.ProductUnitId);
        }


        public List<OrderDetail> GetById(long id)
        {
            return (List<OrderDetail>)Get(c => c.Order.UserId == id);
        }


        public ICollection<OrderDetail> GetByOderId(long id)
        {
            List<OrderDetail> detailsList = Get(c => c.OrderId == id,
                c => c.ProductUnit,
                c => c.ProductUnit.ProductImages,
                c => c.ProductUnit.Product,
                c => c.Order
                , c => c.Order.MasterDepot).ToList();
            return detailsList;
        }

        public ICollection<OrderDetail> GetOrderDetailsForSalesByProduct(SalesByProductParams salesByProductParams)
        {
            IEnumerable<OrderDetail> details = GetAll(c => c.ProductUnit,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.Product.Category,
                c => c.ProductUnit.Product.Category.ProductType,
                c => c.ProductUnit.Product.Brand,
                c => c.Order);

            if (salesByProductParams.FromDate != null)
            {
                details = details.Where(d => d.Order.OrderDate >= salesByProductParams.FromDate);
            }

            if (salesByProductParams.ToDate != null)
            {
                var toDate = ((DateTime)salesByProductParams.ToDate).AddDays(1);
                details = details.Where(d => d.Order.OrderDate < toDate);
            }

            if (salesByProductParams.ProductTypeIds != null)
            {
                if (salesByProductParams.ProductTypeIds != null)
                {
                    details = details.Where(s => salesByProductParams.ProductTypeIds.Contains((long)s.ProductUnit.Product.Category.ProductTypeId));
                }
            }

            if (salesByProductParams.BrandIds != null)
            {
                details = details.Where(s => salesByProductParams.BrandIds.Contains(s.ProductUnit.Product.BrandId));
            }

            if (salesByProductParams.CategoryIds != null)
            {
                details = details.Where(s => salesByProductParams.CategoryIds.Contains(s.ProductUnit.Product.CategoryId));
            }


            return details.ToList();
        }

        public ICollection<OrderDetail> GetOrderDetailsForTotalOrders(TotalOrdersParams ordersParams)
        {
            IEnumerable<OrderDetail> details = GetAll(c => c.ProductUnit,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.Product.Category,
                c => c.Order,
                c => c.Order.MasterDepot,
                c => c.Order.Thana,
                c => c.Order.Thana.District);

            if (ordersParams.MasterDepotIds != null)
            {
                details = details.Where(s => ordersParams.MasterDepotIds.Contains((long)s.Order.MasterDepotId));
            }

            if (ordersParams.FromDate != null)
            {
                details = details.Where(d => d.Order.OrderDate >= ordersParams.FromDate);
            }

            if (ordersParams.ToDate != null)
            {
                var toDate = ((DateTime)ordersParams.ToDate).AddDays(1);
                details = details.Where(d => d.Order.OrderDate < toDate);
            }

            if (ordersParams.ProductTypeIds != null)
            {
                details = details.Where(s => ordersParams.ProductTypeIds.Contains((long)s.ProductUnit.Product.Category.ProductTypeId));
            }

            if (ordersParams.BrandIds != null)
            {
                details = details.Where(s => ordersParams.BrandIds.Contains(s.ProductUnit.Product.BrandId));
            }

            if (ordersParams.CategoryIds != null)
            {
                details = details.Where(s => ordersParams.CategoryIds.Contains(s.ProductUnit.Product.CategoryId));
            }


            return details.ToList();
        }

        public ICollection<OrderDetail> GetOrderDetailsForSalesByLocation(SalesByLocationParams salesByLocationParams)
        {
            IEnumerable<OrderDetail> details = GetAll(c => c.ProductUnit,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.Product.Category,
                c => c.Order,
                c => c.Order.MasterDepot,
                c => c.Order.Thana,
                c => c.Order.Thana.District);

            if (salesByLocationParams.DistrictIds != null)
            {
                details = details.Where(s => salesByLocationParams.DistrictIds.Contains((long)s.Order.Thana.DistrictId));
            }

            if (salesByLocationParams.ThanaIds != null)
            {
                details = details.Where(s => salesByLocationParams.ThanaIds.Contains((long)s.Order.ThanaId));
            }

            if (salesByLocationParams.MasterDepotIds != null)
            {
                details = details.Where(s => salesByLocationParams.MasterDepotIds.Contains((long)s.Order.MasterDepotId));
            }

            if (salesByLocationParams.FromDate != null)
            {
                details = details.Where(d => d.Order.OrderDate >= salesByLocationParams.FromDate);
            }

            if (salesByLocationParams.ToDate != null)
            {
                var toDate = ((DateTime)salesByLocationParams.ToDate).AddDays(1);
                details = details.Where(d => d.Order.OrderDate < toDate);
            }

            if (salesByLocationParams.ProductTypeIds != null)
            {
                details = details.Where(s => salesByLocationParams.ProductTypeIds.Contains((long)s.ProductUnit.Product.Category.ProductTypeId));
            }

            if (salesByLocationParams.BrandIds != null)
            {
                details = details.Where(s => salesByLocationParams.BrandIds.Contains(s.ProductUnit.Product.BrandId));
            }

            if (salesByLocationParams.CategoryIds != null)
            {
                details = details.Where(s => salesByLocationParams.CategoryIds.Contains(s.ProductUnit.Product.CategoryId));
            }


            return details.ToList();
        }


        public ICollection<OrderDetail> GetOrderDetailsForOrdersByStatus(OrdersByStatusParams ordersByStatusParams)
        {
            IEnumerable<OrderDetail> details = GetAll(c => c.ProductUnit,
                c => c.ProductUnit.Product,
                c => c.ProductUnit.Product.Category,
                c => c.Order,
                c => c.Order.MasterDepot,
                c => c.Order.OrderState,
                c => c.Order.Thana,
                c => c.Order.Thana.District);

            if (ordersByStatusParams.OrderStateIds != null)
            {
                details = details.Where(s => ordersByStatusParams.OrderStateIds.Contains((long)s.Order.OrderStateId));
            }

            if (ordersByStatusParams.MasterDepotIds != null)
            {
                details = details.Where(s => ordersByStatusParams.MasterDepotIds.Contains((long)s.Order.MasterDepotId));
            }

            if (ordersByStatusParams.FromDate != null)
            {
                details = details.Where(d => d.Order.OrderDate >= ordersByStatusParams.FromDate);
            }

            if (ordersByStatusParams.ToDate != null)
            {
                var toDate = ((DateTime)ordersByStatusParams.ToDate).AddDays(1);
                details = details.Where(d => d.Order.OrderDate < toDate);
            }

            if (ordersByStatusParams.ProductTypeIds != null)
            {
                details = details.Where(s => ordersByStatusParams.ProductTypeIds.Contains((long)s.ProductUnit.Product.Category.ProductTypeId));
            }

            if (ordersByStatusParams.BrandIds != null)
            {
                details = details.Where(s => ordersByStatusParams.BrandIds.Contains(s.ProductUnit.Product.BrandId));
            }

            if (ordersByStatusParams.CategoryIds != null)
            {
                details = details.Where(s => ordersByStatusParams.CategoryIds.Contains(s.ProductUnit.Product.CategoryId));
            }


            return details.ToList();
        }

        public bool DeleteByOrderNo(string orderNo)
        {
            var orderDetails = Get(c => c.Order.OrderNo.ToLower() == orderNo.ToLower());
            return Delete(orderDetails);
        }
    }
}