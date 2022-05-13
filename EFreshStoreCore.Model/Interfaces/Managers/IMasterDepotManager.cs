using System.Collections;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IMasterDepotManager : ICommonManager<MasterDepot>
    {
        ICollection<MasterDepot> GetAll();
        ICollection<MasterDepot> GetActiveMasterDepots();
        MasterDepot GetById(long id);
        MasterDepot GetByThanaAndProduct(long thanaId);
        bool DoesMasterDepotEmailExist(string email);
        MasterDepot GetByUserId(long id);
        int CountMasterDepot();
    }
}