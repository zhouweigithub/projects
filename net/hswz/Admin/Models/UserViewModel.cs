using System.ComponentModel.DataAnnotations;

namespace Hswz.Admin.Models
{
    /// <summary>
    /// 登录用户模型
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// 自增编号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Display(Name = "登录名")]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "登录密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 角色编码多个以，隔开
        /// </summary>
        public string roleids { get; set; }


        /// <summary>
        /// 页面提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 进入的url
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 状态【0：正常；1：锁定】
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号")]
        public string Mobile { get; set; }
    }
}