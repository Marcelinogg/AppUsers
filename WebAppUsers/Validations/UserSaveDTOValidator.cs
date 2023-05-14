using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppUsers.Models;

namespace WebAppUsers.Validations
{
    public class UserSaveDTOValidator : AbstractValidator<UserSaveDTO>
    {
        public UserSaveDTOValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleSet("New", () =>
            {
                RuleFor(x => x.LoginName)
                    .NotNull()
                    .NotEmpty()
                    .Must(IsUserNameUnique).WithMessage("Alguien ya hace uso del login {LoginName}");
                RuleFor(x => x.Email)
                        .EmailAddress()
                        .When(y=> !string.IsNullOrEmpty(y.Email));
                RuleFor(x => x.Password)
                    .NotNull()
                    .NotEmpty()
                    .Equal(x=> x.PasswordConfirmation);
                RuleFor(x => x.ProfileId)
                    .GreaterThan(0);
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.UserId)
                   .GreaterThan(0);
                RuleFor(x => x.LoginName)
                    .NotNull()
                    .NotEmpty();
                RuleFor(x => x.Password)
                    .NotNull()
                    .NotEmpty()
                    .Equal(x => x.PasswordConfirmation);
                RuleFor(x => x.ProfileId)
                    .GreaterThan(0);
            });

            RuleSet("UpdatePassword", () =>
            {
                RuleFor(x => x.UserId)
                   .GreaterThan(0);
                RuleFor(x => x.Password)
                    .NotNull()
                    .NotEmpty()
                    .Equal(x => x.PasswordConfirmation);
            });
        }

        private bool IsUserNameUnique(string userName)
        {
            bool result = false;

            return result;
        }
    }
}