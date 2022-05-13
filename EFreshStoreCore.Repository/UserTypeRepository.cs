using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class UserTypeRepository : CommonRepository<UserType>, ICommonRepository<UserType>
    {
        public UserTypeRepository() : base(new FreshContext())
        {
        }
    }
}