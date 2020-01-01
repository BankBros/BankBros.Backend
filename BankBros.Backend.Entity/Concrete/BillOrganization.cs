using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("BillOrganizations")]
    public class BillOrganization : IEntity
    {
        public BillOrganization()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "money")]
        public decimal Charge { get; set; }

        [JsonIgnore] 
        public virtual ICollection<Bill> Bills { get; set; }

        [NotMapped, JsonIgnore] 
        public EntityState EntityState { get; set; }
    }
}
