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

namespace Sunny.Common
{
    /// <summary>
    /// 短信验证码使用类型
    /// </summary>
    public enum SmsVerificationCodeTypeEnum
    {
        /// <summary>
        /// 学员注册
        /// </summary>
        StudentRegister,
        /// <summary>
        /// 教练注册
        /// </summary>
        CoachRegister,
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 学员
        /// </summary>
        Student = 0,
        /// <summary>
        /// 教练
        /// </summary>
        Coach = 1
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