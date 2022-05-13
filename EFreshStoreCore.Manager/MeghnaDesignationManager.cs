using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class MeghnaDesignationManager : CommonManager<MeghnaDesignation>, IMeghnaDesignationManager
    {
        public MeghnaDesignationManager() : base(new MeghnaDesignationRepository())
        {
        }

        public ICollection<MeghnaDesignation> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<MeghnaDesignation> GetActiveDesignations()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public MeghnaDesignation GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }
        public bool DoesMeghnaDesignationExist(string name)
        {
            MeghnaDesignation meghnaDesignation = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                                                                       && !c.IsDeleted);
            return meghnaDesignation != null;
        }
        public bool SoftDelete(MeghnaDesignation entity)
        {
            entity.IsDeleted = true;
            return base.Update(entity);
        }
    }
}
