//***********************************************************************************
//文件名称：Converter.cs
//功能描述：Mysql时间戳转换为.net时间操作类
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;

namespace Hswz.Common
{
    public class Converter
    {
        /// <summary>
        /// 将时间转换为 mysql 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="length">时间戳数字长度</param>
        /// <returns></returns>
        public static string ConvertToMySqlTimeStamp(DateTime time, int length = 10)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = time.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, length);
            return timeStamp;
        }

        /// <summary>
        /// 将时间转换为 mysql 时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <param name="length">时间戳数字长度</param>
        /// <returns></returns>
        public static string ConvertToMySqlTimeStamp(string time, int length = 10)
        {
            DateTime aimTime = DateTime.Parse(time);
            string timeStamp = ConvertToMySqlTimeStamp(aimTime, length);
            return timeStamp;
        }

        /// <summary>
        /// 将mysql的10位数字 timestamp 转换为时间类型
        /// </summary>
        /// <param name="mysqlTimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string mysqlTimeStamp)
        {
            long lTime = long.Parse(mysqlTimeStamp + "0000000");
            DateTime dtResult = ConvertToDateTime(lTime);
            return dtResult;
        }

        /// <summary>
        /// 将mysql的10位数字 timestamp 转换为时间类型
        /// </summary>
        /// <param name="mysqlTimeStamp"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(long mysqlTimeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = mysqlTimeStamp * 10000000;
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }
    }
}
