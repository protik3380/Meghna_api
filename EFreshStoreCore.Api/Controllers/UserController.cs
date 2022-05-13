using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Security.AccessControl;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Context.ViewModels;
using EFreshStoreCore.Model.Dtos;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private readonly IUserManager _userManager;
        private readonly IUserTypeManager _usertypeManager;
        private readonly ICorporateUserManager _corporateUserManager;
        private readonly IMeghnaUserManager _meghnaUserManger;
        private readonly ICustomerManager _customerManger;
        private readonly IDeliveryManManager _deliveryManManager;

        public UserController()
        {
            _userManager = new UserManager();
            _usertypeManager = new UserTypeManager();
            _corporateUserManager = new CorporateUserManager();
            _meghnaUserManger = new MeghnaUserManager();
            _customerManger = new CustomerManager();
            _deliveryManManager = new DeliveryManManager();

        }
        [HttpGet]
        public IHttpActionResult Login(string email,string password)
        {
            try
            {
                User validateUser = _userManager.ValidateUser(email, password);
                if (validateUser != null)
                {
                    if (validateUser.UserTypeId == (long)UserTypeEnum.Corporate)
                    {
                        CorporateUser corporateUser = _corporateUserManager.GetByUserId(validateUser.Id);
                        if (corporateUser.CorporateContract.Validity.HasValue &&
                            corporateUser.CorporateContract.Validity.Value.AddDays(1) < DateTime.Now || corporateUser.CorporateContract.IsDeleted)
                        {
                            return Unauthorized();
                        }
                    }
                    return Ok(validateUser);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult DeliveryManLogin(string email, string password)
        {
            try
            {
                User validateUser = _userManager.ValidateDeliveryManUser(email, password);

                if (validateUser != null)
                {
                    DeliveryManUserInfo userInfo = new DeliveryManUserInfo();
                    userInfo.Id = validateUser.Id;
                    userInfo.Username = validateUser.Username;
                    userInfo.Password = validateUser.Password;
                    userInfo.IsActive = (bool) validateUser.IsActive;
                    userInfo.DeliveryManId = _deliveryManManager.GetByUserId(validateUser.Id).Id;
                    return Ok(userInfo);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetAllOrderableUser()
        {
          
            try
            {
                var users = _userManager.TotalAllOrderableUser();
                return Ok(users); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public IHttpActionResult GetByUserEmail(string email)
        {
            try
            {
                User user = _userManager.GetByUserEmail(email);
                if (user != null)
                {
                    return Ok(user);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody]User user)
        {
            User getUser = null;
            if (user.Id == 0)
            {
                getUser = _userManager.GetByUserEmail(user.Username);
            }
            else
            {
                getUser = _userManager.GetById(user.Id);
            }
            try
            {

                if (getUser != null)
                {
                    getUser.Password = user.Password;
                    _userManager.Update(getUser);
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IHttpActionResult ChangePasswordForDeliveryMan([FromBody]ChangePasswordDto changePassword)
        {
            User user = null;
            user = changePassword.UserId == 0 ? _userManager.GetByUserEmail(changePassword.Username) : _userManager.GetById(changePassword.UserId);
            try
            {

                if (user != null)
                {
                    user.Password = changePassword.Password;
                    _userManager.Update(user);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize]
        [HttpGet]
        public IHttpActionResult GetUserById(long id)
        {
            try
            {
                var user = _userManager.GetById(id);
                if (user.UserTypeId == (long)UserTypeEnum.Corporate)
                {
                    var userDetails = _corporateUserManager.GetByUserId(id);
                    if (userDetails == null) return NotFound();
                    return Ok(userDetails);
                }
                if (user.UserTypeId == (long)UserTypeEnum.MeghnaUser)
                {
                    var userDetails = _meghnaUserManger.GetByUserId(id);
                    if (userDetails == null) return NotFound();
                    return Ok(userDetails);
                }
                if (user.UserTypeId == (long)UserTypeEnum.Customer)
                {
                    var userDetails = _customerManger.GetByUserId(id);
                    if (userDetails == null) return NotFound();
                    return Ok(userDetails);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult ForgotPassword(string email)
        {
            try
            {
                User aUser =  _userManager.GetByUserEmail(email);
                //User aUser = _userManager.GetByUserEmail(email);

                if (aUser != null)
                {
                    string subject = "[Meghna e-Commerce] Reset Password";
                    string body = "Dear user" + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "Click the link below to change your password" +
                        Environment.NewLine;
                    body += "http://ecommerce.nerdcastlebd.com/EFreshTest/Home/ResetPassword/?UserName=" + aUser.Username + Environment.NewLine;

                    MailAddress mailAddress = new MailAddress(aUser.Username, aUser.Username);
                    if (!string.IsNullOrWhiteSpace(aUser.Username))
                    {
                        Email.SendEmail(subject, body, mailAddress);
                    }
                
                    return Ok();

                }
                else
                {
                   
                    return BadRequest();

                }
            }
            catch (Exception e )
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult DoesDeliveryManExit(string username)
        {

            try
            {
                var userInformation = _userManager.DoesUsernameExist(username);

                if (userInformation!=null)
                {
                    return Ok(userInformation.Id);
                }
                return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IHttpActionResult ContactUs(ContactUsVm aContactUsVm)
        {
            string body = "Review from a customer: \nName: " + aContactUsVm.Name + "\nContact No: " + aContactUsVm.MobileNo + "\nThe message is given below:\n\n" + aContactUsVm.Message;
            MailAddress mail = new MailAddress("meghna.ecommerce@gmail.com");
            Email.SendEmail(aContactUsVm.Subject, body, mail);
            return Ok("Mail has been sent");
        }
    }
}
