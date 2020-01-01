using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("CreditRequests")]
    public class CreditRequest : IEntity
    {
        public CreditRequest()
        {
            CustomerCreditRequests = new HashSet<CustomerCreditRequest>();
        }

        public int Id { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public int Age { get; set; }

        public int UsedCredits { get; set; }

        public bool HasHouse { get; set; }

        public bool HasPhone { get; set; }

        public bool Result { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomerCreditRequest> CustomerCreditRequests { get; set; }
        [JsonIgnore, NotMapped]
        public EntityState EntityState { get; set; }
    }
}
