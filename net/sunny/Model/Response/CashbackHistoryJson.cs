using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    /// <summary>
    /// 邀请返现汇总
    /// </summary>
    public class CashbackHistoryJson
    {
        /// <summary>
        /// 总邀请人数
        /// </summary>
        public int total_count { get; set; }
        /// <summary>
        /// 总返现金额
        /// </summary>
        public decimal total_money { get; set; }
        /// <summary>
        /// 各天记录
        /// </summary>
        public List<CashbackHistoryItem> items { get; set; }
    }

    /// <summary>
    /// 每天邀请返现记录
    /// </summary>
    public class CashbackHistoryItem
    {
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime crdate { get; set; }
        /// <summary>
        /// 邀请返现金额
        /// </summary>
        public DateTime money { get; set; }
        /// <summary>
        /// 返现笔数
        /// </summary>
        public DateTime count { get; set; }
    }
}
