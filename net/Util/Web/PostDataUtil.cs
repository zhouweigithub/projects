// ****************************************
// FileName:PostDataUtil.cs
// Description: Post数据助手类
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
    /// Post数据助手类
    /// </summary>
    public static class PostDataUtil
    {
        /// <summary>
        /// 创建HttpWebRequest对象(POST方式)
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="byteArray">发送的字节数组</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <returns>HttpWebRequest对象</returns>
        private static HttpWebRequest CreateWebRequest(String url, Byte[] byteArray, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            //构造HttpWebRequest对象
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            //设置HttpWebRequest对象的属性
            request.Method = "POST";
            request.ContentLength = byteArray.Length;
            
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
                request.ContentType = "application/x-www-form-urlencoded";
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

            //发出请求，并将数据发送到远程服务器
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }

            return request;
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">目的地的Url</param>
        /// <param name="postData">发送的数据</param>
        /// <param name="compress">数据压缩枚举</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <exception cref="ArgumentNullException">Thrown when url or postData is null or empty</exception>
        /// <exception cref="System.Text.EncoderFallbackException">System.Text.EncoderFallbackException</exception>
        /// <returns>从页面返回的数据</returns>
        internal static String PostWebData(String url, String postData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostWebData(url, Encoding.UTF8.GetBytes(postData), compress, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">目的地的Url</param>
        /// <param name="postData">发送的数据</param>
        /// <param name="compress">数据压缩枚举</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>从页面返回的数据</returns>
        internal static String PostWebData(String url, Byte[] postData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException("Empty String", "url can't be null or empty.");

            //构造HttpWebRequest对象
            HttpWebRequest request = CreateWebRequest(url, postData, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);

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

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">目的地的Url</param>
        /// <param name="postData">发送的数据</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <exception cref="ArgumentNullException">Thrown when url or postData is null or empty</exception>
        /// <exception cref="System.Text.EncoderFallbackException">System.Text.EncoderFallbackException</exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>从页面返回的数据</returns>
        internal static Byte[] PostWebData(String url, String postData, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostWebData(url, Encoding.UTF8.GetBytes(postData), headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
        }

        /// <summary>
        /// Post数据
        /// </summary>
        /// <param name="url">目的地的Url</param>
        /// <param name="postData">发送的数据</param>
        /// <param name="headers">request头部信息</param>
        /// <param name="timeout">超时的毫秒数</param>
        /// <param name="userAgent">用户代理数据</param>
        /// <param name="contentType">内容类型信息</param>
        /// <param name="httpVersion">HTTP版本</param>
        /// <param name="connectionLimit">连接数量限制</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>从页面返回的数据</returns>
        internal static Byte[] PostWebData(String url, Byte[] postData, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            if (String.IsNullOrEmpty(url)) throw new ArgumentNullException("Empty String", "url can't be null or empty.");

            //构造HttpWebRequest对象
            HttpWebRequest request = CreateWebRequest(url, postData, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);

            //获得请求的回应
            using (WebResponse response = request.GetResponse())
            {
                //定义返回的Byte[]
                Byte[] returnByteArray = null;

                //构造流读取对象
                using (BinaryReader reader = new BinaryReader(response.GetResponseStream()))
                {
                    returnByteArray = new Byte[response.ContentLength];
                    returnByteArray = reader.ReadBytes(returnByteArray.Length);
                }

                return returnByteArray;
            }
        }
    }
}