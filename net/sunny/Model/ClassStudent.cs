using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 上课的学生
    /// </summary>
    public class ClassStudent
    {
        /// <summary>
        /// 上课id
        /// </summary>
        [TableField]
        public int class_id { get; set; }
        /// <summary>
        /// 学员id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 0未上课 1已上课 2已评价
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 学员给本次上课打分
        /// </summary>
        [TableField]
        public float marking { get; set; }
        /// <summary>
        /// 学生评价内容
        /// </summary>
        [TableField]
        public string comment { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }
}
