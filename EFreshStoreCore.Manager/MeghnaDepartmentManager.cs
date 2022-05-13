using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class MeghnaDepartmentManager : CommonManager<MeghnaDepartment>, IMeghnaDepartmentManager
    {
        public MeghnaDepartmentManager() : base(new MeghnaDepartmentRepository())
        {
        }

        public ICollection<MeghnaDepartment> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<MeghnaDepartment> GetActiveDepartments()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public MeghnaDepartment GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }
        public bool DoesMeghnaDepartmentExist(string name)
        {
            MeghnaDepartment meghnaDepartment = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                                                        &&!c.IsDeleted);
            return meghnaDepartment != null;
        }
        public bool SoftDelete(MeghnaDepartment entity)
        {
            entity.IsDeleted = true;
            return base.Update(entity);
        }
    }
}
