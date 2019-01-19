using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    public class PayRecordJson
    {
        public int id { get; set; }
        public string order_id { get; set; }
        public decimal money { get; set; }
        public decimal balance { get; set; }
        public short type { get; set; }
        public string comment { get; set; }
        public DateTime crtime { get; set; }
    }
}
