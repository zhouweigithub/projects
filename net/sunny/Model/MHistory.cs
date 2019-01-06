using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{

    /// <summary>
    /// 浏览历史
    /// </summary>
    public class MHistory
    {
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public int Articalid { get; set; }
        [TableField]
        public string Openid { get; set; }
        public DateTime CrTime { get; set; }
    }
}
