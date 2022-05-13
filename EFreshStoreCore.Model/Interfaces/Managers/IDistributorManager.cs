using System.Collections;
using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IDistributorManager : ICommonManager<Distributor>
    {
        Distributor GetById(long distrobutorId);
        ICollection<Distributor> GetAll();
        ICollection<Distributor> GetDistributorAgainstMasterDepot(long distrobutorId);
        ICollection<Distributor> GetActiveDistributors();
        bool DoesDistributorEmailExist(string email);
    }
   
}