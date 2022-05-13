using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Helpers
{
    public class CouponParams
    {
        public string CouponCode { get; set; }
        public long UserTypeId { get; set; }
        public long UserId { get; set; }
        public double GrandTotal { get; set; }
    }
}
