using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICorporateContractManager : ICommonManager<CorporateContract>
    {
        CorporateContract GetById(long id);
        ICollection<CorporateContract> GetAll();
        bool DoesCorporateCompanyNameExist(string name);

    }
}