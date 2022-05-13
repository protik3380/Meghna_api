using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class BrandManager : CommonManager<Brand>, IBrandManager
    {
        public BrandManager() : base(new BrandRepository())
        {
        }

        public Brand GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && !c.IsDeleted);
        }

        public bool DoesBrandNameExist(string name)
        {
            Brand brand = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                && !c.IsDeleted);
            return brand != null;
        }

        public ICollection<Brand> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<Brand> GetActiveBrands()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public int CountTotalBrand()
        {
            var totalBrand =  Get(c => !c.IsDeleted
                 && c.IsActive).Count;
            return totalBrand;
        }
    }
}