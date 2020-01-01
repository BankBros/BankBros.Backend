using BankBros.Backend.Core.Entities.Concrete;
using BankBros.Backend.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Business.Abstract
{
    public interface IApplicationService
    {
        IDataResult<List<Application>> GetApplications();
        IResult CheckAppIdIsExists(int appId);
        IDataResult<int> GetApplicationIdByName(string appName);

    }
}
