//***********************************************************************************
//文件名称：loginuser.cs
//功能描述：用户实体类
//数据表： promotioncenter.loginuser
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Portal.Model.Common;
using System;

namespace Spetmall.Model
{
    public class LoginUser
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        [TableField]
        public Int32 Id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [TableField]
        public String Username { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [TableField]
        public String Password { get; set; }

        /// <summary>
        /// 角色集
        /// </summary>
        [TableField]
        public String Roleids { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [TableField]
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [TableField]
        public string Name { get; set; }

        /// <summary>
        /// 类别（0普通1客服）
        /// </summary>
        [TableField]
        public int Type { get; set; }

        /// <summary>
        /// 是否超级用户
        /// </summary>
        [TableField]
        public int IfSuper { get; set; }

        /// <summary>
        /// 状态【0：正常；1：锁定】
        /// </summary>
        [TableField]
        public int Status { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [TableField]
        public string Mobile { get; set; }

        /// <summary>
        /// 登录时是否需要短信验证（0不需要验证   1需要验证）
        /// </summary>
        [TableField]
        public int SmsVerify { get; set; }

    }
}
