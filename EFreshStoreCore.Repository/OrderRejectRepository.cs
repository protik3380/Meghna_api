using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
   public class OrderRejectRepository:CommonRepository<OrderReject>,IOrderRejectRepository
    {
        public OrderRejectRepository() : base(new FreshContext())
        {
        }
    }
}
