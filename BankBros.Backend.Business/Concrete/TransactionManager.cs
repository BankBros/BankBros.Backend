using System;
using System.Linq;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Business.Validation.FluentValidation;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Aspects.Autofac.Transaction;
using BankBros.Backend.Core.Aspects.Autofac.Validation;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Date;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;

namespace BankBros.Backend.Business.Concrete
{
    public class TransactionManager : ITransactionService
    {
        private ICustomerService _customerService;
        private IAccountService _accountService;
        private ITransactionDal _transactionDal;

        public TransactionManager(ICustomerService customerService, IAccountService accountService, ITransactionDal transactionDal)
        {
            _customerService = customerService;
            _accountService = accountService;
            _transactionDal = transactionDal;
        }

        [ValidationAspect(typeof(TransferDtoValidator),Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult Transfer(int userId, TransferDto transferDto)
        {
            var senderCustomerResult = _customerService.GetByUserId(userId);
            var receiverCustomerResult = _customerService.Get(transferDto.TargetCustomerId);
            if (!senderCustomerResult.Success)
            {
                return new ErrorResult("Gönderici " + senderCustomerResult.Message);
            }
            if (!receiverCustomerResult.Success)
            {
                return new ErrorResult("Alıcı " + receiverCustomerResult.Message);
            }

            var senderAccount =
                senderCustomerResult.Data.Accounts.FirstOrDefault(x =>
                    x.AccountNumber.Equals(transferDto.SenderAccountNumber));

            if (senderAccount == null)
                return new ErrorResult("Gönderici " + Messages.AccountNotFound);

            if (!senderAccount.Status)
                return new ErrorResult("Gönderici " + Messages.AccountIsPassive);

            var receiverAccount =
                receiverCustomerResult.Data.Accounts.FirstOrDefault(x =>
                    x.AccountNumber.Equals(transferDto.TargetAccountNumber));

            if (receiverAccount == null)
                return new ErrorResult("Alıcı " + Messages.AccountNotFound);

            if (!receiverAccount.Status)
                return new ErrorResult("Alıcı " + Messages.AccountIsPassive);

            if (senderAccount.Balance < transferDto.Amount)
                return new ErrorResult(Messages.UnsufficientBalance);

            var calculatedAmount = transferDto.Amount;
            var transactionType = 1;
            IDataResult<Account> senderAccountResult, receiverAccountResult;
            if (senderAccount.BalanceTypeId != receiverAccount.BalanceTypeId)
            {
                senderAccountResult = _accountService.Get(senderAccount.AccountNumber, senderCustomerResult.Data.Id);

                receiverAccountResult = _accountService.Get(receiverAccount.AccountNumber, transferDto.TargetCustomerId);

                if (!senderAccountResult.Success)
                {
                    return new ErrorResult(senderAccountResult.Message);
                }

                if (!receiverAccountResult.Success)
                {
                    return new ErrorResult(receiverAccountResult.Message);
                }

                calculatedAmount *= senderAccountResult.Data.BalanceType.Currency;
                calculatedAmount /= receiverAccountResult.Data.BalanceType.Currency;

                transactionType = 2;

            }
            else
            {
                senderAccountResult = _accountService.GetWithoutDetails(senderAccount.AccountNumber, senderCustomerResult.Data.Id);

                receiverAccountResult = _accountService.GetWithoutDetails(receiverAccount.AccountNumber, transferDto.TargetCustomerId);

                if (!senderAccountResult.Success)
                {
                    return new ErrorResult(senderAccountResult.Message);
                }

                if (!receiverAccountResult.Success)
                {
                    return new ErrorResult(receiverAccountResult.Message);
                }
            }


            senderAccountResult.Data.Balance -= transferDto.Amount;
            senderAccountResult.Data.EntityState = EntityState.Modified;

            receiverAccountResult.Data.Balance += calculatedAmount;
            receiverAccountResult.Data.EntityState = EntityState.Modified;

            var transaction = new Transaction()
            {
                CreatedAt = DateTime.Now,
                SenderAccountId = senderAccount.Id,
                ReceiverAccountId = receiverAccount.Id,
                Amount = transferDto.Amount,
                ActualAmount = calculatedAmount,
                TransactionResultId = 1,
                TransactionTypeId = transactionType
            };

            // Add transaction
            Add(transaction);

            // Update Sender Account Data
            _accountService.Update(senderAccountResult.Data);
            // Update Receiver Account Data
            _accountService.Update(receiverAccountResult.Data);

            // Check transaction state
            var transactionResult = Get(transaction.CreatedAt, transaction.SenderAccountId,
                transaction.ReceiverAccountId);

            if (!transactionResult.Success)
                return new ErrorResult(transactionResult.Message);

            transaction = transactionResult.Data;
            transaction.TransactionResultId = 2;
            transaction.EntityState = EntityState.Modified;


            var senderAccountBalance = _accountService.GetWithoutDetails(senderAccount.AccountNumber, senderAccount.CustomerId).Data.Balance;

            var receiverAccountBalance = _accountService.GetWithoutDetails(receiverAccount.AccountNumber, receiverAccount.CustomerId).Data.Balance;

            if (senderAccountBalance != senderAccount.Balance - transferDto.Amount || receiverAccountBalance != receiverAccount.Balance + calculatedAmount)
                return new ErrorResult(Messages.TransferFailed);

            // Update transaction result
            Update(transaction);

            return new SuccessResult(Messages.TransferSuccessful);

        }

        [ValidationAspect(typeof(VirementDtoValidator), Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult Virement(int userId,VirementDto virementDto)
        {
            var senderCustomerResult = _customerService.GetByUserId(userId);
            if (!senderCustomerResult.Success)
            {
                return new ErrorResult(senderCustomerResult.Message);
            }

            var senderAccount =
                senderCustomerResult.Data.Accounts.FirstOrDefault(x =>
                    x.AccountNumber.Equals(virementDto.SenderAccountNumber));

            if (senderAccount == null)
                return new ErrorResult("Gönderici " + Messages.AccountNotFound);

            if (!senderAccount.Status)
                return new ErrorResult("Gönderici " + Messages.AccountIsPassive);

            var receiverAccount =
                senderCustomerResult.Data.Accounts.FirstOrDefault(x =>
                    x.AccountNumber.Equals(virementDto.TargetAccountNumber));

            if (receiverAccount == null)
                return new ErrorResult("Alıcı " + Messages.AccountNotFound);

            if (!receiverAccount.Status)
                return new ErrorResult("Alıcı " + Messages.AccountIsPassive);

            if (senderAccount.Balance < virementDto.Amount)
                return new ErrorResult(Messages.UnsufficientBalance);

            var calculatedAmount = virementDto.Amount;
            var transactionType = 3;
            IDataResult<Account> senderAccountResult, receiverAccountResult;
            if (senderAccount.BalanceTypeId != receiverAccount.BalanceTypeId)
            {
                senderAccountResult = _accountService.Get(senderAccount.AccountNumber, senderCustomerResult.Data.Id);

                receiverAccountResult = _accountService.Get(receiverAccount.AccountNumber, senderCustomerResult.Data.Id);

                if (!senderAccountResult.Success)
                {
                    return new ErrorResult(senderAccountResult.Message);
                }

                if (!receiverAccountResult.Success)
                {
                    return new ErrorResult(receiverAccountResult.Message);
                }

                calculatedAmount *= senderAccountResult.Data.BalanceType.Currency;
                calculatedAmount /= receiverAccountResult.Data.BalanceType.Currency;

                transactionType = 4;

            }
            else
            {
                senderAccountResult = _accountService.GetWithoutDetails(senderAccount.AccountNumber, senderCustomerResult.Data.Id);

                receiverAccountResult = _accountService.GetWithoutDetails(receiverAccount.AccountNumber, senderCustomerResult.Data.Id);

                if (!senderAccountResult.Success)
                {
                    return new ErrorResult(senderAccountResult.Message);
                }

                if (!receiverAccountResult.Success)
                {
                    return new ErrorResult(receiverAccountResult.Message);
                }
            }


            senderAccountResult.Data.Balance -= virementDto.Amount;
            senderAccountResult.Data.EntityState = EntityState.Modified;

            receiverAccountResult.Data.Balance += calculatedAmount;
            receiverAccountResult.Data.EntityState = EntityState.Modified;

            var transaction = new Transaction()
            {
                CreatedAt = DateTime.Now,
                SenderAccountId = senderAccount.Id,
                ReceiverAccountId = receiverAccount.Id,
                Amount = virementDto.Amount,
                ActualAmount = calculatedAmount,
                TransactionResultId = 1,
                TransactionTypeId = transactionType
            };

            // Add transaction
            Add(transaction);

            // Update Sender Account Data
            _accountService.Update(senderAccountResult.Data);
            // Update Receiver Account Data
            _accountService.Update(receiverAccountResult.Data);

            // Check transaction state
            var transactionResult = Get(transaction.CreatedAt, transaction.SenderAccountId,
                transaction.ReceiverAccountId);

            if (!transactionResult.Success)
                return new ErrorResult(transactionResult.Message);

            transaction = transactionResult.Data;
            transaction.TransactionResultId = 2;
            transaction.EntityState = EntityState.Modified;


            var senderAccountBalance = _accountService.GetWithoutDetails(senderAccount.AccountNumber, senderAccount.CustomerId).Data.Balance;

            var receiverAccountBalance = _accountService.GetWithoutDetails(receiverAccount.AccountNumber, receiverAccount.CustomerId).Data.Balance;

            if (senderAccountBalance != senderAccount.Balance - virementDto.Amount || receiverAccountBalance != receiverAccount.Balance + calculatedAmount)
                return new ErrorResult(Messages.VirementFailed);

            // Update transaction result
            Update(transaction);

            return new SuccessResult(Messages.VirementSuccessful);
        }

        [ValidationAspect(typeof(DepositDtoValidator), Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult Deposit(int userId, DepositDto depositDto)
        {
            var customerResult = _customerService.GetByUserId(userId);
            if (!customerResult.Success)
                return new ErrorResult(customerResult.Message);
            if (customerResult.Data.Accounts.Any(x => x.AccountNumber.Equals(depositDto.AccountNumber)))
            {
                var accountDetails = _accountService.GetWithoutDetails(depositDto.AccountNumber, customerResult.Data.Id);
                if (!accountDetails.Success)
                    return new ErrorResult(accountDetails.Message);
                if (!accountDetails.Data.Status)
                    return new ErrorResult(Messages.AccountIsPassive);

                accountDetails.Data.Balance += depositDto.Amount;

                var transaction = new Transaction()
                {
                    CreatedAt = DateTime.Now,
                    SenderAccountId = accountDetails.Data.Id,
                    ReceiverAccountId = accountDetails.Data.Id,
                    Amount = depositDto.Amount,
                    ActualAmount = depositDto.Amount,
                    TransactionResultId = 1,
                    TransactionTypeId = 6
                };

                Add(transaction);
                _accountService.Update(accountDetails.Data);

                // Check transaction state
                var transactionResult = Get(transaction.CreatedAt, transaction.SenderAccountId,
                    transaction.ReceiverAccountId);

                if (!transactionResult.Success)
                    return new ErrorResult(transactionResult.Message);

                transaction = transactionResult.Data;
                transaction.TransactionResultId = 2;
                transaction.EntityState = EntityState.Modified;

                Update(transaction);

                return new SuccessResult(Messages.DepositSuccessful);
            }

            return new ErrorResult(Messages.AccountNotFound);
        }

        [ValidationAspect(typeof(PayBillDtoValidator), Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult PayBill(int customerId, PayBillDto payBillDto)
        {
            var customerResult = _customerService.Get(customerId);
            if (!customerResult.Success)
                return new ErrorResult(customerResult.Message);
            if (customerResult.Data.Accounts.Any(x => x.AccountNumber.Equals(payBillDto.AccountNumber)))
            {
                var accountDetails = _accountService.GetWithoutDetails(payBillDto.AccountNumber, customerResult.Data.Id);
                if (!accountDetails.Success)
                    return new ErrorResult(accountDetails.Message);

                if (!accountDetails.Data.Status)
                    return new ErrorResult(Messages.AccountIsPassive);

                if (accountDetails.Data.Balance < payBillDto.Amount)
                {
                    return new ErrorResult(Messages.UnsufficientBalance);
                }

                accountDetails.Data.Balance -= payBillDto.Amount;

                var transaction = new Transaction()

                {
                    CreatedAt = DateTime.Now,
                    SenderAccountId = accountDetails.Data.Id,
                    ReceiverAccountId = accountDetails.Data.Id,
                    Amount = payBillDto.Amount,
                    ActualAmount = payBillDto.Amount,
                    TransactionResultId = 1,
                    TransactionTypeId = 7
                };

                Add(transaction);
                _accountService.Update(accountDetails.Data);

                // Check transaction state
                var transactionResult = Get(transaction.CreatedAt, transaction.SenderAccountId,
                    transaction.ReceiverAccountId);

                if (!transactionResult.Success)
                    return new ErrorResult(transactionResult.Message);

                transaction = transactionResult.Data;
                transaction.TransactionResultId = 2;
                transaction.EntityState = EntityState.Modified;

                Update(transaction);

                return new SuccessResult(Messages.DrawSuccessful);
            }

            return new ErrorResult(Messages.AccountNotFound);
        }

        [ValidationAspect(typeof(DrawDtoValidator), Priority = 1)]
        [TransactionScopeAspect(Priority = 2)]
        public IResult Draw(int userId, DrawDto drawDto)
        {
            var customerResult = _customerService.GetByUserId(userId);
            if (!customerResult.Success)
                return new ErrorResult(customerResult.Message);
            if (customerResult.Data.Accounts.Any(x => x.AccountNumber.Equals(drawDto.AccountNumber)))
            {
                var accountDetails = _accountService.GetWithoutDetails(drawDto.AccountNumber, customerResult.Data.Id);
                if (!accountDetails.Success)
                    return new ErrorResult(accountDetails.Message);

                if (!accountDetails.Data.Status)
                    return new ErrorResult(Messages.AccountIsPassive);

                if (accountDetails.Data.Balance < drawDto.Amount)
                {
                    return new ErrorResult(Messages.UnsufficientBalance);
                }

                accountDetails.Data.Balance -= drawDto.Amount;

                var transaction = new Transaction()

                {
                    CreatedAt = DateTime.Now,
                    SenderAccountId = accountDetails.Data.Id,
                    ReceiverAccountId = accountDetails.Data.Id,
                    Amount = drawDto.Amount,
                    ActualAmount = drawDto.Amount,
                    TransactionResultId = 1,
                    TransactionTypeId = 5
                };

                Add(transaction);
                _accountService.Update(accountDetails.Data);

                // Check transaction state
                var transactionResult = Get(transaction.CreatedAt, transaction.SenderAccountId,
                    transaction.ReceiverAccountId);

                if (!transactionResult.Success)
                    return new ErrorResult(transactionResult.Message);

                transaction = transactionResult.Data;
                transaction.TransactionResultId = 2;
                transaction.EntityState = EntityState.Modified;

                Update(transaction);

                return new SuccessResult(Messages.DrawSuccessful);
            }

            return new ErrorResult(Messages.AccountNotFound);

        }
        [CacheAspect(duration:20)]
        private IDataResult<Transaction> Get(DateTime createdAt, int senderAccountId, int receiverAccountId)
        {
            try
            {
                var transaction = _transactionDal.GetSingle(x =>
                    x.SenderAccountId.Equals(senderAccountId) && x.ReceiverAccountId.Equals(receiverAccountId) &&
                    x.CreatedAt.ToString().Equals(createdAt.ToString()));
                if (transaction != null)
                    return new SuccessDataResult<Transaction>(transaction);
                return new ErrorDataResult<Transaction>(Messages.TransactionNotFound);
            }
            catch (Exception ex)
            {
                throw new Exception(Messages.TransactionNotFound);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get",Priority = 1)]
        [CacheRemoveAspect("IAccountService.Get", Priority = 2)]
        [CacheRemoveAspect("ITransactionService.Get", Priority = 3)]
        private IResult Add(params Transaction[] transactions)
        {
            if (_transactionDal.Add(transactions))
                return new SuccessResult();
            return new ErrorResult();
        }

        [CacheRemoveAspect("ICustomerService.Get", Priority = 1)]
        [CacheRemoveAspect("IAccountService.Get", Priority = 2)]
        [CacheRemoveAspect("ITransactionService.Get", Priority = 3)]
        private IResult Update(params Transaction[] transactions)
        {
            if (_transactionDal.Update(transactions))
                return new SuccessResult();
            return new ErrorResult();
        }
    }
}
