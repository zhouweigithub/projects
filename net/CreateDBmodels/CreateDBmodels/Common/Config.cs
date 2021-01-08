using System;
using System.Configuration;

namespace CreateDBmodels.Common
{
    /// <summary>
    /// 通用配置库，通过本类可以方便获取程序配置项的值
    /// 文件功能描述：公共类，系统配置，通过本类可以快速地访问Web.config及App.Config中的配置项
    /// 依赖说明：无依赖
    /// 异常处理：本类捕获并处理异常，当配置项不存在时，获取该类型的默认值
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 查询配置，返回值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>不会捕获异常</returns>
        public static Object GetConfigValue(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 查询配置，返回字符串
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>捕获异常,当未设置key时,返回空字符串</returns>
        public static String GetConfigToString(String key)
        {
            try
            {
                return GetConfigValue(key).ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 查询配置，返回整数
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>捕获异常,当未设置key 或key不是数字 时,返回 int 最小值</returns>
        public static Int32 GetConfigToInt(String key)
        {
            Object obj = GetConfigValue(key);
            if (obj == null)
            {
                return Int32.MinValue;
            }
            else
            {
                try
                {
                    return Convert.ToInt32(obj);
                }
                catch
                {
                    return Int32.MinValue;
                }

            }
        }

        /// <summary>
        /// 查询配置，返回长整数
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>不会捕获异常,当未设置key 或key不是数字 时,返回 长整数 最小值</returns>
        public static Int64 GetConfigToLong(String key)
        {
            Object obj = GetConfigValue(key);
            if (obj == null)
            {
                return Int64.MinValue;
            }
            else
            {
                try
                {
                    return Convert.ToInt64(obj);
                }
                catch
                {
                    return Int64.MinValue;
                }

            }
        }

        /// <summary>
        /// 查询配置，返回日期
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>不会捕获异常</returns>
        public static DateTime GetConfigToDateTime(String key)
        {
            Object obj = GetConfigValue(key);

            if (obj == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                try
                {
                    return Convert.ToDateTime(GetConfigValue(key));
                }
                catch (Exception)
                {
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// 查询配置，返回逻布尔值
        /// </summary>
        /// <param name="key">键名</param>
        /// <returns>不会捕获异常</returns>
        public static Boolean GetConfigToBool(String key)
        {
            Object obj = GetConfigValue(key);
            if (obj == null)
            {
                return false;
            }
            else
            {
                try
                {
                    return Convert.ToBoolean(GetConfigValue(key));
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 应用程序当前目录 包含最后一个目录分隔符号
        /// </summary>
        public static String BaseDirectory
        {
            get
            {
                String path = System.AppDomain.CurrentDomain.BaseDirectory;
                if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    path += System.IO.Path.DirectorySeparatorChar.ToString();
                }
                return path;
            }
        }

        /// <summary>
        /// 临时目录（当前程序下的temp子目录） 包含最后一个目录分隔符号
        /// </summary>
        public static String TempFilePath
        {
            get
            {
                String path = BaseDirectory + "temp" + System.IO.Path.DirectorySeparatorChar;
                if (!System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    catch
                    {
                        return String.Empty;
                    }
                }

                return path;
            }
        }
    }
}
