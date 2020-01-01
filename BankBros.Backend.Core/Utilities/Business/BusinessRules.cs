using BankBros.Backend.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)
        {
            foreach (var result in logics)
            {
                if (!result.Success)
                    return result;
            }

            return null;
        }
    }
}
