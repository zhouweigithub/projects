//***********************************************************************************
//文件名称：WebConfigData.cs
//功能描述：web.config文件中配置的信息
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;

namespace Hswz.Common
{
    public class WebConfigData
    {

        /// <summary>
        /// 数据库类型：mysql
        /// </summary>
        public static readonly String DataBaseType;
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public static readonly String ConnString;
        /// <summary>
        /// JS版本号
        /// </summary>
        public static readonly String Ver;
        /// <summary>
        /// 显示特殊页面的用户名集
        /// </summary>
        public static readonly String ExtraUserNames;
        /// <summary>
        /// 特殊页面的ID集
        /// </summary>
        public static readonly String ExtraPageMenuIds;
        /// <summary>
        /// 不需要验证手机号就能登录的ip
        /// </summary>
        public static readonly String IgnoreSmsCodeIp;

        /// <summary>
        /// 检索的关键字信息用空格分隔，每组用逗号分隔
        /// </summary>
        public static readonly String SearchKeyWords;
        /// <summary>
        /// 分页链接从哪里获取（1从网站上抓取，2从数据库里取已存在的）
        /// </summary>
        public static readonly String UrlFormatFrom;
        /// <summary>
        /// 获取每页中数据详情的方式（1获取全部，2只获取前几页）
        /// </summary>
        public static readonly String GetDetailType;


        static WebConfigData()
        {
            try
            {
                DataBaseType = Config.GetConfigToString("DataBaseType");
                ConnString = Config.GetConfigToString("ConnString");
                Ver = Config.GetConfigToString("Ver");
                ExtraUserNames = Config.GetConfigToString("ExtraUserNames");
                ExtraPageMenuIds = Config.GetConfigToString("ExtraPageMenuIds");
                IgnoreSmsCodeIp = Config.GetConfigToString("IgnoreSmsCodeIp");
                SearchKeyWords = Config.GetConfigToString("SearchKeyWords");
                UrlFormatFrom = Config.GetConfigToString("UrlFormatFrom");
                GetDetailType = Config.GetConfigToString("GetDetailType");
            }
            catch (Exception e)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取webconfig数据时出错\t" + e.Message);
            }
        }

    }
}
