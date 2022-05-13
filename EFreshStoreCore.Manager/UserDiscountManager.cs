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
   public class UserDiscountManager:CommonManager<UserDiscount>,IUserDiscountManager
    {
       public UserDiscountManager() : base(new UserDiscountRepository())
       {
       }

        public UserDiscount GetActiveDiscount()
        {
            var discount = GetFirstOrDefault(d => d.IsActive.HasValue && d.IsActive.Value && d.IsDeleted.HasValue && !d.IsDeleted.Value);
            return discount;
        }
    }
}
