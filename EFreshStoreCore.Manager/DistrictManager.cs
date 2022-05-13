using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class DistrictManager : CommonManager<District>, IDistrictManager
    {
        public DistrictManager() : base(new DistrictRepository())
        {
        }

        public District GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }

        public bool DoesDistrictNameExist(string name)
        {
            District district = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()) 
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            return district != null;
        }

        public ICollection<District> GetAll()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value);
        }

        public ICollection<District> GetActiveDistricts()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value && c.IsActive.HasValue && c.IsActive.Value);
        }
    }
}