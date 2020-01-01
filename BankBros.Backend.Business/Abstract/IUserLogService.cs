using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Abstract
{
    public interface IUserLogService
    {
        IDataResult<List<UserLog>> GetUserLogByUserId(int userId);
        IDataResult<UserLog> GetLastLoginByUserId(int userId);
        IResult Add(params UserLog[] userLogs);
        IResult Update(params UserLog[] userLogs);
        IResult Delete(params UserLog[] userLogs);
    }
}
