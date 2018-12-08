//***********************************************************************************
//文件名称：ExcelHelper.cs
//功能描述：DataTable直接导出到Excel操作表
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;

namespace Moqikaka.Tmp.Common
{
    public class ExcelHelper
    {
        private static string savePath = "\\Export";
        private static string sheetName = "Sheet1";

        /// <summary>
        /// 导出数据到excel
        /// </summary>
        /// <param name="data">要导出的数据</param>
        /// <returns>生成的excel路径</returns>
        public static string ExportToExcel(DataTable data)
        {
            try
            {
                string errmsg = string.Empty;
                IWorkbook workbook = new XSSFWorkbook();
                //日期格式
                IDataFormat dataformat = workbook.CreateDataFormat();
                ICellStyle dateStyle = workbook.CreateCellStyle();
                dateStyle.DataFormat = dataformat.GetFormat("yyyy-MM-dd HH:mm:ss");
                //创建工作表
                ISheet sheetMain = workbook.CreateSheet(sheetName);
                //sheetMain.DisplayGridlines = false;
                IRow rowFirst = sheetMain.CreateRow(0);


                for (int i = 0; i < data.Columns.Count; i++)
                {
                    rowFirst.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);
                }


                for (int i = 0; i < data.Rows.Count; i++)
                {
                    IRow row = sheetMain.CreateRow(i + 1);
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        ICell cell = row.CreateCell(j);
                        if (data.Columns[j].DataType == typeof(MySql.Data.Types.MySqlDateTime))
                        {   //日期类型，则转换为日期
                            cell.SetCellValue(Convert.ToDateTime(data.Rows[i][j]));
                            cell.CellStyle = dateStyle;
                        }
                        else
                            cell.SetCellValue(data.Rows[i][j].ToString());
                    }
                }

                string _strPath = System.Web.HttpContext.Current.Server.MapPath(savePath);
                if (!Directory.Exists(_strPath))
                {
                    Directory.CreateDirectory(_strPath);
                }

                string fileName = _strPath + "\\" + string.Format("exceldata{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmssffff"));

                FileStream file = new FileStream(fileName, FileMode.Create);
                workbook.Write(file);
                file.Close();
                workbook.Close();
                return fileName;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write(string.Format("导出数据到excel失败:{0}", e.ToString()), Util.Log.LogType.Error);
                return string.Empty;
            }

        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public static int DataTableToExcel(DataTable data, string fileName, string sheetName, bool isColumnWritten)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            IWorkbook workbook = null;
            ISheet sheet = null;

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (fileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();

            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }

                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < data.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < data.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                return count;
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("EXCEL转换DATATABLE失败： " + ex.Message, Util.Log.LogType.Error); return -1;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowTitle">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowTitle)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {   //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);

                    if (isFirstRowTitle)
                    {   //读取标题行
                        for (int i = firstRow.FirstCellNum; i < firstRow.Cells.Count; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {   //生成标题行
                        for (int i = firstRow.FirstCellNum; i < firstRow.LastCellNum; i++)
                        {
                            DataColumn column = new DataColumn("Column" + (i + 1));
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum;
                    }

                    //列数
                    int cellCount = isFirstRowTitle ? data.Columns.Count : firstRow.LastCellNum;

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 读取EXCEL文件内容
        /// </summary>
        /// <param name="fs">EXCEL文件流</param>
        /// <param name="fileName">EXCEL文件名</param>
        /// <param name="sheetName">目标sheet</param>
        /// <param name="isFirstRowTitle">首行是否为标题</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(Stream fs, string fileName, string sheetName, bool isFirstRowTitle)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                IWorkbook workbook = null;
                if (fileName.ToLower().EndsWith(".xlsx")) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.ToLower().EndsWith(".xls")) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {   //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {   //没指定sheet则获取第一个sheet
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);

                    if (isFirstRowTitle)
                    {   //读取首行列名
                        for (int i = firstRow.FirstCellNum; i < firstRow.Cells.Count; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (!string.IsNullOrWhiteSpace(cellValue))
                                {   //只获取有内容的列名
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {   //没标题列则生成列名
                        for (int i = firstRow.FirstCellNum; i < firstRow.LastCellNum; i++)
                        {   //生成列名
                            DataColumn column = new DataColumn("Column" + (i + 1));
                            data.Columns.Add(column);
                        }
                        startRow = sheet.FirstRowNum;
                    }

                    int cellCount = isFirstRowTitle ? data.Columns.Count : firstRow.LastCellNum;

                    //最后一行的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            ICell cell = row.GetCell(j);
                            if (cell != null) //同理，没有数据的单元格都默认是null
                            {
                                if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                                {   //若日期类型则进行转换
                                    dataRow[j] = cell.DateCellValue.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    dataRow[j] = cell.ToString();
                                }
                            }
                            else
                            {   //如果没有获取到当前格的内容，则给空值
                                dataRow[j] = null;
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("EXCEL转换DATATABLE失败： " + ex.Message, Util.Log.LogType.Error);
                return null;
            }
        }
        /// <summary>
        /// 从文件中获取数据
        /// </summary>
        /// <param name="fs">上传的文件流</param>
        /// <returns></returns>
        public static string GetJsonStringFromString(Stream fs)
        {
            var str = string.Empty;
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Seek(0, SeekOrigin.Begin);
            fs.Dispose();

            System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
            str = converter.GetString(bytes);
            return str;
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            if (fs != null)
        //                fs.Close();
        //        }

        //        fs = null;
        //        disposed = true;
        //    }
        //}

    }


}
