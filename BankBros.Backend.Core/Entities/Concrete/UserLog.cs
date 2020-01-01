using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Core.Entities.Concrete
{
    [Table("UserLogs")]
    public partial class UserLog : IEntity
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public int AppId { get; set; }
        public int UserId { get; set; }
        public DateTime? LogOutDate { get; set; }
        [JsonIgnore]
        public virtual Application Application { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
