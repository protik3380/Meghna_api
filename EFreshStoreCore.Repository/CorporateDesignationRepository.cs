using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CorporateDesignationRepository : CommonRepository<CorporateDesignation>, ICorporateDesignationRepository
    {
        public CorporateDesignationRepository() : base(new FreshContext())
        {
        }
    }
}
