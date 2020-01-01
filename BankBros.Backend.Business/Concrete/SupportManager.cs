using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Autofac;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Concrete
{
    public class SupportManager :ISupportService
    {
        private IBalanceTypeDal _balanceTypeDal;
        private ITransactionTypeDal _transactionTypeDal;
        private ITransactionResultDal _transactionResultDal;

        public SupportManager(IBalanceTypeDal balanceTypeDal, ITransactionTypeDal transactionTypeDal, ITransactionResultDal transactionResultDal)
        {
            _balanceTypeDal = balanceTypeDal;
            _transactionTypeDal = transactionTypeDal;
            _transactionResultDal = transactionResultDal;
        }
        [CacheAspect(duration:10)]
        public IDataResult<List<BalanceType>> GetBalanceTypes()
        {
            try
            {
                var balanceTypes = _balanceTypeDal.GetAll();
                return new SuccessDataResult<List<BalanceType>>(balanceTypes.ToList());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BalanceType>>(Messages.BalanceTypesNotReachable);
            }
        }

        [CacheAspect(duration: 10)]
        public IDataResult<List<TransactionType>> GetTransactionTypes()
        {
            try
            {
                var transactionTypes = _transactionTypeDal.GetAll();
                return new SuccessDataResult<List<TransactionType>>(transactionTypes.ToList());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TransactionType>>(Messages.TransactionTypesNotReachable);
            }
        }

        [CacheAspect(duration: 10)]
        public IDataResult<List<TransactionResult>> GetTransactionResults()
        {
            try
            {
                var transactionResults = _transactionResultDal.GetAll();
                return new SuccessDataResult<List<TransactionResult>>(transactionResults.ToList());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<TransactionResult>>(Messages.TransactionResultsNotReachable);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get", Priority = 1)]
        [CacheRemoveAspect("IAccountService.Get", Priority = 2)]
        [CacheRemoveAspect("ITransactionService.Get", Priority = 3)]
        public IResult ClearCachings()
        {
            return new SuccessResult("Bilgileriniz güncellendi.");
        }
    }
}
