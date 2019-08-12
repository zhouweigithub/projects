using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib;
using Spetmall.Model;
using Spetmall.Model.Custom;
using Spetmall.Model.Page;

namespace Spetmall.BLL.Page
{
    /// <summary>
    /// 打印小票
    /// </summary>
    public class PrintReceiptBLL
    {
        private PrintObject _data;
        private static readonly int _pageWidth = GetYc(4.7);
        private readonly int _pageHeight;    //小票整体高度


        /// <summary>
        /// 打印小票
        /// </summary>
        /// <param name="data"></param>
        public PrintReceiptBLL(PrintObject data)
        {
            _data = data;
            //整体高度=非商品高度+商品高度+底部额外高度
            _pageHeight = 400 + (int)(25.26 * _data.Products.Count) + 20;
        }

        public void Print()
        {
            PrintDocument printer = new PrintDocument();
            printer.PrinterSettings.PrinterName = Common.WebConfigData.ReceiptPrinterName;
            PaperSize size = new PaperSize("new1", _pageWidth, _pageHeight);
            printer.DefaultPageSettings.PaperSize = size;
            printer.PrintPage += Printer_PrintPage;
            printer.Print();
        }

        private void Printer_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                int marginLeft = 0;
                int marginTop = 15;
                float height = 30;
                StringFormat formatCenter = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                StringFormat formatLeft = new StringFormat
                {
                    Alignment = StringAlignment.Near
                };
                StringFormat formatRight = new StringFormat
                {
                    Alignment = StringAlignment.Far
                };

                //输出高质量图片
                e.Graphics.SmoothingMode = SmoothingMode.Default;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                string text = $"♛{_data.ShopName}♛";
                Font font1 = new Font("黑体", 8);
                float lineHeight8 = font1.GetHeight(e.Graphics);

                Font font2 = new Font("黑体", 8);
                float lineHeight2 = font2.GetHeight(e.Graphics);

                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, marginTop, _pageWidth, height), formatCenter);
                height += marginTop;

                text = "✧✧✧✧✧✧✧✧✧✧✧";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                text = "收银票据";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                text = "**************************************";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                text = "时ㅤ间 " + _data.Time.ToString("yyyy-MM-dd HH:mm:ss");
                e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height, formatLeft);
                height += lineHeight8;

                text = "订单号 " + _data.OrderNo;
                e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height, formatLeft);
                height += lineHeight8;

                text = "**************************************";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                int moveWidth = 10;
                int p1 = 0;
                int p2 = moveWidth + _pageWidth / 3;
                int p3 = moveWidth + _pageWidth / 9 * 5;
                int p4 = moveWidth + _pageWidth / 9 * 7;


                text = "商品名称";
                e.Graphics.DrawString(text, font1, Brushes.Black, p1, height);
                text = "单价";
                e.Graphics.DrawString(text, font1, Brushes.Black, p2, height);
                text = "数量";
                e.Graphics.DrawString(text, font1, Brushes.Black, p3, height);
                text = "金额";
                e.Graphics.DrawString(text, font1, Brushes.Black, p4, height);

                height += lineHeight8;

                text = "--------------------------------------";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                foreach (PrintProducts item in _data.Products)
                {
                    e.Graphics.DrawString(item.Name, font1, Brushes.Black, p1, height);
                    height += lineHeight8;

                    //e.Graphics.DrawString(item.BarCode, font1, Brushes.Black, p1, height);
                    e.Graphics.DrawString(RemoveLast0(item.Price), font1, Brushes.Black, p2, height);
                    e.Graphics.DrawString(item.Count.ToString(), font1, Brushes.Black, p3, height);
                    e.Graphics.DrawString(RemoveLast0(item.Money), font1, Brushes.Black, p4, height);

                    height += lineHeight8;
                }

                text = "-------------合计--------------";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                if (_data.ReceiptMoney != 0)
                {
                    text = "优惠";
                    e.Graphics.DrawString(text, font2, Brushes.Black, marginLeft, height);

                    text = RemoveLast0(-_data.ReceiptMoney);
                    e.Graphics.DrawString(text, font2, Brushes.Black, new RectangleF(0, height, _pageWidth, lineHeight2), formatRight);
                    height += lineHeight2;
                }

                text = "实付";
                e.Graphics.DrawString(text, font2, Brushes.Black, marginLeft, height);

                text = RemoveLast0(_data.PayMoney);
                e.Graphics.DrawString(text, font2, Brushes.Black, new RectangleF(0, height, _pageWidth, lineHeight2), formatRight);
                height += lineHeight2;

                Image img = Image.FromFile(Common.Const.RootWebPath + "Images\\wx.jpg");
                e.Graphics.DrawImage(img, new Rectangle(_pageWidth / 4, (int)height + 5, _pageWidth / 2, _pageWidth / 2));
                height += _pageWidth / 2 + 5;

                text = "加客服微信好友";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                height += 3;   //增加3的间距

                img = GetBarCode(_data.OrderNo, _pageWidth - 20, 25);
                e.Graphics.DrawImage(img, new Rectangle((_pageWidth - img.Width) / 2, (int)height, img.Width, img.Height));
                height += img.Height;

                height += 3;

                text = $"☏ {_data.Phone}";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                text = "欢迎再次光临";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, _pageWidth, 15), formatCenter);
                height += 15;

                img.Dispose();
                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("打印出错：" + ex, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 获取英寸的100倍长度
        /// </summary>
        /// <param name="cm">厘米</param>
        /// <returns></returns>
        private static int GetYc(double cm)
        {
            return (int)(cm * 10 / 25.4 * 100);
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="content"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private Image GetBarCode(string content, int width, int height)
        {
            try
            {
                using (var barcode = new Barcode()
                {
                    IncludeLabel = false,
                    Alignment = AlignmentPositions.CENTER,
                    Width = width,
                    Height = height,
                    RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                    BackColor = Color.White,
                    ForeColor = Color.Black,
                })
                {
                    return barcode.Encode(TYPE.CODE128, content);
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("生成条码出错：" + e.Message, Util.Log.LogType.Error);
            }
            return null;
        }

        /// <summary>
        /// 移除无用的小数位的数字0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string RemoveLast0(decimal number)
        {
            string tmp = number.ToString("F2");
            return tmp.TrimEnd('0', '.');
        }

    }
}
