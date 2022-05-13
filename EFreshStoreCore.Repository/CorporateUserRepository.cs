using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CorporateUserRepository : CommonRepository<CorporateUser>, ICorporateUserRepository
    {
        public CorporateUserRepository() : base(new FreshContext())
        {
        }
    }
}