using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("Bills")]
    public class Bill : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaymentAt { get; set; }
        public DateTime? LastDateToPay { get; set; }
        public bool Status { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }
        public virtual BillOrganization Organization { get; set; }

        [JsonIgnore, NotMapped]
        public EntityState EntityState { get; set; }
    }
}
