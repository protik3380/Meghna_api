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
    public class SubscriberController : ApiController
    {
        private readonly ISubscriberManager _subscriberManager;

        public SubscriberController()
        {
            _subscriberManager = new SubscriberManager();
        }

        [HttpPost]
        public IHttpActionResult SubscribeUser(Subscriber aSubscriber)
        {
            try
            {
                bool isExist = _subscriberManager.IsEmailExist(aSubscriber.Email);
                if (isExist)
                {
                    return Ok("This email is already subcribed");
                }
                aSubscriber.SubscribedOn = DateTime.Now;
                aSubscriber.IsDeleted = false;
                bool isSaved = _subscriberManager.Add(aSubscriber);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aSubscriber);
                }
                return BadRequest("Failed!");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var subscribedUsers = _subscriberManager.Get();
                if (subscribedUsers == null) return NotFound();
                subscribedUsers = subscribedUsers.OrderBy(c => c.Email).ToList();
                return Ok(subscribedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
