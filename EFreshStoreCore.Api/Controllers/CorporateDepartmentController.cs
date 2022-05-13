using System;
using System.Web.Http;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class CorporateDepartmentController : ApiController
    {
        private readonly ICorporateDepartmentManager _corporateDepartmentManager;

        public CorporateDepartmentController()
        {
            _corporateDepartmentManager = new CorporateDepartmentManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CorporateDepartment department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isFound = _corporateDepartmentManager.DoesCorporateDepartmentExist(department.Name);
                    if (isFound)
                    {
                        return Conflict();
                    }
                    department.CreatedOn = DateTime.UtcNow.AddHours(6);
                    department.IsDeleted = false;
                    bool isSaved = _corporateDepartmentManager.Add(department);

                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), department);
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
                var departments = _corporateDepartmentManager.GetAll();
                if (departments == null) return NotFound();
                return Ok(departments);
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
                var departments = _corporateDepartmentManager.GetActiveDepartments();
                if (departments == null) return NotFound();
                return Ok(departments);
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
                var department = _corporateDepartmentManager.GetById(id);
                if (department == null) return NotFound();
                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit([FromBody] CorporateDepartment department)
        {

            var dept = _corporateDepartmentManager.GetById(department.Id);
            if (dept.Name == department.Name)
            {
                try
                {
                    department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                    department.IsDeleted = false;
                    bool isSaved = _corporateDepartmentManager.Update(department);
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
            bool isFound = _corporateDepartmentManager.DoesCorporateDepartmentExist(department.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                department.IsDeleted = false;
                bool isSaved = _corporateDepartmentManager.Update(department);
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
        public IHttpActionResult Delete(long departmentId, long userId)
        {
            try
            {
                var department = _corporateDepartmentManager.GetById(departmentId);
                if (department == null)
                {
                    return NotFound();
                }

                department.ModifiedBy = userId;
                department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                var isDeleted = _corporateDepartmentManager.SoftDelete(department);

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
