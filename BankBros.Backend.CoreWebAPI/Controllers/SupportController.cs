using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupportController : ControllerBase
    {
        private ISupportService _supportService;

        public SupportController(ISupportService supportService)
        {
            _supportService = supportService;
        }

        /// <summary>
        /// Giriş yapan kullanıcılara Hesap tiplerini/döviz kurlarını getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("balancetypes")]
        [ProducesResponseType(typeof(List<BalanceType>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetBalanceTypes()
        {
            var result = _supportService.GetBalanceTypes();
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Giriş yapan kullanıcılara işlem tiplerini getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactiontypes")]
        [ProducesResponseType(typeof(List<TransactionType>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetTransactionTypes()
        {
            var result = _supportService.GetTransactionTypes();
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcıların işlem sonuçları listesini getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet("transactionresults")]
        [ProducesResponseType(typeof(List<TransactionResult>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetTransactionResults()
        {
            var result = _supportService.GetTransactionResults();
            if (result.Success)
                return Ok(result.Data);
            return BadRequest(result.Message);
        }

        /// <summary>
        /// Cachelerin temizlenmesi için kullanılmalıdır.
        /// </summary>
        /// <returns></returns>
        [HttpGet("clearcache")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult ClearCache()
        {
            var result = _supportService.ClearCachings();
            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }
    }
}