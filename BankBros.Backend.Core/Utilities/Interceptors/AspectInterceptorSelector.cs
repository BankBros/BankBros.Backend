using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BankBros.Backend.Core.Utilities.Interceptors.Autofac;
using Castle.DynamicProxy;
using Microsoft.EntityFrameworkCore.Diagnostics;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace BankBros.Backend.Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name).GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            classAttributes.AddRange(methodAttributes);

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
