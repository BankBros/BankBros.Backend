using BankBros.Backend.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankBros.Backend.CoreWebAPI.Models
{
    public class CreditResponse
    {
        public FlaskDataResult Data { get; set; }
        public string Error { get; set; }
    }

    public class FlaskDataResult
    {
        public string Message { get; set; }
        public int Success { get; set; }
    }
}
