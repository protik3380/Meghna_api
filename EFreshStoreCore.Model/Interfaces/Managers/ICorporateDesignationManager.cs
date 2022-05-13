using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICorporateDesignationManager : ICommonManager<CorporateDesignation>
    {
        ICollection<CorporateDesignation> GetAll();
        ICollection<CorporateDesignation> GetActiveDesignations();
        CorporateDesignation GetById(long id);
        bool DoesCorporateDesignationExist(string name);
        bool SoftDelete(CorporateDesignation entity);
    }
}
