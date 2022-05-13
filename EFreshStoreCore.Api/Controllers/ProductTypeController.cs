using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductTypeController : ApiController
    {
        private readonly IProductTypeManager _productTypeManager;

        public ProductTypeController()
        {
            _productTypeManager = new ProductTypeManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var productTypes = _productTypeManager.GetAll();
                if (productTypes == null) return NotFound();
                return Ok(productTypes.OrderBy(c => c.Name).ToList());
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
                var productTypes = _productTypeManager.GetActiveProductTypes();
                if (productTypes == null) return NotFound();
                return Ok(productTypes.OrderBy(c => c.Name).ToList());
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
                var productType = _productTypeManager.GetById(id);
                if (productType == null) return NotFound();
                return Ok(productType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}