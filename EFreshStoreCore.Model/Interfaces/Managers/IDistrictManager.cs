using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
   public interface IDistrictManager: ICommonManager<District>
    {
        District GetById(long districtId);
        bool DoesDistrictNameExist(string name);
        ICollection<District> GetAll();
        ICollection<District> GetActiveDistricts();
    }
}
