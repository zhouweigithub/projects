//***********************************************************************************
//文件名称：loginuser.cs
//功能描述：用户实体类
//数据表： promotioncenter.loginuser
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Moqikaka.Portal.Model.Common;
using System;

namespace Moqikaka.Tmp.Model
{
    public class MUser
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        [TableField]
        public Int32 Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [TableField]
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [TableField]
        public String Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [TableField]
        public String Name { get; set; }

        /// <summary>
        /// 状态（0正常 1禁用）
        /// </summary>
        [TableField]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [TableField]
        public DateTime CrTime { get; set; }

    }
}
