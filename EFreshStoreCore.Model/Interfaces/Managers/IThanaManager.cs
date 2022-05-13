using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IThanaManager : ICommonManager<Thana>
    {
        Thana GetById(long id);
        ICollection<Thana> GetByDistrictId(long id);
        ICollection<Thana> GetActiveByDistrictId(long id);
        ICollection<Thana> GetAll();
        ICollection<Thana> GetActiveThanas();
        bool DoesThanaNameExistSameDistrict(string name,long? districtId);
    }
}