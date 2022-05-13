using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;

namespace EFreshStoreCore.Repository
{
    public class FAQRepository : CommonRepository<FAQ>, IFAQRepository
    {
        public FAQRepository() : base(new FreshContext())
        {
        }
    }
}
