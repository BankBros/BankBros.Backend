using System;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Utilities.Security.Jwt;
using BankBros.Backend.Entity.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BankBros.Backend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Kullanıcı girişi.
        /// </summary>
        /// <param name="userForLoginDto"></param>
        /// <returns>Access Token objesi döndürür.</returns>
        [HttpPost("login/customer")]
        [ProducesResponseType(typeof(AccessToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Login([FromBody]UserForLoginDto userForLoginDto)
        {
            try
            {
                var userToLogin = _authService.CustomerLogin(userForLoginDto);
                if (!userToLogin.Success)
                {
                    return BadRequest(userToLogin.Message);
                }

                var result = _authService.CreateAccessToken(userToLogin.Data.User);
                if (result.Success)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Müşteri kayıt olma.
        /// </summary>
        /// <param name="userForRegisterDto"></param>
        /// <returns>Access Token objesi döner.</returns>
        [HttpPost("register/customer")]
        [ProducesResponseType(typeof(AccessToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Register([FromBody]UserForCustomerRegisterDto userForRegisterDto)
        {
            string username = String.IsNullOrEmpty(userForRegisterDto.TCKN)
                ? (String.IsNullOrEmpty(userForRegisterDto.Username) ? String.Empty : userForRegisterDto.Username)
                : userForRegisterDto.TCKN;
            
            var userExists = _authService.UserExists(username);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            try
            {
                var registerResult = _authService.CustomerRegister(userForRegisterDto, userForRegisterDto.Password);
                var result = _authService.CreateAccessToken(registerResult.Data.User);
                if (result.Success)
                {
                    return Ok(result.Data);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Kullanıcı çıkışı.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout/customer")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public ActionResult Logout()
        {
            try
            {
                string userId = GetUserId();
                if (!String.IsNullOrEmpty(userId))
                {
                    var result = _authService.Logout(Convert.ToInt32(userId));
                    if (result.Success)
                        return Ok();
                    return BadRequest(result.Message);
                }

                return BadRequest(Messages.InvalidToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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