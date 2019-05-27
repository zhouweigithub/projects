using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 充值记录(BASE TABLE)
    /// </summary>
    public class recharge
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        [TableField]
        public string sno { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [TableField]
        public int memberid { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 余额
        /// </summary>
        [TableField]
        public decimal balance { get; set; }
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
