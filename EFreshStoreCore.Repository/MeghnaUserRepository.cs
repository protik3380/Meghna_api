using EFreshStore.Interfaces.Repositories;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Repository
{
    public class MeghnaUserRepository : CommonRepository<MeghnaUser>, IMeghnaUserRepository
    {
        public MeghnaUserRepository() : base(new FreshContext())
        {
        }
    }
}