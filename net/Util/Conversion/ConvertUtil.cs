//***********************************************************************************
//文件名称：ConvertHelper.cs
//功能描述：转换辅助类，将object对象转换为对应的类型
//数据表：Nothing
//作者：byron
//日期：2014/3/27 14:30:55
//修改记录：
//***********************************************************************************

using System;

namespace Util.Conversion
{
    /// <summary>
    /// 转换辅助类，将object对象转换为对应的类型
    /// </summary>
    public static class ConvertUtil
    {
        /// <summary>
        /// 将对象转换为String类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <param name="allowEmpty">是否允许空值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToString(Object obj, out String result, Boolean allowEmpty = true)
        {
            result = default(String);
            Boolean isOK = false;

            if (obj != null)
            {
                result = obj.ToString();
                isOK = true;

                //处理是否为空的逻辑
                if (allowEmpty == false && String.IsNullOrWhiteSpace(result))
                {
                    isOK = false;
                    result = default(String);
                }
            }

            return isOK;
        }

        /// <summary>
        ///  将对象转换为Byte类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToByte(Object obj, out Byte result)
        {
            result = default(Byte);
            return obj == null ? false : Byte.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为SByte类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToSByte(Object obj, out SByte result)
        {
            result = default(SByte);
            return obj == null ? false : SByte.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为DateTime类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToDateTime(Object obj, out DateTime result)
        {
            result = default(DateTime);
            return obj == null ? false : DateTime.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Guid类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <param name="allowEmpty">是否允许空值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToGuid(Object obj, out Guid result, Boolean allowEmpty = false)
        {
            result = default(Guid);
            Boolean isOK = false;

            if (obj != null)
            {
                isOK = Guid.TryParse(obj.ToString(), out result);
                
                //处理是否为空的逻辑
                if (allowEmpty == true && result == default(Guid))
                {
                    isOK = true;
                    result = default(Guid);
                }
            }

            return isOK;
        }

        /// <summary>
        /// 将对象转换为Boolean类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToBoolean(Object obj, out Boolean result)
        {
            result = false;
            return obj == null ? false : Boolean.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Char类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToChar(Object obj, out Char result)
        {
            result = default(Char);
            return obj == null ? false : Char.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Decimal类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToDecimal(Object obj, out Decimal result)
        {
            result = default(decimal);
            return obj == null ? false : Decimal.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Double类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToDouble(Object obj, out Double result)
        {
            result = default(Double);
            return obj == null ? false : Double.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Int16类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToInt16(Object obj, out Int16 result)
        {
            result = default(Int16);
            return obj == null ? false : Int16.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为int32类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToInt32(Object obj, out Int32 result)
        {
            result = default(Int32);
            return obj == null ? false : Int32.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Int64类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToInt64(Object obj, out Int64 result)
        {
            result = default(Int64);
            return obj == null ? false : Int64.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为Single类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToSingle(Object obj, out Single result)
        {
            result = default(Single);
            return obj == null ? false : Single.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为UInt16类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToUInt16(Object obj, out UInt16 result)
        {
            result = default(UInt16);
            return obj == null ? false : UInt16.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为UInt32类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToUInt32(Object obj, out UInt32 result)
        {
            result = default(UInt32);
            return obj == null ? false : UInt32.TryParse(obj.ToString(), out result);
        }

        /// <summary>
        /// 将对象转换为UInt64类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="result">要保存转换的值，如果转换成功，则为转换后的值；否则为默认值</param>
        /// <returns>如果转换成功，则为true，否则为false</returns>
        public static Boolean TryParseToUInt64(Object obj, out UInt64 result)
        {
            result = default(UInt64);
            return obj == null ? false : UInt64.TryParse(obj.ToString(), out result);
        }

    }
}