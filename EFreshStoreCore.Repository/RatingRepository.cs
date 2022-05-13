using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class RatingRepository : CommonRepository<Rating>, IRatingRepository
    {
        public RatingRepository() : base(new FreshContext())
        {
        }
    }
}
