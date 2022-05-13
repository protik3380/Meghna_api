using System;

namespace EFreshStoreCore.Model.Dtos
{
   public class ViewAssigedOrdersByDeliveryManDto
    {
      
            public string OrderNo { get; set; }
            public long? OrderId { get; set; }
            public double TotalPrice { get; set; }
            public DateTime? OrderDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public long OrderStateId { get; set; }
            public string OrderStatus { get; set; }
        
    }
}
