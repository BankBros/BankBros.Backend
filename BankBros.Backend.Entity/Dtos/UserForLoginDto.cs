using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities;

namespace BankBros.Backend.Entity.Dtos
{
    public class UserForLoginDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
    }
}
