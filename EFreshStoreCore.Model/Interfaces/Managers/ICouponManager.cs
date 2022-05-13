using System;
using System.Collections.Generic;
using EFreshStoreCore.Model.Context;

namespace EFreshStoreCore.Model.Interfaces.Managers
{
    public interface ICouponManager : ICommonManager<Coupon>
    {
        bool DoesCodeExist(string code);
        ICollection<Coupon> GetAll();
        Coupon GetById(long id);
        Coupon GetValidCouponById(string code, long userTypeId, DateTime date);
        bool SoftDelete(Coupon entity);
    }
}
