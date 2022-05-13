using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CategoryRepository : CommonRepository<Category>, ICategoryRepository
    {
        public CategoryRepository() : base(new FreshContext())
        {
        }
    }
}