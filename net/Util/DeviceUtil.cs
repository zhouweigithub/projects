//***********************************************************************************
//文件名称：DeviceUtil.cs
//功能描述：设备助手类
//数据表：Nothing
//作者：Jordan
//日期：2014-03-25 11:34:00
//修改记录：
//***********************************************************************************

using System;

namespace Util
{
    /// <summary>
    /// 设备助手类
    /// </summary>
    public static class DeviceUtil
    {
        /// <summary>
        /// 将MAC地址转化为标准格式
        /// </summary>
        /// <param name="mac">mac</param>
        /// <returns>标准格式的MAC</returns>
        public static String ConvertMACToStandardFormat(String mac)
        {
            //如果是空或默认值，则返回String.Empty
            if (String.IsNullOrEmpty(mac)
                || mac.Equals("00:00:00:00:00:00", StringComparison.Ordinal)
                || mac.Equals("02:00:00:00:00:00", StringComparison.Ordinal)
            )
            {
                return String.Empty;
            }

            //如果mac的长度不为12或17，则是不正确的格式
            if (mac.Length != 12 && mac.Length != 17) return String.Empty;

            //转化为大写
            mac = mac.ToUpper();

            //如果mac地址的长度为17（已经有:），则直接返回
            if (mac.Length == 17) return mac;

            //如果没有分隔符，则添加分隔符
            String newMac = String.Empty;
            for (Int32 i = 0; i < mac.Length; i++)
            {
                newMac += mac[i];
                if (i < mac.Length - 1 && i % 2 == 1)
                {
                    newMac += ":";
                }
            }

            return newMac;
        }

        /// <summary>
        /// 将IDFA地址转化为标准格式
        /// </summary>
        /// <param name="idfa">IDFA</param>
        /// <returns>标准格式的IDFA</returns>
        public static String ConvertIDFAToStandardFormat(String idfa)
        {
            //如果是空或默认值，则返回String.Empty
            if (String.IsNullOrEmpty(idfa) || idfa.Equals("00000000-0000-0000-0000-000000000000", StringComparison.Ordinal)) return String.Empty;

            //如果idfa的长度不为32或36，则代表是Android的数据，则可以直接返回
            if (idfa.Length != 32 && idfa.Length != 36) return idfa;

            //转化为大写
            idfa = idfa.ToUpper();

            //如果idfa地址的长度为36（已经有:），则直接返回
            if (idfa.Length == 36) return idfa;

            //如果没有分隔符，则添加分隔符
            String newIdfa = String.Empty;
            for (Int32 i = 0; i < idfa.Length; i++)
            {
                newIdfa += idfa[i];
                if (i == 7 || i == 11 || i == 15 || i == 19)
                {
                    newIdfa += "-";
                }
            }

            return newIdfa;
        }

        /// <summary>
        /// 根据MAC和IDFA获取唯一标识
        /// </summary>
        /// <param name="mac">设备的mac</param>
        /// <param name="idfa">idfa</param>
        /// <returns>唯一标识</returns>
        public static String GetIdentifier(String mac, String idfa)
        {
            //将MAC和IDFA转化为标准格式
            mac = ConvertMACToStandardFormat(mac);
            idfa = ConvertIDFAToStandardFormat(idfa);

            //如果idfa不为空，则使用idfa，否则使用mac
            return !String.IsNullOrEmpty(idfa) ? idfa : mac;
        }
    }
}