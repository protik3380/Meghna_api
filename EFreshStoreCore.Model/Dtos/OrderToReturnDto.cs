using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class OrderToReturnDto
    {
        public string OrderNo { get; set; }
        public long OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string MasterDepotName { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalPrice { get; set; }
        public ICollection<OrderDetailToReturnDto> OrderDetails { get; set; }
    }
}
