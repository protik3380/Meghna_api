using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;
using ConfigurationManager = EFreshStoreCore.Manager.ConfigurationManager;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ConfigurationController : ApiController
    {
        private readonly IConfigurationManager _configurationManager;

        public ConfigurationController()
        {
            _configurationManager = new ConfigurationManager();
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var configurations = (_configurationManager).GetAll();
                if (configurations == null) return NotFound();
                configurations = configurations.OrderBy(c => c.Name).ToList();
                return Ok(configurations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAllActive()
        {
            try
            {
                var configurations = _configurationManager.GetAllActive();
                if (configurations == null) return NotFound();
                configurations = configurations.OrderBy(c => c.Name).ToList();
                return Ok(configurations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var configuration = _configurationManager.GetById(id);
                if (configuration == null) return NotFound();
                return Ok(configuration);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetActiveConfiguration()
        {
            try
            {
                var configuration = _configurationManager.GetActiveById((long)ConfigurationEnum.LPGCombo);
                if (configuration == null) return NotFound();
                return Ok(configuration);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult ActivateOrDeactivate(long configId, long userId)
        {
            try
            {
                var configuration = _configurationManager.GetById(configId);

                if (configuration == null) return NotFound();

                string message = configuration.IsActive ? "deactivate" : "activate";

                configuration.ModifiedOn = DateTime.UtcNow.AddHours(6);
                configuration.ModifiedBy = userId;
                configuration.IsActive = !configuration.IsActive;

                bool isUpdated = _configurationManager.Update(configuration);

                if (isUpdated) return Ok();

                return BadRequest("Couldn't " + message + " configuration");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
