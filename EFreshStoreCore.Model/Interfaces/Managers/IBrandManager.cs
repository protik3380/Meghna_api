using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IBrandManager : ICommonManager<Brand>
    {
        Brand GetById(long brandId);
        bool DoesBrandNameExist(string name);
        ICollection<Brand> GetAll();
        ICollection<Brand> GetActiveBrands();
        int CountTotalBrand();
    }
}
