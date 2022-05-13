using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Helpers
{
    public class OrderGrowthRateParams
    {
        public DateTime PriorFromDate { get; set; }
        public DateTime PriorToDate { get; set; }
        public DateTime CurrentFromDate { get; set; }
        public DateTime CurrentToDate { get; set; }
    }
}
