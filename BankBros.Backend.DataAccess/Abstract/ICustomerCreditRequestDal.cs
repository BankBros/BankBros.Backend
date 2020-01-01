using BankBros.Backend.Core.DataAccess;
using BankBros.Backend.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.DataAccess.Abstract
{
    public interface ICustomerCreditRequestDal : IEntityRepository<CustomerCreditRequest>
    {

    }
}
