using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Core.Utilities.Results
{
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}
