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
    public class ConfigurationManager : CommonManager<Configuration>, IConfigurationManager
    {
        public ConfigurationManager() : base(new ConfigurationRepository())
        {
        }

        public Configuration GetById(long configId)
        {
            return GetFirstOrDefault(c => c.Id == configId
                                          && !c.IsDeleted);
        }

        public Configuration GetActiveById(long configId)
        {
            return GetFirstOrDefault(c => c.Id == configId && c.IsActive
                                                           && !c.IsDeleted);
        }

        public ICollection<Configuration> GetAll()
        {
            return Get(c => !c.IsDeleted);
        }

        public ICollection<Configuration> GetAllActive()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }
    }
}
