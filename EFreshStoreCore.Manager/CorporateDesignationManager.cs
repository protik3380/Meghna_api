using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CorporateDesignationManager : CommonManager<CorporateDesignation>, ICorporateDesignationManager
    {
        public CorporateDesignationManager() : base(new CorporateDesignationRepository())
        {
        }

        public ICollection<CorporateDesignation> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<CorporateDesignation> GetActiveDesignations()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public CorporateDesignation GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }
        public bool DoesCorporateDesignationExist(string name)
        {
            CorporateDesignation corporateDesignation = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                                                                             && !c.IsDeleted);
            return corporateDesignation != null;
        }
        public bool SoftDelete(CorporateDesignation entity)
        {
            entity.IsDeleted = true;
            return base.Update(entity);
        }
    }
}
