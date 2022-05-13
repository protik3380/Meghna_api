using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class MasterDepotProductLineManager : CommonManager<MasterDepotProductLine>, IMasterDepotProductLineManager
    {
        public MasterDepotProductLineManager() : base(new MasterDepotProductLineRepository())
        {
        }
    }
}