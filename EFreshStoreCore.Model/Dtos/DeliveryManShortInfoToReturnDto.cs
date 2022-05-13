using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class DeliveryManShortInfoToReturnDto
    {
        public long Id { get; set; }
        public string NID { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string ImageUrl { get; set; }
        public string MasterDepots { get; set; }
    
    }
}
