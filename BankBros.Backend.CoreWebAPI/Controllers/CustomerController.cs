using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcının aktif bilgilerini getirir.
        /// </summary>
        /// <returns>Customer</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(Customer),200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Get()
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var result = _customerService.GetByUserId(Convert.ToInt32(userId),false);
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message);
            }

            return BadRequest(Messages.InvalidToken);
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcının tüm bilgilerini getirir.
        /// </summary>
        /// <returns>Customer</returns>
        [HttpGet("full")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetFull()
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var result = _customerService.GetByUserId(Convert.ToInt32(userId));
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message);
            }

            return BadRequest(Messages.InvalidToken);
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcının bilgilerinin güncellemesini sağlar.
        /// </summary>
        /// <param name="customerDetailUpdateDto"></param>
        /// <returns>Customer</returns>
        [HttpPut()]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Put([FromBody]CustomerDetailUpdateDto customerDetailUpdateDto)
        {
            string userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var result = _customerService.GetByUserId(Convert.ToInt32(userId));
                if (result.Success)
                    return Ok(result.Data);
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