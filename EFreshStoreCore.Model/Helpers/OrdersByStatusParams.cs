using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Helpers
{
    public class OrdersByStatusParams
    {
        public long[] BrandIds { get; set; }
        public long[] CategoryIds { get; set; }
        public long[] ProductTypeIds { get; set; }
        public long[] MasterDepotIds { get; set; }
        public long[] OrderStateIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? UserId { get; set; }
    }
}
