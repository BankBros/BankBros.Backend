using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankBros.Backend.CoreWebAPI.Models
{
    public class CreditRequestFlaskDto
    {
        public decimal Amount { get; set; }

        public int Age { get; set; }

        public int HasHouse { get; set; }

        public int UsedCredits { get; set; }

        public int HasPhone { get; set; }
    }
}
