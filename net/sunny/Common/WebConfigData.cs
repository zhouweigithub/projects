//***********************************************************************************
//文件名称：WebConfigData.cs
//功能描述：web.config文件中配置的信息
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;

namespace Sunny.Common
{
    public class WebConfigData
    {

        /// <summary>
        /// 数据库类型：mysql
        /// </summary>
        public static readonly string DataBaseType;
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public static readonly string ConnString;
        /// <summary>
        /// JS版本号
        /// </summary>
        public static readonly string Ver;
        /// <summary>
        /// 显示特殊页面的用户名集
        /// </summary>
        public static readonly string ExtraUserNames;
        /// <summary>
        /// 特殊页面的ID集
        /// </summary>
        public static readonly string ExtraPageMenuIds;
        /// <summary>
        /// 不需要验证手机号就能登录的ip
        /// </summary>
        public static readonly string IgnoreSmsCodeIp;
        /// <summary>
        /// 邀请返现金额
        /// </summary>
        public static readonly int CashbackMoney;
        /// <summary>
        /// 小程序APPID
        /// </summary>
        public static readonly string MiniAppid;
        /// <summary>
        /// 小程序密钥
        /// </summary>
        public static readonly string MiniSecret;

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
                CashbackMoney = Config.GetConfigToInt("CashbackMoney");
                MiniAppid = Config.GetConfigToString("MiniAppid");
                MiniSecret = Config.GetConfigToString("MiniSecret");
            }
            catch (Exception e)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取webconfig数据时出错\t" + e.Message);
            }
        }

    }
}
