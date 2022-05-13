using System;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MasterDepotController : ApiController
    {
        private readonly IMasterDepotManager _masterDepotManager;
        private readonly IThanaWiseMasterDepotManager _thanaWiseMasterDepotManager;
        private readonly IOrderDetailManager _orderDetailManager;
        private readonly IAssignOrderManager _assignOrderManager;

        public MasterDepotController()
        {
            _masterDepotManager = new MasterDepotManager();
            _thanaWiseMasterDepotManager = new ThanaWiseMasterDepotManager();
            _orderDetailManager = new OrderDetailManager();
            _assignOrderManager = new AssignOrderManager();
        }


        public IHttpActionResult GetAll()
        {
            try
            {
                var masterDepo = _masterDepotManager.GetAll();
                if (masterDepo == null) return NotFound();
                return Ok(masterDepo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetAllActive()
        {
            try
            {
                var masterDepo = _masterDepotManager.GetActiveMasterDepots();
                if (masterDepo == null) return NotFound();
                return Ok(masterDepo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult CountTotalMasterDepot()
        {
            try
            {
                int TotalMasterDepo = _masterDepotManager.CountMasterDepot();
                if (TotalMasterDepo == 0) return NotFound();
                return Ok(TotalMasterDepo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Add([FromBody] MasterDepot masterDepot)
        {
            bool isFound = _masterDepotManager.DoesMasterDepotEmailExist(masterDepot.Email);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _masterDepotManager.Add(masterDepot);
                if (isSaved)
                {
                    bool connection = UtilityClass.CheckForInternetConnection();
                    if (!connection)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), masterDepot);
                    }
                    string subject = "[Meghna e-Commerce] Registration Successful";
                    string body = "Dear " + masterDepot.ContactPerson + Environment.NewLine;
                    body += Environment.NewLine;
                    body +=
                        "Congratulations! You have been successfully registered into Meghna e-Commerce as a MASTER DEPOT." +
                        Environment.NewLine;
                    body += "Please find the credential below: " + Environment.NewLine;
                    body += "Username: " + masterDepot.User.Username + Environment.NewLine;
                    body += "Password: " + masterDepot.User.Password + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "Regards" + Environment.NewLine;
                    body += "Meghna Group";
                    MailAddress mailAddress = new MailAddress(masterDepot.Email, masterDepot.ContactPerson);
                    if (!string.IsNullOrWhiteSpace(masterDepot.Email))
                    {
                        Email.SendEmail(subject, body, mailAddress);
                    }
                    return Created(new Uri(Request.RequestUri.ToString()), masterDepot);
                }
                return BadRequest("Master depot not saved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody] MasterDepot masterDepot)
        {
            var depot = _masterDepotManager.GetById(masterDepot.Id);
            if (depot.Email == masterDepot.Email)
            {
                try
                {
                    masterDepot.UserId = depot.UserId;
                    bool isUpdate = _masterDepotManager.Update(masterDepot);
                    if (isUpdate)
                    {
                        return Ok();
                    }
                    return NotFound();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            bool isFound = _masterDepotManager.DoesMasterDepotEmailExist(masterDepot.Email);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isUpdate = _masterDepotManager.Update(masterDepot);
                if (isUpdate)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetById(long id)
        {
            try
            {
                var masterDepo = _masterDepotManager.GetById(id);
                if (masterDepo != null)
                {
                    return Ok(masterDepo);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
   public IHttpActionResult GetByUserId(long userId)
        {
            try
            {
                var masterDepo = _masterDepotManager.GetByUserId(userId);
                if (masterDepo != null)
                {
                    return Ok(masterDepo);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult LinkMasterDepotWithThana([FromBody] ThanaWiseMasterDepot thanaWiseMasterDepot)
        {
            try
            {
                if (_thanaWiseMasterDepotManager.IsExistMasterDepot(thanaWiseMasterDepot))
                {
                    return BadRequest("This master depot is already linked with this thana.");
                }
                _thanaWiseMasterDepotManager.Add(thanaWiseMasterDepot);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IHttpActionResult GetAllThanaWithMasterDepot()
        {
            try
            {
                var thanaWithMasterDepo = _thanaWiseMasterDepotManager.GetAll();
                if (thanaWithMasterDepo == null) return NotFound();
                return Ok(thanaWithMasterDepo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IHttpActionResult GetByThanaWiseMasterDepotId(long id)
        {
            try
            {
                var thanaWiseMasterDepot = _thanaWiseMasterDepotManager.GetFirstOrDefault(c => c.Id == id);
                if (thanaWiseMasterDepot == null) return NotFound();
                return Ok(thanaWiseMasterDepot);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult EditMasterDepotWithThana([FromBody] ThanaWiseMasterDepot thanaWiseMasterDepot)
        {
            try
            {
                if (_thanaWiseMasterDepotManager.IsExistMasterDepot(thanaWiseMasterDepot))
                {
                    return BadRequest("This master depot  is already linked with this thana.");
                }
                bool isUpdate = _thanaWiseMasterDepotManager.Update(thanaWiseMasterDepot);
                return Ok(isUpdate);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetMasterDepotByThanaId(long id)
        {
            try
            {
                var masterDepot = _masterDepotManager.GetByThanaAndProduct(id);
                if (masterDepot == null) return NotFound();
                if(masterDepot.Id==0)
                {
                    return NotFound();
                }
                return Ok(masterDepot);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult DeleteProductFromPendingOrder(OrderDetail orderDetail)
        {
            var orderDetailFromRepo = _orderDetailManager.IsProductExistsInOrderDetails(orderDetail);

            try
            {
                if (orderDetailFromRepo != null)
                {
                    bool isDeleted = _orderDetailManager.Delete(orderDetail);
                    if (isDeleted)
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
    }
}
