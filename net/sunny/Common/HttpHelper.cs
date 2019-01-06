//***********************************************************************************
//文件名称：HttpHelper.cs
//功能描述：获取指定url的html信息操作类
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.IO;
using System.Net;
using System.Text;


namespace Sunny.Common
{

    public class HttpHelper
    {

        private static string contentType = "application/x-www-form-urlencoded";
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Zune 4.7; BOIE9;ZHCN)";
        public static string referer = "http://ui.ptlogin2.qq.com/cgi-bin/login?appid=1006102&s_url=http://id.qq.com/index.html";


        /// <summary>  
        /// 获取字符流  
        /// </summary>  
        /// <param name="url"></param>  
        /// <param name="cookieContainer"></param>  
        /// <returns></returns>  
        public static Stream GetStream(string url, CookieContainer cookieContainer, string method, string paras, out string responseCookie, string refer = "")
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            responseCookie = string.Empty;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contentType;

                if (string.IsNullOrEmpty(refer))
                    httpWebRequest.Referer = referer;
                else
                    httpWebRequest.Referer = refer;

                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = method;
                httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

                if (!string.IsNullOrEmpty(paras))
                {
                    byte[] dataArray = System.Text.Encoding.UTF8.GetBytes(paras);
                    httpWebRequest.ContentLength = dataArray.Length;
                    Stream newStream = httpWebRequest.GetRequestStream();
                    newStream.Write(dataArray, 0, dataArray.Length);
                    newStream.Close();
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                responseCookie = httpWebResponse.Headers["Set-Cookie"];

                return responseStream;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>  
        /// 获取HTML  
        /// </summary>  
        /// <param name="url"></param>  
        /// <param name="cookieContainer"></param>  
        /// <returns></returns>  
        public static string GetHtml(string url, CookieContainer cookieContainer, string method, string paras, out string responseCookie, string contenttype = "application/x-www-form-urlencoded", string refer = "")
        {
            responseCookie = string.Empty;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.AllowAutoRedirect = false;
                httpWebRequest.ContentType = contenttype;

                if (string.IsNullOrEmpty(refer))
                    httpWebRequest.Referer = referer;
                else
                    httpWebRequest.Referer = refer;

                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = method;
                httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

                if (!string.IsNullOrEmpty(paras))
                {
                    byte[] dataArray = System.Text.Encoding.UTF8.GetBytes(paras);
                    httpWebRequest.ContentLength = dataArray.Length;
                    Stream newStream = httpWebRequest.GetRequestStream();
                    newStream.Write(dataArray, 0, dataArray.Length);
                    newStream.Close();
                }

                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                string html = streamReader.ReadToEnd();
                responseCookie = httpWebResponse.Headers["Set-Cookie"];
                cookieContainer.Add(httpWebResponse.Cookies);
                streamReader.Close();

                httpWebRequest.Abort();
                httpWebResponse.Close();

                return html;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 获取回发的URI
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="method"></param>
        /// <param name="paras"></param>
        /// <param name="responseCookie"></param>
        /// <param name="contenttype"></param>
        /// <returns></returns>
        public static Uri GetResponseUri(string url, CookieContainer cookieContainer, string method, string paras, out string responseCookie, string contenttype = "application/x-www-form-urlencoded")
        {
            responseCookie = string.Empty;
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            try
            {
                httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                httpWebRequest.CookieContainer = cookieContainer;
                httpWebRequest.ContentType = contenttype;
                httpWebRequest.Referer = referer;
                httpWebRequest.Accept = accept;
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.Method = method;
                httpWebRequest.ServicePoint.ConnectionLimit = int.MaxValue;

                if (!string.IsNullOrEmpty(paras))
                {
                    byte[] dataArray = System.Text.Encoding.UTF8.GetBytes(paras);
                    httpWebRequest.ContentLength = dataArray.Length;
                    Stream newStream = httpWebRequest.GetRequestStream();
                    newStream.Write(dataArray, 0, dataArray.Length);
                    newStream.Close();
                }
                Uri uri = null;
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                uri = httpWebResponse.ResponseUri;
                httpWebResponse.Close();

                return uri;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }

}
