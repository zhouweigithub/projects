using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class ReceiveBookingRequest
    {
        public int bookingId;
        public string token;
        public DateTime startTime;
        public DateTime endTime;
    }
}
