using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.DataAccess.EntityFramework;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.DataAccess.Concrete.EntityFramework.Contexts;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.DataAccess.Concrete.EntityFramework
{
    public class EfOperationClaimDal : EfEntityRepositoryBase<OperationClaim, BankBrosContext>, IOperationClaimDal
    {
    }
}
