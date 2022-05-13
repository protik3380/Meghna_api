using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class DeliveryManUserInfo
    {
        public long Id { get; set; }
        public long DeliveryManId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
