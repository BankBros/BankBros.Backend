using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.DataAccess;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.DataAccess.Abstract
{
    public interface IBalanceTypeDal : IEntityRepository<BalanceType>
    {
    }
}
