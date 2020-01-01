using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("CustomerCreditRequests")]
    public partial class CustomerCreditRequest : IEntity
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int CreditRequestId { get; set; }

        public DateTime Date { get; set; }
        [JsonIgnore]
        public virtual CreditRequest CreditRequest { get; set; }
        [JsonIgnore]
        public virtual Customer Customer { get; set; }
        [NotMapped]
        public EntityState EntityState { get; set; }
    }
}
