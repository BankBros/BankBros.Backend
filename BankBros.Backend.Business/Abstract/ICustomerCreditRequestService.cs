using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Abstract
{
    public interface ICustomerCreditRequestService
    {
        IResult Add(params CustomerCreditRequest[] customerRequests);
        IResult Update(params CustomerCreditRequest[] customerRequests);
        IResult Delete(params CustomerCreditRequest[] customerRequests);
    }
}
