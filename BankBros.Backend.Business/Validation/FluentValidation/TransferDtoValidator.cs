using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using FluentValidation;

namespace BankBros.Backend.Business.Validation.FluentValidation
{
    public class TransferDtoValidator : AbstractValidator<TransferDto>
    {
        public TransferDtoValidator()
        {
            RuleFor(x => x.Amount)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty,"Tutar"))
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Tutar"))
                .Must(IsDecimal).WithMessage(string.Format(ValidationMessages.InvalidMust,"Tutar"))
                .GreaterThanOrEqualTo(1).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan,"Tutar","0"));

            RuleFor(x=>x.SenderAccountNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Gönderici Hesap"))
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Gönderici Hesap"))
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.InvalidMust,"Gönderici Hesap"))
                .GreaterThanOrEqualTo(1001).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Gönderici Hesap", "1000"));

            RuleFor(x => x.TargetAccountNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Alıcı Hesap"))
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Alıcı Hesap"))
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.InvalidMust, "Alıcı Hesap"))
                .GreaterThanOrEqualTo(1001).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Alıcı Hesap", "1000"));

            RuleFor(x => x.TargetCustomerId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(string.Format(ValidationMessages.NotEmpty, "Alıcı Müşteri"))
                .NotNull().WithMessage(string.Format(ValidationMessages.NotNull, "Alıcı Müşteri"))
                .Must(IsNumber).WithMessage(string.Format(ValidationMessages.InvalidMust, "Alıcı Müşteri"))
                .GreaterThanOrEqualTo(100000001).WithMessage(string.Format(ValidationMessages.MustBeGreaterThan, "Alıcı Müşteri", "100000000"));

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
