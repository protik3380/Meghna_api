using EFreshStoreCore.Model.Context;
using System.Collections.Generic;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IDistributorProductLineManager : ICommonManager<DistributorProductLine>
    {
        DistributorProductLine IsSubscribedToProductLine(DistributorProductLine distributorProductLine);
        ICollection<DistributorProductLine> GetProductLineByDistributorId(long id);
    }
}