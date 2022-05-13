using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Repositories;
namespace EFreshStoreCore.Repository
{
    public class DistrictRepository: CommonRepository<District>, IDistrictRepository
    {
        public DistrictRepository() : base(new FreshContext())
        {
        }
    }
}