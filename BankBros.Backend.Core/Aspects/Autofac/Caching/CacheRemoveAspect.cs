using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.CrossCuttingConcerns.Caching;
using BankBros.Backend.Core.Utilities.Interceptors;
using BankBros.Backend.Core.Utilities.IoC;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace BankBros.Backend.Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheManager _cacheManager;

        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            _cacheManager.RemoveByPattern(_pattern);
        }
    }
}
