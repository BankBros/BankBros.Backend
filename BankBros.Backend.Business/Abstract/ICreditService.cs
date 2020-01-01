using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Abstract
{
    public interface ICreditService
    {
        IDataResult<List<CreditRequest>> GetListByCustomerNumber(int customerNumber);
        IDataResult<CreditRequest> GetByFullDetails(CreditRequestDto creditRequestDto);
        IResult Check(CreditRequestDto creditRequestDto);
        IResult Add(int customerNumber, params CreditRequestDto[] requests);
        IResult Update(params CreditRequest[] requests);
        IResult Delete(params CreditRequest[] requests);
    }
}
