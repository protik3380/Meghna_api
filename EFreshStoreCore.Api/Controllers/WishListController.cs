using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class WishListController : ApiController
    {
        private readonly IWishListManager _wishListManager;
        private readonly IProductDiscountManager _productDiscountManager;
        private readonly ICorporateUserManager _corporateUserManager;
        public WishListController()
        {
            _wishListManager = new WishListManager();
            _productDiscountManager = new ProductDiscountManager();
            _corporateUserManager = new CorporateUserManager();
        }
        [HttpPost]
        public IHttpActionResult Create([FromBody]WishList wishList)
        {
            try
            {
                var wishListFromRepo = _wishListManager.GetWishlistProduct(wishList.UserId, wishList.ProductUnitId);
                if (wishListFromRepo != null)
                {
                    bool isDeleted = _wishListManager.DeleteWishList(wishListFromRepo.Id);
                    if (isDeleted)
                    {
                        return Ok();
                    }
                    return BadRequest("Failed!");
                }
                wishList.AddedOn = DateTime.UtcNow.AddHours(6);
                bool isSaved = _wishListManager.Add(wishList);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), wishList);
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Delete(long id)
        {
            try
            {
                bool isDeleted = _wishListManager.DeleteWishList(id);
                if (isDeleted)
                {
                    return Ok();
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetByUser(long userId)
        {
            try
            {
                var wishList = GetDetailCartByUserId(userId);
                wishList = wishList.OrderByDescending(o => o.AddedOn).ToList();
                return Ok(wishList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private List<WishListVm> GetDetailCartByUserId(long userId)
        {
            var wishList = _wishListManager.GetByUser(userId);
            List<WishListVm> detailWishList = new List<WishListVm>();

            foreach (WishList wishListProduct in wishList)
            {
                if (wishListProduct.ProductUnit.ProductImages.Count!=0)
                {
                    string filePath = wishListProduct.ProductUnit.ProductImages.FirstOrDefault().ImageLocation.ToString();
                    if (!File.Exists( HttpContext.Current.Server.MapPath("~/" + filePath)))
                    {
                        wishListProduct.ProductUnit.ProductImages.FirstOrDefault().ImageLocation= "/Content/img/product/NoProductImage.png";
                    }
                }
                var userTypeId = wishListProduct.User.UserTypeId;
                WishListVm vm = new WishListVm
                {
                    WishListId  = wishListProduct.Id,
                    UserId = wishListProduct.UserId,
                    ProductUnitId = wishListProduct.ProductUnitId,
                    ProductTypeId = (long) wishListProduct.ProductUnit.Product.Category.ProductTypeId,
                    ProductName = wishListProduct.ProductUnit.Product.Name,
                    ProductUnit = wishListProduct.ProductUnit.StockKeepingUnit,
                    Brand = wishListProduct.ProductUnit.Product.Brand.Name,
                    Category = wishListProduct.ProductUnit.Product.Category.Name,
                    ProductImage= wishListProduct.ProductUnit.ProductImages.FirstOrDefault() == null ? null : wishListProduct.ProductUnit.ProductImages.FirstOrDefault().ImageLocation,
                    DistributorPrice = (decimal)wishListProduct.ProductUnit.DistributorPricePerCarton / Convert.ToDecimal(wishListProduct.ProductUnit.CartonSize),
                    UnitPrice = (decimal)wishListProduct.ProductUnit.MaximumRetailPrice,
                    AddedOn = wishListProduct.AddedOn
                };
                ProductDiscount productDiscount = _productDiscountManager.GetByProductUnitId(wishListProduct.ProductUnitId);
                var productDiscountPercentage = 0;
                if (productDiscount != null)
                {
                    if (productDiscount.Validity.Value.AddDays(1) > DateTime.Now)
                    {
                        productDiscountPercentage = (int)productDiscount.DiscountPercentage;
                    }
                }
                if (userTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    vm.Price = (decimal)wishListProduct.ProductUnit.DistributorPricePerCarton / Convert.ToDecimal(wishListProduct.ProductUnit.CartonSize); ;
                }
                else if (userTypeId == (long)UserTypeEnum.Customer)
                {
                    if (productDiscountPercentage > 0)
                    {
                        vm.Price = (decimal)wishListProduct.ProductUnit.MaximumRetailPrice - (decimal)((wishListProduct.ProductUnit.MaximumRetailPrice * productDiscountPercentage) / 100);
                    }
                    else
                    {
                        vm.Price = (decimal)wishListProduct.ProductUnit.MaximumRetailPrice;
                    }

                }
                else if (userTypeId == (long)UserTypeEnum.Corporate)
                {
                    CorporateUser corporateUser = _corporateUserManager.GetByUserId(wishListProduct.UserId);
                    ProductUnitDto productUnit = new ProductUnitDto
                    {
                        MaximumRetailPrice = wishListProduct.ProductUnit.MaximumRetailPrice,
                        Product = wishListProduct.ProductUnit.Product
                    };

                    if (corporateUser.CorporateContract.FMCGDiscountPercentage != null ||
                        corporateUser.CorporateContract.LPGDiscountPercentage != null)
                    {
                        vm.Price = UtilityClass.CalculateDiscountForCorporateUser(corporateUser, productUnit, productDiscountPercentage);
                    }
                }
                else
                {
                    if (productDiscountPercentage > 0)
                    {
                        vm.Price = (decimal)wishListProduct.ProductUnit.MaximumRetailPrice - (decimal)((wishListProduct.ProductUnit.MaximumRetailPrice * productDiscountPercentage) / 100);
                    }
                    else
                    {
                        vm.Price = (decimal)wishListProduct.ProductUnit.MaximumRetailPrice;
                    }
                }


                detailWishList.Add(vm);
            }

            return detailWishList;
        }
    }
}
