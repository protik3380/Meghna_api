using System;

namespace EFreshStoreCore.Model.Helpers
{
    public class SalesByProductParams
    {
        public long[] BrandIds { get; set; }
        public long[] CategoryIds { get; set; }
        public long[] ProductTypeIds { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
