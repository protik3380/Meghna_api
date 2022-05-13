using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Repository
{
    class LoginRepository : IDisposable
    {
        FreshContext context = new FreshContext();
        
        public User ValidateUser(string username, string password)
        {
            return context.Users.FirstOrDefault(user =>
            user.Username.Equals(username)
            && user.Password == password
            && user.IsDeleted.HasValue && !user.IsDeleted.Value
            && user.IsActive.HasValue && user.IsActive.Value);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
