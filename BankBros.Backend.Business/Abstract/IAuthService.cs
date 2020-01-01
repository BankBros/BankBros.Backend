using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.Core.Utilities.Security.Jwt;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;

namespace BankBros.Backend.Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<Customer> CustomerRegister(UserForCustomerRegisterDto userForCustomerRegisterDto, string password);
        IDataResult<Customer> CustomerLogin(UserForLoginDto userForLoginDto);
        IResult UserExists(string username);
        IDataResult<AccessToken> CreateAccessToken(User user);
        public IResult Logout(int userId);
    }
}
