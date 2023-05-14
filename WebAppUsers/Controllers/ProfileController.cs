using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppUsers.Services;

namespace WebAppUsers.Controllers
{
    [RoutePrefix("api/profiles")]
    public class ProfileController : ApiController
    {
        private readonly IProfileService _profileService;

        public ProfileController()
        {
            _profileService = new ProfileService();
        }


        [HttpGet]
        [Route]         // This will be translated  to /api/profiles
        public IHttpActionResult GetAll()
        {
            return Ok(_profileService.GetAll());
        }
    }
}
