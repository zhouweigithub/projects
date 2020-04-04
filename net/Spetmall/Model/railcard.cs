using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 优惠券(BASE TABLE)
    /// </summary>
    public class railcard
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 最大可使用次数
        /// </summary>
        [TableField]
        public int times { get; set; }
        /// <summary>
        /// 宠物名字
        /// </summary>
        [TableField]
        public string petname { get; set; }
        /// <summary>
        /// 宠物名字简拼
        /// </summary>
        [TableField]
        public string py { get; set; }
        /// <summary>
        /// 宠物年龄（岁）
        /// </summary>
        [TableField]
        public short petage { get; set; }
        /// <summary>
        /// 宠物类型
        /// </summary>
        [TableField]
        public string pettype { get; set; }
        /// <summary>
        /// 剩余使用次数
        /// </summary>
        [TableField]
        public int lefttimes { get; set; }
        /// <summary>
        /// 最大可使用金额
        /// </summary>
        [TableField]
        public decimal money { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [TableField]
        public DateTime starttime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [TableField]
        public DateTime endtime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [TableField]
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }
        /// <summary>
        /// 支付方式 1现金 2微信 3支付宝 4余额 5刷卡 6其他
        /// </summary>
        [TableField]
        public short payType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateString
        {
            get
            {
                if (times <= 0)
                    return "已用完";
                else if (DateTime.Now < starttime)
                    return "未开始";
                else if (DateTime.Now > endtime)
                    return "已过期";
                else
                    return "正常";
            }
        }

    }
}
