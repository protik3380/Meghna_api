using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Interfaces.Managers;

namespace EFreshStoreCore.Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserTypeController : ApiController
    {
        private readonly IUserTypeManager _usertypeManager;

        public UserTypeController()
        {
            _usertypeManager = new UserTypeManager();
        }

        public IHttpActionResult GetAll()
        {
            try
            {
                var userTypes = _usertypeManager.GetAll();
                if (userTypes == null) return NotFound();
                userTypes = userTypes.OrderBy(c => c.Name).ToList();
                return Ok(userTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
