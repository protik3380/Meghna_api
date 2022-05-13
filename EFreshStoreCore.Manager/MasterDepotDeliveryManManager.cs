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
    public class MasterDepotDeliveryManManager : CommonManager<MasterDepotDeliveryMan>, IMasterDepotDeliveryManManager
    {
        public MasterDepotDeliveryManManager() : base(new MasterDepotDeliveryManRepository())
        {
        }

        public ICollection<MasterDepotDeliveryMan> GetDeliveryMenByMasterDepotId(long id)
        {
            return Get(c => c.MasterDepotId == id && c.IsActive && !c.IsDeleted,
                c => c.DeliveryMan, c => c.DeliveryMan.Thana,
                c => c.DeliveryMan.Thana.District, c => c.MasterDepot);
        }

        public ICollection<MasterDepotDeliveryMan> GetDeliveryMenByMasterDepotIds(long[] ids)
        {
            return Get(c => ids.Contains((long)c.MasterDepotId) && c.IsActive && !c.IsDeleted,
                c => c.DeliveryMan, c => c.DeliveryMan.Thana,
                c => c.DeliveryMan.Thana.District,
                c=>c.MasterDepot);
        }

        public ICollection<MasterDepotDeliveryMan> GetByDeliveryMenId(long id)
        {
            return Get(c => c.DeliveryManId == id && c.IsActive && !c.IsDeleted, c => c.MasterDepot);
        }
    }
}
