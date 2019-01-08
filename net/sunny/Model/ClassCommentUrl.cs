using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 课程结束后教练上传的图片或视频信息
    /// </summary>
    public class ClassCommentUrl
    {
        public int id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 教练上传的图片或视频地址
        /// </summary>
        [TableField]
        public string url { get; set; }
        /// <summary>
        /// 类型0图片 1视频
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
