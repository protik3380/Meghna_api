using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class DistributorRepository : CommonRepository<Distributor>, IDistributorRepository
    {
        public DistributorRepository() : base(new FreshContext())
        {
        }
    }
}