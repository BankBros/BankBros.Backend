using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Validation.FluentValidation;
using BankBros.Backend.Core.Aspects.Autofac.Validation;
using BankBros.Backend.CoreWebAPI.Helpers;
using BankBros.Backend.CoreWebAPI.Models;
using BankBros.Backend.Entity.Concrete;
using BankBros.Backend.Entity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditController : ControllerBase
    {
        private ICreditService _creditService;

        public CreditController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcı yeni bir kredi hesaplama isteğinde bulunur.
        /// </summary>
        /// <param name="creditRequestDto">Kredi istek Bilgileri</param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult AddRequest([FromBody]CreditRequestDto creditRequestDto)
        {
            try
            {
                var userId = GetUserId();
                if (!String.IsNullOrEmpty(userId))
                {
                    var checkDtoResult = _creditService.Check(creditRequestDto);
                    if (!checkDtoResult.Success)
                        return BadRequest(checkDtoResult.Message);
                    var result = Task.Run(() => ApiHelper<CreditResponse>.Post(new CreditRequestFlaskDto
                    {
                        Age = creditRequestDto.Age,
                        Amount = creditRequestDto.Amount,
                        HasHouse = creditRequestDto.HasHouse ? 1 : 0,
                        HasPhone = creditRequestDto.HasPhone ? 1 : 0,
                        UsedCredits = creditRequestDto.UsedCredits
                    }, "result")).Result;
                    if (result == null || result.Data == null)
                        return BadRequest(result.Error);

                    creditRequestDto.Result = result.Data.Success == 1 ? true : false;
                    var creditResult = _creditService.Add(Convert.ToInt32(userId), creditRequestDto);
                    if (creditResult.Success)
                    {
                        return Ok(creditResult.Message);
                    }
                    return BadRequest(creditResult.Message);
                }
                return Unauthorized("Yetkisiz Eylem.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Giriş yapmış olan kullanıcının yapmış olduğu kredi isteklerini listeler.
        /// </summary>
        /// <returns>List of CreditRequest</returns>
        [HttpGet()]
        [ProducesResponseType(typeof(List<CreditRequest>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult GetRequests()
        {
            var userId = GetUserId();
            if (!String.IsNullOrEmpty(userId))
            {
                var result = _creditService.GetListByCustomerNumber(Convert.ToInt32(userId));
                if (result.Success)
                    return Ok(result.Data);
                return BadRequest(result.Message);
            }
            return Unauthorized("Yetkisiz Eylem.");
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