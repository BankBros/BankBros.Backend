using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Abstract
{
    public interface ICustomerService
    {
        IDataResult<Customer> Get(int customerId);
        IDataResult<Customer> GetWithFullAccounts(int customerId);
        IDataResult<Customer> GetByUserId(int userId, bool isFull = true);
        IResult Add(params Customer[] customers);
        IResult Update(params Customer[] customers);
        IResult Delete(params Customer[] customers);
    }
}
