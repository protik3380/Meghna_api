using System;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using EFreshStoreCore.Model.Enums;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomerController : ApiController
    {
        private readonly ICustomerManager _customerManager;
        private readonly IUserManager _userManager;
        public CustomerController()
        {
            _customerManager = new CustomerManager();
            _userManager = new UserManager();
        }

        //[Authorize(Roles = "Admin")]
        public IHttpActionResult GetAll()
        {
            try
            {
                var customers = _customerManager.Get();
                if (customers == null) return NotFound();
                return Ok(customers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public IHttpActionResult CountTotalCustomer()
        {
            try
            {
                var totalCustomers = _customerManager.CountCustomer();
                if (totalCustomers == 0) return NotFound();
                return Ok(totalCustomers);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[Authorize(Roles = "Admin")]
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost]
        public IHttpActionResult Add(Customer aCustomer)
        {
            try
            {
                bool isSaved = false;
                aCustomer.IsDeleted = false;
                aCustomer.User = new User
                {
                    Username = aCustomer.Email,
                    Password = aCustomer.User.Password,
                    UserTypeId = (long) UserTypeEnum.Customer,
                    IsDeleted = false,
                    IsActive = true
                };
                var user = _userManager.GetByUserEmail(aCustomer.Email);
                if (user != null)
                {
                    return Conflict();
                }

                if (_customerManager.Add(aCustomer))
                {
                    if (Email.CheckForInternetConnection())
                    {
                        string subject = "[Meghna e-Commerce] Registration Successful";
                        string body = "Dear " + aCustomer.Name + Environment.NewLine;
                        body += Environment.NewLine;
                        body +=
                            "Congratulations! You have been successfully registered into Meghna e-Commerce." +
                            Environment.NewLine;
                        body += "Please find the credential below: " + Environment.NewLine;
                        body += "Username: " + aCustomer.User.Username + Environment.NewLine;
                        body += "Password: " + aCustomer.User.Password + Environment.NewLine;
                        body += Environment.NewLine;
                        body += "Regards" + Environment.NewLine;
                        body += "Meghna Group";
                        MailAddress mailAddress = new MailAddress(aCustomer.Email, aCustomer.Name);
                        if (!string.IsNullOrWhiteSpace(aCustomer.Email))
                        {
                            Email.SendEmail(subject, body, mailAddress);
                        }
                    }
                    return Created(new Uri(Request.RequestUri.ToString()), aCustomer);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Customer")]
        public IHttpActionResult GetById(long id)
        {
            try
            {
                Customer customer = _customerManager.GetById(id);
                if (customer == null) return NotFound();
                return Ok(customer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[Authorize(Roles = "Admin, Customer")]
        public IHttpActionResult GetByUserId(long id)
        {
            try
            {
                Customer customer = _customerManager.GetByUserId(id);
                if (customer == null) return NotFound();
                return Ok(customer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

       // [Authorize(Roles = "Admin, Customer")]
        [HttpPost]
        public IHttpActionResult Edit(Customer aCustomer)
        {
            try
            {
                Customer customer = _customerManager.GetById(aCustomer.Id);
                if (customer.Id == aCustomer.Id)
                {
                    customer.Name = aCustomer.Name;
                    customer.DeliveryAddress1 = aCustomer.DeliveryAddress1;
                    customer.DeliveryAddress2 = aCustomer.DeliveryAddress2;
                    customer.MobileNo = aCustomer.MobileNo;
                    customer.AlternativeMobileNo = aCustomer.AlternativeMobileNo;
                    customer.ModifiedOn = DateTime.Now;
                    customer.ModifiedBy = aCustomer.Id;
                    if(_customerManager.Update(customer)) return Ok();
                    return NotFound();
                }
                if (_customerManager.GetByUserEmail(customer.Email))
                {
                    return Conflict();
                }
                _customerManager.Update(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public string Test(Customer aCustomer)
        {
            try
            {
                bool isSaved = false;
                if (_customerManager.GetByUserEmail(aCustomer.Email))
                {
                    return "Customer saved failed.";
                }
                isSaved = _customerManager.Add(aCustomer);
                if (isSaved)
                {
                    string subject = "[Meghna e-Commerce] Registration Successful";
                    string body = "Dear " + aCustomer.Name + Environment.NewLine;
                    body += Environment.NewLine;
                    body +=
                        "Congratulations! You have been successfully registered into Meghna e-Commerce." +
                        Environment.NewLine;
                    body += "Please find the credential below: " + Environment.NewLine;
                    body += "Username: " + aCustomer.User.Username + Environment.NewLine;
                    body += "Password: " + aCustomer.User.Password + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "Regards" + Environment.NewLine;
                    body += "Meghna Group";
                    MailAddress mailAddress = new MailAddress(aCustomer.Email, aCustomer.Name);
                    if (!string.IsNullOrWhiteSpace(aCustomer.Email))
                    {
                        Email.SendEmail(subject, body, mailAddress);
                    }
                }
                return "User registration successfully";

            }
            catch (Exception e)
            {
                return e.Message;
            }
            //return aCustomer.Name+aCustomer.Email;
        }
    }
}
