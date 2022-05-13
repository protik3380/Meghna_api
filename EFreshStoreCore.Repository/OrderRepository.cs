using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{

    public class OrderRepository : CommonRepository<Order>, IOrderRepository
    {
        public OrderRepository() : base(new FreshContext())
        {
        }
    }
}