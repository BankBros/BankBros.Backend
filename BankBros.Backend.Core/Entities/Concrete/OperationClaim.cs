using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Core.Entities.Concrete
{
    [Table("OperationClaims")]
    public partial class OperationClaim : IEntity
    {
        public OperationClaim()
        {
            UserOperationClaims = new HashSet<UserOperationClaim>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }

        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
