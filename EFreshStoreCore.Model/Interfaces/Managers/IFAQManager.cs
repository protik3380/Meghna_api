using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IFAQManager : ICommonManager<FAQ>
    {
        ICollection<FAQ> GetAll();
        ICollection<FAQ> GetAllActive();
        bool DoesFAQExist(string question);
        FAQ GetById(long id);
    }
}
