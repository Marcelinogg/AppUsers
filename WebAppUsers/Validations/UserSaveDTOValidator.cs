using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppUsers.Models;
using WebAppUsers.Services;

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
                    .Must(IsLoginNameUnique).WithMessage("Alguien ya hace uso del login '{PropertyValue}'");
                RuleFor(x => x.FullName)
                    .NotNull()
                    .NotEmpty();
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
                    .NotEmpty()
                    .Must(IsLoginNameUnique).WithMessage("Alguien ya hace uso del login '{PropertyValue}'");
                RuleFor(x => x.FullName)
                    .NotNull()
                    .NotEmpty();
                RuleFor(x => x.Email)
                        .EmailAddress()
                        .When(y => !string.IsNullOrEmpty(y.Email));
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

        private bool IsLoginNameUnique(string loginName)
        {
            return new UserService().IsAvailableLoginName(loginName);
        }
    }
}