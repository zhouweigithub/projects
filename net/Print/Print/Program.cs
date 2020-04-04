using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Print
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "PPPP";
            //printDoc.PrinterSettings.PrinterName = "Fax";
            //printDoc.PrinterSettings.PrintFileName = "xx.pdf";
            printDoc.PrinterSettings.PrintToFile = true;
            printDoc.PrintPage += PrintDoc_PrintPage;

            printDoc.Print();
            Console.ReadKey();
        }

        private static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            float left = 2; //打印区域的左边界
            float top = 70;//打印区域的上边界
            Font font = new Font("仿宋", 8);//内容字体            
            e.Graphics.DrawString("哈哈哈哈", font, Brushes.Red, left, top, StringFormat.GenericDefault);
        }
    }
}
