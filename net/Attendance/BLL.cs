using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Attendance
{
    public static class BLL
    {
        public static readonly String startDate = Util.AppConfigUtil.GetAppConfig("startDate");
        public static readonly String endDate = Util.AppConfigUtil.GetAppConfig("endDate");
        public static readonly String queryUurl = Util.AppConfigUtil.GetAppConfig("url");
        public static String name = String.Empty;

        private static RespModel GetRemoteData(Int32 userid)
        {
            try
            {
                String url = $"{queryUurl}?deviceId=1&userId={userid}&startTimeStr={startDate}&endTimeStr={endDate}";
                String resp = Util.Web.WebUtil.GetWebData(url, String.Empty, Util.Web.DataCompress.NotCompress);
                RespModel model = Util.Json.JsonUtil.Deserialize<RespModel>(resp);
                name = model.code == 0 ? model.data.name : String.Empty;
                return model;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetRemoteData 从远程服务器获取考勤数据失败：" + e, Util.Log.LogType.Error);
            }
            return null;
        }

        private static List<ViewData> GetList(Int32 userid)
        {
            RespModel data = GetRemoteData(userid);
            if (data == null || data.data == null || data.data.logData == null || data.data.logData.Count == 0)
                return new List<ViewData>();

            List<ViewData> result = new List<ViewData>();
            var dates = data.data.logData.GroupBy(a => new { a.Date }).Select(b => b.Key.Date);
            foreach (DateTime date in dates)
            {
                var yesterdays = data.data.logData.Where(a => a.Date == date.AddDays(-1));
                String yesterdayMaxTime = yesterdays.Count() == 0 ? String.Empty : yesterdays?.Max().ToString("HH:mm:ss");
                ViewData vd = new ViewData()
                {
                    date = date,
                    minTime = data.data.logData.Where(a => a.Date == date).Min().ToString("HH:mm:ss"),
                    maxTime = data.data.logData.Where(a => a.Date == date).Max().ToString("HH:mm:ss"),
                };
                vd.isLate = IsLate(vd.minTime, yesterdayMaxTime) ? "迟到" : String.Empty;
                result.Add(vd);
            }

            return result;
        }

        private static Boolean IsLate(String todayMinTime, String yesterdayMaxTime)
        {
            Boolean result = false;
            if (todayMinTime.CompareTo("09:10") == -1)
            {
                result = false;
            }
            else if (todayMinTime.CompareTo("10:00") == -1)
            {
                if (yesterdayMaxTime.CompareTo("20:00") >= 0)
                    result = false;
                else
                    result = true;
            }
            else
            {
                result = true;
            }

            return result;
        }

        public static String ToExcel(Int32 userid, String absoluteSavePath)
        {
            List<ViewData> datas = GetList(userid);
            String result = String.Empty;
            if (datas?.Count > 0)
            {
                try
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    //日期格式
                    IDataFormat dataformat = workbook.CreateDataFormat();
                    ICellStyle dateStyle = workbook.CreateCellStyle();
                    dateStyle.DataFormat = dataformat.GetFormat("yyyy-MM-dd");
                    //创建工作表
                    ISheet sheetMain = workbook.CreateSheet(String.Format("考勤记录({0})", name));
                    sheetMain.DisplayGridlines = true;
                    sheetMain.SetColumnWidth(0, 12 * 256);
                    IRow rowFirst = sheetMain.CreateRow(0);

                    String[] titles = new String[] { "日期", "首次打卡", "末次打卡", "工作时长", "加班时长", "星期", "迟到标识", "早退标识", "加班标识", "加班餐补", "加班车补", "周末补助" };
                    for (Int32 i = 0; i < titles.Length; i++)
                    {
                        rowFirst.CreateCell(i).SetCellValue(titles[i]);
                    }

                    sheetMain.CreateFreezePane(50, 1);

                    Int32 rowIndex = 0;
                    foreach (ViewData item in datas)
                    {
                        Int32 cellIndex = 0;
                        IRow row = sheetMain.CreateRow(++rowIndex);

                        ICell dateCell = row.CreateCell(cellIndex++);
                        dateCell.SetCellValue(item.date);   //日期格式
                        dateCell.CellStyle = dateStyle;

                        row.CreateCell(cellIndex++).SetCellValue(item.minTime);
                        row.CreateCell(cellIndex++).SetCellValue(item.maxTime);
                        row.CreateCell(cellIndex++).SetCellValue(item.workTime);

                        if (String.IsNullOrEmpty(item.overTimeState))
                            row.CreateCell(cellIndex++).SetCellValue(String.Empty);
                        else
                            row.CreateCell(cellIndex++).SetCellValue(item.overTime);

                        row.CreateCell(cellIndex++).SetCellValue(item.date.ToString("dddd"));
                        row.CreateCell(cellIndex++).SetCellValue(item.isLate);
                        row.CreateCell(cellIndex++).SetCellValue(item.leaveEarly);
                        row.CreateCell(cellIndex++).SetCellValue(item.overTimeState);

                        if (item.mealMoney == 0)
                            row.CreateCell(cellIndex++).SetCellValue(String.Empty);
                        else
                            row.CreateCell(cellIndex++).SetCellValue(item.mealMoney);

                        if (item.carMoney == 0)
                            row.CreateCell(cellIndex++).SetCellValue(String.Empty);
                        else
                            row.CreateCell(cellIndex++).SetCellValue(item.carMoney);

                        if (item.weekMoney == 0)
                            row.CreateCell(cellIndex++).SetCellValue(String.Empty);
                        else
                            row.CreateCell(cellIndex++).SetCellValue(item.weekMoney);

                    }

                    if (!Directory.Exists(absoluteSavePath))
                    {
                        Directory.CreateDirectory(absoluteSavePath);
                    }

                    String fileName = absoluteSavePath + "\\" + String.Format("考勤记录({0}){1}.{2}.xlsx", name, DateTime.Now.ToString("yyyyMMddHHmmss"), userid);
                    using (FileStream file = new FileStream(fileName, FileMode.Create))
                    {
                        workbook.Write(file);
                    }
                    workbook.Close();

                    result = fileName;
                }
                catch (Exception e)
                {
                    Util.Log.LogUtil.Write("导出考勤记录失败\n" + e.ToString(), Util.Log.LogType.Error);
                }
            }

            return result;
        }

    }
}
