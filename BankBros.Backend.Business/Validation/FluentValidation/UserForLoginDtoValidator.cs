using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using FluentValidation;

namespace BankBros.Backend.Business.Validation.FluentValidation
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            ValidatorOptions.LanguageManager.Enabled = false;
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");

            RuleFor(x => x.Username)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Kullanıcı adı"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Kullanıcı adı"))
                .Length(11).WithMessage(string.Format(ValidationMessages.ExactLength, "Kullanıcı adı", "{MaxLength}",
                    "{TotalLength}"))
                .Must(UserForCustomerRegisterDtoValidator.IsFullDigit)
                .WithMessage("Geçersiz kullanıcı adı TC Kimlik numaranız olmalıdır.");

            RuleFor(x => x.Password)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Şifre"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Şifre"))
                .Length(4, 6).WithMessage(string.Format(ValidationMessages.LengthFull, "Şifre", "{MinLength}", "{MaxLength}", "{TotalLength}"));

            RuleFor(x => x.ApplicationId)
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.InvalidMust, "Uygulama Bilgisi"));

        }

        public static bool IsNumber(string text)
        {
            int value;
            if (Int32.TryParse(text, out value))
                return true;
            return false;
        }
    }
}
