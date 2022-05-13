using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CorporateDepartmentRepository : CommonRepository<CorporateDepartment>, ICorporateDepartmentRepository
    {
        public CorporateDepartmentRepository() : base(new FreshContext())
        {
        }
    }
}
