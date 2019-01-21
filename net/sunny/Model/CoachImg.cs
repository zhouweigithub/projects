using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 教练上传的自身相关证件照片
    /// </summary>
    public class CoachImg
    {
        public int id { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [TableField]
        public int coach_id { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [TableField]
        public string url { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string comment { get; set; }
        /// <summary>
        /// 类型0身份证照1自身照2教练证照
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 状态0正常1禁用
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
