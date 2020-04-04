// ****************************************
// FileName:ReceiptVerificationUtil.cs
// Description:Receipt验证助手类
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Util.AppCharge
{
    using Util.Web;

    /// <summary>
    /// Receipt验证助手类
    /// </summary>
    public static class ReceiptVerificationUtil
    {
        #region Constants

        //Receipt验证地址
        private const String urlSandbox = "https://sandbox.itunes.apple.com/verifyReceipt";
        private const String urlProduction = "https://buy.itunes.apple.com/verifyReceipt";

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Make a String with the receipt encoded
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns></returns>
        private static String ConvertReceiptToPost(String receipt)
        {
            return String.Format(@"{{""receipt-data"":""{0}""}}", receipt);
        }

        /// <summary>
        /// Takes the receipt from Apple's App Store and converts it to bytes
        /// that we can understand
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static Byte[] ConvertAppStoreTokenToBytes(String token)
        {
            token = token.Replace(" ", String.Empty);
            Int32 i = 0;
            Int32 b = 0;
            List<Byte> bytes = new List<Byte>();
            while (i < token.Length)
            {
                bytes.Add(Convert.ToByte(token.Substring(i, 2), 16));
                i += 2;
                b++;
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Make a String with the receipt encoded
        /// </summary>
        /// <param name="receipt"></param>
        /// <param name="dataEncoded">数据编码枚举</param>
        /// <returns></returns>
        private static String ConvertReceiptToPost(String receipt, DataEncoded dataEncoded)
        {
            //如果未经过编码，则先进行编码，再进行格式化(通过HTTP传过来的数据)
            String receiptToPost = receipt;

            //如果receipt未进行过Base64编码，则先进行编码(通过Socket传递过来的数据)；
            if (dataEncoded == DataEncoded.NotEncoded)
            {
                String itunesDecodedReceipt = Encoding.UTF8.GetString(ConvertAppStoreTokenToBytes(receipt.Replace("<", String.Empty).Replace(">", String.Empty))).Trim();
                receiptToPost = StringUtil.Base64Encode(itunesDecodedReceipt);
            }

            //进行格式化
            return ConvertReceiptToPost(receiptToPost);
        }

        /// <summary>
        /// 获取Receipt对象
        /// </summary>
        /// <param name="url">Receipt验证地址</param>
        /// <param name="receiptData">ReceiptData</param>
        /// <param name="dataEncoded">数据编码枚举</param>
        /// <returns>Receipt对象</returns>
        private static Receipt GetReceipt(String url, String receiptData, DataEncoded dataEncoded)
        {
            Receipt receipt = null;

            //将receiptData Post到指定url
            String post = PostDataUtil.PostWebData(url, ConvertReceiptToPost(receiptData, dataEncoded), DataCompress.NotCompress);

            //判断结果是否为空
            if (String.IsNullOrEmpty(post)) return receipt;

            return new Receipt(post);
        }

        /// <summary>
        /// 获取Receipt对象
        /// </summary>
        /// <param name="receiptData">ReceiptData</param>
        /// <param name="sandbox">是否sandbox模式</param>
        /// <param name="dataEncoded">数据编码枚举</param>
        /// <returns>Receipt对象</returns>
        private static Receipt GetReceipt(String receiptData, Boolean sandbox, DataEncoded dataEncoded)
        {
            return GetReceipt(sandbox ? urlSandbox : urlProduction, receiptData, dataEncoded);
        }

        #endregion Private Static Methods

        #region Public Static Methods

        /// <summary>
        /// 验证充值信息是否合法
        /// </summary>
        /// <param name="bundleIdentifier">软件包唯一标识</param>
        /// <param name="productID">产品ID</param>
        /// <param name="receiptData">Receipt数据</param>
        /// <param name="sandBox">是否沙盒</param>
        /// <param name="dataEncoded">数据编码枚举</param>
        /// <param name="receipt">输出的receipt对象</param>
        /// <exception cref="System.Net.WebException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <returns>是否合法</returns>
        public static Boolean ValidateCharge(String[] bundleIdentifier, String productID, String receiptData, Boolean sandBox, DataEncoded dataEncoded, out Receipt receipt)
        {
            receipt = null;

            //判断参数是否为空
            if (bundleIdentifier.Length == 0 || String.IsNullOrEmpty(productID) || String.IsNullOrEmpty(receiptData)) 
                return false;

            //获取Receipt对象
            receipt = GetReceipt(receiptData, sandBox, dataEncoded);

            //如果receipt==null，或者status != 0，说明传过来的数据异常，直接返回失败
            if (receipt == null || receipt.Status != 0)
                return false;

            //如果ProductID不为空，则判断产品ID是否等于receipt中的productID
            if (!bundleIdentifier.Contains(receipt.BundleIdentifier) || !productID.Equals(receipt.ProductId, StringComparison.Ordinal))
                return false;

            return true;
        }

        #endregion
    }
}