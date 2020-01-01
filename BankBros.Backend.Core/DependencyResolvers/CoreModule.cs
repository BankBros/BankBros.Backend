using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BankBros.Backend.Core.CrossCuttingConcerns.Caching;
using BankBros.Backend.Core.CrossCuttingConcerns.Caching.Microsoft;
using BankBros.Backend.Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace BankBros.Backend.Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();

            // Change entities when you need to switch your caching system by Manager.
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            
            // Added to get User Principal from HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<Stopwatch>();
        }
    }
}
