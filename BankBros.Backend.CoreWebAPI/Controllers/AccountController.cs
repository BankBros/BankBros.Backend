using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private ICustomerService _customerService;

        public AccountController(IAccountService accountService, ICustomerService customerService)
        {
            _accountService = accountService;
            _customerService = customerService;
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcıya yeni bir hesap açar.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult AddAccount()
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var userResult = _customerService.GetByUserId(Convert.ToInt32(userId));
                if (!userResult.Success)
                    return BadRequest(userResult.Message);

                var result = _accountService.Add(userResult.Data.Id, new Account());
                if (result.Success)
                    return Ok(result.Message);
                return BadRequest(result.Message);
            }

            return BadRequest(Messages.InvalidToken);
        }
        /// <summary>
        /// Giriş yapmış olan kullanıcın hesaplarından id'si gönderileni siler.
        /// </summary>
        /// <param name="id">Hesap No</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation(Summary = "Hesap Sil", Description = "Giriş yapmış olan kullanıcın hesaplarından id'si gönderileni siler.")]
        public IActionResult DeleteAccount(int id)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var customerResult = _customerService.GetByUserId(Convert.ToInt32(userId));
                var result = _accountService.Delete(customerResult.Data.Accounts.Where(x => x.AccountNumber.Equals(id)).ToArray());
                if (result.Success)
                    return Ok(result.Message);

                return BadRequest(result.Message);
            }

            return BadRequest(Messages.InvalidToken);
        }

        private string GetUserId()
        {
            var claims = HttpContext.User.Claims;
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    if (claim.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))
                        return claim.Value;
                }
            }

            return String.Empty;
        }
    }
}