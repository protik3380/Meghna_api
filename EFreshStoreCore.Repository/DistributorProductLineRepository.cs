using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class DistributorProductLineRepository : CommonRepository<DistributorProductLine>, IDistributorProductLineRepository
    {
        public DistributorProductLineRepository() : base(new FreshContext())
        {
        }
    }
}