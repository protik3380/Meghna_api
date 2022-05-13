using System;
using System.Collections.Generic;
using System.Data.Entity;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;
using EFreshStoreCore.Repository;

namespace EFreshStoreCore.Manager
{
    public class CouponManager : CommonManager<Coupon>, ICouponManager
    {
        public CouponManager() : base(new CouponRepository())
        {
        }

        public bool DoesCodeExist(string code)
        {
            Coupon coupon = GetFirstOrDefault(c => c.Code.ToLower().Equals(code.ToLower())
                                                       && !c.IsDeleted);
            return coupon != null;
        }

        public ICollection<Coupon> GetAll()
        {
            return Get(c => !c.IsDeleted, 
                c => c.UserType);
        }

        public Coupon GetById(long id)
        {
            return GetFirstOrDefault(c => c.Id == id
                                          && !c.IsDeleted);
        }

        public Coupon GetValidCouponById(string code, long userTypeId, DateTime date)
        {
            return GetFirstOrDefault(c => c.Code.ToLower().Equals(code.ToLower())
                                                   && (c.UserTypeId == null || c.UserTypeId == userTypeId)             
                                                   && (c.Validity == null || (DbFunctions.TruncateTime(c.Validity) >= date.Date))
                                                   && !c.IsDeleted
                                                   && c.IsActive);
        }

        public bool SoftDelete(Coupon entity)
        {
            entity.IsDeleted = true;
            return base.Update(entity);
        }
    }
}
