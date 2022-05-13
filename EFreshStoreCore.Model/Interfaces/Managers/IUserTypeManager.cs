using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IUserTypeManager : ICommonManager<UserType>
    {
        ICollection<UserType> GetAll();
    }
}
