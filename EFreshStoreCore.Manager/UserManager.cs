using System;
using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class UserManager : CommonManager<User>, IUserManager
    {
        public UserManager() : base(new UserRepository())
        {
        }

        public User GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id&&c.IsActive.HasValue&&c.IsActive.Value&&
            c.IsDeleted.HasValue&&!c.IsDeleted.Value, c=> c.CorporateUsers, c=> c.MeghnaUsers, c=> c.Customers);
        }

        public User ValidateUser(string username, string password)
        {
            User user = GetFirstOrDefault(c =>
                c.Username.ToLower().Equals(username.ToLower())
                && c.Password.Equals(password)
                && c.IsActive == true &&
                c.IsDeleted == false,
                u=> u.UserType);
            return user;
        }
        public User ValidateDeliveryManUser(string username, string password)
        {
            User user = GetFirstOrDefault(c =>
                c.Username.ToLower().Equals(username.ToLower())
                && c.Password.Equals(password)
                && c.IsActive == true &&
                c.IsDeleted == false,
                u => u.DeliveryMen);
            return user;
        }
        public User GetByUserEmail(string email)
        {
            return GetFirstOrDefault(c => c.Username == email && c.IsActive.HasValue && c.IsActive.Value && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            //User user = GetFirstOrDefault(c => c.Username == email);
            //return user;
        }

        public ICollection<User> GetAllActiveUser()
        {
            var userList = Get(c=> c.IsActive.HasValue && c.IsActive.Value && c.IsDeleted.HasValue && !c.IsDeleted.Value).ToList();
            return userList;
        }
        public int TotalAllOrderableUser()
        {
            var userList = Get(c=> c.IsActive.HasValue && c.IsActive.Value && c.IsDeleted.HasValue && !c.IsDeleted.Value && c.UserTypeId!=(long)UserTypeEnum.MasterDepotUser && c.UserTypeId!=(long)UserTypeEnum.Admin && c.UserTypeId!=(long)UserTypeEnum.DeliveryMan).Count();
            return userList;
        }

        public User DoesUsernameExist(string username)
        {
            User user =  GetFirstOrDefault(c => c.Username.Equals(username) && c.IsActive.HasValue && c.IsActive.Value && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            return user;
        }
        
    }
}