using System;
using System.Collections.Generic;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class DistributorManager : CommonManager<Distributor>, IDistributorManager
    {
        public DistributorManager() : base(new DistributorRepository())
        {
        }

        public ICollection<Distributor> GetAll()
        {
            return Get(c => !c.IsDeleted, c => c.Thana);
        }

        public ICollection<Distributor> GetActiveDistributors()
        {
            return Get(c => !c.IsDeleted && c.IsActive);
        }

        public bool DoesDistributorEmailExist(string email)
        {
            Distributor distributor = GetFirstOrDefault(c => c.Email.ToLower().Equals(email.ToLower()));
            return distributor != null;
        }

        public Distributor GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id && !c.IsDeleted, c => c.MasterDepot, c => c.Thana);
        }
        public ICollection<Distributor> GetDistributorAgainstMasterDepot(long masterDepotId)
        {
            return Get(c => c.MasterDepotId == masterDepotId && !c.IsDeleted, c => c.MasterDepot, c => c.Thana);
        }
    }
}