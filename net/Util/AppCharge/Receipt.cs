// ****************************************
// FileName:Receipt.cs
// Description:APP Store充值收据对象
// Tables:Nothing
// Author:Jordan Zuo
// Create Date:2013-10-10
// Revision History:
// ****************************************

using System;
using System.Collections.Generic;

namespace Util.AppCharge
{
    using Util.Json;

    /// <summary>
    /// APP Store充值收据对象
    /// </summary>
    [Serializable]
    public sealed class Receipt
    {
        #region Properties

        public String ProductId
        { get; set; }

        public String TransactionId
        { get; set; }

        public String OriginalTransactionId
        { get; set; }

        public String Bvrs
        { get; set; }

        public DateTime PurchaseDate
        { get; set; }

        public DateTime? OriginalPurchaseDate
        { get; set; }

        public Int32 Quantity
        { get; set; }

        public String BundleIdentifier
        { get; set; }

        public Int32 Status
        { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Creates the receipt from Apple's Response
        /// </summary>
        /// <param name="receipt">Apple's Response</param>
        public Receipt(String receipt)
        {
            //将接收的数据转化为Dictionary类型的对象
            Dictionary<String, Object> json = JsonUtil.Deserialize(receipt);

            //定义、并判断返回状态
            Int32 status = -1;
            Int32.TryParse(json["status"].ToString(), out status);
            if (status != 0) return;

            //给属性赋值
            this.Status = status;

            //Receipt is actually a child
            json = JsonUtil.Deserialize(json["receipt"].ToString());

            //用返回值对本对象的属性进行赋值
            this.Bvrs = json["bvrs"].ToString();
            this.ProductId = json["product_id"].ToString();
            this.BundleIdentifier = json["bid"].ToString();
            this.TransactionId = json["transaction_id"].ToString();
            this.OriginalTransactionId = json["original_transaction_id"].ToString();

            DateTime purchaseDate = DateTime.MinValue;
            if (DateTime.TryParseExact(json["purchase_date"].ToString().Replace(" Etc/GMT", String.Empty).Replace("\"", String.Empty).Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out purchaseDate))
            {  
                this.PurchaseDate = purchaseDate;
            }

            DateTime originalPurchaseDate = DateTime.MinValue;
            if (DateTime.TryParseExact(json["original_purchase_date"].ToString().Replace(" Etc/GMT", String.Empty).Replace("\"", String.Empty).Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out originalPurchaseDate))
            {
                this.OriginalPurchaseDate = originalPurchaseDate;
            }

            Int32 quantity = 1;
            Int32.TryParse(json["quantity"].ToString(), out quantity);
            this.Quantity = quantity;
        }

        #endregion Constructor
    }
}