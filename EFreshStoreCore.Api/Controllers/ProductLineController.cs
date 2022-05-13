using System;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductLineController : ApiController
    {
        private readonly IProductLineManager _productLineManager;
        private readonly IProductLineDetailManager _productLineDetailManager;

        public ProductLineController()
        {
            _productLineManager=new ProductLineManager();
            _productLineDetailManager=new ProductLineDetailManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var productLineList = _productLineManager.GetAll();
                if (productLineList == null) return NotFound();
                return Ok(productLineList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Add([FromBody] ProductLine productLine)
        {
            bool isFound = _productLineManager.IsExistByName(productLine.Name);
            if (isFound)
            {
                return Conflict();
            }
            else
            {
                try
                {
                    if (_productLineManager.IsExistByName(productLine.Name))
                    {
                        return BadRequest("Sorry! This product line name is already created.");
                    }
                    bool isSaved = _productLineManager.Add(productLine);
                    if (!isSaved)
                    {
                        return BadRequest();
                    }
                    return Created(new Uri(Request.RequestUri.ToString()), productLine);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
          }

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var productLine= _productLineManager.GetFirstOrDefault(c=>c.Id==id);
                if (productLine == null) return NotFound();
                return Ok(productLine);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] ProductLine productLine)
        {
            ProductLine aProductLine = _productLineManager.GetById(productLine.Id);
            if (aProductLine.Name == productLine.Name)
            {
                try
                {
                    aProductLine.Name = productLine.Name;
                    aProductLine.Description = productLine.Description;
                    aProductLine.ModifiedBy = productLine.ModifiedBy;
                    aProductLine.ModifiedOn = productLine.ModifiedOn;
                    aProductLine.IsActive = productLine.IsActive;
                    aProductLine.IsDeleted = productLine.IsDeleted;
                    bool isUpdate = _productLineManager.Update(aProductLine);
                    if (!isUpdate)
                    {
                        return BadRequest();
                    }
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            bool isFound = _productLineManager.IsExistByName(productLine.Name);
            if (isFound)
            {
                return Conflict();
            }
            else
            {
                try
                {
                    bool isUpdate = _productLineManager.Update(productLine);
                    if (!isUpdate)
                    {
                        return BadRequest();
                    }
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDetail([FromBody] ProductLineDetail productLineDetail)
        {
            var prodLineDetail = _productLineDetailManager.IsExistProductLine(productLineDetail);
            try
            {
                if (prodLineDetail != null)
                {
                    return Conflict();
                }
                bool isSaved = _productLineDetailManager.Add(productLineDetail);
                if (!isSaved)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult Detail()
        {
            try
            {
                var productLineDetails = _productLineDetailManager.GetAll();
                if (productLineDetails == null) return NotFound();

                return Ok(productLineDetails);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetByDetailId(long id)
        {
            try
            {
                var productLineDetail = _productLineDetailManager.GetById(id);
                if (productLineDetail == null) return NotFound();
                return Ok(productLineDetail);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        public IHttpActionResult GetProductsByLineId(long id)
        {
            try
            {
                var productLineDetail = _productLineDetailManager.GetProductsByLineId(id);
                if (productLineDetail == null) return NotFound();
                return Ok(productLineDetail);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult EditDetail([FromBody] ProductLineDetail productLineDetail)
        {
            try
            {
                var productLine = _productLineDetailManager.IsExistProductLine(productLineDetail);
                if (productLine != null)
                {
                    return BadRequest("This product line  is already linked with product.");
                }
                bool isUpdate = _productLineDetailManager.Update(productLineDetail);
                if (!isUpdate)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteDetail(ProductLineDetail productLineDetail)
        {
            var productLineDetailFromRepo = _productLineDetailManager.IsExistProductLine(productLineDetail);

            try
            {
                if (productLineDetailFromRepo != null)
                {
                    bool isDeleted = _productLineDetailManager.Delete(productLineDetailFromRepo);
                    if (isDeleted)
                    {
                        return Ok();
                    }
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
