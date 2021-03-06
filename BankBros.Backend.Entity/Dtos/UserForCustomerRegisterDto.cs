﻿using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities;

namespace BankBros.Backend.Entity.Dtos
{
    public class UserForCustomerRegisterDto : IDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }
        public string TCKN { get; set; }
        public string PhoneNumber { get; set; }
    }
}
