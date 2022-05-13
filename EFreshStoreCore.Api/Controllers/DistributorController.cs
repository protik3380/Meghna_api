using System;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DistributorController : ApiController
    {
        private readonly IDistributorManager _distributorManager;
        private readonly IDistributorProductLineManager _distributorProductLineManager;

        public DistributorController()
        {
            _distributorManager=new DistributorManager();
            _distributorProductLineManager = new DistributorProductLineManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var distributors = _distributorManager.GetAll();
                if (distributors == null) return NotFound();
                return Ok(distributors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Create([FromBody]Distributor aDistributor)
        {
            bool isFound = _distributorManager.DoesDistributorEmailExist(aDistributor.Email);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _distributorManager.Add(aDistributor);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), aDistributor);
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] Distributor aDistributor)
        {
            var distributor = _distributorManager.GetById(aDistributor.Id);
            if (distributor.Email == aDistributor.Email)
            {
                try
                {
                    bool isSaved = _distributorManager.Update(aDistributor);
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
            bool isFound = _distributorManager.DoesDistributorEmailExist(aDistributor.Email);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _distributorManager.Update(aDistributor);
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

        //[Authorize(Roles = "Admin")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var distributor = _distributorManager.GetById(id);
                if (distributor == null) return NotFound();
                return Ok(distributor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult SubscribeToProductLine([FromBody] DistributorProductLine distributorProductLine)
        {
            var subscriptionToProductLine = _distributorProductLineManager.IsSubscribedToProductLine(distributorProductLine);
            try
            {
                if (subscriptionToProductLine != null)
                {
                    if (subscriptionToProductLine.IsDeleted == false)
                    {
                        return Conflict();
                    }
                    subscriptionToProductLine.IsActive = true;
                    subscriptionToProductLine.IsDeleted = false;
                    subscriptionToProductLine.CreatedOn = DateTime.Now;
                    bool isUpdated = _distributorProductLineManager.Update(subscriptionToProductLine);
                    if (!isUpdated)
                    {
                        return BadRequest();
                    }
                } else {
                    distributorProductLine.IsActive = true;
                    distributorProductLine.IsDeleted = false;
                    distributorProductLine.CreatedOn = DateTime.Now;
                    bool isSaved = _distributorProductLineManager.Add(distributorProductLine);
                    if (!isSaved)
                    {
                        return BadRequest();
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetProductLineByDistributorId(long id)
        {
            try
            {
                var distributorSubscriptionList = _distributorProductLineManager.GetProductLineByDistributorId(id);
                if (distributorSubscriptionList == null) return NotFound();
                return Ok(distributorSubscriptionList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteSubscription(DistributorProductLine distributorProductLine)
        {
            var distributorProductLineFromRepo = _distributorProductLineManager.IsSubscribedToProductLine(distributorProductLine);

            try
            {
                if (distributorProductLineFromRepo != null)
                {
                    distributorProductLineFromRepo.IsDeleted = true;
                    distributorProductLineFromRepo.IsActive = false;
                    bool isUpdated = _distributorProductLineManager.Update(distributorProductLineFromRepo);
                    if (isUpdated)
                    {
                        return Ok();
                    }
                }
                return BadRequest("Failed!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult GetAllDistributorAgainstMasterDepot(long id)
        {
            try
            {
                var distributors = _distributorManager.GetDistributorAgainstMasterDepot(id);
                if (distributors == null) return NotFound();
                return Ok(distributors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
