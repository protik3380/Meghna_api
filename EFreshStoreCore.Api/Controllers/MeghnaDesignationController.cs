﻿using System;
using System.Web.Http;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class MeghnaDesignationController : ApiController
    {
        private readonly IMeghnaDesignationManager _meghnaDesignationManager;

        public MeghnaDesignationController()
        {
            _meghnaDesignationManager = new MeghnaDesignationManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] MeghnaDesignation designation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isFound = _meghnaDesignationManager.DoesMeghnaDesignationExist(designation.Name);
                    if (isFound)
                    {
                        return Conflict();
                    }
                    designation.CreatedOn = DateTime.UtcNow.AddHours(6);
                    designation.IsDeleted = false;
                    bool isSaved = _meghnaDesignationManager.Add(designation);

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
                var designations = _meghnaDesignationManager.GetAll();
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
                var designations = _meghnaDesignationManager.GetActiveDesignations();
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
                var designation = _meghnaDesignationManager.GetById(id);
                if (designation == null) return NotFound();
                return Ok(designation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit([FromBody] MeghnaDesignation designation)
        {

            var des = _meghnaDesignationManager.GetById(designation.Id);
            if (des.Name == designation.Name)
            {
                try
                {
                    designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                    designation.IsDeleted = false;
                    bool isSaved = _meghnaDesignationManager.Update(designation);
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
            bool isFound = _meghnaDesignationManager.DoesMeghnaDesignationExist(designation.Name);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                designation.IsDeleted = false;
                bool isSaved = _meghnaDesignationManager.Update(designation);
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
                var designation = _meghnaDesignationManager.GetById(designationId);
                if (designation == null)
                {
                    return NotFound();
                }

                designation.ModifiedBy = userId;
                designation.ModifiedOn = DateTime.UtcNow.AddHours(6);
                var isDeleted = _meghnaDesignationManager.SoftDelete(designation);

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
