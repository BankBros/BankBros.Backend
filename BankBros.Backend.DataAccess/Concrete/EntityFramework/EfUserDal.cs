using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankBros.Backend.Core.DataAccess;
using BankBros.Backend.Core.DataAccess.EntityFramework;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore.Internal;

namespace BankBros.Backend.DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, BankBrosContext>, IUserDal
    {
        public List<OperationClaim> GetClaims(User user)
        {
            using (var context = new BankBrosContext())
            {
                var result = from operationClaim in context.OperationClaims
                    join userOperationClaim in context.UserOperationClaims
                        on operationClaim.Id equals userOperationClaim.OperationClaimId
                    where userOperationClaim.UserId == user.Id
                    select new OperationClaim {Id = operationClaim.Id, Name = operationClaim.Name};
                return result.ToList();
            }
        }
    }
}
