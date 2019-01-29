using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 已约满的时间段
    /// </summary>
    public class CustBookingFullTimes
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }
}
