using BankBros.Backend.Core.DataAccess.EntityFramework;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.DataAccess.Concrete.EntityFramework.Contexts;
using BankBros.Backend.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.DataAccess.Concrete.EntityFramework
{
    public class EfCustomerCreditRequestDal : EfEntityRepositoryBase<CustomerCreditRequest, BankBrosContext>, ICustomerCreditRequestDal
    {
    }
}
