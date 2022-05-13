using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class BrandRepository : CommonRepository<Brand>, IBrandRepository
    {
        public BrandRepository() : base(new FreshContext())
        {
        }
    }
}