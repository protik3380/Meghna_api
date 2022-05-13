using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class ThanaRepository : CommonRepository<Thana>, IThanaRepository
    {
        public ThanaRepository() : base(new FreshContext())
        {
        }
    }
}