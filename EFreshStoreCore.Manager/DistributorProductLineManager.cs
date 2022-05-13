using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;
using System.Collections.Generic;

namespace EFreshStoreCore.Manager
{
    public class DistributorProductLineManager: CommonManager<DistributorProductLine>, IDistributorProductLineManager
    {
        public DistributorProductLineManager() : base(new DistributorProductLineRepository())
        {
        }

        public DistributorProductLine IsSubscribedToProductLine(DistributorProductLine distributorProductLine)
        {
            return GetFirstOrDefault(d => d.ProductLineId == distributorProductLine.ProductLineId &&
                d.DistributorId == distributorProductLine.DistributorId);
        }

        public ICollection<DistributorProductLine> GetProductLineByDistributorId(long id)
        {
            return Get(d => d.DistributorId == id && d.IsActive.HasValue && d.IsActive == true && d.IsDeleted.HasValue && d.IsDeleted == false, d => d.ProductLine);
        }
    }
}