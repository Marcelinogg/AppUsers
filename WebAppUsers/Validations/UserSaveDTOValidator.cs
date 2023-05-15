using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                    .MinimumLength(8)
                    .Equal(x => x.PasswordConfirmation).WithMessage("La confirmación del password es incorrecta");
                RuleFor(x => x.ProfileId)
                    .GreaterThan(0);
                RuleFor(x => x.Avatar)
                        .Must(IsImageFile)
                        .When(y => !string.IsNullOrEmpty(y.Avatar)).WithMessage("El archivo no es una imagen")
                        .Must(CheckMaxSizeFile)
                        .When(y => !string.IsNullOrEmpty(y.Avatar)).WithMessage("El archivo debe ser menor de 2MB de peso");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.UserId)
                   .GreaterThan(0);
                RuleFor(x => x.FullName)
                    .NotNull()
                    .NotEmpty();
                RuleFor(x => x.Email)
                        .EmailAddress()
                        .When(y => !string.IsNullOrEmpty(y.Email));
                RuleFor(x => x.ProfileId)
                    .GreaterThan(0);
                RuleFor(x => x.Avatar)
                        .Must(IsImageFile)
                        .When(y => !string.IsNullOrEmpty(y.Avatar)).WithMessage("El archivo no es una imagen")
                        .Must(CheckMaxSizeFile)
                        .When(y => !string.IsNullOrEmpty(y.Avatar)).WithMessage("El archivo debe ser menor de 2MB de peso");
            });

            RuleSet("UpdatePassword", () =>
            {
                RuleFor(x => x.UserId)
                   .GreaterThan(0);
                RuleFor(x => x.Password)
                    .NotNull()
                    .NotEmpty()
                    .MinimumLength(8)
                    .Equal(x => x.PasswordConfirmation).WithMessage("La confirmación del password es incorrecta");
            });
        }

        private bool IsLoginNameUnique(string loginName)
        {
            return new UserService().IsAvailableLoginName(loginName);
        }

        private bool IsImageFile(string base64String)
        {
            return GetRawImage(base64String) != null;
        }

        private bool CheckMaxSizeFile(string base64String)
        {
            int maxfileSize = 2 * 1024 * 1024;  // 2MB is the maximun size file allowed
            int fileSize = maxfileSize +1;

            try
            {
                string stringFile = base64String.Split(',')[1];
                byte[] fileBytes = Convert.FromBase64String(stringFile);
                fileSize = fileBytes.Length;
            }
            catch { }

            return fileSize <= maxfileSize;
        }

        // Tries to convert the base 64 string to image when fails is not a image file
        private Image GetRawImage(string base64String)
        {
            Image fileImage = null;

            try
            {
                string stringFile = base64String.Split(',')[1];
                byte[] fileBytes = Convert.FromBase64String(stringFile);

                using (MemoryStream fileStream = new MemoryStream(fileBytes))
                {
                    fileImage = Image.FromStream(fileStream);
                }                    
            }
            catch
            {
                return fileImage;
            }

            return fileImage;
        }
    }
}