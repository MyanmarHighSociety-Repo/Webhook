using FacebookMessenger.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FacebookMessenger.Helper
{
    public static class DateConverter
    {
        public static DateTime? ConvertJSDT_To_Datetime(long value)
        {
            if (value == 0)
                return null;
            else
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                     .AddMilliseconds(value)
                     .ToLocalTime();
        }
    }
     
}