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
    public class DistrictController : ApiController
    {
        private readonly IDistrictManager _districtManager;
        public DistrictController()
        {
            _districtManager = new DistrictManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var districts = _districtManager.GetAll();
                if (districts == null) return NotFound();
                var district = districts.OrderBy(d => d.Name);
                return Ok(district);
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
                var districts = _districtManager.GetActiveDistricts();
                if (districts == null) return NotFound();
                var district = districts.OrderBy(d => d.Name);
                return Ok(district);
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
                var brand = _districtManager.GetById(id);
                if (brand == null) return Conflict();
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] District aDistrict)
        {
            if (ModelState.IsValid)
            {
                bool isFound = _districtManager.DoesDistrictNameExist(aDistrict.Name);
                if (isFound)
                {
                    return Conflict();
                }
                aDistrict.CreatedOn = DateTime.UtcNow.AddHours(6);
                aDistrict.IsDeleted = false;
                var isSaved = _districtManager.Add(aDistrict);

                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aDistrict);
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]District aDistrict)
        {
            var district = _districtManager.GetById(aDistrict.Id);
            if (district.Name == aDistrict.Name)
            {
                try
                {
                    bool isSaved = _districtManager.Update(aDistrict);
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
            bool isFound = _districtManager.DoesDistrictNameExist(aDistrict.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _districtManager.Update(aDistrict);
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
    }
}
