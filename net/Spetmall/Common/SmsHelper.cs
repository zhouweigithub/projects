using System;
using System.Security.Cryptography;
using System.Text;

namespace Spetmall.Common
{
    /// <summary>
    /// 发送短信辅助类
    /// </summary>
    public class SmsHelper
    {

        /// <summary>
        ///  短信应用SDK AppID
        /// </summary>
        static readonly int _appid = 1400027050;
        /// <summary>
        /// 短信应用SDK AppKey
        /// </summary>
        static readonly string _appkey = "a4bfbf5df3043ee858dd21a8ae80d8be";
        /// <summary>
        /// 发送短信服务地址
        /// </summary>
        static readonly string _urlFormat = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid={0}&random={1}";
        /// <summary>
        /// 预定义好的短信内容
        /// </summary>
        static readonly string _content = "尊敬的用户您的短信验证码为{0}，验证码时间为10分钟内有效，请尽快完成填报，感谢您的支持";

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <param name="vcode">验证码</param>
        /// <returns></returns>
        public static SmsSendResult SingleSend(string phone, string vcode)
        {
            try
            {
                long random = GetRandom();
                long time = long.Parse(Converter.ConvertToMySqlTimeStamp(DateTime.Now));
                string sigString = string.Format("appkey={0}&random={1}&time={2}&mobile={3}", _appkey, random, time, phone);
                SmsRequest request = new SmsRequest()
                {
                    tel = new SmsTelInfo()
                    {
                        nationcode = "86",
                        mobile = phone
                    },
                    msg = string.Format(_content, vcode),
                    sig = SHA256(sigString),
                    ext = "",
                    extend = "",
                    type = 0,
                    time = time
                };

                string url = string.Format(_urlFormat, _appid, random);
                string requestString = Util.Json.JsonUtil.Serialize(request);
                string responseString = Util.Web.WebUtil.PostWebData(url, requestString, Util.Web.DataCompress.NotCompress);
                SmsSendResult result = Util.Json.JsonUtil.Deserialize<SmsSendResult>(responseString);
                return result;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(string.Format("发送短信失败：{0}  ,  {1}  \r\n{2}", phone, vcode, e.ToString()), Util.Log.LogType.Error);
            }

            return null;
        }

        /// <summary>
        /// sha256加密
        /// </summary>
        /// <param name="rawString">原文</param>
        /// <returns>密文</returns>
        public static string SHA256(string rawString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(rawString);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }

        private static long GetCurrentTime()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private static long GetRandom()
        {
            return (new Random((int)GetCurrentTime())).Next() % 900000L + 100000L;
        }

        /// <summary>
        /// 短信发送参数
        /// </summary>
        public class SmsRequest
        {
            /// <summary>
            /// 如需使用国际电话号码通用格式，如："+8613788888888" ，请使用sendisms接口见下注
            /// </summary>
            public SmsTelInfo tel;
            /// <summary>
            /// 0:普通短信;1:营销短信（强调：要按需填值，不然会影响到业务的正常使用）
            /// </summary>
            public int type;
            /// <summary>
            /// utf8编码，需要匹配审核通过的模板内容 
            /// </summary>
            public string msg;
            /// <summary>
            /// app凭证，sha256加密
            /// </summary>
            public string sig;
            /// <summary>
            /// unix时间戳，请求发起时间，如果和系统时间相差超过10分钟则会返回失败
            /// </summary>
            public long time;
            /// <summary>
            /// 通道扩展码，可选字段，默认没有开通(需要填空)。
            /// </summary>
            public string extend;
            /// <summary>
            /// 用户的session内容，腾讯server回包中会原样返回，可选字段，不需要就填空。
            /// </summary>
            public string ext;
        }

        /// <summary>
        /// 手机号实体
        /// </summary>
        public class SmsTelInfo
        {
            /// <summary>
            /// 国家码
            /// </summary>
            public string nationcode;
            /// <summary>
            /// 手机号码
            /// </summary>
            public string mobile;
        }

        /// <summary>
        /// 短信发送结果
        /// </summary>
        public class SmsSendResult
        {
            /// <summary>
            /// 0表示成功(计费依据)，非0表示失败
            /// </summary>
            public int result;
            /// <summary>
            /// result非0时的具体错误信息
            /// </summary>
            public string errmsg;
            /// <summary>
            /// 用户的session内容，腾讯server回包中会原样返回
            /// </summary>
            public string ext;
            /// <summary>
            /// 标识本次发送id，标识一次短信下发记录
            /// </summary>
            public string sid;
            /// <summary>
            /// 短信计费的条数
            /// </summary>
            public int fee;
        }
    }
}
