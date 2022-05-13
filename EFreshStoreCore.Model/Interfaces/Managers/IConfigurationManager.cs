using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IConfigurationManager : ICommonManager<Configuration>
    {
        Configuration GetById(long configId);
        Configuration GetActiveById(long configId);
        ICollection<Configuration> GetAll();
        ICollection<Configuration> GetAllActive();
    }
}
