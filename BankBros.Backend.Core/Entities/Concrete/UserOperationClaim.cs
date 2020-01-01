using System.ComponentModel.DataAnnotations.Schema;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Core.Entities.Concrete
{
    [Table("UserOperationClaims")]
    public partial class UserOperationClaim : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public virtual OperationClaim OperationClaim { get; set; }
        public virtual User Users { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
