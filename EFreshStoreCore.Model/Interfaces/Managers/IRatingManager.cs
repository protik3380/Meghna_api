using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface IRatingManager : ICommonManager<Rating>
    {
        Rating IsRatingExists(long userId, long productUnitId);
        ICollection<Rating> GetRatingByProductUnit(long id);
    }
}
