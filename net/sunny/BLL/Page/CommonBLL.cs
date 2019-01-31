using Sunny.Common;
using Sunny.DAL;
using Sunny.Model;
using Util.Log;
using Util.Web;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Sunny.Model.Common;

namespace Sunny.BLL.Page
{
    public class CommonBLL
    {
        /// <summary>
        /// 从缓存中获取短信验证码
        /// </summary>
        /// <param name="type">短信用途</param>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public static string GetSmsVerificationCodeFromCache(SmsVerificationCodeTypeEnum type, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            string key = Enum.GetName(typeof(SmsVerificationCodeTypeEnum), type) + "_" + phone;
            if (MemoryCacheManager.IsSet(key))
            {
                return MemoryCacheManager.Get<string>(key);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 创建短信验证码，并存入缓存，有效时间5分钟
        /// </summary>
        /// <param name="type">短信用途</param>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public static string CreateSmsVerificationCode(SmsVerificationCodeTypeEnum type, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return string.Empty;

            string key = Enum.GetName(typeof(SmsVerificationCodeTypeEnum), type) + "_" + phone;
            string verificationCode = Function.GetRangeCharaters(4, RangeType.Number);
            MemoryCacheManager.Set(key, verificationCode, 5);
            return verificationCode;
        }
    }


}
