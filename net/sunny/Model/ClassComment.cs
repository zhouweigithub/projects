﻿using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 课程结束后教练评论的内容
    /// </summary>
    public class ClassComment
    {
        public int id { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 教练评论的文字内容
        /// </summary>
        [TableField]
        public string comment { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
