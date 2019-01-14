using Sunny.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    public class Student
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
        public short sex { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        [TableField]
        public string phone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [TableField]
        public DateTime birthday { get; set; }
        /// <summary>
        /// 头像图片地址
        /// </summary>
        [TableField]
        public string headimg { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        [TableField]
        public string cash { get; set; }
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
    public class StudentRequest : Student
    {
        /// <summary>
        /// 邀请码
        /// </summary>
        public string Invitationcode { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SmsVerificationCode { get; set; }
    }
}
