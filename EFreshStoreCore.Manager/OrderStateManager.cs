using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
   public class OrderStateManager:CommonManager<OrderState>,IOrderStateManager
    {
        public OrderStateManager() : base(new OrderStateRepository())
        {
        }
    }
}
