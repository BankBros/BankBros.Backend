using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Abstract
{
    public interface ISupportService
    {
        IDataResult<List<BalanceType>> GetBalanceTypes();
        IDataResult<List<TransactionType>> GetTransactionTypes();
        IDataResult<List<TransactionResult>> GetTransactionResults();
        IResult ClearCachings();
    }
}
