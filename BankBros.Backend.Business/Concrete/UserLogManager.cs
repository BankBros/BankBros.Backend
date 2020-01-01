using BankBros.Backend.Business.Abstract;
using BankBros.Backend.Business.Constants;
using BankBros.Backend.Core.Aspects.Autofac.Caching;
using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using BankBros.Backend.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankBros.Backend.Business.Concrete
{
    public class UserLogManager : IUserLogService
    {
        private IUserLogDal _userLogDal;
        public UserLogManager(IUserLogDal userLogDal)
        {
            _userLogDal = userLogDal;
        }

        [CacheAspect(duration:10)]
        public IDataResult<UserLog> GetLastLoginByUserId(int userId)
        {
            try
            {
                var last = _userLogDal.GetList(x => x.Id.Equals(userId)).Where(x=>x.LogOutDate == null).LastOrDefault();
                if (last != null)
                    return new SuccessDataResult<UserLog>(last);
                return new ErrorDataResult<UserLog>(Messages.UserLogsNotFound);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserLog>(ex.Message);
            }
        }

        [CacheAspect(duration: 10)]
        public IDataResult<List<UserLog>> GetUserLogByUserId(int userId)
        {
            try
            {
                var logs = _userLogDal.GetList(x => x.Id.Equals(userId)).ToList();
                if (logs != null)
                    return new SuccessDataResult<List<UserLog>>(logs);
                return new ErrorDataResult<List<UserLog>>(Messages.UserLogsNotFound);
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<UserLog>>(ex.Message);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get", Priority = 1)]
        [CacheRemoveAspect("IUserLogService.Get", Priority = 2)]
        public IResult Add(params UserLog[] userLogs)
        {
            try
            {
                if (_userLogDal.Add(userLogs))
                    return new SuccessResult();
                return new ErrorResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get", Priority = 1)]
        [CacheRemoveAspect("IUserLogService.Get", Priority = 2)]
        public IResult Update(params UserLog[] userLogs)
        {
            try
            {
                if (_userLogDal.Update(userLogs))
                    return new SuccessResult();
                return new ErrorResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [CacheRemoveAspect("ICustomerService.Get", Priority = 1)]
        [CacheRemoveAspect("IUserLogService.Get", Priority = 2)]
        public IResult Delete(params UserLog[] userLogs)
        {
            try
            {
                if (_userLogDal.Remove(userLogs))
                    return new SuccessResult();
                return new ErrorResult();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
