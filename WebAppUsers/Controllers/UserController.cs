using FluentValidation.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppUsers.Models;
using WebAppUsers.Services;

namespace WebAppUsers.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }


        [HttpGet]
        [Route]         // This will be translated  to api/users
        public IHttpActionResult GeAll()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet]
        [Route("{userId}")]         // This will be translated  to api/users//1
        public IHttpActionResult GetById(int userId)
        {
            UserDTO user = _userService.GetById(userId);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route]         // This will be translated  to api/users
        public IHttpActionResult Save([FromBody][CustomizeValidator(RuleSet = "New")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            int newUserId = _userService.Save(user);

            return Ok(_userService.GetById(newUserId));
        }

       [HttpPut]
       [Route]         // This will be translated  to api/users
        public IHttpActionResult Update([FromBody][CustomizeValidator(RuleSet = "Update")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            _userService.Update(user);

            return Ok(_userService.GetById(user.UserId));
        }

        [HttpPut]
        [Route("changepassword")]         // This will be translated  to api/users/changepassword
        public IHttpActionResult ChangePassword([FromBody][CustomizeValidator(RuleSet = "UpdatePassword")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            _userService.ChangePassword(user);

            return Ok(_userService.GetById(user.UserId));
        }

        [HttpDelete]
        [Route("{userId}")]         // This will be translated  to api/users//1
        public IHttpActionResult Remove(int userId)
        {
            _userService.Remove(userId);

            return Ok(_userService.GetAll());
        }


        private string GetErrors()
        {
            return string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
        }
    }
}
