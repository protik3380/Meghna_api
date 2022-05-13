using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class OrderDetailRepository : CommonRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository() : base(new FreshContext())
        {
        }
    }
}