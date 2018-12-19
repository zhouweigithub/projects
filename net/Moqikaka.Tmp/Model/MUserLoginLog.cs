using Moqikaka.Tmp.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class MUserLoginLog
    {
        public int Id { get; set; }
        public DateTime CrTime { get; set; }
        [TableField]
        public int DeviceToken { get; set; }
        [TableField]
        public string Openid { get; set; }
        [TableField]
        public int Brand { get; set; }
        [TableField]
        public string Model { get; set; }
        [TableField]
        public string Version { get; set; }
        [TableField]
        public string PixelRatio { get; set; }
        [TableField]
        public string ScreenWidth { get; set; }
        [TableField]
        public string ScreenHeight { get; set; }
        [TableField]
        public string Language { get; set; }
        [TableField]
        public string System { get; set; }
        [TableField]
        public string Platform { get; set; }
        [TableField]
        public string SDKVersion { get; set; }
    }
}
