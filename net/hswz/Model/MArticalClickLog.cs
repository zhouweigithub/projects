using Hswz.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.Model
{
    /// <summary>
    /// 文章点击量
    /// </summary>
    public class MArticalClickLog
    {
        [TableField]
        public DateTime CrDate { get; set; }
        [TableField]
        public int Articalid { get; set; }
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public int Clicks { get; set; }
    }
}
