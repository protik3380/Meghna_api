using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class OrdersByStatusToReturnDto
    {
        public string OrderNo { get; set; }
        public long? OrderId { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerName { get; set; }
        public double TotalPrice { get; set; }
        public string Thana { get; set; }
        public string District { get; set; }
        public DateTime? OrderDate { get; set; }
        public string MasterDepotName { get; set; }
    }
}
