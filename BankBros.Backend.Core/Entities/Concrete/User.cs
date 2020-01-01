using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Core.Entities.Concrete
{
    [Table("Users")]
    public partial class User : IEntity
    {
        public User()
        {
            UserOperationClaims = new HashSet<UserOperationClaim>();
            UserLogs = new HashSet<UserLog>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public bool Status { get; set; }
        public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
        public virtual ICollection<UserLog> UserLogs { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
