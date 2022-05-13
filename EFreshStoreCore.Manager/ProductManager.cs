using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class ProductManager : CommonManager<Product>, IProductManager
    {
        public ProductManager() : base(new ProductRepository())
        {
        }

        public Product GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && c.Brand.IsActive && c.Category.IsActive, 
                c => c.Category, c => c.Brand, c=> c.Category.ProductType);
        }
        public List<Product> GetAll()
        {
            return Get(c=> c.IsDeleted.HasValue 
                           && !c.IsDeleted.Value
                           && c.Brand.IsActive && c.Category.IsActive
                , c => c.Category, c => c.Brand, c => c.Category.ProductType).ToList();
        }

        //public ICollection<Product> Get()
        //{
        //    return Get(c => c.IsDeleted.HasValue && !c.IsDeleted.Value
        //        && c.IsActive.HasValue && c.IsActive.Value, c => c.Category, c => c.Brand);
        //}

        public ICollection<Product> GetActiveProducts()
        {
            return Get(c => c.IsDeleted.HasValue 
                            && !c.IsDeleted.Value 
                            && c.IsActive.HasValue 
                            && c.IsActive.Value
                            && c.Brand.IsActive 
                            && c.Category.IsActive
                , c => c.Category, c => c.Brand, c => c.Category.ProductType);
        }
        public ICollection<Product> GetByProductTypeIdAndCategoryId(long productTypeId, long categoryId)
        {
            return Get(c => c.CategoryId==categoryId && c.Category.ProductTypeId==productTypeId && c.IsDeleted.HasValue
                            && !c.IsDeleted.Value
                            && c.IsActive.HasValue
                            && c.IsActive.Value
                            && c.Brand.IsActive
                            && c.Category.IsActive
                , c => c.Category, c => c.Brand, c => c.Category.ProductType);
        }

        public bool DoesProductNameExist(string name)
        {
            Product product = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower())
                && c.IsDeleted.HasValue && !c.IsDeleted.Value);
            return product != null;
        }

        public ICollection<Product> GetByBrand(long brandId)
        {
            return base.Get(c=>c.BrandId == brandId && c.Brand.IsActive && c.Category.IsActive, 
                
                c => c.Category, c => c.Category.ProductType).ToList();
        }
        public ICollection<Product> GetByCategory(long categoryId)
        {
            return base.Get(c=>c.CategoryId == categoryId && c.Brand.IsActive && c.Category.IsActive, 
                c => c.Brand, c => c.Category.ProductType).ToList();
        }
    }
}