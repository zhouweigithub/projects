using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ResourceSpider
{
    internal class Comm
    {
        /// <summary>
        /// 所有链接
        /// </summary>
        private const String linkRegString = "<a .*?href=[\"'](?<url>.*?)[\"']";
        /// <summary>
        /// 所有链接
        /// </summary>
        private static readonly Regex linkReg = new Regex(linkRegString);

        /// <summary>
        /// 同时在屏幕和文件中输出日志
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public static void WriteLog(String msg, Util.Log.LogType logType)
        {
            Console.WriteLine(msg);
            Util.Log.LogUtil.Write(msg, logType);
        }

        /// <summary>
        /// 获取url地址的html内容
        /// </summary>
        /// <param name="url">url地址，缺少http时，会自动补上</param>
        /// <param name="timeout">超时时间（秒）</param>
        /// <returns></returns>
        public static String GetHtml(String url, Int32 timeout)
        {
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            return Hswz.Common.HttpHelper.GetHtml(url, null, "get", null, out _, timeout * 1000);
        }

        /// <summary>
        /// 获取内容中的链接地址，已去重，仅针对<a>标签内hrf属性
        /// </summary>
        /// <param name="content">页面内容</param>
        /// <param name="host">主域名（可空，如果不为空，则会在相对地址前面补上主域名）</param>
        /// <returns></returns>
        public static List<String> GetUrls(String content, String host)
        {
            List<String> urls = new List<String>();
            var matches = linkReg.Matches(content);
            foreach (Match item in matches)
            {
                String url = item.Groups["url"].Value;
                if (IsUrlValid(url))
                {
                    if (!url.StartsWith("http"))
                    {
                        url = host + url;
                    }

                    if (!urls.Contains(url))
                    {
                        urls.Add(url);
                    }
                }
            }

            return urls;
        }

        /// <summary>
        /// 判断URL地址是否合法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Boolean IsUrlValid(String url)
        {
            return !url.StartsWith("javascript:") && url != "/" && !url.StartsWith("mailto:") && !url.StartsWith("tell:") && !url.StartsWith("#");
        }

        /// <summary>
        /// 获取地址的主域名，带http前缀
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String GetUrlHost(String url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                return String.Empty;
            }

            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    return uri.Scheme + "://" + uri.Host;
                }
            }

            return String.Empty;
        }
    }
}
