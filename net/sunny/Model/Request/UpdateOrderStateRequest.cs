using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class UpdateOrderStateRequest
    {
        public string orderId { get; set; }
        public short state { get; set; }
    }
}
