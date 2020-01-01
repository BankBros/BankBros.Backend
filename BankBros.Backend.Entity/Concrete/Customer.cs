using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Entities.Concrete;
using Newtonsoft.Json;

namespace BankBros.Backend.Entity.Concrete
{
    [Table("Customers")]
    public partial class Customer : IEntity
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            CustomerCreditRequests = new HashSet<CustomerCreditRequest>();
        }
        public int Id { get; set; }
        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreditScore { get; set; }
        public int UserId { get; set; }
        public int DetailsId { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual CustomerDetail CustomerDetails { get; set; }
        public virtual ICollection<CustomerCreditRequest> CustomerCreditRequests { get; set; }
        public virtual User User { get; set; }
        [NotMapped, JsonIgnore]
        public EntityState EntityState { get; set; }
    }
}
