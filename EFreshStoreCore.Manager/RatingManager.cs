using System.Collections.Generic;
using System.Linq;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class RatingManager : CommonManager<Rating>, IRatingManager
    {
        public RatingManager() : base(new RatingRepository())
        {
        }
        public Rating IsRatingExists(long userId, long productUnitId)
        {
            Rating rating = GetFirstOrDefault(r => r.ProductUnit.Id == productUnitId && r.UserId == userId,
                r => r.ProductUnit,
                r=> r.ProductUnit.Product,
                r=>r.User);
            return rating;

        }
        public ICollection<Rating> GetRatingByProductUnit(long id)
        {
            return Get(r => r.ProductUnitId == id,
                r => r.ProductUnit,
                r=>r.ProductUnit.Product,
                r=>r.User,
                r=>r.User.UserType);
        }

    }
}
