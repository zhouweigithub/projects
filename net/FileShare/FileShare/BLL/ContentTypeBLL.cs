using System;
using System.Collections.Generic;
using System.IO;
using Public.CSUtil.Log;

namespace FileShare.BLL
{
    public class ContentTypeBLL
    {
        /// <summary>
        /// 类型文件名
        /// </summary>
        private const String contentTypeFileName = "contenttype.txt";

        /// <summary>
        /// 最近更新时间
        /// </summary>
        private static DateTime LastUpdateTime = default;

        /// <summary>
        /// 后缀与类型的关系
        /// </summary>
        private static Dictionary<String, String> dic = new Dictionary<String, String>();


        /// <summary>
        /// 根据文件名后缀获取ContentType
        /// </summary>
        /// <param name="type">文件名后缀</param>
        /// <returns></returns>
        public static String GetContentType(String type)
        {
            if (DateTime.Now.Subtract(LastUpdateTime).TotalMinutes > 10)
            {
                RefreshDic();
            }
            type = type.ToLower();
            if (dic.ContainsKey(type.ToLower()))
            {
                return dic[type];
            }
            else
            {
                return "application/octet-stream";
            }
        }

        /// <summary>
        /// 更新dic
        /// </summary>
        private static void RefreshDic()
        {
            String fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", contentTypeFileName);

            if (!File.Exists(fullFileName))
            {
                LogUtil.Error("App_Data目录中未找到文件contenttype.txt");
                return;
            }

            LogUtil.Debug($"读取磁盘文件内容：{contentTypeFileName}");

            String[] lines = File.ReadAllLines(fullFileName, Common.encoding);

            dic.Clear();

            foreach (String item in lines)
            {
                String tmpStr = item.Replace("\"", String.Empty);
                String[] tmpArry = tmpStr.Split(new Char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmpArry.Length == 2 && !dic.ContainsKey(tmpArry[0].Trim()))
                {
                    dic.Add(tmpArry[0].Trim(), tmpArry[1].Trim());
                }
            }

            LastUpdateTime = DateTime.Now;
        }
    }
}