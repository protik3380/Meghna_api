using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Repository
{
    class LoginRepository : IDisposable
    {
        FreshContext context = new FreshContext();
        protected readonly IUserTypeManager _UserTypeManager = new UserTypeManager();
        
        public User ValidateUser(string username, string password)
        {
            User aUser = context.Users.FirstOrDefault(user =>
            user.Username.Equals(username)
            && user.Password == password
            && user.IsDeleted.HasValue && !user.IsDeleted.Value
            && user.IsActive.HasValue && user.IsActive.Value);
            var allUserType = _UserTypeManager.GetAll();
            //var type = usert.FirstOrDefault(u => u.Id.Equals(aUser.UserTypeId));
            
            if (aUser != null)
            {
                aUser.UserType = allUserType.FirstOrDefault(u => u.Id.Equals(aUser.UserTypeId));
            }
            return aUser;
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
