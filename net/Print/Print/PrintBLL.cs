using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Print
{
    internal class PrintBLL
    {
        /// <summary>
        /// 打印彩票
        /// </summary>
        public static void PrintLottery(System.Drawing.Printing.PrintPageEventArgs e, string serialNum, List<string> numList)
        {
            //生成条形码
            //Fath.BarcodeX barCode = new Fath.BarcodeX
            //{
            //    Text = serialNum,//条码数据
            //    Symbology = Fath.bcType.Code128,//设置条码格式
            //    ShowText = true//同时显示文本           
            //};//创建条码生成对象
            //e.Graphics.DrawImage(barCode.Image(240, 50), new Point(20, 5));//画条形码

            //生成彩票信息
            float left = 2; //打印区域的左边界
            float top = 70;//打印区域的上边界
            Font titlefont = new Font("仿宋", 10);//标题字体
            Font font = new Font("仿宋", 8);//内容字体            
            e.Graphics.DrawString("天津百万奖彩票中心", titlefont, Brushes.Blue, left + 20, top, new StringFormat());//打印标题
                                                                                                            //画一条分界线
            Pen pen = new Pen(Color.Green, 1);
            e.Graphics.DrawLine(pen, new Point((int)left - 2, (int)top + 20), new Point((int)left + 180, (int)top + 20));

            //循环打印选号
            for (int i = 0; i < numList.Count; i++)
            {
                e.Graphics.DrawString(numList[i], font, Brushes.Blue, left,
                    top + titlefont.GetHeight(e.Graphics) + font.GetHeight(e.Graphics) * i + 12, new StringFormat());
            }

            //画一条分界线           
            float topPoint = titlefont.GetHeight(e.Graphics) + font.GetHeight(e.Graphics) * (numList.Count) + 22;

            e.Graphics.DrawLine(pen, new Point((int)left - 2, (int)top + (int)topPoint),
                new Point((int)left + 180, (int)top + (int)topPoint));

            //打印时间
            string time = "购买时间：" + DateTime.Now.ToString("yyy-MM-dd  HH:mm:ss");
            e.Graphics.DrawString(time, font, Brushes.Blue, left, top + titlefont.GetHeight(e.Graphics)
                + font.GetHeight(e.Graphics) * (numList.Count + 1) + 12, new StringFormat());

            //二维码图片left和top坐标
            int qrcodetop = (int)(top + titlefont.GetHeight(e.Graphics) + font.GetHeight(e.Graphics) * (numList.Count + 3) + 12);
            int qrcodeleft = (int)left + 32;

            //生成二维码图片
            // Image bmp = QRcodeCreator.GetQRCodeBmp("http://www.xiketang.com/duijiang/query?id=" + serialNum);//直接添加流水号，将来从网站直接查询
            //Image bmp = QRcodeCreator.GetQRCodeBmp("http://www.xiketang.com");
            ////打印
            //e.Graphics.DrawImage(bmp, new Point(qrcodeleft, qrcodetop));//不同的URL图片大小不同，可以根据需要调整left坐标
            //e.Graphics.DrawString("扫描二维码可直接查询兑奖结果", font, Brushes.Blue, left, qrcodetop + bmp.Height + 10, new StringFormat());
        }
    }
}
