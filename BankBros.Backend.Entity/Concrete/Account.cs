using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("Accounts")]
    public partial class Account : IEntity
    {
        public Account()
        {
            Sends = new HashSet<Transaction>();
            Receives = new HashSet<Transaction>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AccountNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }
        public int BalanceTypeId { get; set; }
        public bool Status { get; set; }
        public virtual BalanceType BalanceType { get; set; }
        [JsonIgnore]
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Transaction> Sends { get; set; }
        public virtual ICollection<Transaction> Receives { get; set; }

        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
