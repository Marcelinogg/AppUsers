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
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }


        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return Ok(_userService.GetById(id));
        }

        [HttpPost]
        public IHttpActionResult Save([FromBody][CustomizeValidator(RuleSet = "New")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            _userService.Save(user);

            return Ok();
        }

       [HttpPut]
        public IHttpActionResult Update([FromBody][CustomizeValidator(RuleSet = "Update")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            _userService.Update(user);

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult ChangePassword([FromBody][CustomizeValidator(RuleSet = "UpdatePassword")] UserSaveDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetErrors());
            }

            _userService.ChangePassword(user);

            return Ok();
        }

        [HttpDelete]
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
