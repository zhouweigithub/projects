// ****************************************
// FileName:GetDataUtil.cs
// Description: Get数据助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Util.Web
{
    using Util.Zlib;

    /// <summary>
    /// Get数据助手类
    /// </summary>
    public static class GetDataUtil
    {
        /// <summary>
        /// 创建HttpWebRequest对象(GET方式)
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="getData">请求的数据</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <returns>HttpWebRequest对象</returns>
        private static HttpWebRequest CreateWebRequest(String url, String getData, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (getData == "" ? "" : "?") + getData);

            //设置HttpWebRequest对象的属性
            request.Method = "GET";

            //设置其它属性
            if (headers != null)
            {
                foreach (String key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }

            if (timeout > 0)
            {
                request.Timeout = timeout;
            }
            else
            {
                request.Timeout = 1000 * 30;
            }

            if (!String.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (!String.IsNullOrEmpty(contentType))
            {
                request.ContentType = contentType;
            }
            else
            {
                request.ContentType = "text/html;charset=UTF-8";
            }

            if (httpVersion != null)
            {
                request.ProtocolVersion = httpVersion;
            }
            else
            {
                request.ProtocolVersion = HttpVersion.Version11;
            }

            if (connectionLimit > 0)
            {
                request.ServicePoint.ConnectionLimit = connectionLimit;
            }
            else
            {
                request.ServicePoint.ConnectionLimit = 3000;
            }

            return request;
        }

        /// <summary>
        /// 通过GET方式提交数据到url
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="getData">请求的参数</param>
        /// <param name="compress">数据压缩枚举</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>从页面返回的数据</returns>
        internal static String GetWebData(String url, String getData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException("Empty String", "url can't be null or empty.");

            //构造HttpWebRequest对象
            HttpWebRequest request = CreateWebRequest(url, getData, headers, timeout, userAgent, contentType, httpVersion);

            //获得请求的回应
            using (WebResponse response = request.GetResponse())
            {
                //如果数据进行了压缩，则进行解压缩后再返回；否则直接返回
                if (compress == DataCompress.Compress)
                {
                    //定义返回的Byte[]
                    Byte[] returnByteArray = null;

                    //构造流读取对象
                    using (BinaryReader reader = new BinaryReader(response.GetResponseStream()))
                    {
                        returnByteArray = new Byte[response.ContentLength];
                        returnByteArray = reader.ReadBytes(returnByteArray.Length);
                    }

                    //解压缩
                    Byte[] uncompBytes = ZlibUtil.Uncompress(returnByteArray);

                    //转换为字符串
                    return UTF8Encoding.UTF8.GetString(uncompBytes);
                }
                else
                {
                    //构造流读取对象
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //返回获得的请求数据
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}