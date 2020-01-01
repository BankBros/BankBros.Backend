using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("Transactions")]
    public partial class Transaction : IEntity
    {
        public int Id { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal ActualAmount { get; set; }
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TransactionResultId { get; set; }
        public int TransactionTypeId { get; set; }
        [JsonIgnore]
        public virtual Account Sender { get; set; }
        [JsonIgnore]
        public virtual Account Receiver { get; set; }
        public virtual TransactionResult TransactionResult { get; set; }
        public virtual TransactionType TransactionType { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
