using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CategoryManager : CommonManager<Category>, ICategoryManager
    {
        public CategoryManager() : base(new CategoryRepository())
        {
        }

        public ICollection<Category> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<Category> GetActiveCategories()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public Category GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted);
        }

        public bool DoesCategoryNameExist(string name)
        {
            Category category = GetFirstOrDefault(c => c.Name.ToLower().Equals(name.ToLower()) 
                                                       && !c.IsDeleted);
            return category != null;
        }

        public int CountTotalCategory()
        {
            return Get(c =>  c.IsActive && !c.IsDeleted).Count;
        }
    }
}