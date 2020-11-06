using System;
using System.Text;

namespace FileShare
{
    public class Common
    {
        /// <summary>
        /// 上传目录
        /// </summary>
        public const String UploadFolder = "UploadAndShare";

        /// <summary>
        /// 上传的日志目录名
        /// </summary>
        public const String UploadLogFolder = "UploadLog";

        /// <summary>
        /// 编码格式
        /// </summary>
        public static readonly Encoding encoding = Encoding.UTF8;
    }
}