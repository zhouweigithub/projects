//***********************************************************************************
//文件名称：WebConfigData.cs
//功能描述：web.config文件中配置的信息
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;

namespace Spetmall.Common
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
        /// 小票打印机名字
        /// </summary>
        public static readonly string ReceiptPrinterName;
        /// <summary>
        /// 小票上的店名
        /// </summary>
        public static readonly string ReceiptPrinterShopName;
        /// <summary>
        /// 小票上的手机号
        /// </summary>
        public static readonly string ReceiptPrinterPhone;

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
                ReceiptPrinterName = Config.GetConfigToString("ReceiptPrinterName");
                ReceiptPrinterShopName = Config.GetConfigToString("ReceiptPrinterShopName");
                ReceiptPrinterPhone = Config.GetConfigToString("ReceiptPrinterPhone");
            }
            catch (Exception e)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "读取webconfig数据时出错\t" + e.Message);
            }
        }

    }
}
