using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Business.Validation.FluentValidation;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Aspects.Autofac.Validation;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Business;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankBros.Backend.Business.Concrete
{
    public class CreditManager : ICreditService
    {
        private ICreditRequestDal _creditRequestDal;
        private ICustomerCreditRequestService _customerCreditRequestService;
        private ICustomerService _customerService;

        public CreditManager(ICreditRequestDal creditRequestDal, ICustomerCreditRequestService customerCreditRequestService, ICustomerService customerService)
        {
            _creditRequestDal = creditRequestDal;
            _customerService = customerService;
            _customerCreditRequestService = customerCreditRequestService;
        }

        [CacheAspect(10)]
        public IDataResult<List<CreditRequest>> GetListByCustomerNumber(int customerNumber)
        {
            try
            {
                var result = _customerService.GetByUserId(customerNumber);
                if (!result.Success)
                    return new ErrorDataResult<List<CreditRequest>>(result.Message);

                var creditRequests = _creditRequestDal.GetAll(x => x.CustomerCreditRequests).Where(x => x.CustomerCreditRequests.Any(y => y.CustomerId.Equals(result.Data.Id))).ToList();
                return new SuccessDataResult<List<CreditRequest>>(creditRequests);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<CreditRequest>>(ex.Message);
            }
        }

        [CacheAspect(10)]
        public IDataResult<CreditRequest> GetByFullDetails(CreditRequestDto creditRequestDto)
        {
            try
            {
                var creditRequest = _creditRequestDal.GetSingle(x =>
                x.Age.Equals(creditRequestDto.Age) &&
                x.Amount.Equals(creditRequestDto.Amount) &&
                x.UsedCredits == creditRequestDto.UsedCredits &&
                x.HasHouse == creditRequestDto.HasHouse &&
                x.HasPhone == creditRequestDto.HasPhone
                );
                if(creditRequestDto != null)
                    return new SuccessDataResult<CreditRequest>(creditRequest);
                else 
                    return new ErrorDataResult<CreditRequest>(creditRequest);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CreditRequest>(ex.Message);
            }
        }

        [ValidationAspect(typeof(CreditRequestDtoValidator), Priority = 1)]
        [CacheRemoveAspect("ICreditService.Get", Priority = 2)]
        [CacheRemoveAspect("ICustomerCreditRequestService.Get", Priority = 3)]
        [CacheRemoveAspect("ICustomerService.Get", Priority = 4)]
        public IResult Add(int customerNumber, params CreditRequestDto[] requestDtos)
        {
            try
            {
                var result = _customerService.GetByUserId(customerNumber, false);
                var rules = BusinessRules.Run(result);
                if (rules != null)
                    return rules;

                List<CreditRequest> requests = new List<CreditRequest>();
                foreach (var request in requestDtos)
                {
                    requests.Add(new CreditRequest
                    {
                        Age = Convert.ToInt32(request.Age),
                        Amount = Convert.ToDecimal(request.Amount),
                        UsedCredits = request.UsedCredits,
                        HasHouse = request.HasHouse,
                        HasPhone = request.HasPhone,
                        Result = request.Result,
                        CustomerCreditRequests = new List<CustomerCreditRequest>()
                    });


                    requests.Last().CustomerCreditRequests.Add(new CustomerCreditRequest
                    {
                        CustomerId = result.Data.Id,
                        CreditRequest = requests.Last(),
                        Date = DateTime.Now
                    });
                }
                if (_creditRequestDal.Add(requests.ToArray()))
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

        [CacheRemoveAspect("ICreditService.Get")]
        [CacheRemoveAspect("ICustomerService.Get")]
        [CacheRemoveAspect("ICustomerCreditRequestService.Get")]
        public IResult Update(params CreditRequest[] requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    request.EntityState = EntityState.Modified;
                }
                if (_creditRequestDal.Update(requests))
                    return new SuccessResult(Messages.CreditRequestUpdated);

                return new ErrorResult(Messages.CreditRequestUpdateFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.CreditRequestUpdateFailure);
            }
        }

        [CacheRemoveAspect("ICreditService.Get")]
        [CacheRemoveAspect("ICustomerService.Get")]
        [CacheRemoveAspect("ICustomerCreditRequestService.Get")]
        public IResult Delete(params CreditRequest[] requests)
        {
            try
            {
                foreach (var request in requests)
                {
                    request.EntityState = EntityState.Deleted;
                }
                if (_creditRequestDal.Update(requests))
                    return new SuccessResult(Messages.CreditRequestDeleted);

                return new ErrorResult(Messages.CreditRequestDeleteFailure);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ErrorResult(Messages.CreditRequestDeleteFailure);
            }
        }

        [CacheAspect(10)]
        public IResult IsExists(CreditRequestDto creditRequestDto)
        {
            var result = GetByFullDetails(creditRequestDto);
            if (result.Success)
                return new SuccessResult(Messages.CreditRequestAlreadyExists);
            return new ErrorResult(Messages.CreditRequestNotExists);
        }

        [ValidationAspect(typeof(CreditRequestDtoValidator))]
        public IResult Check(CreditRequestDto creditRequestDto)
        {
            var result = GetByFullDetails(creditRequestDto);
            if (result.Success)
                return new SuccessResult();
            return new ErrorResult(Messages.CreditRequestNotExists);
        }
    }
}
