//***********************************************************************************
//文件名称：PayTypes.cs
//功能描述：游戏内支付方式实体类
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;

namespace Spetmall.Common
{
    public enum GameVersion
    {
        /// <summary>
        /// 安卓
        /// </summary>
        ANDROID,
        /// <summary>
        /// IOS
        /// </summary>
        IOS,
        /// <summary>
        /// 未知版本
        /// </summary>
        UNDEFINED
    }

    [Flags]
    public enum PayTypes
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        flag_Alipay = 1,
        /// <summary>
        /// 微信
        /// </summary>
        flag_WeiXin = 2,
        /// <summary>
        /// 银联
        /// </summary>
        flag_Union = 4,
        /// <summary>
        /// appStore
        /// </summary>
        flag_AppStore = 8,
        /// <summary>
        /// googlePlay
        /// </summary>
        flag_GooglePlay = 16,
        /// <summary>
        /// AlipayWeb
        /// </summary>
        flag_AlipayWeb = 32,
        /// <summary>
        /// 微信H5
        /// </summary>
        flag_weixin_h5 = 64,
    }

    public class PayTypesHelper
    {
        /// <summary>
        /// 获取支付方式值与中文名称的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetPayTypesValueAndChineseTextDictionary()
        {
            Dictionary<int, string> result = GetPayTypesDictionary();
            result[1] = "支付宝支付";
            result[2] = "微信支付";
            result[4] = "银联支付";
            result[8] = "appStore支付";
            result[16] = "googlePlay支付";
            result[32] = "支付宝网页支付";
            result[64] = "微信H5支付";

            return result;
        }

        /// <summary>
        /// 获取支付方式值与名称的集合
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetPayTypesDictionary()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            string[] typeNames = Enum.GetNames(typeof(PayTypes));
            Array typeValues = Enum.GetValues(typeof(PayTypes));
            for (int i = 0; i < typeNames.Length; i++)
            {
                result.Add(Convert.ToInt32(typeValues.GetValue(i)), typeNames[i]);
            }

            return result;
        }

        /// <summary>
        /// 根据充值方式的值，获取所有的充值方式的中文名称列表 
        /// </summary>
        /// <param name="payType"></param>
        /// <returns></returns>
        public static List<string> GetPayTypeList(int payType)
        {
            List<string> result = new List<string>();

            IEnumerable<PayTypes> types = EnumHelper.GetEnumValuesFromFlagsEnum<PayTypes>(payType);
            Dictionary<int, string> chinesesNames = PayTypesHelper.GetPayTypesValueAndChineseTextDictionary();
            foreach (PayTypes item in types)
            {
                if (chinesesNames.ContainsKey((int)item))
                    result.Add(chinesesNames[(int)item]);
            }

            return result;
        }
    }

    public class EnumHelper
    {
        /// <summary>
        /// 根据某个数值，获取Flags枚举所代表的所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnumValuesFromFlagsEnum<T>(Int32 value) where T : struct
        {
            T[] values = (T[])Enum.GetValues(typeof(T));

            foreach (var itemValue in values)
            {
                if ((value & Convert.ToInt32(itemValue)) != 0)
                {
                    yield return itemValue;
                }
            }
        }
    }
}