using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;

namespace EFreshStoreCore.Api.Utility
{
    public class UtilityClass
    {
        public static Image ConvertByteToImage(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }
        public static string GenerateImageNameFromTimestamp()
        {
            DateTime value = DateTime.UtcNow.AddHours(6);
            return value.ToString("yyyyMMddHHmmssffff") + ".jpg";
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static CouponDiscountDto CalculateCouponDiscount(Coupon coupon, double totalAmount)
        {
            CouponDiscountDto couponDiscount = new CouponDiscountDto();
            couponDiscount.GrandTotal = totalAmount;
            var maximumDiscount = Convert.ToDouble(coupon.MaximumDiscount);
            if (coupon.DiscountPercentage != null)
            {
                var couponDiscountAmount = (totalAmount * Convert.ToDouble(coupon.DiscountPercentage)) / 100;
                if (coupon.MaximumDiscount != null)
                {
                    if (couponDiscountAmount > maximumDiscount)
                    {
                        couponDiscount.TotalUpdatedPrice = totalAmount - maximumDiscount;
                        couponDiscount.FinalCouponDiscount = maximumDiscount;
                    }
                    else
                    {
                        couponDiscount.TotalUpdatedPrice = totalAmount - couponDiscountAmount;
                        couponDiscount.FinalCouponDiscount = couponDiscountAmount;
                    }
                }
                else
                {
                    couponDiscount.TotalUpdatedPrice = totalAmount - couponDiscountAmount;
                    couponDiscount.FinalCouponDiscount = couponDiscountAmount;
                }
            }
            else
            {
                if (totalAmount >= maximumDiscount)
                {
                    couponDiscount.TotalUpdatedPrice = totalAmount - maximumDiscount;
                    couponDiscount.FinalCouponDiscount = maximumDiscount;
                }
                else
                {
                    couponDiscount.TotalUpdatedPrice = totalAmount - totalAmount;
                    couponDiscount.FinalCouponDiscount = totalAmount;
                }
            }

            return couponDiscount;
        }

        public static decimal CalculateDiscountForCorporateUser(CorporateUser corporateUser, ProductUnitDto productUnit, int discountPercentage)
        {
            decimal userDiscount = 0;
            decimal productDiscountPrice = (decimal)productUnit.MaximumRetailPrice;
            if (corporateUser.CorporateContract.FMCGDiscountPercentage != null)
            {
                userDiscount = (decimal) corporateUser.CorporateContract.FMCGDiscountPercentage;
                if (productUnit.Product.Category.ProductTypeId == (long)ProductTypeEnum.FMCG)
                {
                    if (discountPercentage > 0)
                    {
                        if (userDiscount > discountPercentage && 
                            (!corporateUser.CorporateContract.Validity.HasValue || (corporateUser.CorporateContract.Validity.Value.AddDays(1) > DateTime.Now)))
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice * userDiscount) /
                                                              100));
                        }
                        else
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice *
                                                               discountPercentage) / 100));
                        }

                    }
                    else
                    {
                        if (userDiscount > 0 &&
                            (!corporateUser.CorporateContract.Validity.HasValue || (corporateUser.CorporateContract.Validity.Value.AddDays(1) > DateTime.Now)))
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice * userDiscount) /
                                                              100));
                        }
                        else
                        {
                            productDiscountPrice = (decimal)productUnit.MaximumRetailPrice;
                        }
                    }
                }
            }

            if (corporateUser.CorporateContract.LPGDiscountPercentage != null)
            {
                userDiscount = (decimal)corporateUser.CorporateContract.LPGDiscountPercentage;
                if (productUnit.Product.Category.ProductTypeId == (long)ProductTypeEnum.LPG)
                {
                    if (discountPercentage > 0)
                    {
                        if (userDiscount > discountPercentage &&
                             (!corporateUser.CorporateContract.Validity.HasValue || (corporateUser.CorporateContract.Validity.Value.AddDays(1) > DateTime.Now)))
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice * userDiscount) /
                                                              100));
                        }
                        else
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice *
                                                               discountPercentage) / 100));
                        }

                    }
                    else
                    {
                        if (userDiscount > 0 &&
                            (!corporateUser.CorporateContract.Validity.HasValue || (corporateUser.CorporateContract.Validity.Value.AddDays(1) > DateTime.Now)))
                        {
                            productDiscountPrice = (decimal)(productUnit.MaximumRetailPrice -
                                                             ((productUnit.MaximumRetailPrice * userDiscount) /
                                                              100));
                        }
                        else
                        {
                            productDiscountPrice = (decimal)productUnit.MaximumRetailPrice;
                        }
                    }
                }
            }

            return productDiscountPrice;

        }

    }
}