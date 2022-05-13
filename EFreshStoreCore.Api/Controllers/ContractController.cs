using System;
using System.Net.Mail;
using System.Web.Http;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    public class ContractController : ApiController
    {
        private readonly ICorporateContractManager _contractManager;
        private readonly ICorporateUserManager _corporateUserManager;
        private readonly IUserManager _userManager;

        public ContractController()
        {
            _contractManager = new CorporateContractManager();
            _corporateUserManager = new CorporateUserManager();
            _userManager = new UserManager();
        }

        //Corporate contracts
        public IHttpActionResult GetAll()
        {
            try
            {
                var contracts = _contractManager.GetAll();
                return Ok(contracts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult CountTotalCorporateUser()
        {
            try
            {
                int totalContracts = _corporateUserManager.CountCorporateUser();
                return Ok(totalContracts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IHttpActionResult GetByCorporateId(long id)
        {
            try
            {
                var corporateUser = _corporateUserManager.GetByCorporateId(id);
                if (corporateUser == null)
                {
                    return NotFound();
                }

                return Ok(corporateUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                var contracts = _contractManager.GetById(id);
                return Ok(contracts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Add([FromBody]CorporateContract aContract)
        {
            bool isFound = _contractManager.DoesCorporateCompanyNameExist(aContract.CompanyName);
            if (isFound)
            {
                return Conflict();
            }
            else
            {
                try
                {
                    bool isSaved = _contractManager.Add(aContract);
                    if (isSaved)
                    {
                        return Created(new Uri(Request.RequestUri.ToString()), aContract.Id);
                    }
                    return BadRequest();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult Edit([FromBody]CorporateContract aContract)
        {
            var contract = _contractManager.GetById(aContract.Id);
            if (contract.CompanyName == aContract.CompanyName)
            {
                try
                {
                    bool isSaved = _contractManager.Update(aContract);
                    if (isSaved)
                    {
                        return Ok(aContract);
                    }
                    return BadRequest();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            bool isFound = _contractManager.DoesCorporateCompanyNameExist(aContract.CompanyName);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isSaved = _contractManager.Update(aContract);
                if (isSaved)
                {
                    return Ok(aContract);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Corporate User
        [Authorize(Roles = "Admin")]
        public bool GetByEmail(string email)
        {
            var corporateUser = _corporateUserManager.GetByUserEmail(email);
            if (corporateUser)
            {
                return true;
            }
            return false;
        }
        //check internet connection

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult AddUser([FromBody]CorporateUser aCorporateUser)
        {
            try
            {
                //bool isFound = _corporateUserManager.GetByUserEmail(aCorporateUser.Email);
                var user = _userManager.GetByUserEmail(aCorporateUser.Email);

                if (user == null)
                {
                    bool isSaved = _corporateUserManager.Add(aCorporateUser);
                    if (isSaved)
                    {
                        bool connection = UtilityClass.CheckForInternetConnection();
                        if (!connection)
                        {
                            return Created(new Uri(Request.RequestUri.ToString()), aCorporateUser);
                        }
                        string subject = "[Meghna e-Commerce] Registration Successful";
                        string body = "Dear " + aCorporateUser.Name + Environment.NewLine;
                        body += Environment.NewLine;
                        body +=
                            "Congratulations! You have been successfully registered into Meghna e-Commerce." +
                            Environment.NewLine;
                        body += "Please find the credential below: " + Environment.NewLine;
                        body += "Username: " + aCorporateUser.User.Username + Environment.NewLine;
                        body += "Password: " + aCorporateUser.User.Password + Environment.NewLine;
                        body += Environment.NewLine;
                        body += "Regards" + Environment.NewLine;
                        body += "Meghna Group";
                        MailAddress mailAddress = new MailAddress(aCorporateUser.Email, aCorporateUser.Name);
                        if (!string.IsNullOrWhiteSpace(aCorporateUser.Email))
                        {
                            Email.SendEmail(subject, body, mailAddress);
                        }
                        return Created(new Uri(Request.RequestUri.ToString()), aCorporateUser);
                    }
                    return BadRequest();
                }
                return Conflict();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult EditUser(CorporateUser aCorporateUser)
        {
            CorporateUser user = _corporateUserManager.GetById(aCorporateUser.Id);
            if (user.Id == aCorporateUser.Id)
            {
                try
                {
                    user.Name = aCorporateUser.Name;
                    user.DeliveryAddress1 = aCorporateUser.DeliveryAddress1;
                    user.DeliveryAddress2 = aCorporateUser.DeliveryAddress2;
                    user.AlternativeMobileNo = aCorporateUser.AlternativeMobileNo;
                    user.Designation = aCorporateUser.Designation;
                    user.CorporateDepartmentId = aCorporateUser.CorporateDepartmentId;
                    user.CorporateDesignationId = aCorporateUser.CorporateDesignationId;
                    user.ModifiedOn = DateTime.Now;
                    user.ModifiedBy = aCorporateUser.Id;                    
                    bool isUpdate = _corporateUserManager.Update(user);
                    if (!isUpdate)
                    {
                        return BadRequest("Update failed");
                    }

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            bool isFound = _corporateUserManager.GetByUserEmail(aCorporateUser.Email);
            if (isFound)
            {
                return Conflict();
            }
            try
            {
                bool isUpdate = _corporateUserManager.Update(user);
                if (!isUpdate)
                {
                    return BadRequest("Update failed");
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet]
        public IHttpActionResult GetCorporateUserById(long id)
        {
            try
            {
                var corporateUser = _corporateUserManager.GetByUserId(id);
                if (corporateUser == null)
                {
                    return BadRequest("Data not found");
                }

                return Ok(corporateUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
