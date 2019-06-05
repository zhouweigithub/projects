using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 挂单信息
    /// </summary>
    public class guadan
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderid { get; set; }
        /// <summary>
        /// 会员姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
        /// <summary>
        /// 散客或会员
        /// </summary>
        public string type
        {
            get
            {
                return string.IsNullOrEmpty(name) ? "散客" : "会员";
            }
        }
    }
}
