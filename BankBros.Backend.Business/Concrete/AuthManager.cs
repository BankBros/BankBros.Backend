using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Autofac;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Business.Validation.FluentValidation;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Aspects.Autofac.Validation;
using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Core.Utilities.Security.Hashing;
using BankBros.Backend.Core.Utilities.Security.Jwt;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;

namespace BankBros.Backend.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private ICustomerService _customerService;
        private IUserLogService _userLogService;
        private IApplicationService _appService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICustomerService customerService, IUserLogService userLogService, IApplicationService appService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _customerService = customerService;
            _userLogService = userLogService;
            _appService = appService;
        }

        [ValidationAspect(typeof(UserForCustomerRegisterDtoValidator))]
        public IDataResult<Customer> CustomerRegister(UserForCustomerRegisterDto userForCustomerRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var customer = new Customer()
            {
                CreatedAt = DateTime.Now,
                CreditScore = 0,
                SecretAnswer = userForCustomerRegisterDto.SecretAnswer,
                SecretQuestion = userForCustomerRegisterDto.SecretQuestion,
                CustomerDetails = new CustomerDetail()
                {
                    Address = userForCustomerRegisterDto.Address,
                    TCKN = userForCustomerRegisterDto.TCKN,
                    Email = userForCustomerRegisterDto.Email,
                    FirstName = userForCustomerRegisterDto.FirstName,
                    LastName = userForCustomerRegisterDto.LastName,
                    PhoneNumber = userForCustomerRegisterDto.PhoneNumber
                },
                User = new User
                {
                    Username = userForCustomerRegisterDto.TCKN,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Status = true
                }
            };

            var result = _customerService.Add(customer);
            if (result.Success)
                return new SuccessDataResult<Customer>(customer, Messages.RegisterSucessfull);

            return new ErrorDataResult<Customer>(Messages.RegisterFailed);
        }


        [ValidationAspect(typeof(UserForLoginDtoValidator), Priority = 1)]
        public IDataResult<Customer> CustomerLogin(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByUsername(userForLoginDto.Username);
            if (userToCheck == null)
            {
                return new ErrorDataResult<Customer>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash,
                userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<Customer>(Messages.PasswordError);
            }

            if (!userToCheck.Status)
            {
                return new ErrorDataResult<Customer>(Messages.UserBanned);
            }

            var result = _customerService.GetByUserId(userToCheck.Id);
            if (result.Success)
            {
                if (!String.IsNullOrEmpty(userForLoginDto.ApplicationId))
                {
                    Login(result.Data.UserId, Convert.ToInt32(userForLoginDto.ApplicationId));
                }
                else if (!String.IsNullOrEmpty(userForLoginDto.ApplicationName))
                {
                    var appResult = _appService.GetApplicationIdByName(userForLoginDto.ApplicationName);
                    if(appResult.Success)
                        Login(result.Data.UserId, appResult.Data);
                }
                else
                {
                    Login(result.Data.UserId, 1);
                }
                return new SuccessDataResult<Customer>(result.Data, Messages.LoginSuccessful);
            }
            return new ErrorDataResult<Customer>(Messages.UserNotFound);
        }

        [CacheAspect(20)]
        public IResult UserExists(string username)
        {
            if (_userService.GetByUsername(username) != null)
                return new ErrorResult(Messages.UserAlreadyExists);
            return new SuccessResult(Messages.UserAvailable);
        }

        private IResult Login(int userId, int appId)
        {
            return _userLogService.Add(new UserLog
            {
                AppId = appId,
                LogDate = DateTime.Now,
                UserId = userId
            });
        }
        public IResult Logout(int userId)
        {
            var result = _userLogService.GetLastLoginByUserId(userId);
            if (result.Success)
            {
                result.Data.EntityState = EntityState.Modified;
                result.Data.LogOutDate = DateTime.Now;
                return _userLogService.Update(result.Data);
            }
            return new ErrorResult(result.Message);
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            if (user != null)
            {
                var claims = _userService.GetClaims(user);
                var accessToken = _tokenHelper.CreateToken(user, claims);
                return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
            }
            return new ErrorDataResult<AccessToken>(Messages.AccessTokenFailed);
        }
    }
}
