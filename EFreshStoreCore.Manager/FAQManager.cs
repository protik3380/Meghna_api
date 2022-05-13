using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class FAQManager : CommonManager<FAQ>, IFAQManager
    {
        public FAQManager() : base(new FAQRepository())
        {
        }
        public ICollection<FAQ> GetAll()
        {
            return Get(c =>!c.IsDeleted);
        }
        public ICollection<FAQ> GetAllActive()
        {
            return Get(c =>c.IsActive && !c.IsDeleted);
        }

        public bool DoesFAQExist(string question)
        {
            FAQ faq = GetFirstOrDefault(c => c.Question.Contains(question) && !c.IsDeleted);
            return faq != null;
        }
        public FAQ GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                && !c.IsDeleted);
        }

    }
}
