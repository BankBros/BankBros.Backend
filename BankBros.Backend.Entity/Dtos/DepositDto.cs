using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BankBros.Backend.Core.Entities;

namespace BankBros.Backend.Entity.Dtos
{
    public class DepositDto : IDto
    {
        [Column(TypeName="money")]
        public decimal Amount { get; set; }
        public int AccountNumber { get; set; }
    }
}
