using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICategoryManager : ICommonManager<Category>
    {
        ICollection<Category> GetAll();
        ICollection<Category> GetActiveCategories();
        Category GetById(long id);
        bool DoesCategoryNameExist(string name);
        int CountTotalCategory();
    }
}