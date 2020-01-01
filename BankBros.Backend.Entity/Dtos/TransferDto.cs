using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BankBros.Backend.Core.Entities;

namespace BankBros.Backend.Entity.Dtos
{
    public class TransferDto : IDto
    {
        public int SenderAccountNumber;
        public int TargetCustomerId;
        public int TargetAccountNumber;
        [Column(TypeName = "money")]
        public decimal Amount;
    }
}
