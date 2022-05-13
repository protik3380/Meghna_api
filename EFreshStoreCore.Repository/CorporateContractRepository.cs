using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CorporateContractRepository : CommonRepository<CorporateContract>, ICorporateContractRepository
    {
        public CorporateContractRepository() : base(new FreshContext())
        {
        }
    }
}