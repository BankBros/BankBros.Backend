using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Extensions;
using BankBros.Backend.Core.Utilities.Interceptors;
using BankBros.Backend.Core.Utilities.IoC;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BankBros.Backend.Business.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;
        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();

            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                    return;
            }

            throw new Exception(Messages.AuthorizationDenied);
        }
    }
}
