using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 会员信息(BASE TABLE)
    /// </summary>
    public class member
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [TableField]
        public string email { get; set; }
        /// <summary>
        /// 性别0男 1女
        /// </summary>
        [TableField]
        public short sex { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 打几折
        /// </summary>
        [TableField]
        public double discount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
