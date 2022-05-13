using System;
using System.Linq;
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
    public class MeghnaUserController : ApiController
    {
        private readonly IMeghnaUserManager _meghnaUserManager;
        private readonly IUserDiscountManager _userDiscountManager;
        private readonly IUserManager _userManager;

        public MeghnaUserController()
        {
            _userDiscountManager = new UserDiscountManager();
            _meghnaUserManager = new MeghnaUserManager();
            _userManager = new UserManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var user = _meghnaUserManager.GetAll();
                if(user==null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult CountTotalMeghnaUser()
        {
            try
            {
                int totalMeghnaUser = _meghnaUserManager.CountMeghnaUser();
                if(totalMeghnaUser == 0)
                {
                    return NotFound();
                }
                return Ok(totalMeghnaUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public bool GetByEmail(string email)
        {
            bool isFound = _meghnaUserManager.GetByUserEmail(email);
            if (isFound)
            {
                return true;
            }
            return false;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IHttpActionResult AddMeghnaUser([FromBody]MeghnaUser aMeghnaUser)
        {
            try
            {
                var user = _userManager.GetByUserEmail(aMeghnaUser.Email);

                if (user == null)
                {
                    bool isSaved = _meghnaUserManager.Add(aMeghnaUser);
                    if (isSaved)
                    {
                        bool connection = Email.CheckForInternetConnection();
                        if (!connection)
                        {
                            return Created(new Uri(Request.RequestUri.ToString()), aMeghnaUser);
                        }
                        string subject = "[Meghna e-Commerce] Registration Successful";
                        string body = "Dear " + aMeghnaUser.Name + Environment.NewLine;
                        body += Environment.NewLine;
                        body +=
                            "Congratulations! You have been successfully registered into Meghna e-Commerce." +
                            Environment.NewLine;
                        body += "Please find the credential below: " + Environment.NewLine;
                        body += "Username: " + aMeghnaUser.User.Username + Environment.NewLine;
                        body += "Password: " + aMeghnaUser.User.Password + Environment.NewLine;
                        body += Environment.NewLine;
                        body += "Regards" + Environment.NewLine;
                        body += "Meghna Group";
                        MailAddress mailAddress = new MailAddress(aMeghnaUser.Email, aMeghnaUser.Name);
                        if (!string.IsNullOrWhiteSpace(aMeghnaUser.Email))
                        {
                            Email.SendEmail(subject, body, mailAddress);
                        }
                        return Created(new Uri(Request.RequestUri.ToString()), aMeghnaUser);
                    }
                    return BadRequest();
                }
                return Conflict();
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
                var meghnaUser = _meghnaUserManager.GetById(id);
                if (meghnaUser == null) return NotFound();
                return Ok(meghnaUser);
            }
            catch (Exception e)
            {
               return BadRequest(e.Message);
            }
        }

        //[Authorize(Roles = "Admin, MeghnaUser")]
        public IHttpActionResult GetByUserId(long id)
        {
            try
            {
                var meghnaUser = _meghnaUserManager.GetByUserId(id);
                if (meghnaUser == null) return NotFound();
                return Ok(meghnaUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Add discount
        public IHttpActionResult AddDiscount([FromBody] UserDiscount discount)
        {
            if (ModelState.IsValid)
            {
                var previousDiscounts = _userDiscountManager.GetAll();
                if (previousDiscounts.Count != 0)
                {
                    foreach (var previousDiscount in previousDiscounts)
                    {
                        if (previousDiscount.IsActive == true)
                        {
                            previousDiscount.IsActive = false;
                            previousDiscount.IsDeleted = true;
                            _userDiscountManager.Update(previousDiscount);
                        }
                    }
                }
                discount.CreatedOn = DateTime.UtcNow.AddHours(6);
                discount.IsActive = true;
                discount.IsDeleted = false;
                bool isSaved = _userDiscountManager.Add(discount);
                if (isSaved)
                {
                    return Created(new Uri(Request.RequestUri.ToString()), discount);
                }
            }
            return BadRequest();
        }

        public IHttpActionResult GetMeghnaUserDiscount()
        {
            try
            {
                var discount = _userDiscountManager.GetActiveDiscount();
                if (discount == null) return NotFound();
                return Ok(discount);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IHttpActionResult GetAllMeghnaUserDiscounts()
        {
            try
            {
                var discount = _userDiscountManager.GetAll();
                if (discount == null) return NotFound();
                var discounts = discount.OrderBy(o => o.DiscountPercentage);
                return Ok(discounts);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       // [Authorize(Roles = "Admin, MeghnaUser")]
        [HttpPost]
        public IHttpActionResult Edit(MeghnaUser aMeghnaUser)
        {
            try
            {
                MeghnaUser meghnaUser = _meghnaUserManager.GetById(aMeghnaUser.Id);
                if (meghnaUser.Id == aMeghnaUser.Id)
                {
                    meghnaUser.Name = aMeghnaUser.Name;
                    meghnaUser.DeliveryAddress1 = aMeghnaUser.DeliveryAddress1;
                    meghnaUser.DeliveryAddress2 = aMeghnaUser.DeliveryAddress2;
                    meghnaUser.MobileNo = aMeghnaUser.MobileNo;
                    meghnaUser.AlternativeMobileNo = aMeghnaUser.AlternativeMobileNo;
                    meghnaUser.Designation = aMeghnaUser.Designation;
                    meghnaUser.MeghnaDepartmentId = aMeghnaUser.MeghnaDepartmentId;
                    meghnaUser.MeghnaDesignationId = aMeghnaUser.MeghnaDesignationId;
                    meghnaUser.ModifiedOn = DateTime.Now;
                    meghnaUser.ModifiedBy = aMeghnaUser.Id;
                    if (_meghnaUserManager.Update(meghnaUser)) return Ok();
                    return NotFound();
                }
                if (_meghnaUserManager.GetByUserEmail(aMeghnaUser.Email))
                {
                    return Conflict();
                }
                _meghnaUserManager.Update(meghnaUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
