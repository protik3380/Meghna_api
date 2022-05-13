using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Http;
using CrystalDecisions.CrystalReports.Engine;
using EFreshStoreCore.Api.Models;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderManager _orderManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly IMasterDepotManager _masterDepotManager;
        private readonly IPaymentDetailManager _paymentDetailManager;
        private readonly IUserManager _userManager;
        private readonly ICorporateUserManager _corporateUserManager;
        private readonly ICustomerManager _customerManager;
        private readonly IMeghnaUserManager _meghnaUserManager;
        private readonly IOrderRejectManager _orderRejectManager;
        private readonly IOrderStateManager _orderStateManager;
        private readonly IProductDiscountManager _productDiscountManager;
        private readonly IOrderHistoryManager _orderHistoryManager;
        private readonly IAssignOrderManager _assignOrderManager;

        public OrderController()
        {
            _orderManager = new OrderManager();
            _masterDepotManager = new MasterDepotManager();
            _orderDetailManager = new OrderDetailManager();
            _userManager = new UserManager();
            _corporateUserManager = new CorporateUserManager();
            _customerManager = new CustomerManager();
            _meghnaUserManager = new MeghnaUserManager();
            _orderRejectManager = new OrderRejectManager();
            _orderStateManager = new OrderStateManager();
            _productDiscountManager = new ProductDiscountManager();
            _orderHistoryManager = new OrderHistoryManager();
            _assignOrderManager = new AssignOrderManager();
            _paymentDetailManager = new PaymentDetailManager();
        }

        //[Authorize(Roles = "Admin")]
        public IHttpActionResult GetAllOrders()
        {
            try
            {
                var orders = _orderManager.GetAll();
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Customer, Corporate")]
        public IHttpActionResult GetOrderByUserId(long id)
        {
            try
            {
                var order = _orderManager.GetByUserId(id);
                if (order != null) return Ok(order);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, MasterDepotUser")]
        public IHttpActionResult GetOrderByMasterdepot(long id)
        {
            try
            {
                var masterDepotUser = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetByMasterDepot(masterDepotUser.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetReceivedOrders(long id)
        {
            try
            {
                var masterDepotUser = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetReceivedOrdersByMasterDepot(masterDepotUser.Id);
                if (orders == null) NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetSavedOrders(long id)
        {
            try
            {
                var masterDepotUser = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetSavedOrdersByMasterDepot(masterDepotUser.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetRejectedOrders(long id)
        {
            try
            {
                var masterDepotUser = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetRejectedOrdersByMasterDepot(masterDepotUser.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetOnProcessingOrders(long id)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetOnProcessingOrdersByMasterDepot(masterDepot.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetConfirmedOrders(long id)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetConfirmedOrdersByMasterDepot(masterDepot.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetShippedOrders(long id)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetShippedOrdersByMasterDepot(masterDepot.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetDeliveredOrders(long id)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByUserId(id);
                var orders = _orderManager.GetDeliveredOrdersByMasterDepot(masterDepot.Id);
                if (orders == null) return NotFound();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Customer, Corporate, MasterDepotUser")]
        public IHttpActionResult GetDetails(long id)
        {
            try
            {
                Order anOrder = _orderManager.GetById(id);

                if (anOrder == null)
                    return NotFound();

                return Ok(anOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetOrderDetailsForUser(long id, long userId)
        {
            try
            {
                Order anOrder = _orderManager.GetById(id);

                if (anOrder == null)
                    return NotFound();

                if (anOrder.UserId != userId)
                    return Unauthorized();

                var deliveryManInfo = _assignOrderManager.GetDeliveryManByOrderId(anOrder.Id);
                if (deliveryManInfo == null) return Ok(anOrder);
                anOrder.DeliveryManName = deliveryManInfo.DeliveryMan.Name;
                anOrder.DeliveryManMobile = deliveryManInfo.DeliveryMan.MobileNo;
                anOrder.DeliveryManEmail = deliveryManInfo.DeliveryMan.Email;
                anOrder.DeliveryManImageUrl = deliveryManInfo.DeliveryMan.ImageUrl;
                return Ok(anOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetOrderByOrderNo(string orderNo)
        {
            try
            {
                var order = _orderManager.GetByOrderNo(orderNo);
                if (order == null) return NotFound();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Delete(string orderNo)
        {
            try
            {
                var isDeleted = DeleteByOrderNo(orderNo);
                if (isDeleted)
                {
                    return Ok();
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteOrderForSkippedTransaction(string orderNo)
        {
            try
            {
                var payment = _paymentDetailManager.GetByOrderNo(orderNo);
                if (payment == null)
                {
                    var order = _orderManager.GetOnlinePaymentOrderByOrderNo(orderNo);
                    if (order != null)
                    {
                        var isDeleted = DeleteByOrderNo(orderNo);
                        if (isDeleted)
                        {
                            return Ok();
                        }
                    }
                }
                
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private bool DeleteByOrderNo(string orderNo)
        {
            _orderHistoryManager.DeleteByOrderNo(orderNo);
            _orderDetailManager.DeleteByOrderNo(orderNo);
            return _orderManager.DeleteOrder(orderNo);
        }


        [HttpGet]
        public IHttpActionResult GetOrderDetailsForDeliveryManByOderId(long orderId, long deliveryManId)
        {
            try
            {
                var assignedOrder = _assignOrderManager.GetDeliveryManByOrderId(orderId);
                if (assignedOrder == null) return NotFound();
                if (assignedOrder.DeliveryManId != deliveryManId)
                {
                    return BadRequest();
                }

                var orderDetails = _orderDetailManager.GetByOderId(orderId);
                if (orderDetails == null) return NotFound();
                var orderDetailToReturn = orderDetails.Select(o => new OrderDetailToReturnDto
                {
                    ProductName = o.ProductUnit.Product.Name,
                    ProductQuantity = (int)o.PacketQuantity,
                    ProductImageUrl = o.ProductUnit.ProductImages.First().ImageLocation,
                    StockKeepingUnit = o.ProductUnit.StockKeepingUnit,
                    TotalPrice = (double)o.Price
                }).ToList();

                var orderToReturn = orderDetails.Select(od => od.Order).Select(o => new OrderToReturnDto
                {
                    OrderNo = o.OrderNo,
                    OrderId = (long)o.Id,
                    CustomerName = o.CustomerName,
                    CustomerMobileNo = o.MobileNo,
                    CustomerEmail = o.Email,
                    CustomerAddress = o.DeliveryAddress,
                    OrderDate = (DateTime)o.OrderDate,
                    MasterDepotName = o.MasterDepot.Name
                }).First();

                orderToReturn.OrderDetails = orderDetailToReturn;
                orderToReturn.TotalPrice = orderDetails.Sum(x => (double)x.Price);

                return Ok(orderToReturn);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult GetOrderDetailsByOderNo(string orderNo)
        {
            try
            {
                var orderDetail = _orderManager.GetByOrderNo(orderNo);
                var deliveryManInfo = _assignOrderManager.GetDeliveryManByOrderId(orderDetail.Id);
                if (orderDetail == null)
                {
                    return NotFound();
                }
                var trackOrder = new TrackOrderVm
                {
                    OrderNo = orderDetail.OrderNo,
                    OrderDate = orderDetail.OrderDate,
                    CurrentOrderStateId = orderDetail.OrderStateId,
                    OrderHistories = orderDetail.OrderHistories
                };
                if (deliveryManInfo == null) return Ok(trackOrder);
                trackOrder.DeliveryManName = deliveryManInfo.DeliveryMan.Name;
                trackOrder.DeliveryManMobile = deliveryManInfo.DeliveryMan.MobileNo;
                trackOrder.DeliveryManEmail = deliveryManInfo.DeliveryMan.Email;
                trackOrder.DeliveryManImageUrl = deliveryManInfo.DeliveryMan.ImageUrl;
                return Ok(trackOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Post orders
        //[Authorize(Roles = "Customer, Corporate")]
        [HttpPost]
        public IHttpActionResult Checkout([FromBody] Order anOrder)
        {
            try
            {
                if (anOrder.OrderNo != null)
                {
                    var payment = _paymentDetailManager.GetByOrderNo(anOrder.OrderNo);
                    if (payment == null)
                    {
                        var order = _orderManager.GetOnlinePaymentOrderByOrderNo(anOrder.OrderNo);
                        if (order != null)
                        {
                            DeleteByOrderNo(anOrder.OrderNo);
                        }
                    }
                }
                var masterDepo = _masterDepotManager.GetByThanaAndProduct((long)anOrder.ThanaId);
                if (masterDepo == null || masterDepo.Id == 0)
                {
                    return NotFound();
                }

                anOrder.IsPaid = anOrder.PaymentMethod != "COD";

                anOrder.MasterDepotId = masterDepo.Id;
                string todaysDate = DateTime.UtcNow.AddHours(6).ToString("yy") +
                                    DateTime.UtcNow.AddHours(6).Month.ToString("00") +
                                    DateTime.UtcNow.AddHours(6).Day.ToString("00");
                anOrder.OrderNo = todaysDate + (_orderManager.CountOrder(todaysDate) + 1).ToString("000000");
                anOrder.OrderStateId = (long)OrderStatusEnum.Pending;
                anOrder.OrderDate = DateTime.UtcNow.AddHours(6);
                _orderManager.Add(anOrder);
                OrderHistory history = new OrderHistory
                {
                    OrderId = anOrder.Id,
                    OrderStateId = anOrder.OrderStateId.Value,
                    OrderStateChangedOn = DateTime.UtcNow.AddHours(6),
                    OrderStateChangedBy = anOrder.OrderStatusChangedBy
                };
                var addHistory = _orderHistoryManager.Add(history);
                string subject = "Your Order " + anOrder.OrderNo + " has been placed";
                string boady = "Dear " + anOrder.CustomerName + "," + Environment.NewLine + "\nThis is an e-mail notification to inform you that your order no " + anOrder.OrderNo + " has been placed." + "\n\nSincerely," + Environment.NewLine +
                               "EFresh";
                MailAddress address = new MailAddress(anOrder.Email);

                Email.SendEmail(subject, boady, address);
                //GenerateInvoiceSendMail(anOrder);
                if (anOrder.UserId == null)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), anOrder);
                }
                var user = _userManager.GetById((long)anOrder.UserId);
                if (user != null)
                {
                    if (user.UserTypeId == (long)UserTypeEnum.Corporate)
                    {
                        var corporateUser = _corporateUserManager.GetByUserId(user.Id);
                        if (corporateUser != null)
                        {
                            corporateUser.DeliveryAddress1 = anOrder.UpdateAddress;
                            _corporateUserManager.Update(corporateUser);
                        }
                    }
                    if (user.UserTypeId == (long)UserTypeEnum.Customer)
                    {
                        var customer = _customerManager.GetByUserId(user.Id);
                        if (customer != null)
                        {
                            customer.DeliveryAddress1 = anOrder.UpdateAddress;
                            _customerManager.Update(customer);
                        }
                    }
                    var meghnaUser = _meghnaUserManager.GetByUserId(user.Id);
                    if (meghnaUser != null)
                    {
                        meghnaUser.DeliveryAddress1 = anOrder.UpdateAddress;
                        _meghnaUserManager.Update(meghnaUser);
                    }
                }
                return Created(new Uri(Request.RequestUri + anOrder.Id.ToString()), anOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //private void GenerateInvoiceSendMail(Order anOrder)
        //{
        //    var orderDetail = _orderDetailManager.GetByOderId(anOrder.Id);
        //    var invoiceInfos = new List<InvoiceInfoVm>();
        //    foreach (var item in orderDetail)
        //    {
        //        var invoiceInfo = new InvoiceInfoVm();
        //        invoiceInfo.OrderNo = anOrder.OrderNo;
        //        invoiceInfo.CustomerName = anOrder.CustomerName;
        //        invoiceInfo.MobileNo = anOrder.MobileNo;
        //        invoiceInfo.Email = anOrder.Email;
        //        invoiceInfo.DeliveryAddress = anOrder.DeliveryAddress;
        //        if (item.Quantity != null) invoiceInfo.Quantity = (int) item.Quantity;
        //        if (item.Price != null) invoiceInfo.SubTotalPrice = (decimal) item.Price;
        //        if (item.PacketQuantity != null) invoiceInfo.PacketQuantity = (int) item.PacketQuantity;
        //        invoiceInfo.ProductName = item.ProductUnit.Product.Name;
        //        invoiceInfo.Description = item.ProductUnit.Product.Description;
        //        if (item.ProductUnit.MaximumRetailPrice != null)
        //            invoiceInfo.UnitPrice = (decimal) item.ProductUnit.MaximumRetailPrice;
        //        var productDiscount = _productDiscountManager.GetByProductUnitId(item.ProductUnitId);
        //        if (productDiscount!= null)
        //        {
        //            if (productDiscount.DiscountPercentage != null)
        //                invoiceInfo.DiscountPercentage = (decimal) productDiscount.DiscountPercentage;
        //        }
        //        else
        //        {
        //            invoiceInfo.DiscountPercentage = Convert.ToDecimal(0);
        //        }
        //        invoiceInfos.Add(invoiceInfo);
        //    }

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(HttpContext.Current.Server.MapPath("~/Invoice/rptInvoice.rpt")));
        //    rd.SetDataSource(invoiceInfos);
        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    Attachment pdf = new Attachment(stream, "Invoice.pdf", "application/pdf");          
        //    string subject = "Your Order " + anOrder.OrderNo + " has been placed";
        //    string boady = "Dear " + anOrder.CustomerName + "," + Environment.NewLine + "\nThis is an e-mail notification to inform you that your order no " + anOrder.OrderNo + " has been placed." + "\n\nSincerely," + Environment.NewLine +
        //    "EFresh";           
        //    MailAddress address = new MailAddress(anOrder.Email);
        //    Email.SendEmailWithAttachment(subject, boady, address, pdf);
        //}


        //order reject details
        public IHttpActionResult GetOrderRejectStatus()
        {
            try
            {
                var orderRejectStatus = _orderRejectManager.GetAll();
                if (orderRejectStatus == null) return NotFound();
                return Ok(orderRejectStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //order reject details
        public IHttpActionResult GetOrderStates()
        {
            try
            {
                var orderStates = _orderStateManager.GetAll();
                if (orderStates == null) return NotFound();
                return Ok(orderStates);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, MasterDepotUser")]
        public IHttpActionResult GetTotalOrderByStatus(long id, long orderStateId)
        {
            try
            {
                var masterDepotUser = _masterDepotManager.GetByUserId(id);
                int totalOrders = _orderManager.CountTotalOrderByOrderStatus(masterDepotUser.Id, orderStateId);
                if (totalOrders <= 0) return NotFound();
                return Ok(totalOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //update order status
        [Authorize(Roles = "Admin, MasterDepotUser")]
        [HttpPost]
        public IHttpActionResult UpdateOrderStatus([FromBody]Order anOrder)
        {
            try
            {
                bool isSaved = _orderManager.Update(anOrder);
                OrderHistory history = new OrderHistory
                {
                    OrderId = anOrder.Id,
                    OrderStateId = anOrder.OrderStateId.Value,
                    OrderStateChangedOn = DateTime.UtcNow.AddHours(6),
                    OrderStateChangedBy = anOrder.OrderStatusChangedBy
                };
                if (isSaved)
                {
                    var addHistory = _orderHistoryManager.Add(history);
                    if (anOrder.OrderStateId == (long)OrderStatusEnum.Received)
                    {
                        string subject = "Your Order " + anOrder.OrderNo + " has been received";
                        string boady = "Dear " + anOrder.CustomerName + "," + Environment.NewLine + "\nThis is an e-mail notification to inform you that your order no " + anOrder.OrderNo + " has been received." + "\n\nSincerely," + Environment.NewLine +
                                       "EFresh";
                        MailAddress address = new MailAddress(anOrder.Email);
                        Email.SendEmail(subject, boady, address);
                    }
                    return Ok();
                }
                return BadRequest("Failed to update Order status");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //update order status by delivery man
        // [Authorize(Roles = "Admin, MasterDepotUser")]
        [HttpPost]
        public IHttpActionResult UpdateOrderStatusByDeliveryMan(long orderId, long orderStatusId, long userId)
        {
            try
            {

                Order order = _orderManager.GetById(orderId);
                order.OrderStateId = orderStatusId;
                bool isSaved = _orderManager.Update(order);
                OrderHistory history = new OrderHistory
                {
                    OrderId = order.Id,
                    OrderStateId = order.OrderStateId.Value,
                    OrderStateChangedOn = DateTime.UtcNow.AddHours(6),
                    OrderStateChangedBy = userId
                };
                if (isSaved)
                {
                    if (order.OrderStateId == (long)OrderStatusEnum.Delivered || order.OrderStateId == (long)OrderStatusEnum.Received)
                    {
                        var assignedOrder = _assignOrderManager.GetByOrderId(orderId);
                        assignedOrder.IsDelivered = true;
                        assignedOrder.ModifiedOn = DateTime.UtcNow.AddHours(6);
                        assignedOrder.ModifiedBy = userId;
                        _assignOrderManager.Update(assignedOrder);
                    }

                    var addHistory = _orderHistoryManager.Add(history);
                    if (order.OrderStateId == (long)OrderStatusEnum.Received)
                    {
                        string subject = "Your Order " + order.OrderNo + " has been received";
                        string boady = "Dear " + order.CustomerName + "," + Environment.NewLine + "\nThis is an e-mail notification to inform you that your order no " + order.OrderNo + " has been received." + "\n\nSincerely," + Environment.NewLine +
                                       "EFresh";
                        MailAddress address = new MailAddress(order.Email);
                        Email.SendEmail(subject, boady, address);
                    }
                    return Ok();
                }
                return BadRequest("Failed to update Order status");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //update order details
        [Authorize(Roles = "Admin, MasterDepotUser")]
        [HttpPost]
        public IHttpActionResult UpdateOrderDetails([FromBody]Order anOrder)
        {
            try
            {
                bool isSaved = _orderManager.Update(anOrder);
                OrderHistory history = new OrderHistory
                {
                    OrderId = anOrder.Id,
                    OrderStateId = anOrder.OrderStateId.Value,
                    OrderStateChangedOn = DateTime.UtcNow.AddHours(6),
                    OrderStateChangedBy = anOrder.OrderStatusChangedBy
                };
                if (isSaved)
                {
                    var addHistory = _orderHistoryManager.Add(history);
                    foreach (var orderdetails in anOrder.OrderDetails)
                    {
                        OrderDetail oDetails = _orderDetailManager.GetByOrderDetailsId(orderdetails.Id);
                        oDetails.PacketQuantity = orderdetails.PacketQuantity;
                        oDetails.Price = (oDetails.Price / oDetails.Quantity) * oDetails.PacketQuantity;
                        _orderDetailManager.Update(oDetails);
                    }
                    return Ok();
                }
                return BadRequest("Failed to update order details");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
