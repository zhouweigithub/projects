namespace Moqikaka.Tmp.Common
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

    }
}
