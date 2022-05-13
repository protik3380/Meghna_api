using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CorporateContractManager : CommonManager<CorporateContract>, ICorporateContractManager
    {
        public CorporateContractManager() : base(new CorporateContractRepository())
        {
        }

        public ICollection<CorporateContract> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public bool DoesCorporateCompanyNameExist(string name)
        {
            CorporateContract corporateContract = GetFirstOrDefault(c => c.CompanyName.ToLower().Equals(name.ToLower()));
            return corporateContract != null;
        }

        public CorporateContract GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }        
    }
}