using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Date;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using Microsoft.EntityFrameworkCore.Internal;

namespace BankBros.Backend.Business.Concrete
{
    public class AccountManager : IAccountService
    {
        private IAccountDal _accountDal;

        public AccountManager(IAccountDal accountDal)
        {
            _accountDal = accountDal;
        }

        [CacheAspect(20)]
        public IDataResult<Account> Get(int accountNumber, int customerNumber)
        {
            try
            {
                return new SuccessDataResult<Account>
                (
                    _accountDal.GetSingle
                    (
                        x=>x.AccountNumber.Equals(accountNumber) && 
                             x.CustomerId.Equals(customerNumber) &&
                             x.Status,
                        t => t.BalanceType, y=> y.Receives, z=>z.Sends
                    )
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorDataResult<Account>(Messages.AccountNotFound);
            }
        }

        [CacheAspect(20)]
        public IDataResult<Account> GetWithoutDetails(int accountNumber, int customerNumber)
        {
            try
            {
                return new SuccessDataResult<Account>
                (
                    _accountDal.GetSingle
                    (
                        x => x.AccountNumber.Equals(accountNumber) &&
                             x.CustomerId.Equals(customerNumber)
                    )
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorDataResult<Account>(Messages.AccountNotFound);
            }
        }

        [CacheAspect(20)]
        public IDataResult<List<Account>> GetList()
        {
            try
            {
                var accounts = _accountDal.GetAll().ToList();
                return new SuccessDataResult<List<Account>>
                (
                    accounts,
                    string.Format(Messages.Listed,accounts.Count.ToString())
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorDataResult<List<Account>>(Messages.UserNotFound);
            }
        }

        [CacheAspect(20)]
        public IDataResult<List<Account>> GetListByCustomerNumber(int customerNumber)
        {
            try
            {
                var accounts = _accountDal.GetList(x=>x.CustomerId.Equals(customerNumber)).ToList();
                return new SuccessDataResult<List<Account>>
                (
                    accounts,
                    string.Format(Messages.Listed, accounts.Count.ToString())
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorDataResult<List<Account>>(Messages.UserNotFound);
            }
        }

        [CacheAspect(20)]
        public IDataResult<List<Account>> GetFullListByCustomerNumber(int customerNumber)
        {
            try
            {
                var accounts = _accountDal.GetList(x => x.CustomerId.Equals(customerNumber)).Where(x=>x.Status).ToList();
                foreach (var account in accounts)
                {
                    var result = Get(account.AccountNumber, customerNumber);
                    var last30Days = DateTime.Now.Date.Subtract(TimeSpan.FromDays(30));
                    if (result.Success)
                    {
                        account.Receives = result.Data.Receives?.Where(x=>  x.CreatedAt.Date >= last30Days).ToList();
                        account.Sends = result.Data.Sends?.Where(x => x.CreatedAt.Date >= last30Days).ToList();
                        account.BalanceType = result.Data.BalanceType;
                    }
                }
                return new SuccessDataResult<List<Account>>
                (
                    accounts,
                    string.Format(Messages.Listed, accounts.Count.ToString())
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorDataResult<List<Account>>(Messages.UserNotFound);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        [CacheRemoveAspect("IAccountService.Get")]
        [CacheRemoveAspect("ITransactionService.Get")]
        public IResult Add(int customerNumber, params Account[] accounts)
        {
            try
            {
                var result = GetListByCustomerNumber(customerNumber);
                if (result.Success)
                {
                    foreach (var account in accounts)
                    {
                        account.AccountNumber = 1000 + result.Data.Count + 1 + accounts.IndexOf(account);
                        account.Balance = 0;
                        account.BalanceTypeId = 1;
                        account.Status = true;
                        account.CustomerId = customerNumber;
                        account.CreatedAt = DateHelper.Now();
                    }
                    if(_accountDal.Add(accounts))
                        return new SuccessResult(Messages.AccountAddedSuccessfully);
                    else
                        return new ErrorResult(Messages.AccountAddingFailure);
                }
                return new ErrorResult(Messages.UserNotFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.AccountAddingFailure);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        [CacheRemoveAspect("IAccountService.Get")]
        [CacheRemoveAspect("ITransactionService.Get")]
        public IResult Update(params Account[] accounts)
        {
            try
            {
                foreach (var account in accounts)
                { 
                    account.EntityState = EntityState.Modified;
                }
                if (_accountDal.Update(accounts))
                    return new SuccessResult(Messages.AccountUpdated);

                return new ErrorResult(Messages.AccountUpdateFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
                return new ErrorResult(Messages.AccountUpdateFailure);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get")]
        [CacheRemoveAspect("IAccountService.Get")]
        [CacheRemoveAspect("ITransactionService.Get")]
        public IResult Delete(params Account[] accounts)
        {
            try
            {
                List<Account> updatedAccounts = new List<Account>();
                foreach (var account in accounts)
                {
                    var accountWithoutDetailResult = GetWithoutDetails(account.AccountNumber, account.CustomerId);
                    if(accountWithoutDetailResult.Success && accountWithoutDetailResult.Data != null)
                        updatedAccounts.Add(accountWithoutDetailResult.Data);
                }

                foreach (var updatedAccount in updatedAccounts)
                {
                    updatedAccount.Status = false;
                    updatedAccount.EntityState = EntityState.Modified;
                    if (updatedAccount.Balance != 0)
                        return new ErrorResult(Messages.AccountBalanceIsNotEqualsZero);
                }
                if (updatedAccounts.Count == 0 || _accountDal.Update(updatedAccounts.ToArray()))
                    return new SuccessResult(Messages.AccountDeleted);

                return new ErrorResult(Messages.AccountDeletedFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.AccountDeletedFailure);
            }
        }
    }
}
