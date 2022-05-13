using System;
using System.Web.Http;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class MeghnaDepartmentController : ApiController
    {
        private readonly IMeghnaDepartmentManager _meghnaDepartmentManager;

        public MeghnaDepartmentController()
        {
            _meghnaDepartmentManager = new MeghnaDepartmentManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] MeghnaDepartment department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isFound = _meghnaDepartmentManager.DoesMeghnaDepartmentExist(department.Name);
                    if (isFound)
                    {
                        return Conflict();
                    }
                    department.CreatedOn = DateTime.UtcNow.AddHours(6);
                    department.IsDeleted = false;
                    bool isSaved = _meghnaDepartmentManager.Add(department);

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
                var departments = _meghnaDepartmentManager.GetAll();
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
                var departments = _meghnaDepartmentManager.GetActiveDepartments();
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
                var department = _meghnaDepartmentManager.GetById(id);
                if (department == null) return NotFound();
                return Ok(department);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit([FromBody] MeghnaDepartment department)
        {
            var dept = _meghnaDepartmentManager.GetById(department.Id);
            if (dept.Name == department.Name)
            {
                try
                {
                    department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                    department.IsDeleted = false;
                    bool isSaved = _meghnaDepartmentManager.Update(department);
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
            bool isFound = _meghnaDepartmentManager.DoesMeghnaDepartmentExist(department.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                department.IsDeleted = false;
                bool isSaved = _meghnaDepartmentManager.Update(department);
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
                var department = _meghnaDepartmentManager.GetById(departmentId);
                if (department == null)
                {
                    return NotFound();
                }

                department.ModifiedBy = userId;
                department.ModifiedOn = DateTime.UtcNow.AddHours(6);
                var isDeleted = _meghnaDepartmentManager.SoftDelete(department);

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