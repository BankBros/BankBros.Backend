using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Extensions;
using BankBros.Backend.Core.Utilities.Date;
using BankBros.Backend.Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BankBros.Backend.Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; }
        private TokenOptions _tokenOptions;
        private DateTime _accesTokenExpiration;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accesTokenExpiration = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_tokenOptions.AccessTokenExpiration));
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt); 
            return new AccessToken
            {
                Token = token, 
                Expiration = _accesTokenExpiration
            };
        }

        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            return new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accesTokenExpiration,
                notBefore: DateTime.UtcNow,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
                );
        }

        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddUsername(user.Username);
            claims.AddRoles(operationClaims.Select(x => x.Name).ToArray());
            return claims;
        }
    }
}
