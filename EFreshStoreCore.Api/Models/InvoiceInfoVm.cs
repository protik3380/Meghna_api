using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFreshStoreCore.Api.Models
{
    public class InvoiceInfoVm
    {
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotalPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int PacketQuantity { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
    }
}