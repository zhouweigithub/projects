using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 资金流水(BASE TABLE)
    /// </summary>
    public class financial_flow
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 0无效 -1支出 1收入
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 金额（元）
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        [TableField]
        public DateTime date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }

        public string TypeString
        {
            get
            {
                return type == 0 ? "无效" : type == 1 ? "收入" : "支出";
            }
        }

    }
}
