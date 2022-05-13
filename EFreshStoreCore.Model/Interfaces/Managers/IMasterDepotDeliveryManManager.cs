using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IMasterDepotDeliveryManManager : ICommonManager<MasterDepotDeliveryMan>
    {
        ICollection<MasterDepotDeliveryMan> GetDeliveryMenByMasterDepotId(long id);
        ICollection<MasterDepotDeliveryMan> GetDeliveryMenByMasterDepotIds(long[] ids);
        ICollection<MasterDepotDeliveryMan> GetByDeliveryMenId(long id);
    }
}
