using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Abstract
{
    public interface IAccountService
    {
        IDataResult<Account> Get(int accountNumber, int customerNumber);
        IDataResult<Account> GetWithoutDetails(int accountNumber, int customerNumber);
        IDataResult<List<Account>> GetList();
        IDataResult<List<Account>> GetListByCustomerNumber(int customerNumber);
        IDataResult<List<Account>> GetFullListByCustomerNumber(int customerNumber);
        IResult Add(int customerNumber, params Account[] accounts);
        IResult Update(params Account[] accounts);
        IResult Delete(params Account[] accounts);
    }
}
