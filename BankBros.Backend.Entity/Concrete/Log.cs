using BankBros.Backend.Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankBros.Backend.Entity.Concrete
{
    public class Log : IEntity
    {
        public int Id { get; set; }
        public string Detail { get; set; }
        public string Audit { get; set; }
        public DateTime Date { get; set; }

        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
