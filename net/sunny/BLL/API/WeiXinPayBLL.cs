using Sunny.Common;
using Sunny.DAL;
using Sunny.Model.Custom;
using Sunny.Model.WXPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.BLL.API
{

    /// <summary>
    /// 微信支付
    /// </summary>
    public class WeiXinPayBLL
    {
        #region 微信统一下单

        /// <summary>
        /// 微信统一下单
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="body">商品简单描述</param>
        /// <param name="openId">openid</param>
        /// <param name="spbill_create_ip">调用微信支付API的机器IP</param>
        /// <returns></returns>
        public static WXPayToClientPara SendPreOrder(string orderId, string openId)
        {
            try
            {
                WXPayRequestParameter para = CreatePreOrderPara(orderId, openId);
                if (para != null)
                {
                    string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
                    string xmlString = XmlHelper.XmlSerialize(para, Encoding.UTF8);
                    string postResult = Util.Web.WebUtil.PostWebData(url, xmlString, Util.Web.DataCompress.NotCompress);

                    Util.Log.LogUtil.Write($"统一下单结果：orderId {orderId} " + postResult, Util.Log.LogType.Debug);
                    //将xml中的根节点名字改为反序列化后对象的名字，不然反序列化会失败
                    postResult = postResult.Replace("<xml>", "<WXPayRequestResult>").Replace("</xml>", "</WXPayRequestResult>");
                    WXPayRequestResult resultObject = XmlHelper.XmlDeserialize<WXPayRequestResult>(postResult, Encoding.UTF8);

                    return new WXPayToClientPara()
                    {
                        appId = resultObject.appid,
                        timeStamp = Converter.ConvertToMySqlTimeStamp(DateTime.Now),
                        nonceStr = resultObject.nonce_str,
                        package = $"prepay_id={resultObject.prepay_id}",
                        signType = "MD5",
                        return_msg = resultObject.return_msg,
                    };
                }
                else
                {
                    Util.Log.LogUtil.Write($"WeiXinPayBLL.SendPreOrder 微信统一下单生成接口参数失败：orderid: {orderId} ", Util.Log.LogType.Error);
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"WeiXinPayBLL.SendPreOrder 微信统一下单失败：orderid: {orderId} /r/n {e}", Util.Log.LogType.Error);
            }
            return null;
        }

        /// <summary>
        /// 创建统一下单参数
        /// </summary>
        /// <param name="orderId">订单号</param>
        /// <param name="body">商品简单描述</param>
        /// <param name="openId">openid</param>
        /// <param name="spbill_create_ip">调用微信支付API的机器IP</param>
        /// <returns></returns>
        private static WXPayRequestParameter CreatePreOrderPara(string orderId, string openId)
        {
            List<CustOrderProduct> orders = OrderDAL.GetOrderProductList(0, 0, orderId);
            if (orders.Count == 1)
            {
                string body = "sunnybaby-购物";
                string device_info = "phone";
                string randStr = Function.GetRangeCharaters(10, RangeType.NumberAndLetter);
                string spbill_create_ip = WebHelper.GetClientIP();

                CustOrderProduct order = orders[0];
                if (order.state != 0)
                {
                    Util.Log.LogUtil.Write($"WeiXinPayBLL.CreatePreOrderPara 当前订单状态为 {order.state} 不需要非支付：orderid: {orderId} ", Util.Log.LogType.Warn);
                    return null;
                }

                WXPayRequestParameter para = new WXPayRequestParameter()
                {
                    appid = WebConfigData.MiniAppid,
                    mch_id = WebConfigData.MchId,
                    trade_type = "JSAPI",
                    out_trade_no = orderId,
                    total_fee = (int)(order.money * 100),
                    fee_type = "CNY",
                    notify_url = "",
                    time_start = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    time_expire = DateTime.Now.AddMinutes(45).ToString("yyyyMMddHHmmss"),
                    spbill_create_ip = spbill_create_ip,
                    attach = "sunny",
                    body = body,
                    sign_type = "MD5",
                    nonce_str = randStr,
                    openid = openId,
                    device_info = device_info,
                };

                string sign = GetSign(para);
                para.sign = sign;

                return para;
            }
            else
            {
                Util.Log.LogUtil.Write($"WeiXinPayBLL.CreatePreOrderPara 生成接口参数失败：orderid: {orderId} ", Util.Log.LogType.Error);
            }

            return null;
        }

        #endregion

        #region 微信关闭订单

        public static WXPayCloseOrderResult CloseOrder(string orderId)
        {
            string randStr = Function.GetRangeCharaters(10, RangeType.NumberAndLetter);

            WXPayCloseOrderParameter para = new WXPayCloseOrderParameter()
            {
                appid = WebConfigData.MiniAppid,
                mch_id = WebConfigData.MchId,
                nonce_str = randStr,
                out_trade_no = orderId,
                sign_type = "MD5",
            };

            string sign = GetSign(para);
            para.sign = sign;

            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            string xmlString = XmlHelper.XmlSerialize(para, Encoding.UTF8);
            string postResult = Util.Web.WebUtil.PostWebData(url, xmlString, Util.Web.DataCompress.NotCompress);

            Util.Log.LogUtil.Write($"关闭订单结果：orderId {orderId} " + postResult, Util.Log.LogType.Debug);
            //将xml中的根节点名字改为反序列化后对象的名字，不然反序列化会失败
            postResult = postResult.Replace("<xml>", "<WXPayCloseOrderResult>").Replace("</xml>", "</WXPayCloseOrderResult>");
            WXPayCloseOrderResult resultObject = XmlHelper.XmlDeserialize<WXPayCloseOrderResult>(postResult, Encoding.UTF8);

            return resultObject;

        }


        #endregion

        #region 微信申请退款

        public static WXPayRefundResult RefundOrder(string orderId)
        {
            string randStr = Function.GetRangeCharaters(10, RangeType.NumberAndLetter);

            WXPayRefundParameter para = new WXPayRefundParameter()
            {
                appid = WebConfigData.MiniAppid,
                mch_id = WebConfigData.MchId,
                nonce_str = randStr,
                out_trade_no = orderId,
                sign_type = "MD5",
                //transaction_id=
            };

            string sign = GetSign(para);
            para.sign = sign;

            string url = "https://api.mch.weixin.qq.com/secapi/pay/refund";
            string xmlString = XmlHelper.XmlSerialize(para, Encoding.UTF8);
            string postResult = Util.Web.WebUtil.PostWebData(url, xmlString, Util.Web.DataCompress.NotCompress);

            Util.Log.LogUtil.Write($"申请退款结果：orderId {orderId} " + postResult, Util.Log.LogType.Debug);
            //将xml中的根节点名字改为反序列化后对象的名字，不然反序列化会失败
            postResult = postResult.Replace("<xml>", "<WXPayRefundResult>").Replace("</xml>", "</WXPayRefundResult>");
            WXPayRefundResult resultObject = XmlHelper.XmlDeserialize<WXPayRefundResult>(postResult, Encoding.UTF8);

            return resultObject;
        }


        #endregion

        #region 支付结果通知


        #endregion

        #region 退款结果通知


        #endregion

        /// <summary>
        /// 生成微信签名
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static string GetSign(object obj)
        {
            /*
            签名生成的通用步骤如下：

            第一步，设所有发送或者接收到的数据为集合M，将集合M内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），
            使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串stringA。

            特别注意以下重要规则：

            ◆ 参数名ASCII码从小到大排序（字典序）；
            ◆ 如果参数的值为空不参与签名；
            ◆ 参数名区分大小写；
            ◆ 验证调用返回或微信主动通知签名时，传送的sign参数不参与签名，将生成的签名与该sign值作校验。
            ◆ 微信接口可能增加字段，验证签名时必须支持增加的扩展字段

            第二步，在stringA最后拼接上key得到stringSignTemp字符串，并对stringSignTemp进行MD5运算，
            再将得到的字符串所有字符转换为大写，得到sign值signValue。       
            */

            var properties = GetProperties(obj);
            StringBuilder sb = new StringBuilder();
            foreach (var item in properties)
            {
                if (item.Value != null && item.Value.ToString() != string.Empty)
                    sb.Append($"{item.Key}={item.Value}&");
            }
            sb.Length--;

            return Util.Security.MD5Util.MD5(sb.ToString(), Util.Security.LetterCase.UpperCase);
        }

        private static IOrderedEnumerable<KeyValuePair<string, object>> GetProperties(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] ps = type.GetProperties();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (PropertyInfo property in ps)
            {
                dic.Add(property.Name, property.GetValue(obj, null));
            }
            return dic.OrderBy(a => a.Key);
        }

    }
}
