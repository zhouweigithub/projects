// ****************************************
// FileName:WebUtil.cs
// Description: Web助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Web;
using System.Collections.Generic;

namespace Util.Web
{
    /// <summary>
    /// Web助手类
    /// </summary>
    public static class WebUtil
    {
        /// <summary>
        /// 获取Http请求的IP地址
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <param name="ifAllowDelegate">是否允许代理</param>
        /// <returns>IP地址</returns>
        public static String GetRequestIP(HttpRequest request, Boolean ifAllowDelegate = true)
        {
            //如果允许代理，则获取代理中的IP地址
            if(ifAllowDelegate)
            {
                String str = String.Empty;
                if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    str = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    if (str.Length >= 10)
                    {
                        String[] strArray = str.Split(new Char[] { ',' });
                        if (strArray.Length > 0)
                        {
                            str = strArray[0].ToString().Trim();
                        }
                    }
                    if ((str.Length > 0) && (str.Length <= 15))
                    {
                        return str;
                    }
                }
                if (request.Headers["X-Real-IP"] != null)
                {
                    return request.Headers["X-Real-IP"].ToString();
                }
            }

            return request.UserHostAddress.ToString();
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
        public static String GetWebData(String url, String getData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null)
        {
            return GetDataUtil.GetWebData(url, getData, compress, headers, timeout, userAgent, contentType, httpVersion);
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
        public static String PostWebData(String url, String postData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostDataUtil.PostWebData(url, postData, compress, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
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
        public static String PostWebData(String url, Byte[] postData, DataCompress compress, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostDataUtil.PostWebData(url, postData, compress, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
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
        ///<exception cref="ArgumentNullException"></exception>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>从页面返回的数据</returns>
        public static Byte[] PostWebData(String url, String postData, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostDataUtil.PostWebData(url, postData, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
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
        public static Byte[] PostWebData(String url, Byte[] postData, Dictionary<String, String> headers = null,
            Int32 timeout = 0, String userAgent = "", String contentType = "", Version httpVersion = null, Int32 connectionLimit = 0)
        {
            return PostDataUtil.PostWebData(url, postData, headers, timeout, userAgent, contentType, httpVersion, connectionLimit);
        }
    }
}