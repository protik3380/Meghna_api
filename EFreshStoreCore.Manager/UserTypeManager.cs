using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class UserTypeManager : CommonManager<UserType>, IUserTypeManager
    {
        public UserTypeManager() : base(new UserTypeRepository())
        {
        }

        public ICollection<UserType> GetAll()
        {
            return Get(c => c.IsActive.HasValue && (bool)c.IsActive);
        }
    }
}