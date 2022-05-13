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
    public class LPGComboDiscountController : ApiController
    {
        private readonly ILPGComboDiscountManager _lpgComboDiscountManager;
        private readonly IConfigurationManager _configurationManager;

        public LPGComboDiscountController()
        {
            _lpgComboDiscountManager = new LPGComboDiscountManager();
            _configurationManager = new ConfigurationManager();
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var discounts = (_lpgComboDiscountManager).GetAll();
                if (discounts == null) return NotFound();
                return Ok(discounts);
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
                var discounts = _lpgComboDiscountManager.GetAllActive();
                if (discounts == null) return NotFound();
                return Ok(discounts);
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
                var discount = _lpgComboDiscountManager.GetById(id);
                if (discount == null) return NotFound();
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        public IHttpActionResult GetLPGComboDiscount()
        {
            try
            {
                var discount = _lpgComboDiscountManager.GetLpgComboDiscount();
                if (discount == null) return NotFound();
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] LPGComboDiscount discount)
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
                    var activeDiscount = _lpgComboDiscountManager.GetLpgComboDiscount();

                    if (activeDiscount != null) return Conflict();

                    discount.CreatedOn = DateTime.UtcNow.AddHours(6);
                    discount.IsActive = true;
                    var isSaved = _lpgComboDiscountManager.Add(discount);

                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), discount);
                    }
                }

                return BadRequest("Couldn't update LPG Combo discount");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] LPGComboDiscount discount)
        {
            try
            {
                var lpgComboConfig = _configurationManager.GetActiveById((long)ConfigurationEnum.LPGCombo);

                if (lpgComboConfig == null)
                {
                    return BadRequest("You need to activate LPG combo configuration first");
                }

                var discountFromRepo = _lpgComboDiscountManager.GetById(discount.Id);

                if (discountFromRepo == null) return NotFound();

                discount.ModifiedOn = DateTime.UtcNow.AddHours(6);
                discount.CreatedBy = discountFromRepo.CreatedBy;
                discount.CreatedOn = discountFromRepo.CreatedOn;
                discount.IsActive = discountFromRepo.IsActive;
                discount.IsDeleted = discountFromRepo.IsDeleted;

                bool isUpdated = _lpgComboDiscountManager.Update(discount);

                if (isUpdated) return Ok();

                return BadRequest("Couldn't update LPG Combo discount");
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
