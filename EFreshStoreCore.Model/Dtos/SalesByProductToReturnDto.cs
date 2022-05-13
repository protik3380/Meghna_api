using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class SalesByProductToReturnDto
    {
        public string ProductName { get; set; }
        public string ProductUnit { get; set; }
        public int TotalProducts { get; set; }
        public double Price { get; set; }
        public long TotalOrders { get; set; }
        public string ProductTypeName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}
