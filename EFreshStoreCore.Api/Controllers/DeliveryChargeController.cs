using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DeliveryChargeController : ApiController
    {
        private readonly IDeliveryChargeManager _deliveryChargeManager;
        private readonly IConfigurationManager _configurationManager;

        public DeliveryChargeController()
        {
            _configurationManager = new ConfigurationManager();
            _deliveryChargeManager = new DeliveryChargeManager();
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var deliveryCharge = _deliveryChargeManager.GetById(id);
                if (deliveryCharge == null) return NotFound();
                return Ok(deliveryCharge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        public IHttpActionResult GetDeliveryCharge()
        {
            try
            {
                var deliveryCharge = _deliveryChargeManager.GetActiveDeliveryCharge();
                if (deliveryCharge == null) return NotFound();
                return Ok(deliveryCharge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult GetValidDeliveryCharge()
        {
            try
            {
                var deliveryCharge = _deliveryChargeManager.GetActiveDeliveryCharge();
                if (deliveryCharge == null) return NotFound();
                var deliveryChargeConfig = _configurationManager.GetActiveById((long)ConfigurationEnum.DeliveryCharge);

                if (deliveryChargeConfig == null) return NotFound();
                return Ok(deliveryCharge);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] DeliveryCharge deliveryCharge)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var lpgComboConfig = _configurationManager.GetActiveById((long)ConfigurationEnum.LPGCombo);

                    if (lpgComboConfig == null)
                    {
                        return BadRequest("You need to activate LPG combo configuration first");
                    }
                    var activeDeliveryCharge = _deliveryChargeManager.GetActiveDeliveryCharge();

                    if (activeDeliveryCharge != null) return Conflict();

                    deliveryCharge.CreatedOn = DateTime.UtcNow.AddHours(6);
                    deliveryCharge.IsActive = true;
                    var isSaved = _deliveryChargeManager.Add(deliveryCharge);

                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), deliveryCharge);
                    }
                }

                return BadRequest("Couldn't create delivery charge");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] DeliveryCharge deliveryCharge)
        {
            try
            {
                var lpgComboConfig = _configurationManager.GetActiveById((long)ConfigurationEnum.DeliveryCharge);

                if (lpgComboConfig == null)
                {
                    return BadRequest("You need to activate delivery charge configuration first");
                }

                var deliveryChargeFromRepo = _deliveryChargeManager.GetById(deliveryCharge.Id);

                if (deliveryChargeFromRepo == null) return NotFound();

                deliveryCharge.ModifiedOn = DateTime.UtcNow.AddHours(6);
                deliveryCharge.CreatedBy = deliveryChargeFromRepo.CreatedBy;
                deliveryCharge.CreatedOn = deliveryChargeFromRepo.CreatedOn;
                deliveryCharge.IsActive = deliveryChargeFromRepo.IsActive;
                deliveryCharge.IsDeleted = deliveryChargeFromRepo.IsDeleted;

                bool isUpdated = _deliveryChargeManager.Update(deliveryCharge);

                if (isUpdated) return Ok();

                return BadRequest("Couldn't update delivery charge");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }


    }
}
