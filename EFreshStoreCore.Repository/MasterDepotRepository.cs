using EFreshStore.Interfaces.Repositories;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Repository
{
    public class MasterDepotRepository : CommonRepository<MasterDepot>, IMasterDepotRepository
    {
        public MasterDepotRepository() : base(new FreshContext())
        {
        }
    }
}