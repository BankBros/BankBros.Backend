using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities.Concrete;

namespace BankBros.Backend.Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);

        void Add(User user);

        User GetByUsername(string username);
    }
}
