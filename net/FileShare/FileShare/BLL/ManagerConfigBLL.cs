using System;
using System.Collections.Generic;
using System.IO;
using Public.CSUtil.Log;

namespace FileShare.BLL
{
    public static class ManagerConfigBLL
    {
        /// <summary>
        /// 文件名
        /// </summary>
        private const String fileName = "config.txt";

        /// <summary>
        /// 最近更新时间
        /// </summary>
        private static DateTime LastUpdateTime = default;

        /// <summary>
        /// 数据字典
        /// </summary>
        private static Dictionary<String, String> dic = new Dictionary<String, String>();


        /// <summary>
        /// 获取配置参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public static String GetConfig(String key)
        {
            if (DateTime.Now.Subtract(LastUpdateTime).TotalMinutes > 10)
            {
                RefreshDic();
            }

            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 更新dic
        /// </summary>
        private static void RefreshDic()
        {
            String fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", fileName);

            if (!File.Exists(fullFileName))
            {
                LogUtil.Error("App_Data目录中未找到文件config.txt");
                return;
            }

            LogUtil.Debug($"读取磁盘文件内容：{fileName}");

            String[] lines = File.ReadAllLines(fullFileName, Common.encoding);

            dic.Clear();

            foreach (String item in lines)
            {
                if (item.StartsWith("#"))
                    continue;

                Int32 index = item.IndexOf('=');
                if (index == -1)
                {
                    LogUtil.Error($"config.txt 中出现无效参数【{item}】");
                    continue;
                }

                String key = item.Substring(0, index).Trim();
                String value = item.Substring(index + 1).Trim();

                if (dic.ContainsKey(key))
                {
                    LogUtil.Error($"config.txt 中出现重复参数【{key}】");
                }

                dic[key] = value;
            }

            LastUpdateTime = DateTime.Now;
        }
    }
}