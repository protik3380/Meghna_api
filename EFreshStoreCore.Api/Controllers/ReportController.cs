using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CrystalDecisions.Shared;
using EFreshStoreCore.Api.Models;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Helpers;
using EFreshStoreCore.Model.Interfaces.Managers;
using Microsoft.Ajax.Utilities;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportController : ApiController
    {
        private readonly IOrderManager _orderManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly IUserManager _userManager;
        private readonly IMasterDepotManager _masterDepotManager;

        public ReportController()
        {
            _orderManager = new OrderManager();
            _orderDetailManager = new OrderDetailManager();
            _userManager = new UserManager();
            _masterDepotManager = new MasterDepotManager();
        }

        [HttpGet]
        public IHttpActionResult GetSalesByProduct([FromUri]SalesByProductParams salesParams)
        {
            try
            {
                var orderDetails = _orderDetailManager.GetOrderDetailsForSalesByProduct(salesParams);
                var sales = orderDetails.GroupBy(o => o.ProductUnit.Id).Select(s => new SalesByProductToReturnDto
                {
                    ProductName = s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.Product.Name,
                    BrandName = s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.Product.Brand.Name,
                    CategoryName = s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.Product.Category.Name,
                    ProductTypeName = s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.Product.Category.ProductType.Name,
                    ProductUnit = s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.StockKeepingUnit + " " + s.First(p => p.ProductUnit.Id == s.Key).ProductUnit.CartonSizeUnit,
                    TotalProducts = Convert.ToInt16(s.Sum(p => p.PacketQuantity)),
                    TotalOrders = s.Count(),
                    Price = Convert.ToDouble(s.Sum(p => p.Price)),
                }).ToList();

                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetTotalOrders([FromUri]TotalOrdersParams ordersParams)
        {
            try
            {
                if (ordersParams.UserId != null)
                {
                    var masterDepotUser = _masterDepotManager.GetByUserId((long)ordersParams.UserId);
                    ordersParams.MasterDepotIds = new[] { masterDepotUser.Id };
                }
                var orderDetails = _orderDetailManager.GetOrderDetailsForTotalOrders(ordersParams);
                var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new TotalOrdersToReturnDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                    CustomerName = o.First(p => p.OrderId == o.Key).Order.CustomerName,
                    Thana = o.First(p => p.OrderId == o.Key).Order.Thana.Name,
                    District = o.First(p => p.OrderId == o.Key).Order.Thana.District.Name,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    TotalProducts = Convert.ToInt16(o.Sum(p => p.PacketQuantity)),
                    MasterDepotName = o.First(p => p.OrderId == o.Key).Order.MasterDepot.Name
                }).ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAnalysisReport([FromUri]TotalOrdersParams ordersParams)
        {
            try
            {
                if (ordersParams.UserId != null)
                {
                    var masterDepotUser = _masterDepotManager.GetByUserId((long)ordersParams.UserId);
                    ordersParams.MasterDepotIds = new[] { masterDepotUser.Id };
                }
                var orderDetails = _orderDetailManager.GetOrderDetailsForTotalOrders(ordersParams);
                var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new TotalOrdersToReturnDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                    CustomerName = o.First(p => p.OrderId == o.Key).Order.CustomerName,
                    Thana = o.First(p => p.OrderId == o.Key).Order.Thana.Name,
                    District = o.First(p => p.OrderId == o.Key).Order.Thana.District.Name,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    TotalProducts = Convert.ToInt16(o.Sum(p => p.PacketQuantity)),
                    MasterDepotName = o.First(p => p.OrderId == o.Key).Order.MasterDepot.Name
                }).ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetSalesByLocation([FromUri]SalesByLocationParams saleParams)
        {
            try
            {
                if (saleParams.UserId != null)
                {
                    var masterDepotUser = _masterDepotManager.GetByUserId((long)saleParams.UserId);
                    saleParams.MasterDepotIds = new[] { masterDepotUser.Id };
                }
                var orderDetails = _orderDetailManager.GetOrderDetailsForSalesByLocation(saleParams);
                var sales = orderDetails.GroupBy(o => o.OrderId).Select(o => new SalesByLocationToReturnDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).OrderId,
                    CustomerName = o.First(p => p.OrderId == o.Key).Order.CustomerName,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    MasterDepotName = o.First(p => p.OrderId == o.Key).Order.MasterDepot.Name,
                    Thana = o.First(p => p.OrderId == o.Key).Order.Thana.Name,
                    District = o.First(p => p.OrderId == o.Key).Order.Thana.District.Name,
                }).ToList();

                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetOrdersByOrderStatus([FromUri]OrdersByStatusParams ordersParams)
        {
            try
            {
                if (ordersParams.UserId != null)
                {
                    var masterDepotUser = _masterDepotManager.GetByUserId((long)ordersParams.UserId);
                    ordersParams.MasterDepotIds = new[] { masterDepotUser.Id };
                }
                var orderDetails = _orderDetailManager.GetOrderDetailsForOrdersByStatus(ordersParams);
                var orders = orderDetails.GroupBy(o => o.OrderId).Select(o => new OrdersByStatusToReturnDto()
                {
                    OrderNo = o.First(p => p.OrderId == o.Key).Order.OrderNo,
                    OrderId = o.First(p => p.OrderId == o.Key).Order.Id,
                    CustomerName = o.First(p => p.OrderId == o.Key).Order.CustomerName,
                    Thana = o.First(p => p.OrderId == o.Key).Order.Thana.Name,
                    District = o.First(p => p.OrderId == o.Key).Order.Thana.District.Name,
                    OrderDate = o.First(p => p.OrderId == o.Key).Order.OrderDate,
                    TotalPrice = Convert.ToDouble(o.Sum(p => p.Price)),
                    MasterDepotName = o.First(p => p.OrderId == o.Key).Order.MasterDepot.Name,
                    OrderStatus = o.First(p => p.OrderId == o.Key).Order.OrderState.Status
                }).ToList();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetReturningCustomerRate([FromUri] SalesByProductParams salesParams)
        {
            try
            {
                var userList = _userManager.GetAllActiveUser();
                var orderList = _orderManager.GetAllDistictUserOrders().GroupBy(c=> c.UserId).Select(c => c.FirstOrDefault());
                if (salesParams.FromDate != null)
                {
                    orderList = orderList.Where(d => d.OrderDate >= salesParams.FromDate).ToList();
                }

                if (salesParams.ToDate != null)
                {
                    orderList = orderList.Where(d => d.OrderDate <= salesParams.ToDate).ToList();
                }
          
                var totalReturningCustomerRate = Convert.ToDouble(orderList.Count() * 100) / userList.Count();
                return Ok(totalReturningCustomerRate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult GetOrderGrowthRate([FromUri] OrderGrowthRateParams orderGrowthRate)
        {
            try
            {
                var growthRate = _orderManager.GetOrderGrowthRate(orderGrowthRate);

                return Ok(growthRate);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet]
        public IHttpActionResult SalesOverTime([FromUri]SalesOverTimeVm salesOverTime)
        {
            try
            {
               
                List<OrderVsMonthOrYearVm> orderVsMonthOrYears = new List<OrderVsMonthOrYearVm>();
                if (salesOverTime.ReportType == 1)
                {
                    var totalOrders = _orderManager.GetAll().Count();
                    for (int i = salesOverTime.FromYear; i <= salesOverTime.ToYear; i++)
                    {

                        OrderVsMonthOrYearVm aOrderVsMonthOrYear = new OrderVsMonthOrYearVm();
                        aOrderVsMonthOrYear.Year = i;
                        var orderThisYear = _orderManager.Get(c => c.OrderDate.Value.Year == i).Count();
                        aOrderVsMonthOrYear.OrderRate = (Convert.ToDouble(orderThisYear)/totalOrders)*100;
                        orderVsMonthOrYears.Add(aOrderVsMonthOrYear);
                    }
                }
                else
                {
                    var totalOrdersThisYear = _orderManager.Get(c=>c.OrderDate.Value.Year== salesOverTime.Year).Count();
                    for (int i = salesOverTime.FromMonth; i <= salesOverTime.ToMonth; i++)
                    {
                        OrderVsMonthOrYearVm aOrderVsMonthOrYear = new OrderVsMonthOrYearVm();
                        aOrderVsMonthOrYear.Month = i;
                        var orderThisMonth = _orderManager.Get(c => c.OrderDate.Value.Month == i && c.OrderDate.Value.Year == salesOverTime.Year).Count();
                        aOrderVsMonthOrYear.OrderRate = (Convert.ToDouble(orderThisMonth) / totalOrdersThisYear) * 100;
                        orderVsMonthOrYears.Add(aOrderVsMonthOrYear);
                    }
                }
                
                
                return Ok(orderVsMonthOrYears);
            }
            catch (Exception ex)
            {

                return BadRequest();
            }
        }


     
    }
}
