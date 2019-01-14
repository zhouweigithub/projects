using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    /// <summary>
    /// 上课后都教练的评论内容
    /// </summary>
    public class ClassCommentJson
    {
        /// <summary>
        /// 教练名字
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 教练头像图片
        /// </summary>
        public string headimg { get; set; }
        /// <summary>
        /// 评论文字内容
        /// </summary>
        public string comment { get; set; }
        /// <summary>
        /// 评论图片或视频地址，多个以逗号分隔
        /// </summary>
        public string urls { get; set; }
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
