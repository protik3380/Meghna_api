using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Helpers;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class CouponController : ApiController
    {
        private readonly ICouponManager _couponManager;
        private readonly IOrderManager _orderManager;
        public CouponController()
        {
            _couponManager = new CouponManager();
            _orderManager = new OrderManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var categories = _couponManager.GetAll();
                if (categories == null) return NotFound();
                return Ok(categories.OrderBy(c => c.CreatedOn).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                bool isFound = _couponManager.DoesCodeExist(coupon.Code);
                if (isFound)
                {
                    return Conflict();
                }
                bool isSaved = _couponManager.Add(coupon);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), coupon);
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]Coupon coupon)
        {
            var existingCoupon = _couponManager.GetById(coupon.Id);
            if (coupon.Code == existingCoupon.Code)
            {
                try
                {
                    bool isSaved = _couponManager.Update(coupon);
                    if (isSaved)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            bool isFound = _couponManager.DoesCodeExist(coupon.Code);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _couponManager.Update(coupon);
                if (isSaved)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult CheckValidity([FromBody]CouponParams couponParams)
        {
            try
            {
                var dateToday = DateTime.UtcNow.AddHours(6);
                var coupon = _couponManager.GetValidCouponById(couponParams.CouponCode, couponParams.UserTypeId, dateToday);
                if (coupon == null) return NotFound();
                //if (coupon.MaximumOrderNo != null)
                //{
                //    var orderCount = _orderManager.GetTotalOrdersByUserIdAndCouponCode(couponParams);
                //    if (orderCount >= coupon.MaximumOrderNo)
                //    {
                //        return BadRequest("Maximum number of orders exceeded for this coupon!");
                //    }
                //}
                CouponDiscountDto couponDiscount = new CouponDiscountDto();
                if (coupon.MinimumOrderAmount != null)
                {
                    if (couponParams.GrandTotal >= Convert.ToDouble(coupon.MinimumOrderAmount))
                    {
                        if (coupon.DiscountPercentage == null && coupon.MaximumDiscount == null)
                        {
                            return BadRequest("Invalid Coupon Code !");
                        }

                        couponDiscount = UtilityClass.CalculateCouponDiscount(coupon, couponParams.GrandTotal);
                    }
                    else
                    {
                        return BadRequest("You need to buy a minimum of " + coupon.MinimumOrderAmount + " taka");
                    }
                }
                else
                {
                    if (coupon.DiscountPercentage == null && coupon.MaximumDiscount == null)
                    {
                        return BadRequest("Invalid Coupon Code !");
                    }

                    couponDiscount = UtilityClass.CalculateCouponDiscount(coupon, couponParams.GrandTotal);
                }
                return Ok(couponDiscount);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var coupon = _couponManager.GetById(id);
                if (coupon == null) return NotFound();
                return Ok(coupon);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public IHttpActionResult Delete(long couponId, long userId)
        {
            try
            {
                var coupon = _couponManager.GetById(couponId);
                if (coupon == null)
                {
                    return NotFound();
                }

                coupon.ModifiedBy = userId;
                coupon.ModifiedOn = DateTime.UtcNow.AddHours(6);
                var isDeleted = _couponManager.SoftDelete(coupon);

                if (isDeleted)
                {
                    return Ok();
                }
                return BadRequest("Delete failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
