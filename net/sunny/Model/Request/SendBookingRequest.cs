using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class SendBookingRequest
    {
        public int courseId;
        public DateTime startTime;
        public DateTime endTime;
    }
}
