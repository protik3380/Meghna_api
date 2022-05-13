using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class MasterDepotProductLineRepository: CommonRepository<MasterDepotProductLine>, IMasterDepotProductLineRepository
    {
        public MasterDepotProductLineRepository() : base(new FreshContext())
        {
        }
    }
}