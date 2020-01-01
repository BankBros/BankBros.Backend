using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using Castle.Core;
using FluentValidation;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace BankBros.Backend.Business.Validation.FluentValidation
{
    public class UserForCustomerRegisterDtoValidator : AbstractValidator<UserForCustomerRegisterDto>
    {
        public UserForCustomerRegisterDtoValidator()
        {
            RuleFor(x => x.TCKN)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "TC Kimlik No"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "TC Kimlik No"))
                .Length(11).WithMessage(string.Format(ValidationMessages.ExactLength, "TC Kimlik No","{MaxLength}","{TotalLength}"))
                .Must(IsFullDigit).WithMessage(string.Format(ValidationMessages.InvalidMust, "TC Kimlik"));


            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull,"Ad"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Ad"))
                .Length(3, 50).WithMessage(string.Format(ValidationMessages.LengthFull, "Ad", "{MinLength}", "{MaxLength}", "{TotalLength}"));

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Soyad"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Soyad"))
                .Length(3, 50)
                .WithMessage(string.Format(ValidationMessages.LengthFull, "Soyad", "{MinLength}", "{MaxLength}", "{TotalLength}"));

            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Adres"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Adres"));
            
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Email"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Email"))
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Şifre"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Şifre"))
                .Length(6).WithMessage(string.Format(ValidationMessages.ExactLength, "Şifre", "{MaxLength}",
                    "{TotalLength}"));

            RuleFor(x => x.SecretAnswer)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Gizli Cevap"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Gizli Cevap"));

            RuleFor(x => x.SecretQuestion)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Gizli Soru"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Gizli Soru"));

            RuleFor(x => x.PhoneNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Telefon No"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Telefon No"))
                .Length(10)
                .WithMessage(string.Format(ValidationMessages.ExactLength, "Telefon No", "{MaxLength}",
                    "{TotalLength}"))
                .Must(IsFullDigit).WithMessage(string.Format(ValidationMessages.InvalidMust, "Telefon No"));
        }

        public static bool IsFullDigit(string text)
        {
            return text.ToCharArray().Any(x => !Char.IsDigit(x)) ? false : true;
        }
    }
}
