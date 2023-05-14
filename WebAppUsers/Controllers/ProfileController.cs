using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppUsers.Services;

namespace WebAppUsers.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IProfileService _profileService;

        public ProfileController()
        {
            _profileService = new ProfileService();
        }


        [HttpGet]
        public IHttpActionResult GetProfiles()
        {
            return Ok(_profileService.GetAll());
        }
    }
}
