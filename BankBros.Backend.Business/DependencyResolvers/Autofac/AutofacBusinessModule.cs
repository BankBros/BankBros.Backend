using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Extras.DynamicProxy;
using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Concrete;
using BankBros.Backend.Business.Validation.FluentValidation;
using BankBros.Backend.Core.Utilities.Interceptors;
using BankBros.Backend.Core.Utilities.Security.Jwt;
using BankBros.Backend.DataAccess.Abstract;
using BankBros.Backend.DataAccess.Concrete.EntityFramework;
using BankBros.Backend.Entity.Dtos;
using Castle.DynamicProxy;

namespace BankBros.Backend.Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerManager>().As<ICustomerService>();
            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<AccountManager>().As<IAccountService>();
            builder.RegisterType<TransactionManager>().As<ITransactionService>();
            builder.RegisterType<SupportManager>().As<ISupportService>();
            builder.RegisterType<ApplicationManager>().As<IApplicationService>();
            builder.RegisterType<UserLogManager>().As<IUserLogService>();
            builder.RegisterType<CustomerCreditRequestManager>().As<ICustomerCreditRequestService>();
            builder.RegisterType<CreditManager>().As<ICreditService>();

            builder.RegisterType<EfAccountDal>().As<IAccountDal>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();
            builder.RegisterType<EfUserOperationClaimDal>().As<IUserOperationClaimDal>();
            builder.RegisterType<EfOperationClaimDal>().As<IOperationClaimDal>();
            builder.RegisterType<EfCustomerDetailDal>().As<ICustomerDetailDal>();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>();
            builder.RegisterType<EfTransactionDal>().As<ITransactionDal>();
            builder.RegisterType<EfTransactionTypeDal>().As<ITransactionTypeDal>();
            builder.RegisterType<EfTransactionResultDal>().As<ITransactionResultDal>();
            builder.RegisterType<EfBalanceTypeDal>().As<IBalanceTypeDal>();
            builder.RegisterType<EfCustomerCreditRequestDal>().As<ICustomerCreditRequestDal>();
            builder.RegisterType<EfCreditRequestDal>().As<ICreditRequestDal>();
            builder.RegisterType<EfApplicationDal>().As<IApplicationDal>();
            builder.RegisterType<EfUserLogDal>().As<IUserLogDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();
            
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
