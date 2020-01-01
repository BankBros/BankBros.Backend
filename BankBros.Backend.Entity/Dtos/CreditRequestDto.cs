using BankBros.Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Entity.Dtos
{
    public class CreditRequestDto : IDto
    {
        public decimal Amount { get; set; }

        public int Age { get; set; }

        public bool HasHouse { get; set; }

        public int UsedCredits { get; set; }

        public bool HasPhone { get; set; }

        public bool Result { get; set; }
    }
}
