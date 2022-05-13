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
    public class PaymentDetailController : ApiController
    {
        private readonly IPaymentDetailManager _paymentDetailManager;

        public PaymentDetailController()
        {
            _paymentDetailManager = new PaymentDetailManager();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] PaymentDetail paymentDetail)
        {
            try
            {
                bool isSaved = _paymentDetailManager.Add(paymentDetail);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), paymentDetail);
                }
                return BadRequest("Something went wrong!");
            }
            catch (Exception e)
            {
                return BadRequest("Something went wrong!");
            }
           
        }

    }
}
