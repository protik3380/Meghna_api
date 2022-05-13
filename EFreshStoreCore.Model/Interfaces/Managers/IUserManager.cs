using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IUserManager : ICommonManager<User>
    {
        User GetById(long id);
        User ValidateUser(string username, string password);
        User ValidateDeliveryManUser(string username, string password);
        User GetByUserEmail(string email);
        ICollection<User> GetAllActiveUser();
        int TotalAllOrderableUser();
        User DoesUsernameExist(string username);
    }
}
