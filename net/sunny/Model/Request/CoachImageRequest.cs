using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Request
{
    public class CoachImageRequest
    {
        public string token { get; set; }
        /// <summary>
        /// 类型0身份证照1自身照2教练证照
        /// </summary>
        public short type { get; set; }
        public string comment { get; set; }
        public string url { get; set; }
    }
}
