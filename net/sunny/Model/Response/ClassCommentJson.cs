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
        public int Name;
        /// <summary>
        /// 教练头像图片
        /// </summary>
        public int HeadImg;
        /// <summary>
        /// 评论文字内容
        /// </summary>
        public int Comment;
        /// <summary>
        /// 评论图片或视频地址，多个以逗号分隔
        /// </summary>
        public int Urls;
        /// <summary>
        /// 上课时间
        /// </summary>
        public DateTime Crtime;
    }
}
