using Hswz.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.Model
{
    /// <summary>
    /// 页面点击量
    /// </summary>
    public class MPageClickLog
    {
        [TableField]
        public DateTime CrDate { get; set; }
        [TableField]
        public string Page { get; set; }
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public int Clicks { get; set; }
    }
}
