using System;
using System.Web.Http;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class CorporateDesignationController : ApiController
    {
        private readonly ICorporateDesignationManager _corporateDesignationManager;

        public CorporateDesignationController()
        {
            _corporateDesignationManager = new CorporateDesignationManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CorporateDesignation designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isFound = _corporateDesignationManager.DoesCorporateDesignationExist(designation.Name);
                    if (isFound)
                    {
                        return Conflict();
                    }
                    designation.CreatedOn = DateTime.UtcNow.AddHours(6);
                    designation.IsDeleted = false;
                    bool isSaved = _corporateDesignationManager.Add(designation);

                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), designation);
                    }
                    return BadRequest("Save failed.");
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
                var designations = _corporateDesignationManager.GetAll();
                if (designations == null) return NotFound();
                return Ok(designations);
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
                var designations = _corporateDesignationManager.GetActiveDesignations();
                if (designations == null) return NotFound();
                return Ok(designations);
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
                var designation = _corporateDesignationManager.GetById(id);
                if (designation == null) return NotFound();
                return Ok(designation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit([FromBody] CorporateDesignation designation)
        {

            var des = _corporateDesignationManager.GetById(designation.Id);
            if (des.Name == designation.Name)
            {
                try
                {
                    designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                    designation.IsDeleted = false;
                    bool isSaved = _corporateDesignationManager.Update(designation);
                    if (isSaved)
                    {
                        return Ok();
                    }
                    return BadRequest("Update failed.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            bool isFound = _corporateDesignationManager.DoesCorporateDesignationExist(designation.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                designation.IsDeleted = false;
                bool isSaved = _corporateDesignationManager.Update(designation);
                if (isSaved)
                {
                    return Ok();
                }
                return BadRequest("Update failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult Delete(long designationId, long userId)
        {
            try
            {
                var designation = _corporateDesignationManager.GetById(designationId);
                if (designation == null)
                {
                    return NotFound();
                }

                designation.ModifiedBy = userId;
                designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                var isDeleted = _corporateDesignationManager.SoftDelete(designation);

                if (isDeleted)
                {
                    return Ok();
                }
                return BadRequest("Delete failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
