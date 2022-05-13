using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IProductManager : ICommonManager<Product>
    {
        Product GetById(long id);
        List<Product> GetAll();
        ICollection<Product> GetByBrand(long brandId);
        ICollection<Product> GetByCategory(long categoryId);
        //ICollection<Product> Get();
        ICollection<Product> GetActiveProducts();
        ICollection<Product> GetByProductTypeIdAndCategoryId(long productTypeId, long categoryId);
        bool DoesProductNameExist(string name);
    }
}