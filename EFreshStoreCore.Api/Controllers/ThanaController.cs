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
    public class ThanaController : ApiController
    {
        private readonly IThanaManager _thanaManager;
        private readonly IThanaWiseMasterDepotManager _thanaWiseMasterDepotManager;

        public ThanaController()
        {
            _thanaManager = new ThanaManager();
            _thanaWiseMasterDepotManager = new ThanaWiseMasterDepotManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]Thana aThana)
        {
            try
            {
                bool isFound = _thanaManager.DoesThanaNameExistSameDistrict(aThana.Name,aThana.DistrictId);
                if (isFound)
                {
                    return Conflict();
                }
                bool isSaved = _thanaManager.Add(aThana);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aThana);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult Edit([FromBody]Thana aThana)
        {

            var thana = _thanaManager.GetById(aThana.Id);
            if (thana.Name == aThana.Name && thana.DistrictId==aThana.DistrictId)
            {
                try
                {
                    bool isSaved = _thanaManager.Update(aThana);
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
            bool isFound = _thanaManager.DoesThanaNameExistSameDistrict(aThana.Name,aThana.DistrictId);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _thanaManager.Update(aThana);
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

        public IHttpActionResult GetAll()
        {
            try
            {
                var thanas = _thanaManager.GetAll();
                if (thanas == null) return NotFound();
                var thana = thanas.OrderBy(t => t.Name);
                return Ok(thana);
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
                var thanas = _thanaManager.GetActiveThanas();
                if (thanas == null) return NotFound();
                var thana = thanas.OrderBy(t => t.Name);
                return Ok(thana);
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
                var thanas = _thanaManager.GetById(id);
                if (thanas == null) return NotFound();
                return Ok(thanas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetByDistrictId(long id)
        {
            try
            {
                var thanas = _thanaManager.GetByDistrictId(id);
                if (thanas == null) return NotFound();
                var thana = thanas.OrderBy(t => t.Name);
                return Ok(thana);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult GetActiveThanaByDistrictId(long id)
        {
            try
            {
                var thanas = _thanaManager.GetActiveByDistrictId(id);
                if (thanas == null) return NotFound();
                var thana = thanas.OrderBy(t => t.Name);
                return Ok(thana);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetByMasterDepotId(long id)
        {
            try
            {
                var thanas = _thanaWiseMasterDepotManager.GetByMasterDepotId(id);
                if (thanas == null) return NotFound();
                //var thana = thanas.OrderBy(t => t.Thana.Name);
                return Ok(thanas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
