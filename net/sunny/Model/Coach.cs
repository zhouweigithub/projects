﻿using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// 注册教练信息表
    /// </summary>
    public class Coach
    {
        public int id { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [TableField]
        public string username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [TableField]
        public string password { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [TableField]
        public string name { get; set; }
        /// <summary>
        /// 性别0男 1女
        /// </summary>
        [TableField]
        public short sex { get; set; }
        /// <summary>
        /// 类型0游泳1画画2弹琴3...
        /// </summary>
        [TableField]
        public short type { get; set; }
        /// <summary>
        /// 教练评级
        /// </summary>
        [TableField]
        public short level { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 头像图片地址
        /// </summary>
        [TableField]
        public string headimg { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        [TableField]
        public decimal cash { get; set; }
        /// <summary>
        /// 0正常 1受限
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime crtime { get; set; }
    }

    /// <summary>
    /// 注册请求数据
    /// </summary>
    public class CoachRequest : Coach
    {
        /// <summary>
        /// 教练队长电话号码
        /// </summary>
        public string CaptionPhone { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsVerificationCode { get; set; }
    }
}
