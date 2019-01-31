using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    public class CustVenue
    {
        public int venue_id { get; set; }
        public int campus_id { get; set; }
        public string venue_name { get; set; }
        public string campus_name { get; set; }
        public decimal price { get; set; }
    }
}
