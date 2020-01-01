using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("BalanceTypes")]
    public partial class BalanceType : IEntity
    {
        public BalanceType()
        {
            Accounts = new HashSet<Account>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        [Column(TypeName = "money")]
        public decimal Currency { get; set; }
        public string Symbol { get; set; }
        [JsonIgnore]
        public virtual ICollection<Account> Accounts { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
