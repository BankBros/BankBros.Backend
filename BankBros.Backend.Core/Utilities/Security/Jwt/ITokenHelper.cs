using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Entities.Concrete;

namespace BankBros.Backend.Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
    }
}
