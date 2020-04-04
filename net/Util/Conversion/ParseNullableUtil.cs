//***********************************************************************************
//文件名称：ParseNullableUtil.cs
//功能描述：转换为可空类型工具类
//数据表：Nothing
//作者：byron
//日期：2014/9/10 17:59:46
//修改记录：
//***********************************************************************************

using System;

namespace Util.Conversion
{
    /// <summary>
    /// 转换为可空类型工具类
    /// </summary>
    public static class ParseNullableUtil
    {
        /// <summary>
        ///  将对象转换为Byte类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Byte? ParseNullableToByte(Object obj)
        {
            if (obj != null)
            {
                Byte temp = default(Byte);
                if (Byte.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为SByte类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static SByte? ParseNullableToSByte(Object obj)
        {
            if (obj != null)
            {
                SByte temp = default(SByte);
                if (SByte.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为DateTime类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static DateTime? ParseNullableToDateTime(Object obj)
        {
            if (obj != null)
            {
                DateTime temp = default(DateTime);
                if (DateTime.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为guid
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Guid? ParseNullableToGuid(Object obj)
        {
            if (obj != null)
            {
                Guid temp = Guid.Empty;
                if (Guid.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Boolean类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Boolean? ParseNullableToBoolean(Object obj)
        {
            if (obj != null)
            {
                Boolean temp = default(Boolean);
                if (Boolean.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Char类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Char? ParseNullableToChar(Object obj)
        {
            if (obj != null)
            {
                Char temp = default(Char);
                if (Char.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Decimal类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Decimal? ParseNullableToDecimal(Object obj)
        {
            if (obj != null)
            {
                Decimal temp = default(Decimal);
                if (Decimal.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Double类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Double? ParseNullableToDouble(Object obj)
        {
            if (obj != null)
            {
                Double temp = default(Double);
                if (Double.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Int16类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Int16? ParseNullableToInt16(Object obj)
        {
            if (obj != null)
            {
                Int16 temp = default(Int16);
                if (Int16.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为int32类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Int32? ParseNullableToInt32(Object obj)
        {
            if (obj != null)
            {
                Int32 temp = default(Int32);
                if (Int32.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Int64? ParseNullableToInt64(Object obj)
        {
            if (obj != null)
            {
                Int64 temp = default(Int64);
                if (Int64.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为Single类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static Single? ParseNullableToSingle(Object obj)
        {
            if (obj != null)
            {
                Single temp = default(Single);
                if (Single.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为UInt16类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static UInt16? ParseNullableToUInt16(Object obj)
        {
            if (obj != null)
            {
                UInt16 temp = default(UInt16);
                if (UInt16.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为UInt32类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static UInt32? ParseNullableToUInt32(Object obj)
        {
            if (obj != null)
            {
                UInt32 temp = default(UInt32);
                if (UInt32.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }

        /// <summary>
        /// 将对象转换为UInt64类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的数据</returns>
        public static UInt64? ParseNullableToUInt64(Object obj)
        {
            if (obj != null)
            {
                UInt64 temp = default(UInt64);
                if (UInt64.TryParse(obj.ToString(), out temp)) return temp;
            }

            return null;
        }
    }
}
