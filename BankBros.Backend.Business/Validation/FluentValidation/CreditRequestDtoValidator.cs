using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using FluentValidation;
using System;

namespace BankBros.Backend.Business.Validation.FluentValidation
{
    public class CreditRequestDtoValidator : AbstractValidator<CreditRequestDto>
    {
        public CreditRequestDtoValidator()
        {
            RuleFor(x => x.Amount)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Tutar"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Tutar"))
                .Must(IsDecimal).WithMessage(string.Format(ValidationMessages.NotDecimal, "Tutar"))
                .GreaterThan(1)
                .WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Tutar", "1"))
                .LessThanOrEqualTo(500000)
                .WithMessage(string.Format(ValidationMessages.MustBeLessThan, "Tutar", "500000"));

            RuleFor(x => x.Age)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Yaş"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Yaş"))
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.NotDecimal, "Yaş"))
                .GreaterThanOrEqualTo(18)
                .WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Yaş", "17"))
                .LessThanOrEqualTo(100)
                .WithMessage(string.Format(ValidationMessages.MustBeLessThan, "Yaş", "100"));

            RuleFor(x => x.UsedCredits)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Kredi Sayısı"))
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Kredi Sayısı"))
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.NotDecimal, "Kredi Sayısı"))
                .GreaterThanOrEqualTo(0)
                .WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Kredi Sayısı", "-1"))
                .LessThanOrEqualTo(100)
                .WithMessage(string.Format(ValidationMessages.MustBeLessThan, "Kredi Sayısı", "100"));

        }

        public static bool IsDecimal(decimal text)
        {
            decimal value;
            if (Decimal.TryParse(text.ToString(), out value))
                return true;
            return false;
        }


        public static bool IsNumber(int text)
        {
            int value;
            if (Int32.TryParse(text.ToString(), out value))
                return true;
            return false;
        }
    }
}
