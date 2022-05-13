using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;
using AutoMapper;
using EFreshStore.Models;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        private readonly IProductManager _productManager;
        private readonly IProductUnitManager _productUnitManager;
        private readonly IProductDiscountManager _productDiscountManager;
        private readonly ICorporateUserManager _corporateUserManager;
        private readonly IUserManager _userManager;
        private readonly IRatingManager _ratingManager;
        private readonly IMeghnaUserManager _meghnaUserManager;
        private readonly ICustomerManager _customerManager;
        private readonly IWishListManager _wishListManager;

        public ProductController()
        {
            _productManager = new ProductManager();
            _productUnitManager = new ProductUnitManager();
            _productDiscountManager = new ProductDiscountManager();
            _corporateUserManager = new CorporateUserManager();
            _userManager = new UserManager();
            _ratingManager = new RatingManager();
            _meghnaUserManager = new MeghnaUserManager();
            _customerManager = new CustomerManager();
            _wishListManager = new WishListManager();
        }
        [HttpGet]
        public IHttpActionResult CountTotalProducts()
        {
            try
            {
                int products = _productUnitManager.Get().Count;
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult GetAllProducts()
        {
            try
            {
                var products = _productUnitManager.Get();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult GetAllProductsByUserId(long? userId)
        {
            try
            {
                var products = _productUnitManager.Get();
                var productDtos = GetAllProductsWithCalculatedPrice(userId,products);
                return Ok(productDtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetAllProductsByBrandIds(long? userId, [FromUri] long[] brandIds )
        {
            try
            {
                ICollection<ProductUnit> products = new List<ProductUnit>();
                var brandIdcount = brandIds == null ? 0 : brandIds.Length;
                if (brandIdcount > 0)
                {
                    products = _productUnitManager.GetByBrandIds(brandIds);
                }
                var productDtos = GetAllProductsWithCalculatedPrice(userId, products);
                return Ok(productDtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult GetAllProductsByCategoryIds(long? userId, [FromUri] long[] categoryIds)
        {
            try
            {
                ICollection<ProductUnit> products = new List<ProductUnit>();
                var categoryIdcount = categoryIds == null ? 0 : categoryIds.Length;
                if (categoryIdcount > 0)
                {
                    products = _productUnitManager.GetByCategoryIds(categoryIds);
                }
                var productDtos = GetAllProductsWithCalculatedPrice(userId, products);
                return Ok(productDtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetAllProductsByBrandIdsAndCategoryIds(long? userId, [FromUri] long[] brandIds, [FromUri] long[] categoryIds)
        {
            try
            {
                ICollection<ProductUnit> products = new List<ProductUnit>();
                var brandIdcount = brandIds == null ? 0 : brandIds.Length;
                var categoryIdcount = categoryIds == null ? 0 : categoryIds.Length;

                if (brandIdcount > 0 && categoryIdcount > 0)
                {
                    products = _productUnitManager.GetByBrandIdsAndCategoryIds(brandIds,categoryIds);
                }
                var productDtos = GetAllProductsWithCalculatedPrice(userId, products);
                return Ok(productDtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private List<ProductUnitDto> GetAllProductsWithCalculatedPrice(long? userId, ICollection<ProductUnit> products)
        {
            
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProductUnit, ProductUnitDto>(); });

            IMapper iMapper = config.CreateMapper();
            var productUnits = iMapper.Map<IEnumerable<ProductUnit>, IEnumerable<ProductUnitDto>>(products);
            var user = new User();
            if (userId != null)
            {
                user = _userManager.GetById((long)userId);
            }
            
            foreach (ProductUnitDto productUnit in productUnits)
            {
                var ratings = GetRatingByProductUnitId(productUnit.Id);
                productUnit.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Rating1) : 0;
                productUnit.Ratings = ratings;
                if (userId != null)
                {
                    bool isExist = _wishListManager.IsProductExist((long)userId, productUnit.Id);
                    if (isExist)
                    {
                        productUnit.ExistsInWishList = true;
                    }
                }
                if (productUnit.ProductImages.Count != 0)
                {
                    string filePath = productUnit.ProductImages.FirstOrDefault().ImageLocation.ToString();
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/" + productUnit.ProductImages.FirstOrDefault().ImageLocation.ToString())))
                    {
                        productUnit.ProductImages.FirstOrDefault().ImageLocation = "Content/img/product/NoProductImage.png";
                    }

                }
                ProductDiscount productDiscount =
                    _productDiscountManager.GetByProductUnitId(productUnit.Id);
                var productDiscountPercentage = 0;
                if (productDiscount != null)
                {
                    if (productDiscount.Validity.Value.AddDays(1) > DateTime.Now)
                    {
                        productDiscountPercentage = (int)productDiscount.DiscountPercentage;
                    }
                }



                if (user.UserTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    productUnit.PorductDiscountPrice = (decimal)productUnit.DistributorPricePerCarton /
                               Convert.ToDecimal(productUnit.CartonSize);
                }
                else if (user.UserTypeId == (long)UserTypeEnum.Customer)
                {
                    if (productDiscountPercentage > 0)
                    {
                        productUnit.PorductDiscountPrice = (decimal)productUnit.MaximumRetailPrice -
                                   (decimal)((productUnit.MaximumRetailPrice *
                                               productDiscountPercentage) / 100);
                    }

                }
                else if (user.UserTypeId == (long)UserTypeEnum.Corporate)
                {
                    CorporateUser corporateUser = _corporateUserManager.GetByUserId((long)userId);
                    
                    if (corporateUser.CorporateContract.FMCGDiscountPercentage != null ||
                        corporateUser.CorporateContract.LPGDiscountPercentage != null)
                    {
                        productUnit.PorductDiscountPrice = UtilityClass.CalculateDiscountForCorporateUser(corporateUser, productUnit, productDiscountPercentage);
                    }
                    
                }
                else
                {
                    if (productDiscountPercentage > 0)
                    {
                        productUnit.PorductDiscountPrice = (decimal)productUnit.MaximumRetailPrice -
                                   (decimal)((productUnit.MaximumRetailPrice *
                                               productDiscountPercentage) / 100);
                    }
                }
            }

            return productUnits.ToList();
        }

        

        private List<RatingToReturnDto> GetRatingByProductUnitId(long id)
        {
            var ratings = _ratingManager.GetRatingByProductUnit(id);
            if (ratings != null)
            {
                foreach (var rating in ratings)
                {
                    if (rating.User.UserTypeId == (long) UserTypeEnum.MeghnaUser)
                    {
                        var meghnaUser = _meghnaUserManager.GetByUserId(rating.UserId);
                        rating.User.Username = meghnaUser.Name;
                    }

                    if (rating.User.UserTypeId == (long) UserTypeEnum.Corporate)
                    {
                        var corporateUser = _corporateUserManager.GetByUserId(rating.UserId);
                        rating.User.Username = corporateUser.Name;
                    }

                    if (rating.User.UserTypeId == (long) UserTypeEnum.Customer)
                    {
                        var customer = _customerManager.GetByUserId(rating.UserId);
                        rating.User.Username = customer.Name;
                    }
                }
            }
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Rating, RatingToReturnDto>()
                    .ForMember(d => d.UserName, opts => opts.MapFrom(s => s.User.Username))
                    .ForMember(d => d.ProductName, opts => opts.MapFrom(s => s.ProductUnit.Product.Name));
            });

            IMapper iMapper = config.CreateMapper();
            var ratingsToReturn = iMapper.Map<IEnumerable<Rating>, IEnumerable<RatingToReturnDto>>(ratings);
            ratingsToReturn = ratingsToReturn.OrderByDescending(r => r.RatingTime);
            return ratingsToReturn.ToList();
        }

        public IHttpActionResult GetAllProducts(string searchString)
        {
            try
            {

                var products = _productUnitManager.GetProductBySearch(searchString);
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var products = _productManager.GetAll();
                return Ok(products);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var products = _productManager.GetActiveProducts();
                if (products == null) return NotFound();
                products = products.OrderBy(c => c.Name).ToList();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetProductDetails(long? id)
        {
            try
            {
                var product = _productUnitManager.Get((long)id);
                if (product == null)
                {
                    return BadRequest("Data not found");
                }
                return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetProductDetailsWithUserId(long? id, long? userId)
        {
            try
            {
                var product = GetProductDetailsWithCalculatedPrice(id, userId);
                if (product == null)
                {
                    return BadRequest("Data not found");
                }
                if (product.ProductImages.Count != 0)
                {
                    string filePath = product.ProductImages.FirstOrDefault().ImageLocation.ToString();
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/"+product.ProductImages.FirstOrDefault().ImageLocation.ToString())))
                    {
                        product.ProductImages.FirstOrDefault().ImageLocation = "Content/img/product/NoProductImage.png";
                    }
                    
                }
                    return Ok(product);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private ProductUnitDto GetProductDetailsWithCalculatedPrice(long? id, long? userId)
        {

            var product = _productUnitManager.Get((long)id);
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<ProductUnit, ProductUnitDto>(); });

            IMapper iMapper = config.CreateMapper();
            var productUnit = iMapper.Map<ProductUnit,ProductUnitDto>(product);
            var ratings = GetRatingByProductUnitId(productUnit.Id);
            productUnit.AverageRating = ratings.Count > 0 ? ratings.Average(r => r.Rating1) : 0;
            productUnit.Ratings = ratings;
            var user = new User();
            if (userId != null)
            {
                user = _userManager.GetById((long)userId);
                bool isExist = _wishListManager.IsProductExist((long)userId, productUnit.Id);
                if (isExist)
                {
                    productUnit.ExistsInWishList = true;
                }
            }
           
            ProductDiscount productDiscount =
                _productDiscountManager.GetByProductUnitId(productUnit.Id);
            var productDiscountPercentage = 0;
            if (productDiscount != null)
            {
                if (productDiscount.Validity.Value.AddDays(1) > DateTime.Now)
                {
                    productDiscountPercentage = (int)productDiscount.DiscountPercentage;
                }
            }

            if (user.UserTypeId == (long)UserTypeEnum.MeghnaUser)
            {
                productUnit.PorductDiscountPrice = (decimal)productUnit.DistributorPricePerCarton /
                           Convert.ToDecimal(productUnit.CartonSize);
            }
            else if (user.UserTypeId == (long)UserTypeEnum.Customer)
            {
                if (productDiscountPercentage > 0)
                {
                    productUnit.PorductDiscountPrice = (decimal)productUnit.MaximumRetailPrice -
                               (decimal)((productUnit.MaximumRetailPrice *
                                           productDiscountPercentage) / 100);
                }

            }
            else if (user.UserTypeId == (long)UserTypeEnum.Corporate)
            {
                CorporateUser corporateUser = _corporateUserManager.GetByUserId((long)userId);
                if (corporateUser.CorporateContract.FMCGDiscountPercentage != null ||
                    corporateUser.CorporateContract.LPGDiscountPercentage != null)
                {
                    productUnit.PorductDiscountPrice = UtilityClass.CalculateDiscountForCorporateUser(corporateUser, productUnit, productDiscountPercentage);
                }

            }
            else
            {
                if (productDiscountPercentage > 0)
                {
                    productUnit.PorductDiscountPrice = (decimal)productUnit.MaximumRetailPrice -
                               (decimal)((productUnit.MaximumRetailPrice *
                                           productDiscountPercentage) / 100);
                }
            }

            return productUnit;

        }

        [Authorize(Roles = "Admin")]
        //post product details
        [HttpPost]
        public IHttpActionResult AddDetails([FromBody]ProductUnitV productDetails)
        {
            ProductImage pImages;
            try
            {
                if (ModelState.IsValid)
                {
                    ProductUnit productUnit = new ProductUnit();
                    productUnit.Product = productDetails.Product;
                    productUnit.ProductId = productDetails.ProductId;
                    productUnit.StockKeepingUnit = productDetails.StockKeepingUnit;
                    productUnit.ProductUnitPrices = productDetails.ProductUnitPrices;
                    productUnit.TradePricePerCarton = productDetails.TradePricePerCarton;
                    productUnit.DistributorPricePerCarton = productDetails.DistributorPricePerCarton;
                    productUnit.MaximumRetailPrice = productDetails.MaximumRetailPrice;
                    productUnit.CartonSize = productDetails.CartonSize;
                    productUnit.CartonSizeUnit = productDetails.CartonSizeUnit;
                    productUnit.EffectiveDate = productDetails.EffectiveDate;
                    productUnit.IsActive = true;
                    productUnit.IsDeleted = false;
                    productUnit.CreatedOn = DateTime.Now;
                    productUnit.CreatedBy = productDetails.CreatedBy;
                    if (productDetails.ImageBytes != null)
                    if (productDetails.ImageBytes!=null)
                    {
                        foreach (var byteImage in productDetails.ImageBytes)
                        {
                            string imageName = UtilityClass.GenerateImageNameFromTimestamp();
                            Image image = UtilityClass.ConvertByteToImage(byteImage);
                            if (image != null)
                            {
                                string fileLocation = "Content/img/product/";
                                //string fileName = DateTime.UtcNow.AddHours(6).Ticks + Path.GetFileName(imageName);
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                                image.Save(path, ImageFormat.Jpeg);
                                string productils = fileLocation + "/" + imageName;
                                pImages=new ProductImage();
                                pImages.ImageLocation = productils;
                                pImages.ProductUnitId = productDetails.Id;
                                pImages.IsDeleted = false;
                                pImages.CreatedOn = DateTime.Now;
                                productUnit.ProductImages.Add(pImages);
                            }
                           
                        }
                        if (productDetails.ImageBytes.Count == 0)
                        {
                            pImages = new ProductImage();
                            pImages.ImageLocation = "Content/img/product/NoProductImage.png";
                            pImages.ProductUnitId = productDetails.Id;
                            pImages.IsDeleted = false;
                            
                            productUnit.ProductImages.Add(pImages);
                        }
                    }
                    bool isSaved = _productUnitManager.SaveProductDetails(productUnit);
                }
                return Created(new Uri(Request.RequestUri.ToString()), productDetails);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult EditDetails([FromBody]ProductUnitV productUnitvm)
        {
            ProductImage pImages;
            try
            {
                ProductUnit productUnit = new ProductUnit();
                productUnit.Id = productUnitvm.Id;
                productUnit.Product = productUnitvm.Product;
                productUnit.ProductId = productUnitvm.ProductId;
                productUnit.StockKeepingUnit = productUnitvm.StockKeepingUnit;
                productUnit.ProductUnitPrices = productUnitvm.ProductUnitPrices;
                productUnit.TradePricePerCarton = productUnitvm.TradePricePerCarton;
                productUnit.DistributorPricePerCarton = productUnitvm.DistributorPricePerCarton;
                productUnit.MaximumRetailPrice = productUnitvm.MaximumRetailPrice;
                productUnit.CartonSize = productUnitvm.CartonSize;
                productUnit.CartonSizeUnit = productUnitvm.CartonSizeUnit;
                productUnit.EffectiveDate = productUnitvm.EffectiveDate;
                productUnit.IsActive = true;
                productUnit.IsDeleted = false;
                productUnit.ModifiedOn = DateTime.Now;
                productUnit.ModifiedBy = productUnitvm.ModifiedBy;
                var hasNewImages = false;
                if (productUnitvm.ImageBytes != null)
                {
                    if (productUnitvm.ImageBytes.Count > 0)
                    {
                        hasNewImages = true;
                    }
                   
                    foreach (var byteImage in productUnitvm.ImageBytes)
                    {
                        string imageName = UtilityClass.GenerateImageNameFromTimestamp();
                        Image image = UtilityClass.ConvertByteToImage(byteImage);
                        if (byteImage != null)
                        {
                            string fileLocation = "Content/img/product/";
                            //string fileName = DateTime.UtcNow.AddHours(6).Ticks + Path.GetFileName(imageName);
                            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                            image.Save(path, ImageFormat.Jpeg);
                            string productils = fileLocation + "/" + imageName;
                            pImages = new ProductImage();
                            pImages.ImageLocation = productils;
                            pImages.ProductUnitId = productUnitvm.Id;
                            pImages.IsDeleted = false;
                            pImages.ModifiedOn = DateTime.Now;
                            productUnit.ProductImages.Add(pImages);
                        }
                    }

                }
                
                bool isUpdate = _productUnitManager.EditProductDetails(productUnit, hasNewImages);
                if (isUpdate)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Add([FromBody]Product aProduct)
        {
            bool isFound = _productManager.DoesProductNameExist(aProduct.Name);
            if (isFound)
            {
                return Conflict();
            }
            else
            {
                try
                {
                    bool isSaved = _productManager.Add(aProduct);
                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), aProduct = null);
                    }
                    return BadRequest();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]Product aProduct)
        {
            Product product = _productManager.GetById(aProduct.Id);
            if (product.Name == aProduct.Name)
            {
                try
                {
                    bool isSaved = _productManager.Update(aProduct);
                    if (isSaved)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            bool isFound = _productManager.DoesProductNameExist(aProduct.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _productManager.Update(aProduct);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aProduct = null);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var product = _productManager.GetById(id);
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetByProductId(long id)
        {
            try
            {
                var product = _productUnitManager.GetByProductId(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        public IHttpActionResult GetByProductTypeAndCategory(long productTypeId, long categoryId)
        {
            try
            {
                var product = _productManager.GetByProductTypeIdAndCategoryId(productTypeId, categoryId);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product.OrderBy(c => c.Name).ToList());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        //Post product line
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public IHttpActionResult PostProductLine(ProductLine aProductLine)
        //{
        //    try
        //    {
        //        if (aProductLine != null)
        //        {
        //            bool isSaved = _productLineManager.Add(aProductLine);
        //            if (isSaved)
        //            {
        //                return Created(new Uri(Request.RequestUri.ToString()), aProductLine);
        //            }
        //            return BadRequest();
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }
        //}


        [HttpPost]
        //product discount
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult AddDiscount([FromBody] ProductDiscount discount)
        {
            if (ModelState.IsValid)
            {
                var previousDiscount = _productDiscountManager.GetByProductUnitId(discount.ProductUnitId);
                if (previousDiscount != null)
                {
                    previousDiscount.IsActive = false;
                    previousDiscount.IsDeleted = true;
                    _productDiscountManager.Update(previousDiscount);
                }
                discount.CreatedOn = DateTime.UtcNow.AddHours(6);
                discount.IsActive = true;
                discount.IsDeleted = false;
                bool isSaved = _productDiscountManager.Add(discount);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), discount);
                }
            }
            return BadRequest();
        }

        [HttpPost]
        //product discount
        //[Authorize(Roles = "Admin")]
        public IHttpActionResult EditDiscount([FromBody] ProductDiscount discount)
        {
            if (ModelState.IsValid)
            {
                var previousDiscount = _productDiscountManager.GetByProductUnitId(discount.ProductUnitId);
                if (previousDiscount == null)
                {
                    return BadRequest();
                }
                if (previousDiscount.Id != discount.Id)
                {
                    previousDiscount.IsActive = false;
                    previousDiscount.IsDeleted = true;
                    previousDiscount.ModifiedOn = DateTime.UtcNow.AddHours(6);
                    _productDiscountManager.Update(previousDiscount);
                }
                discount.ModifiedOn = DateTime.UtcNow.AddHours(6);
                discount.IsActive = true;
                discount.IsDeleted = false;
                _productDiscountManager.Update(discount);
                return Ok();
            }
            return BadRequest();
        }

        public IHttpActionResult GetAllDiscountedProducts()
        {
            try
            {
                var product = _productDiscountManager.GetByProductId();
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public IHttpActionResult GetDiscountByProductUnit(long? id)
        {
            try
            {
                var product = _productDiscountManager.GetByProductUnitId(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public IHttpActionResult GetProductDiscountByDiscountId(long? id)
        {
            try
            {
                var product = _productDiscountManager.GetByProductDiscountId(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
