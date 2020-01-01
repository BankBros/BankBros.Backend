using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        private ICustomerService _customerService;

        public TransactionController(ITransactionService transactionService, ICustomerService customerService)
        {
            _transactionService = transactionService;
            _customerService = customerService;
        }

        /// <summary>
        /// Giriş yapmış kullanıcının para yatırma işlemleri için kullanılır.
        /// </summary>
        /// <param name="depositDto"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Deposit([FromBody] DepositDto depositDto)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    var result = _transactionService.Deposit(Convert.ToInt32(userId), depositDto);
                    if (result.Success)
                        return Ok(result.Message);
                    return BadRequest(result.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(Messages.InvalidToken);
        }

        /// <summary>
        /// Giriş yapmış kullanıcının para çekme işlemleri için kullanılır. 
        /// </summary>
        /// <param name="drawDto"></param>
        /// <returns></returns>
        [HttpPost("draw")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Draw([FromBody]DrawDto drawDto)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    var result = _transactionService.Draw(Convert.ToInt32(userId), drawDto);
                    if (result.Success)
                        return Ok(result.Message);
                    return BadRequest(result.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(Messages.InvalidToken);
        }

        /// <summary>
        /// Giriş yapmış kullanıcının havale işlemleri için kullanılır.
        /// </summary>
        /// <param name="transferDto"></param>
        /// <returns></returns>
        [HttpPost("transfer")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Transfer([FromBody] TransferDto transferDto)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    var result = _transactionService.Transfer(Convert.ToInt32(userId), transferDto);
                    if (result.Success)
                        return Ok(result.Message);
                    return BadRequest(result.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(Messages.InvalidToken);
        }

        /// <summary>
        /// Giriş yapmış kullanıcının virman işlemleri için kullanılır.
        /// </summary>
        /// <param name="virementDto"></param>
        /// <returns></returns>
        [HttpPost("virement")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Transfer([FromBody] VirementDto virementDto)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                try
                {
                    var result = _transactionService.Virement(Convert.ToInt32(userId), virementDto);
                    if (result.Success)
                        return Ok(result.Message);
                    return BadRequest(result.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
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