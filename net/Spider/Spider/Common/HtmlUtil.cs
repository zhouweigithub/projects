using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider.Common
{
    class HtmlUtil
    {
        private static readonly Regex urlStartReg = new Regex("^(/|\\\\)\\w+");

        /// <summary>
        /// 获取源代码（UTF8格式）
        /// </summary>
        /// <param name="url">地址（以http(s)开头）</param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            return GetHtml(url, Encoding.UTF8);
        }

        /// <summary>
        /// 获取源代码
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="encoding">编码格式</param>
        /// <returns></returns>
        public static string GetHtml(string url, Encoding encoding)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                url = ValidUrl(url);

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                request.AllowAutoRedirect = false;

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding);
                    else
                        reader = new StreamReader(response.GetResponseStream(), encoding);
                    string html = reader.ReadToEnd();

                    return html;
                }
            }
            catch
            {
            }
            finally
            {

                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();

                if (request != null)
                    request = null;

            }

            return string.Empty;
        }

        /// <summary>
        /// 获取网页的编码格式
        /// </summary>
        /// <param name="url">地址（以http(s)开头）</param>
        /// <returns></returns>
        public static string GetEncoding(string url)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                url = ValidUrl(url);

                request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 20000;
                request.AllowAutoRedirect = true;

                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK && response.ContentLength < 1024 * 1024)
                {
                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                    else
                        reader = new StreamReader(response.GetResponseStream(), Encoding.ASCII);

                    string html = reader.ReadToEnd();

                    Regex reg_charset = new Regex(@"charset\b\s*=\s*(?<charset>[^""]*)");
                    if (reg_charset.IsMatch(html))
                    {
                        return reg_charset.Match(html).Groups["charset"].Value;
                    }
                    else if (response.CharacterSet != string.Empty)
                    {
                        return response.CharacterSet;
                    }
                    else
                        return Encoding.Default.BodyName;
                }
            }
            catch
            {
            }
            finally
            {

                if (response != null)
                {
                    response.Close();
                    response = null;
                }
                if (reader != null)
                    reader.Close();

                if (request != null)
                    request = null;

            }

            return string.Empty;
        }

        /// <summary>
        /// 将不合法的URL修正为合法的URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="scheme">http/https</param>
        /// <returns></returns>
        public static string ValidUrl(string url, string scheme = "", string domain = "")
        {
            if (url.Contains("javascript") || url.Contains("("))
                url = string.Empty;

            url = url.Replace("\\", "/");

            //如果是相对路径，则加上域名
            if (urlStartReg.IsMatch(url) && !string.IsNullOrWhiteSpace(domain))
                url = domain + url;

            url = url.Replace(" ", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);

            url = url.TrimStart("/#;+-=*&$%@!~:'\"?><.,\\".ToArray());

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (!string.IsNullOrWhiteSpace(scheme))
                    {
                        url = scheme + "://" + url;
                    }
                    else
                    {
                        url = "https://" + url;
                    }
                }
            }

            return url;
        }
    }
}
