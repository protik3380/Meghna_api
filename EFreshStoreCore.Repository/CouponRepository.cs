using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CouponRepository : CommonRepository<Coupon>, ICouponRepository
    {
        public CouponRepository() : base(new FreshContext())
        {
        }
    }
}
