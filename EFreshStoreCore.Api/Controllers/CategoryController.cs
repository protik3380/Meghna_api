using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IProductUnitManager _productUnitManager;

        public CategoryController()
        {
            _categoryManager = new CategoryManager();
            _productUnitManager = new ProductUnitManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var categories = _categoryManager.GetAll();
                if (categories == null) return NotFound();
                return Ok(categories.OrderBy(c => c.Name).ToList());
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
                var categories = _categoryManager.GetActiveCategories();
                if (categories == null) return NotFound();
                return Ok(categories.OrderBy(c => c.Name).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetProductsByCategory(long id)
        {
            try
            {
                var products = _productUnitManager.GetByCategory(id);
                if (products == null) return NotFound();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] Category aCategory)
        {
            if (ModelState.IsValid)
            {
                bool isFound = _categoryManager.DoesCategoryNameExist(aCategory.Name);
                if (isFound)
                {
                    return Conflict();
                }
                bool isSaved = _categoryManager.Add(aCategory);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aCategory);
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]Category aCategory)
        {
            var cat = _categoryManager.GetById(aCategory.Id);
            if (aCategory.Name == cat.Name)
            {
                try
                
                {
                    bool isSaved = _categoryManager.Update(aCategory);
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
            bool isFound = _categoryManager.DoesCategoryNameExist(aCategory.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _categoryManager.Update(aCategory);
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

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var category = _categoryManager.GetById(id);
                if (category == null) return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult CountTotalCategory()
        {
            try
            {
                int totalBrands = _categoryManager.CountTotalCategory();
                return Ok(totalBrands);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
