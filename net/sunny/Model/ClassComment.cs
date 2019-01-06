using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    public class ClassComment
    {
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 教练上传的图片地址
        /// </summary>
        [TableField]
        public string img { get; set; }
        /// <summary>
        /// 教练上传的视频地址
        /// </summary>
        [TableField]
        public string video { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
