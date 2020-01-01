using BankBros.Backend.Core.Entities;
using BankBros.Backend.Core.Utilities.Interceptors;
using BankBros.Backend.Core.Utilities.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using BankBros.Backend.Business.Concrete;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Core.Utilities.IoC;
using System.Diagnostics;

namespace BankBros.Backend.Business.Autofac
{
    public class UserLogOperation : MethodInterception
    {
        private Type _entityType;
        public UserLogOperation(Type entityType)
        {
            if (!typeof(IDto).IsAssignableFrom(entityType))
            {
                throw new Exception(AspectMessages.WrongEntityType);
            }
            //_userManager = Activator.CreateInstance<UserManager>();
            _entityType = entityType;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var entityType = _entityType;
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (var entity in entities)
            {
                var appIdInfo = entity.GetType().GetProperty("ApplicationId");
                var appId = (int)(appIdInfo.GetValue(entity, null));
                Debug.WriteLine(appId.ToString());
            }
        }
    }
}
