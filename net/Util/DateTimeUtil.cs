// ****************************************
// FileName:DateTimeUtil.cs
// Description: 时间助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;

namespace Util
{
    /// <summary>
    /// 时间助手类
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// 获得int型的日期（如20140812）
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>int型的日期</returns>
        public static Int32 DateTimeToInt32(DateTime time)
        {
            return time.Year * 10000 + time.Month * 100 + time.Day;
        }

        /// <summary>
        /// 获取格林威治时间格式字符串(yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        /// <returns>格林威治时间格式字符串</returns>
        public static String GetGreenWichTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取格林威治时间格式字符串(yyyy-MM-dd)
        /// </summary>
        /// <param name="time">需要转换的时间</param>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        /// <returns>格林威治时间格式字符串</returns>
        public static String GetShortGreenWichTime(DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取指定时间之间的月数
        /// </summary>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>间隔的月数</returns>
        public static Int32 GetIntervalMonths(DateTime begin, DateTime end)
        {
            if (begin > end) throw new ArgumentException("Error", "begin time can't be later than end time.");

            //获取间隔的年数和月数，并计算最终的结果
            return 12 * (end.Year - begin.Year) + (end.Month - begin.Month);
        }

        /// <summary>
        /// 获取指定时间之间的天数差
        /// </summary>
        /// <param name="begin">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>间隔的天数</returns>
        public static Int32 GetIntervalDays(DateTime begin, DateTime end)
        {
            if (begin > end) throw new ArgumentException("Error", "begin time can't be later than end time.");
 
            return (Int32)System.Math.Floor((end.Date - begin.Date).TotalDays);
        }

        /// <summary>
        /// 获取1970-1-1 00:00:00至指定时间的时间戳（单位：秒）
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>时间戳（单位：秒）</returns>
        public static Int64 GetUnixTimeStamp(DateTime time)
        {
            //获得基础时间
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            if (startTime > time) throw new ArgumentNullException("Error", "time can't be earlier than 1970-1-1 00:00:00.");

            return (Int64)System.Math.Round((time - startTime).TotalSeconds, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取1970-1-1 00:00:00至指定时间的时间戳（单位：秒）
        /// </summary>
        /// <param name="time">指定时间</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <returns>时间戳（单位：秒）</returns>
        public static Int32 GetUnixTimeStampX32(DateTime time)
        {
            //获得基础时间
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            if (startTime > time) throw new ArgumentNullException("Error", "time can't be earlier than 1970-1-1 00:00:00.");

            return (Int32)System.Math.Round((time - startTime).TotalSeconds, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 将时间部分转化为小数（如9:30->9.5）
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>转化后的小数</returns>
        public static Double ConvertTimeToDecimal(DateTime time)
        {
            return time.Hour + (Double)time.Minute / 60;
        }

        /// <summary>
        /// 将一周中的每天的枚举值转化为数字
        /// </summary>
        /// <param name="dayOfWeek">一周中的每天的枚举值</param>
        /// <returns>1-7</returns>
        public static Int32 FormatDate(DayOfWeek dayOfWeek)
        {
            Int32 weekday = 1;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    weekday = 1;
                    break;
                case DayOfWeek.Tuesday:
                    weekday = 2;
                    break;
                case DayOfWeek.Wednesday:
                    weekday = 3;
                    break;
                case DayOfWeek.Thursday:
                    weekday = 4;
                    break;
                case DayOfWeek.Friday:
                    weekday = 5;
                    break;
                case DayOfWeek.Saturday:
                    weekday = 6;
                    break;
                case DayOfWeek.Sunday:
                    weekday = 7;
                    break;
            }

            return weekday;
        }
    }
}