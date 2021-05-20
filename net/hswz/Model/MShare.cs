using Hswz.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.Model
{

    /// <summary>
    /// 分享记录
    /// </summary>
    public class MShare
    {
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public int Articalid { get; set; }
        public DateTime CrTime { get; set; }
    }
}
