using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Context
{
    public partial class Order
    {
        public string UpdateAddress { get; set; }
        public long? OrderStatusChangedBy { get; set; }
        public string DeliveryManName { get; set; }
        public string DeliveryManMobile { get; set; }
        public string DeliveryManEmail { get; set; }
        public string DeliveryManImageUrl { get; set; }
    }
}
