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
    public class AssignOrderController : ApiController
    {
        private readonly IAssignOrderManager _assignOrderManager;

        public AssignOrderController()
        {
            _assignOrderManager = new AssignOrderManager();
        }
        [HttpPost]
        public IHttpActionResult Create([FromBody] AssignOrder assignOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isFound = _assignOrderManager.DoesAssignOrderExist((long) assignOrder.OrderId);
                    if (isFound)
                    {
                        return Conflict();
                    }
                    bool isSaved = _assignOrderManager.Add(assignOrder);
                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), assignOrder);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Edit([FromBody] AssignOrder assignOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var assignedOrderFromDb = _assignOrderManager.GetById(assignOrder.Id);
                    if (assignedOrderFromDb == null)
                    {
                        return NotFound();
                    }

                    assignedOrderFromDb.ModifiedOn = DateTime.Now;
                    assignedOrderFromDb.ModifiedBy = assignOrder.ModifiedBy;
                    assignedOrderFromDb.DeliveryManId = assignOrder.DeliveryManId;
                    bool isUpdated = _assignOrderManager.Update(assignedOrderFromDb);
                    if (isUpdated)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var assignedOrders = _assignOrderManager.GetAll();
                if (assignedOrders == null) return NotFound();
                assignedOrders = assignedOrders.ToList();
                return Ok(assignedOrders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetDeliveryManByOrderId(long orderId)
        {
            try
            {
                DeliveryMan deliveryMan = _assignOrderManager.GetDeliveryManByOrderId(orderId).DeliveryMan;
                if (deliveryMan != null)
                {
                    return Ok(deliveryMan);

                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }
    }
}
