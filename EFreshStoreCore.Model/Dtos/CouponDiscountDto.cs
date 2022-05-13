using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class CouponDiscountDto
    {
        public long CouponId { get; set; }
        public string CouponCode { get; set; }
        public double FinalCouponDiscount { get; set; }
        public double TotalUpdatedPrice { get; set; }
        public double GrandTotal { get; set; }
    }
}
