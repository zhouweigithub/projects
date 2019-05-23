//***********************************************************************************
//文件名称：ConvertHelper.cs
//功能描述：转换辅助类，将object对象转换为对应的类型
//数据表：Nothing
//作者：Jordan
//日期：2014/3/27 14:30:55
//修改记录：
//***********************************************************************************

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Util
{
    /// <summary>
    /// Marshal辅助类，用于扩展Marshal中未直接实现的方法
    /// </summary>
    public static class MarshalUtil
    {
        /// <summary>
        /// 将IntPtr指向的UTF8数据转化成字符数组
        /// </summary>
        /// <param name="ptr">指针</param>
        /// <param name="length">数据长度</param>
        /// <returns>字符串</returns>
        public static Byte[] PtrToByteArray(IntPtr ptr, Int32 length)
        {
            Byte[] byteArray = new Byte[length];

            //遍历读取二进制（由于Marshal只提供读取单个字符的方法，而不提供读取连续长度的字符方法，所以需要循环读取）
            for (Int32 i = 0; i < length; i++)
            {
                byteArray[i] = Marshal.ReadByte(ptr, i);
            }

            return byteArray;
        }

        /// <summary>
        /// 将IntPtr指向的UTF8数据转化成字符串
        /// </summary>
        /// <param name="ptr">指针</param>
        /// <param name="length">数据长度</param>
        /// <returns>字符串</returns>
        public static String PtrToStringUTF8(IntPtr ptr, Int32 length)
        {
            Byte[] byteArray = new Byte[length];

            //遍历读取二进制（由于Marshal只提供读取单个字符的方法，而不提供读取连续长度的字符方法，所以需要循环读取）
            for (Int32 i = 0; i < length; i++)
            {
                byteArray[i] = Marshal.ReadByte(ptr, i);
            }

            //转化成字符串
            return Encoding.UTF8.GetString(byteArray);
        }
    }
}