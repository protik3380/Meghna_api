using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    
    public class FAQController : ApiController
    {
        private readonly IFAQManager _faqManager;

        public FAQController()
        {
            _faqManager = new FAQManager();
        }


        public IHttpActionResult GetAll()
        {
            try
            {
                var faq = _faqManager.GetAll();
                if (faq == null) return NotFound();
                return Ok(faq.OrderBy(c => c.CreatedOn).ToList());
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
                var faq = _faqManager.GetAllActive();
                if (faq == null) return NotFound();
                return Ok(faq.OrderBy(c => c.CreatedOn).ToList());
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
                var brand = _faqManager.GetById(id);
                if (brand == null) return NotFound();
                return Ok(brand);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody] FAQ faq)
        {
            if (ModelState.IsValid)
            {
                bool isFound = _faqManager.DoesFAQExist(faq.Question);
                if (isFound)
                {
                    return Conflict();
                }
                bool isSaved = _faqManager.Add(faq);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), faq);
                }
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]FAQ faq)
        {
            var anFaq = _faqManager.GetById(faq.Id);
            if (anFaq.Question == faq.Question)
            {
                try
                {
                    bool isSaved = _faqManager.Update(faq);
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
            bool isFound = _faqManager.DoesFAQExist(faq.Question);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _faqManager.Update(faq);
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
