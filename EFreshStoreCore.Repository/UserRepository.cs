using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        public UserRepository() : base(new FreshContext())
        {
        }
    }
}