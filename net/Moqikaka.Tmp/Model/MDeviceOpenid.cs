using Moqikaka.Tmp.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model
{
    public class MDeviceOpenid
    {
        [TableField]
        public string DeviceToken { get; set; }
        [TableField]
        public string Openid { get; set; }
    }
}
