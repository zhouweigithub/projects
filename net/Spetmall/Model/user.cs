using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 用户信息(BASE TABLE)
    /// </summary>
    public class user
    {

        /// <summary>
        /// 
        /// </summary>
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
        /// 状态0正常 1受限
        /// </summary>
        [TableField]
        public short state { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        [TableField]
        public DateTime lastlogintime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime crtime { get; set; }

    }
}
