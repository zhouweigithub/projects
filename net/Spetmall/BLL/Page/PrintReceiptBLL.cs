using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarcodeLib;
using Spetmall.Model;
using Spetmall.Model.Page;

namespace Spetmall.BLL.Page
{
    /// <summary>
    /// 打印小票
    /// </summary>
    public class PrintReceiptBLL
    {
        private order _orderInfo;
        private List<receipt_confirm_products> _datas;
        private static int pageWidth = GetYc(4.7);

        ///// <summary>
        ///// 调整字符间距
        ///// </summary>
        ///// <param name="hdc"></param>
        ///// <param name="nCharExtra"></param>
        ///// <returns></returns>
        //[DllImport("gdi32.dll")]
        //public static extern int SetTextCharacterExtra(IntPtr hdc, int nCharExtra);
        public static readonly int productCount = 3;

        /// <summary>
        /// 打印小票
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="datas"></param>
        public PrintReceiptBLL(order orderInfo, List<receipt_confirm_products> datas)
        {
            _orderInfo = orderInfo;
            _datas = datas;
        }

        public void Print()
        {
            PrintDocument printer = new PrintDocument();
            printer.PrinterSettings.PrinterName = Common.WebConfigData.ReceiptPrinterName;
            int height = 400 + (int)(25.26 * productCount) + 20;
            PaperSize size = new PaperSize("new1", pageWidth, height);
            printer.DefaultPageSettings.PaperSize = size;

            //printer.DefaultPageSettings.Color = false;
            //printer.PrintController = new PreviewPrintController();

            //printer.PrinterSettings.PrintToFile = true;
            //printer.PrinterSettings.PrintFileName = "printer.jpg";
            //printer.DefaultPageSettings.PrinterResolution.X;
            //设置边距红半厘米
            //printer.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
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
                StringFormat formatCenter = new StringFormat();
                formatCenter.Alignment = StringAlignment.Center;
                formatCenter.LineAlignment = StringAlignment.Center;
                StringFormat formatLeft = new StringFormat();
                formatLeft.Alignment = StringAlignment.Near;
                //formatLeft.LineAlignment = StringAlignment.Center;
                StringFormat formatRight = new StringFormat();
                formatRight.Alignment = StringAlignment.Far;

                //输出高质量图片宋体
                e.Graphics.SmoothingMode = SmoothingMode.Default;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                //调整字符间距
                IntPtr hdc = e.Graphics.GetHdc();
                //SetTextCharacterExtra(hdc, 1);
                e.Graphics.ReleaseHdc(hdc);

                string text = "♛红旗连锁♛";
                Font font1 = new Font("黑体", 8);
                float lineHeight8 = font1.GetHeight(e.Graphics);

                Font font2 = new Font("黑体", 8);
                float lineHeight2 = font2.GetHeight(e.Graphics);

                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, marginTop, pageWidth, height), formatCenter);
                height += marginTop;

                text = "✧✧✧✧✧✧✧✧✧✧✧";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                text = "收银票据";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                text = "**************************************";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                text = "时ㅤ间 " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height, formatLeft);
                height += lineHeight8;

                text = "订单号 28452151542";
                e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height, formatLeft);
                height += lineHeight8;

                text = "**************************************";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                int moveWidth = 10;
                int p1 = 0, p2 = moveWidth + pageWidth / 3, p3 = moveWidth + pageWidth / 9 * 5, p4 = moveWidth + pageWidth / 9 * 7;


                text = "商品名称";
                e.Graphics.DrawString(text, font1, Brushes.Black, p1, height);
                text = "单价";
                e.Graphics.DrawString(text, font1, Brushes.Black, p2, height);
                text = "数量";
                e.Graphics.DrawString(text, font1, Brushes.Black, p3, height);
                text = "金额";
                e.Graphics.DrawString(text, font1, Brushes.Black, p4, height);


                //text = "商品名称\t单价\t数量\t金额";
                //e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height);
                height += lineHeight8;

                text = "--------------------------------------";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                for (int i = 0; i < productCount; i++)
                {
                    text = "喜之郎果粒橙乐翻天500ml";
                    e.Graphics.DrawString(text, font1, Brushes.Black, p1, height);
                    height += lineHeight8;

                    text = "35.00";
                    e.Graphics.DrawString(text, font1, Brushes.Black, p2, height);
                    text = "3.00";
                    e.Graphics.DrawString(text, font1, Brushes.Black, p3, height);
                    text = "99.55";
                    e.Graphics.DrawString(text, font1, Brushes.Black, p4, height);

                    //text = "喜之郎果粒橙乐翻天500ml";
                    //e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height);

                    //text = "\t20.56\t3.00\t60.77";
                    //e.Graphics.DrawString(text, font1, Brushes.Black, marginLeft, height);
                    height += lineHeight8;
                }

                text = "**************合计**************";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;


                text = "优惠";
                e.Graphics.DrawString(text, font2, Brushes.Black, marginLeft, height);

                text = "-21.55";
                e.Graphics.DrawString(text, font2, Brushes.Black, new RectangleF(0, height, pageWidth, lineHeight2), formatRight);
                height += lineHeight2;

                text = "实付";
                e.Graphics.DrawString(text, font2, Brushes.Black, marginLeft, height);

                text = "958.88";
                e.Graphics.DrawString(text, font2, Brushes.Black, new RectangleF(0, height, pageWidth, lineHeight2), formatRight);
                height += lineHeight2;

                //text = "**************************************";
                //e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                //height += 15;

                Image img = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "wx.jpg");
                e.Graphics.DrawImage(img, new Rectangle((int)(pageWidth / 4.3), (int)height + 5, pageWidth / 2, pageWidth / 2));
                height += (pageWidth - 10) / 2 + 5;

                text = "添加客服微信，更多优惠早知道！";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                height += 10;   //增加10的间距
                img = GetBarCode("28452151542", pageWidth - 20, 10);
                e.Graphics.DrawImage(img, new Rectangle(2, (int)height, pageWidth - 10, 20));
                height += 20;

                text = "☏ 13254845784";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                text = "谢谢惠顾，欢迎下次光临！";
                e.Graphics.DrawString(text, font1, Brushes.Black, new RectangleF(0, height, pageWidth, 15), formatCenter);
                height += 15;

                img.Dispose();
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

    }
}
