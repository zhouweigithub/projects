using System.Text;

namespace Hswz.Common
{
    public class Const
    {
        /// <summary>
        /// 程序主目录路径
        /// </summary>
        public static System.String RootWebPath;
        /// <summary>
        /// 导出目录
        /// </summary>
        public const System.String ExportPath = "\\Export";
        /// <summary>
        /// 连续输入错误密码最大次数（超过次数后账号将被锁定）
        /// </summary>
        public const System.Int32 MaxLoginFailedTimes = 5;
        /// <summary>
        /// application key + username（存储连续输入错误密码最大次数）
        /// </summary>
        public const System.String Application_Login_Failed_Times_ = "_application_login_failed_times_";

        /// <summary>
        /// 默认编码格式
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

    }
}
