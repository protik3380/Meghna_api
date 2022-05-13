using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFreshStoreCore.Model.Dtos
{
    public class RatingToReturnDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductUnitId { get; set; }
        public short Rating1 { get; set; }
        public string Review { get; set; }
        public System.DateTime RatingTime { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
    }
}
