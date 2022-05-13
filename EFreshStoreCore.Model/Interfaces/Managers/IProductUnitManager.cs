using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductUnitManager:ICommonManager<ProductUnit>
    {
        ICollection<ProductUnit> GetAll();
        ProductUnit GetById(long id);
        ICollection<ProductUnit> GetByBrand(long brandId);
        ICollection<ProductUnit> GetByBrandId(long brandId);
        ICollection<ProductUnit> GetByBrandIds(long[] brandIds);
        ICollection<ProductUnit> GetByCategoryIds(long[] categoryIds);
        ICollection<ProductUnit> GetByCategory(long categoryId);
        ICollection<ProductUnit> GetByBrandIdsAndCategoryIds(long[] brandIds, long[] categoryIds);

        bool SaveProductDetails(ProductUnit productUnit);
        bool EditProductDetails(ProductUnit productUnit, bool hasNewImages);
        ProductUnit Get(long id);
        ICollection<ProductUnit> Get();
        ICollection<ProductUnit> GetProductBySearch(string searchString);

        ICollection<ProductUnit> GetByProductId(long id);
    }
}
