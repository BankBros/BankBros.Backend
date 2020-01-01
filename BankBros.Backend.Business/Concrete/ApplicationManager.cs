using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Concrete
{
    public class ApplicationManager : IApplicationService
    {
        private IApplicationDal _applicationDal;

        public ApplicationManager(IApplicationDal applicationDal)
        {
            _applicationDal = applicationDal;
        }

        [CacheAspect()]
        public IResult CheckAppIdIsExists(int appId)
        {
            try
            {
                if (_applicationDal.GetSingle(x => x.Id.Equals(appId)) != null)
                    return new SuccessResult();
                return new ErrorResult(Messages.ApplicationNotFound);
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        [CacheAspect()]
        public IDataResult<int> GetApplicationIdByName(string appName)
        {
            try
            {
                var app = _applicationDal.GetSingle(x => x.Name.Equals(appName));
                if(app != null)
                    return new SuccessDataResult<int>(app.Id);
                return new ErrorDataResult<int>(Messages.ApplicationNotFound);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<int>(ex.Message);
            }
        }

        [CacheAspect()]
        public IDataResult<List<Application>> GetApplications()
        {
            try
            {
                return new SuccessDataResult<List<Application>>();
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Application>>(ex.Message);
            }
        }
    }
}
