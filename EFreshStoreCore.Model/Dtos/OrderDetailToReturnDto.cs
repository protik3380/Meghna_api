using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class OrderDetailToReturnDto
    {
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int ProductQuantity { get; set; }
        public string StockKeepingUnit { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
