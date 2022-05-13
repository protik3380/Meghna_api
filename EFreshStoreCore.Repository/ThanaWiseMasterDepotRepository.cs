using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ThanaWiseMasterDepotRepository : CommonRepository<ThanaWiseMasterDepot>, IThanaWiseMasterDepotRepository
    {
        public ThanaWiseMasterDepotRepository() : base(new FreshContext())
        {
        }
    }
}