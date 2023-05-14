using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppUsers.Validations;

namespace WebAppUsers.Models
{
    [Validator(typeof(UserSaveDTOValidator))]
    public class UserSaveDTO
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Avatar { get; set; }
        public int ProfileId { get; set; }
    }
}