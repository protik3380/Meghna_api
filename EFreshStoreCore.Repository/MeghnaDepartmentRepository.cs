using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class MeghnaDepartmentRepository : CommonRepository<MeghnaDepartment>, IMeghnaDepartmentRepository
    {
        public MeghnaDepartmentRepository() : base(new FreshContext())
        {
        }
    }
}
