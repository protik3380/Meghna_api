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
    public class CartController : ApiController
    {
        private readonly ICartManager _cartManager;
        private readonly IProductDiscountManager _productDiscountManager;
        private readonly ICorporateUserManager _corporateUserManager;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILPGComboDiscountManager _lpgComboDiscountManager;
        private readonly IProductUnitManager _productUnitManager;


        public CartController()
        {
            _cartManager = new CartManager();
            _productDiscountManager = new ProductDiscountManager();
            _corporateUserManager = new CorporateUserManager();
            _configurationManager = new ConfigurationManager();
            _lpgComboDiscountManager = new LPGComboDiscountManager();
            _productUnitManager = new ProductUnitManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]Cart cart)
        {
            try
            {

                var existedProduct = _cartManager.IsProductExist(cart.UserId, cart.ProductUnitId);
                if (existedProduct != null)
                {
                    if (cart.Quantity > 0)
                    {
                        cart.Id = existedProduct.Id;
                        cart.User = existedProduct.User;
                        cart.AddedOn = DateTime.Now;
                        cart.ProductUnit = existedProduct.ProductUnit;
                        bool isUpdate = _cartManager.Update(cart);
                        if (isUpdate)
                        {
                            var cartList = GetDetailCartByUserId(cart.UserId);
                            return Ok(cartList);
                        }
                    }
                }
                else
                {
                    cart.AddedOn = DateTime.UtcNow.AddHours(6);
                    bool isSaved = _cartManager.Add(cart);
                    if (isSaved)
                    {
                        var cartList = GetDetailCartByUserId(cart.UserId);
                        return Ok(cartList);
                    }
                }

                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Delete(long userId, long productUnitId)
        {
            var existedProduct = _cartManager.IsProductExist(userId, productUnitId);

            try
            {
                bool isDeleted = _cartManager.DeleteCart(existedProduct.Id);
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

        [HttpPost]
        public IHttpActionResult DeleteAll(long userId)
        {
            try
            {
                bool isDeleted = _cartManager.DeleteAllFromCart(userId);
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

        [HttpGet]
        public IHttpActionResult GetByUser(long userId)
        {
            try
            {
                var cartList = GetDetailCartByUserId(userId);
                return Ok(cartList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult GetCalculatedCartProductPrice([FromBody]List<CartVm> cart)
        {
            try
            {
                var cartList = GetCalculatedCartProductPriceForGuestUser(cart);
                return Ok(cartList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private List<CartVm> GetDetailCartByUserId(long userId)
        {
            var cartList = _cartManager.GetByUser(userId);
            List<CartVm> detailCartList = new List<CartVm>();
            var lpg = cartList.FirstOrDefault(x =>
                x.ProductUnit.Product.Category.ProductTypeId == (long)ProductTypeEnum.LPG);

            var activeConfig = _configurationManager.GetActiveById((long) ConfigurationEnum.LPGCombo);

            var lpgDiscount = _lpgComboDiscountManager.GetValidLpgComboDiscount();
            if (lpg != null && activeConfig != null && lpgDiscount != null)
            {
                var maxLPGPrice = cartList.Where(x =>
                        x.ProductUnit.Product.Category.ProductTypeId == (long)ProductTypeEnum.LPG)
                    .Max(x => x.ProductUnit.MaximumRetailPrice);

                detailCartList = GetCalculatedPriceForLPGDiscount(cartList.ToList(), (decimal)maxLPGPrice, lpgDiscount);
            }
            else
            {
                detailCartList = GetCalculatedPriceWithoutLPGDiscount(cartList.ToList());
            }

            return detailCartList;

        }

        private List<CartVm> GetCalculatedPriceForLPGDiscount(List<Cart> cartList, decimal maxLPGPrice, LPGComboDiscount lpgDiscount)
        {
            decimal totalDiscount = 0;
            List<CartVm> detailCartList = new List<CartVm>();
            foreach (Cart cart in cartList)
            {
                CartVm vm = new CartVm();
                vm.CartId = cart.Id;
                vm.UserId = cart.UserId;
                vm.ProductUnitId = cart.ProductUnitId;
                vm.ProductName = cart.ProductUnit.Product.Name;
                vm.ProductUnit = cart.ProductUnit.StockKeepingUnit;
                vm.Brand = cart.ProductUnit.Product.Brand.Name;
                vm.Category = cart.ProductUnit.Product.Category.Name;
                vm.Quantity = cart.Quantity;
                if (cart.ProductUnit.ProductImages.Count == 0)
                {
                    vm.ProductImage = null;
                }
                else
                {
                    string filePath = cart.ProductUnit.ProductImages.FirstOrDefault().ImageLocation.ToString();
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/" + cart.ProductUnit.ProductImages.FirstOrDefault()
                                                                   .ImageLocation.ToString())))
                    {
                        vm.ProductImage = "/Content/img/product/NoProductImage.png";
                    }
                    else
                    {
                        vm.ProductImage = cart.ProductUnit.ProductImages.FirstOrDefault().ImageLocation;
                    }

                }
                vm.CartonSize = cart.ProductUnit.CartonSize;
                var maximumRetailPrice = (decimal) cart.ProductUnit.MaximumRetailPrice;
                decimal discountPrice = 0;

                if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.TradePrice)
                {
                    discountPrice = Convert.ToDecimal(cart.ProductUnit.TradePricePerCarton) / 
                               Convert.ToDecimal(cart.ProductUnit.CartonSize);
                } else if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.DistributorPrice)
                {
                    discountPrice = Convert.ToDecimal(cart.ProductUnit.DistributorPricePerCarton) /
                                    Convert.ToDecimal(cart.ProductUnit.CartonSize);
                }
                else if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.DiscountPercentage)
                {
                    if (lpgDiscount.DiscountPercentage > 0)
                    {
                        discountPrice = maximumRetailPrice - (decimal)((maximumRetailPrice * lpgDiscount.DiscountPercentage) / 100);
                    }
                    else
                    {
                        discountPrice = maximumRetailPrice;
                    }
                }
                else
                {
                    discountPrice = maximumRetailPrice;
                }

                if (cart.ProductUnit.Product.Category.ProductTypeId == (long)ProductTypeEnum.LPG)
                {
                    discountPrice = maximumRetailPrice;
                }

                var discount = maximumRetailPrice - discountPrice;

                if (discount > 0)
                {
                    if (totalDiscount + discount * cart.Quantity > maxLPGPrice)
                    {
                        discount = maxLPGPrice - totalDiscount;
                        discountPrice = (maximumRetailPrice * cart.Quantity) - discount;
                    }
                    else
                    {
                        discount = discount * cart.Quantity;
                        discountPrice = discountPrice * cart.Quantity;
                    }
                }
                else
                {
                    discountPrice = discountPrice * cart.Quantity;
                }

                totalDiscount += discount;
                
                vm.Price = discountPrice;
                vm.DistributorPrice = Convert.ToDecimal(cart.ProductUnit.DistributorPricePerCarton) /
                                      Convert.ToDecimal(cart.ProductUnit.CartonSize);
                vm.DistributorPrice = vm.DistributorPrice * vm.Quantity;
                vm.UnitPrice = maximumRetailPrice * vm.Quantity;
                vm.TotalLPGDiscount = totalDiscount;

                detailCartList.Add(vm);
            }

            return detailCartList;
        }
        private List<CartVm> GetCalculatedPriceWithoutLPGDiscount(List<Cart> cartList)
        {
            List<CartVm> detailCartList = new List<CartVm>();
            foreach (Cart cart in cartList)
            {

                var userTypeId = cart.User.UserTypeId;

                CartVm vm = new CartVm();
                vm.CartId = cart.Id;
                vm.UserId = cart.UserId;
                vm.ProductUnitId = cart.ProductUnitId;
                vm.ProductTypeId = (long)cart.ProductUnit.Product.Category.ProductTypeId;
                vm.ProductName = cart.ProductUnit.Product.Name;
                vm.ProductUnit = cart.ProductUnit.StockKeepingUnit;
                vm.Brand = cart.ProductUnit.Product.Brand.Name;
                vm.Category = cart.ProductUnit.Product.Category.Name;
                vm.Quantity = cart.Quantity;
                if (cart.ProductUnit.ProductImages.Count == 0)
                {
                    vm.ProductImage = null;
                }
                else
                {
                    string filePath = cart.ProductUnit.ProductImages.FirstOrDefault().ImageLocation.ToString();
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/" + cart.ProductUnit.ProductImages.FirstOrDefault()
                                                                   .ImageLocation.ToString())))
                    {
                        vm.ProductImage = "/Content/img/product/NoProductImage.png";
                    }
                    else
                    {
                        vm.ProductImage = cart.ProductUnit.ProductImages.FirstOrDefault().ImageLocation;
                    }

                }
                
                vm.CartonSize = cart.ProductUnit.CartonSize;
                ProductDiscount productDiscount = _productDiscountManager.GetByProductUnitId(cart.ProductUnitId);

                var maximumRetailPrice = (decimal) cart.ProductUnit.MaximumRetailPrice;
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
                    vm.Price = (decimal)cart.ProductUnit.DistributorPricePerCarton / Convert.ToDecimal(cart.ProductUnit.CartonSize);
                }
                else if (userTypeId == (long)UserTypeEnum.Customer)
                {
                    if (productDiscountPercentage > 0)
                    {
                        vm.Price = maximumRetailPrice - (decimal)((maximumRetailPrice * productDiscountPercentage) / 100);
                    }
                    else
                    {
                        vm.Price = maximumRetailPrice;
                    }

                }
                else if (userTypeId == (long)UserTypeEnum.Corporate)
                {
                    CorporateUser corporateUser = _corporateUserManager.GetByUserId(cart.UserId);

                    ProductUnitDto productUnit = new ProductUnitDto
                    {
                        MaximumRetailPrice = maximumRetailPrice,
                        Product = cart.ProductUnit.Product
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
                        vm.Price = maximumRetailPrice - (decimal)((maximumRetailPrice * productDiscountPercentage) / 100);
                    }
                    else
                    {
                        vm.Price = maximumRetailPrice;
                    }
                }
                vm.DistributorPrice = Convert.ToDecimal(cart.ProductUnit.DistributorPricePerCarton) /
                                      Convert.ToDecimal(cart.ProductUnit.CartonSize);
                vm.DistributorPrice = vm.DistributorPrice * vm.Quantity;
                vm.UnitPrice = maximumRetailPrice * vm.Quantity;
                vm.Price = vm.Price * vm.Quantity;
                detailCartList.Add(vm);
            }

            return detailCartList;
        }

        private List<CartVm> GetCalculatedCartProductPriceForGuestUser(List<CartVm> cartList)
        {
            var lpg = cartList.FirstOrDefault(x =>
                x.ProductTypeId == (long)ProductTypeEnum.LPG);

            var activeConfig = _configurationManager.GetActiveById((long)ConfigurationEnum.LPGCombo);

            var lpgDiscount = _lpgComboDiscountManager.GetValidLpgComboDiscount();
            if (lpg == null || activeConfig == null || lpgDiscount == null)
            {
                foreach (CartVm cart in cartList)
                {
                    var productUnit = _productUnitManager.GetById(cart.ProductUnitId);
                    var maximumRetailPrice = (decimal)productUnit.MaximumRetailPrice;
                    ProductDiscount productDiscount = _productDiscountManager.GetByProductUnitId(cart.ProductUnitId);
                    
                    var productDiscountPercentage = 0;
                    if (productDiscount != null)
                    {
                        if (productDiscount.Validity.Value.AddDays(1) > DateTime.Now)
                        {
                            productDiscountPercentage = (int)productDiscount.DiscountPercentage;
                        }
                    }
                    if (productDiscountPercentage > 0)
                    {
                        cart.Price = maximumRetailPrice - (decimal)((maximumRetailPrice * productDiscountPercentage) / 100);
                    }
                    else
                    {
                        cart.Price = maximumRetailPrice;
                    }
                    cart.Price = cart.Price * cart.Quantity;
                    cart.UnitPrice = cart.UnitPrice * cart.Quantity;
                    cart.DistributorPrice = cart.DistributorPrice * cart.Quantity;
                    cart.TotalLPGDiscount = 0;
                }
                return cartList;
            }

            //var maxLPGPrice = cartList.Where(x =>
            //        x.ProductTypeId == (long)ProductTypeEnum.LPG)
            //    .Max(x => x.UnitPrice);

            decimal? largestLPGMaximumRetailPrice = 0;
            var LPGProducts = cartList.Where(x =>
                x.ProductTypeId == (long)ProductTypeEnum.LPG);
            foreach (var LPGProduct in LPGProducts)
            {
                var maximumRetailPrice = _productUnitManager.GetById(LPGProduct.ProductUnitId).MaximumRetailPrice;
                if (maximumRetailPrice > largestLPGMaximumRetailPrice)
                {
                    largestLPGMaximumRetailPrice = maximumRetailPrice;
                }
            }
            var maxLPGPrice = Convert.ToDecimal(largestLPGMaximumRetailPrice);

            decimal totalDiscount = 0;
            foreach (CartVm cart in cartList)
            {
                var productUnit = _productUnitManager.GetById(cart.ProductUnitId);
                var maximumRetailPrice = (decimal) productUnit.MaximumRetailPrice;
                decimal discountPrice = 0;

                if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.TradePrice)
                {
                    discountPrice = Convert.ToDecimal(productUnit.TradePricePerCarton) /
                               Convert.ToDecimal(productUnit.CartonSize);
                }
                else if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.DistributorPrice)
                {
                    discountPrice = Convert.ToDecimal(productUnit.DistributorPricePerCarton) /
                                    Convert.ToDecimal(productUnit.CartonSize);
                }
                else if (lpgDiscount.DiscountType == (int)DiscountTypeEnum.DiscountPercentage)
                {
                    if (lpgDiscount.DiscountPercentage > 0)
                    {
                        discountPrice = maximumRetailPrice - (decimal)((maximumRetailPrice * lpgDiscount.DiscountPercentage) / 100);
                    }
                    else
                    {
                        discountPrice = maximumRetailPrice;
                    }
                }
                else
                {
                    discountPrice = maximumRetailPrice;
                }

                if (cart.ProductTypeId == (long)ProductTypeEnum.LPG)
                {
                    discountPrice = maximumRetailPrice;
                }

                var discount = maximumRetailPrice - discountPrice;

                if (discount > 0)
                {
                    if (totalDiscount + discount * cart.Quantity > maxLPGPrice)
                    {
                        discount = maxLPGPrice - totalDiscount;
                        discountPrice = (maximumRetailPrice * cart.Quantity) - discount;
                    }
                    else
                    {
                        discount = discount * cart.Quantity;
                        discountPrice = discountPrice * cart.Quantity;
                    }
                }
                else
                {
                    discountPrice = discountPrice * cart.Quantity;
                }

                totalDiscount += discount;

                cart.Price = discountPrice;
                cart.UnitPrice = cart.UnitPrice * cart.Quantity;
                cart.DistributorPrice = cart.DistributorPrice * cart.Quantity;
                cart.TotalLPGDiscount = totalDiscount;
            }

            return cartList;
        }
    }
}
