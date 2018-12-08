//***********************************************************************************
//文件名称：LoginUserModel.cs
//功能描述：用户信息实体类
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model
{
    /// <summary>
    /// 用户登录信息
    /// </summary>
    public class LoginUserModel
    {

        /// <summary>
        /// 用户id
        /// </summary>
        public Int32 id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }


        /// <summary>
        /// Pwd
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 用户角色编码【多个以,隔开】
        /// </summary>
        public string roleids { get; set; }

        public string appids { get; set; }


        /// <summary>
        /// 角色名称
        /// </summary>
        public string rolenames { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }

        /// <summary>
        /// 是否超级用户【1：超级用户；0一般用户】
        /// </summary>
        public int IfSuper { get; set; }
        /// <summary>
        /// 状态【1：正常；0：锁定】
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 类别(0普通1客服)
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 登录时是否需要短信验证
        /// </summary>
        public int SmsVerify { get; set; }

    }

    /// <summary>
    /// 用户角色信息列表Model
    /// </summary>
    public class ListLoginUserModel
    {
        /// <summary>
        /// 用户登录信息列表
        /// </summary>
        public List<LoginUserModel> LoginUserInfoList { get; set; }
    }
}
