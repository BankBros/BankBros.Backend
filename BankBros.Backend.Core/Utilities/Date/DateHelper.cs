using System;
using System.Collections.Generic;
using System.Text;

namespace BankBros.Backend.Core.Utilities.Date
{
    public class DateHelper
    {
        public static DateTime Now()
        {
            var turkeyTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
            var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, turkeyTimeZone);

            return currentDateTime;
        }
    }
}
