using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Spetmall.BLL.Page;
using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;
using Spetmall.Model.Custom;
using Spetmall.Model.Page;

namespace Spetmall.Admin.Controllers
{
    [Common.CustomAuthorize]
    public class TransactionController : Controller
    {

        public ActionResult Index(string keyword, string starttime, string endtime, string time = "1")
        {
            IList<order_detail> datas = orderDAL.GetOrderList(keyword, time, starttime, endtime, 0);
            ViewBag.time = time;
            ViewBag.datas = datas;
            ViewBag.keyword = keyword;
            ViewBag.starttime = starttime;
            ViewBag.endtime = endtime;
            return View();
        }

        public ActionResult Detail(string id)
        {
            ReceiptOrderInfo data = ReceiptBLL.GetOrderInfo(id, 0);
            return View(data);
        }

        //打印小票
        public ActionResult Print(string id)
        {
            try
            {
                ReceiptOrderInfo data = ReceiptBLL.GetOrderInfo(id, 0);
                PrintObject printObject = new PrintObject()
                {
                    ShopName = WebConfigData.ReceiptPrinterShopName,
                    Phone = WebConfigData.ReceiptPrinterPhone,
                    QRCodeImg = Const.RootWebPath + "Images\\wx.jpg",
                    OrderNo = id,
                    Time = data.crtime,
                    ReceiptMoney = data.discount_total_price + data.adjust_price,
                    PayMoney = data.activitytotalprice,
                };
                foreach (ReceiptOrderProductInfo item in data.receiptgoodsdata.Values)
                {
                    PrintProducts pdts = new PrintProducts()
                    {
                        Name = item.name,
                        Price = item.price,
                        Count = item.number,
                        Money = item.goods_total_price,
                        BarCode = item.barcode,
                    };
                    printObject.Products.Add(pdts);
                }
                PrintReceiptBLL printer = new PrintReceiptBLL(printObject);
                printer.Print();
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("打印小票出错啦：" + e, Util.Log.LogType.Error);
            }

            return new EmptyResult();
        }
    }
}
