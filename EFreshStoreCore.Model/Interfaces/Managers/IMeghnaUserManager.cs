using System.Collections.ObjectModel;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IMeghnaUserManager : ICommonManager<MeghnaUser>
    {
        MeghnaUser GetById(long id);
        MeghnaUser GetByUserId(long id);
        bool GetByUserEmail(string email);
        ICollection<MeghnaUser> GetAll();
        int CountMeghnaUser();
    }
}