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
    public class DeliveryChargeManager : CommonManager<DeliveryCharge>, IDeliveryChargeManager
    {
        public DeliveryChargeManager() : base(new DeliveryChargeRepository())
        {
            
        }

        public DeliveryCharge GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted);

        }

        public DeliveryCharge GetActiveDeliveryCharge()
        {
            return GetFirstOrDefault(c => c.IsActive
                                          && !c.IsDeleted);
        }
    }
}
