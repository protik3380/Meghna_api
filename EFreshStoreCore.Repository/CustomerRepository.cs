using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class CustomerRepository : CommonRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository() : base(new FreshContext())
        {
        }
    }
}