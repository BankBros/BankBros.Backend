using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Concrete
{
    public class CustomerCreditRequestManager : ICustomerCreditRequestService
    {
        private ICustomerCreditRequestDal _customerCreditRequestDal;
        public CustomerCreditRequestManager(ICustomerCreditRequestDal customerCreditRequestDal)
        {
            _customerCreditRequestDal = customerCreditRequestDal;
        }
        public IResult Add(params CustomerCreditRequest[] customerRequests)
        {
            try
            {
                if (_customerCreditRequestDal.Add(customerRequests))
                    return new SuccessResult(Messages.CreditRequestAddedSuccessfully);
                else
                    return new ErrorResult(Messages.CreditRequestAddingFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.CreditRequestAddingFailure);
            }
        }

        public IResult Update(params CustomerCreditRequest[] customerRequests)
        {
            try
            {
                foreach (var request in customerRequests)
                {
                    request.EntityState = EntityState.Modified;
                }
                if (_customerCreditRequestDal.Update(customerRequests))
                    return new SuccessResult(Messages.CreditRequestUpdated);

                return new ErrorResult(Messages.CreditRequestUpdateFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.CreditRequestUpdateFailure);
            }
        }

        public IResult Delete(params CustomerCreditRequest[] customerRequests)
        {
            try
            {
                foreach (var request in customerRequests)
                {
                    request.EntityState = EntityState.Deleted;
                }
                if (_customerCreditRequestDal.Update(customerRequests))
                    return new SuccessResult(Messages.CreditRequestDeleted);

                return new ErrorResult(Messages.CreditRequestDeleteFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.CreditRequestDeleteFailure);
            }
        }
    }
}
