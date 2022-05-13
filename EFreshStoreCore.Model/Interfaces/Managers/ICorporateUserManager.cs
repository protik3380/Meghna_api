using System.Collections;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICorporateUserManager : ICommonManager<CorporateUser>
    {
        CorporateUser GetById(long id);
        CorporateUser GetByUserId(long id);
        bool GetByUserEmail(string email);
        ICollection<CorporateUser> GetAll();
        ICollection<CorporateUser> GetByCorporateId(long id);
        int CountCorporateUser();

    }
}
