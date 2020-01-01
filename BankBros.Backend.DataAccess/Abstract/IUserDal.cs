using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.DataAccess;
using BankBros.Backend.Core.Entities.Concrete;

namespace BankBros.Backend.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        List<OperationClaim> GetClaims(User user);
    }
}
