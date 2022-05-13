using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ThanaManager : CommonManager<Thana>, IThanaManager
    {
        public ThanaManager() : base(new ThanaRepository())
        {
        }

        public Thana GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value,c=>c.District);
        }

        public ICollection<Thana> GetByDistrictId(long id)
        {
            return Get(c => c.DistrictId == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value,c=>c.District);
        }
        public ICollection<Thana> GetActiveByDistrictId(long id)
        {
            return Get(c => c.DistrictId == id
                && c.IsDeleted.HasValue && !c.IsDeleted.Value && c.IsActive.HasValue && c.IsActive.Value);
        }
        public ICollection<Thana> GetAll()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value, c => c.District);
        }

        public ICollection<Thana> GetActiveThanas()
        {
            return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value && c.IsActive.HasValue && !c.IsActive.Value, c => c.District);
        }

        public bool DoesThanaNameExistSameDistrict(string name, long? districtId)
        {
            Thana thana = GetFirstOrDefault(c => c.Name.ToLower().Trim().Equals(name.ToLower().Trim())
                                                       && c.IsDeleted.HasValue && !c.IsDeleted.Value
                                                       && c.DistrictId==districtId);
            return thana != null;
        }
    }
}