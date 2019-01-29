namespace Sunny.Common
{
    public class Const
    {
        /// <summary>
        /// 程序主目录路径
        /// </summary>
        public static string RootWebPath;
        /// <summary>
        /// 导出目录
        /// </summary>
        public const string ExportPath = "\\Export";
        /// <summary>
        /// 连续输入错误密码最大次数（超过次数后账号将被锁定）
        /// </summary>
        public const int MaxLoginFailedTimes = 5;
        /// <summary>
        /// application key + username（存储连续输入错误密码最大次数）
        /// </summary>
        public const string Application_Login_Failed_Times_ = "_application_login_failed_times_";
        /// <summary>
        /// 获取自动生成的主键，与插入数据的连接相同才有效
        /// </summary>
        public const string SELECT_LAST_INSERT_ID_SQL = "SELECT LAST_INSERT_ID()";
        /// <summary>
        /// 客服电话
        /// </summary>
        public const string SitePhone = "客服电话";
        /// <summary>
        /// 用户注册返现金额
        /// </summary>
        public const string RegisterBackMoney = "用户注册返现金额";
        /// <summary>
        /// 邀请返现金额文本
        /// </summary>
        public const string BackCachMoney = "邀请返现金额";
        /// <summary>
        /// 邀请返现金额文本
        /// </summary>
        public const string SecondBackCachPercent = "二级返现比例";

    }
}
