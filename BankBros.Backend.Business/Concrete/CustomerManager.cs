using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Aspects.Autofac.Logging;
using BankBros.Backend.Core.Aspects.Autofac.Performance;
using BankBros.Backend.Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;
        private IAccountService _accountService;
        public CustomerManager(ICustomerDal customerDal, IAccountService accountService)
        {
            _customerDal = customerDal;
            _accountService = accountService;
        }
        
        [CacheAspect(20)]
        public IDataResult<Customer> Get(int customerId)
        {
            var customer = _customerDal.GetSingle(x => x.Id.Equals(customerId),  z => z.User, t => t.CustomerDetails);

            if (customer == null)
                return new ErrorDataResult<Customer>(Messages.UserNotFound);

            var result = _accountService.GetFullListByCustomerNumber(customer.Id);
            if (result.Success)
                customer.Accounts = result.Data;

            return new SuccessDataResult<Customer>(customer);
        }

        [CacheAspect(20)]
        public IDataResult<Customer> GetWithFullAccounts(int customerId)
        {
            var customer = _customerDal.GetSingle(x => x.Id.Equals(customerId),  z => z.User, t => t.CustomerDetails);
            if (customer != null)
                return new SuccessDataResult<Customer>(customer);

            var result = _accountService.GetFullListByCustomerNumber(customer.Id);
            if (result.Success)
                customer.Accounts = result.Data;
            return new ErrorDataResult<Customer>();
        }

        [LogAspect(typeof(FileLogger))]
        [CacheAspect(20)]
        public IDataResult<Customer> GetByUserId(int userId, bool isFull = true)
        {
            var customer = _customerDal.GetSingle(x => x.UserId.Equals(userId), z => z.User, t => t.CustomerDetails);

            if (customer == null)
                return new ErrorDataResult<Customer>(Messages.UserNotFound);

            var result = _accountService.GetFullListByCustomerNumber(customer.Id);
            if (result.Success)
                customer.Accounts = result.Data;

            return new SuccessDataResult<Customer>(customer);
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Add(params Customer[] customers)
        {
            if (_customerDal.Add(customers))
                return new SuccessResult();
            return new ErrorResult();
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Update(params Customer[] customers)
        {
            if (_customerDal.Update(customers))
                return new SuccessResult();
            return new ErrorResult();
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        public IResult Delete(params Customer[] customers)
        {
            if (_customerDal.Remove(customers))
                return new SuccessResult();
            return new ErrorResult();
        }
    }
}
