using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CorporateDepartmentManager : CommonManager<CorporateDepartment>, ICorporateDepartmentManager
    {
        public CorporateDepartmentManager() : base(new CorporateDepartmentRepository())
        {
        }

        public ICollection<CorporateDepartment> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<CorporateDepartment> GetActiveDepartments()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public CorporateDepartment GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }
        public bool DoesCorporateDepartmentExist(string name)
        {
            CorporateDepartment corporateDepartment = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                                                                       && !c.IsDeleted);
            return corporateDepartment != null;
        }
        public bool SoftDelete(CorporateDepartment entity)
        {
            entity.IsDeleted = true;
            return base.Update(entity);
        }
    }
}
