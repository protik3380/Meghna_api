using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class SubscriberRepository : CommonRepository<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository() : base(new FreshContext())
        {
        }
    }
}
