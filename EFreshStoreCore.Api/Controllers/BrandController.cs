using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStore.Models.Context;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BrandController : ApiController
    {
        private readonly IBrandManager _brandManager;
        private readonly IProductManager _productManager;
        private readonly IProductUnitManager _productUnitManager;
        public BrandController()
        {
            _brandManager = new BrandManager();
            _productManager = new ProductManager();
            _productUnitManager = new ProductUnitManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var brands = _brandManager.GetAll();
                if (brands == null) return NotFound();
                brands = brands.OrderBy(c => c.Name).ToList();
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetAllActive()
        {
            try
            {
                var brands = _brandManager.GetActiveBrands();
                if (brands == null) return NotFound();
                brands = brands.OrderBy(c => c.Name).ToList();
                return Ok(brands);
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
                var brand = _brandManager.GetById(id);
                if (brand == null) return NotFound();
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetProductsByBrand(long id)
        {
            try
            {
                //var products = _productManager.GetByBrand(Convert.ToInt64(id)).ToList();
                var products = _productManager.GetByBrand(Convert.ToInt64(id));
                if (products == null) return NotFound();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] BrandVm aBrand)
        {
            if (ModelState.IsValid)
            {
                bool isFound = _brandManager.DoesBrandNameExist(aBrand.Name);
                if (isFound)
                {
                    return Conflict();
                }

                Brand brand = new Brand();
                string imageName = UtilityClass.GenerateImageNameFromTimestamp();
                if (aBrand.ImageByte != null)
                {
                    Image image = UtilityClass.ConvertByteToImage(aBrand.ImageByte);
                    string fileLocation = "Content/img/product/";
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                    image.Save(path, ImageFormat.Png);
                    string productils = fileLocation + "/" + imageName;

                    brand.BrandImage = productils;
                }
                else
                {
                    brand.BrandImage = "Content/img/product/no-image.jpg";
                }

                brand.CreatedBy = aBrand.CreatedBy;
                brand.Name = aBrand.Name;
                brand.Description = aBrand.Description;
                brand.CreatedOn = DateTime.UtcNow.AddHours(6);
                brand.IsActive = aBrand.IsActive;
                brand.IsDeleted = false;
                bool isSaved = _brandManager.Add(brand);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aBrand);
                }
            }
            return BadRequest();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]BrandVm aBrand)
        {
            var brand = _brandManager.GetById(aBrand.Id);
            if (brand.Name != aBrand.Name)
            {
                bool isFound = _brandManager.DoesBrandNameExist(aBrand.Name);
                if (isFound)
                {
                    return Conflict();
                }
                try
                {
                    string imageName = UtilityClass.GenerateImageNameFromTimestamp();
                    if (aBrand.ImageByte != null)
                    {
                        Image image = UtilityClass.ConvertByteToImage(aBrand.ImageByte);
                        if (image != null)
                        {
                            string fileLocation = "Content/img/product/";
                            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                            image.Save(path, ImageFormat.Png);
                            string productils = fileLocation + "/" + imageName;

                            brand.BrandImage = productils;
                        }
                    }
                    brand.Name = aBrand.Name;
                    brand.Description = aBrand.Description;
                    brand.ModifiedOn = aBrand.ModifiedOn;
                    brand.ModifiedBy = aBrand.ModifiedBy;
                    brand.IsActive = aBrand.IsActive;
                    brand.IsDeleted = false;
                    bool isSaved = _brandManager.Update(brand);
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
            try
            {
                string imageName = UtilityClass.GenerateImageNameFromTimestamp();
                if (aBrand.ImageByte != null)
                {
                    Image image = UtilityClass.ConvertByteToImage(aBrand.ImageByte);
                    if (image != null)
                    {
                        string fileLocation = "Content/img/product/";
                        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/" + fileLocation), imageName);
                        image.Save(path, ImageFormat.Png);
                        string productils = fileLocation + "/" + imageName;

                        brand.BrandImage = productils;
                    }
                }
                brand.Description = aBrand.Description;
                brand.ModifiedOn = aBrand.ModifiedOn;
                brand.ModifiedBy = aBrand.ModifiedBy;
                brand.IsActive = aBrand.IsActive;
                brand.IsDeleted = false;
                bool isSaved = _brandManager.Update(brand);
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
        
        public IHttpActionResult GetProductUnitByBrand(long id)
        {
            try
            {
                var products = _productUnitManager.GetByBrandId(Convert.ToInt64(id)).ToList();
                return Ok(products);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult CountTotalBrand()
        {
            try
            {
                int totalBrands = _brandManager.CountTotalBrand();
                return Ok(totalBrands);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
