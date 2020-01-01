using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using FluentValidation;

namespace BankBros.Backend.Business.Validation.FluentValidation
{
    public class DepositDtoValidator : AbstractValidator<DepositDto>
    {
        public DepositDtoValidator()
        {
            RuleFor(x => x.Amount)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Tutar"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Tutar"))
                .Must(IsDecimal).WithMessage(string.Format(ValidationMessages.NotDecimal, "Tutar"))
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan,"Tutar","1"));

            RuleFor(x => x.AccountNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Hesap Numarası"))
                .GreaterThan(1000)
                .WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Hesap Numarası", "1000"))
                .LessThanOrEqualTo(2000)
                .WithMessage(string.Format(ValidationMessages.MustBeLessThan, "Hesap Numarası", "2000"));

        }

        public static bool IsDecimal(decimal text)
        {
            decimal value;
            if (Decimal.TryParse(text.ToString(), out value))
                return true;
            return false;
        }
    }
}
