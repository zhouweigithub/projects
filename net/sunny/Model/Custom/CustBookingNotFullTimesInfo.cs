using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 未约满的信息
    /// </summary>
    public class CustBookingNotFullTimesInfo
    {
        public int venue_id { get; set; }
        public int product_id { get; set; }
        public int hour { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
    }
}
