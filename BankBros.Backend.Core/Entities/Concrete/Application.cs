using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Core.Entities.Concrete
{
    [Table("Applications")]
    public partial class Application : IEntity
    {
        public Application()
        {
            UserLogs = new HashSet<UserLog>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserLog> UserLogs { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
