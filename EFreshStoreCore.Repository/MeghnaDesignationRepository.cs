using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class MeghnaDesignationRepository : CommonRepository<MeghnaDesignation>, IMeghnaDesignationRepository
    {
        public MeghnaDesignationRepository() : base(new FreshContext())
        {
        }
    }
}
