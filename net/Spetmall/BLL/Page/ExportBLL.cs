using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Spetmall.Common;
using Spetmall.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.BLL.Page
{
    public class ExportBLL
    {
        public static string ExportProductToExcel(List<product_show> datas, string absoluteSavePath)
        {
            string result = string.Empty;
            if (datas != null)
            {
                try
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    //创建工作表
                    ISheet sheetMain = workbook.CreateSheet("Sheet1");
                    //sheetMain.DisplayGridlines = false;
                    IRow rowFirst = sheetMain.CreateRow(0);

                    string[] titles = new string[] { "ID", "商品名称", "条码", "分类", "成本", "卖价", "库存", "库存预警", "销量", "会员折扣" };

                    for (int i = 0; i < titles.Length; i++)
                    {
                        rowFirst.CreateCell(i).SetCellValue(titles[i]);
                    }

                    int rowIndex = 0;
                    foreach (product_show item in datas)
                    {
                        int cellIndex = 0;
                        IRow row = sheetMain.CreateRow(++rowIndex);
                        row.CreateCell(cellIndex++).SetCellValue(item.id);
                        row.CreateCell(cellIndex++).SetCellValue(item.name);
                        row.CreateCell(cellIndex++).SetCellValue(item.barcode);
                        row.CreateCell(cellIndex++).SetCellValue(item.categoryName);
                        row.CreateCell(cellIndex++).SetCellValue((double)item.cost);
                        row.CreateCell(cellIndex++).SetCellValue((double)item.price);
                        row.CreateCell(cellIndex++).SetCellValue(item.store);
                        row.CreateCell(cellIndex++).SetCellValue(item.warn);
                        row.CreateCell(cellIndex++).SetCellValue(item.sales);
                        row.CreateCell(cellIndex++).SetCellValue(item.ismemberdiscount);
                    }

                    if (!Directory.Exists(absoluteSavePath))
                    {
                        Directory.CreateDirectory(absoluteSavePath);
                    }

                    string fileName = absoluteSavePath + "\\" + string.Format("exceldata{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                    using (FileStream file = new FileStream(fileName, FileMode.Create))
                    {
                        workbook.Write(file);
                    }
                    workbook.Close();

                    result = fileName;
                }
                catch (Exception e)
                {
                    WriteLog.Write(WriteLog.LogLevel.Error, "商品数据导出EXCEL失败\r\t" + e.ToString());
                }

            }

            return result;
        }

    }
}
