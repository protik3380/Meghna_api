using System.Collections;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IThanaWiseMasterDepotManager : ICommonManager<ThanaWiseMasterDepot>
    {
        ThanaWiseMasterDepot GetByThana(long thanaId);
        bool IsExistMasterDepot(ThanaWiseMasterDepot thanaWiseMasterDepot);
        ICollection<ThanaWiseMasterDepot> GetAll();
        ICollection<ThanaWiseMasterDepot> GetByMasterDepotId(long id);
    }
}