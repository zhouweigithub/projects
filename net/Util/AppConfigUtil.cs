//***********************************************************************************
//文件名称：AppConfigUtil.cs
//功能描述：AppConfig文件处理助手类
//数据表：Nothing
//作者：Jordan
//日期：2014-04-09 11:30:00
//修改记录：
//***********************************************************************************

using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// AppConfig文件处理助手类
    /// </summary>
    public static class AppConfigUtil
    {
        #region 字段与属性

        /// <summary>
        /// 当前读取的配置文件路径
        /// </summary>
        public static String ConfigPath { get; private set; }

        ///配置对象
        private static Configuration mConfiguration = null;

        ///配置文件未指定提示
        private const String ConfigFileNotSpecified = "请指定需要读取的配置文件";

        #endregion

        #region 初始化

        /// <summary>
        /// 静态初始化 (默认加载当前程序目录下, 唯一的*.config配置文档)
        /// </summary>
        static AppConfigUtil()
        {
            List<String> configFileList = new List<String>();

            ////当前目录下的config配置文件集合, 排除特殊文件
            foreach (var item in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.config"))
            {
                String fileName = Path.GetFileName(item).ToLower();
                if (!fileName.Contains("vshost") && !fileName.Contains("debug") && !fileName.Contains("release"))
                {
                    configFileList.Add(item);
                }
            }

            if (configFileList.Count != 1) return;

            //设置ConfigPath
            ConfigPath = configFileList[0];

            //设置配置文件路径
            SetConfigFile(ConfigPath);
        }

        #endregion

        #region 公开方法

        ///<summary>
        ///依据连接串名字connectionName返回数据连接字符串
        ///</summary>
        ///<param name="connectionName">连接名称</param>
        ///<returns></returns>
        public static String GetConnectionStringsConfig(String connectionName)
        {
            if (mConfiguration == null)
            {
                throw new Exception(ConfigFileNotSpecified);
            }

            return mConfiguration.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
        }

        ///<summary>
        ///更新连接字符串
        ///</summary>
        ///<param name="newName">连接字符串名称</param>
        ///<param name="newConString">连接字符串内容</param>
        ///<param name="newProviderName">数据提供程序名称</param>
        public static void UpdateConnectionStringsConfig(String newName, String newConString, String newProviderName)
        {
            if (mConfiguration == null)
            {
                throw new Exception(ConfigFileNotSpecified);
            }

            //如果要更改的连接串已经存在,则删除
            if (mConfiguration.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                mConfiguration.ConnectionStrings.ConnectionStrings.Remove(newName);
            }

            //新建一个连接字符串实例
            ConnectionStringSettings mySettings = new ConnectionStringSettings(newName, newConString, newProviderName);

            // 将新的连接串添加到配置文件中.
            mConfiguration.ConnectionStrings.ConnectionStrings.Add(mySettings);

            // 保存对配置文件所作的更改
            mConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        ///<summary>
        ///返回＊.exe.config文件中appSettings配置节的value项
        ///</summary>
        ///<param name="strKey">key</param>
        ///<returns></returns>
        public static String GetAppConfig(String strKey)
        {
            if (mConfiguration == null)
            {
                throw new Exception(ConfigFileNotSpecified);
            }

            if (!mConfiguration.AppSettings.Settings.AllKeys.Contains(strKey))
            {
                throw new Exception(String.Format("Key={0}的配置不存在!", strKey));
            }

            return mConfiguration.AppSettings.Settings[strKey].Value;
        }

        ///<summary>
        ///在＊.exe.config文件中appSettings配置节更新或增加一对键、值对
        ///</summary>
        ///<param name="newKey">新的key</param>
        ///<param name="newValue">新的value</param>
        public static void UpdateAppConfig(String newKey, String newValue)
        {
            if (mConfiguration == null)
            {
                throw new Exception(ConfigFileNotSpecified);
            }

            //Check if the key exists, remove it
            if (mConfiguration.AppSettings.Settings.AllKeys.Contains(newKey))
            {
                mConfiguration.AppSettings.Settings.Remove(newKey);
            }

            // Add an Application Setting.
            mConfiguration.AppSettings.Settings.Add(newKey, newValue);

            // Save the changes in App.config file.
            mConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 设置配置文件路径
        /// </summary>
        /// <param name="path">配置文件路径</param>
        public static void SetConfigFile(String path)
        {
            //配置文件映射
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = path;

            //设置ConfigPath
            ConfigPath = path;

            //初始化Configuration对象
            mConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
        }

        #endregion
    }
}