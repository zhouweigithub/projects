using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 收货地址
    /// </summary>
    public class ReceiverInfo
    {
        public int id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [TableField]
        public int student_id { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        [TableField]
        public string address { get; set; }
        /// <summary>
        /// 立减金额
        /// </summary>
        [TableField]
        public DateTime crtime { get; set; }
    }
}
