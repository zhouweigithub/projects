using Moqikaka.Tmp.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model
{

    /// <summary>
    /// 反馈
    /// </summary>
    public class MFeedback
    {
        public int Id { get; set; }
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public string Openid { get; set; }
        [TableField]
        public string Content { get; set; }
        public DateTime CrTime { get; set; }
    }
}
