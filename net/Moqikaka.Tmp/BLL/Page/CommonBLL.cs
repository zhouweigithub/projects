using Moqikaka.Tmp.Common;
using Moqikaka.Tmp.DAL;
using Moqikaka.Tmp.Model;
using Moqikaka.Util.Log;
using Moqikaka.Util.Web;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Moqikaka.Tmp.BLL.Page
{
    public class CommonBLL
    {

        /// <summary>
        /// 获取加粗文本格式
        /// </summary>
        /// <param name="workbook">IWorkbook实例</param>
        /// <returns></returns>
        public static ICellStyle GetBoldStyle(IWorkbook workbook)
        {
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            ICellStyle boldStyle = workbook.CreateCellStyle();
            boldStyle.SetFont(font);
            return boldStyle;
        }

        /// <summary>
        /// 获取日期格式
        /// </summary>
        /// <param name="workbook">IWorkbook实例</param>
        /// <param name="dateFormatString">日期格式化字符串</param>
        /// <returns></returns>
        public static ICellStyle GetDateStyle(IWorkbook workbook, string dateFormatString)
        {
            IDataFormat dataformat = workbook.CreateDataFormat();
            ICellStyle dateStyle = workbook.CreateCellStyle();
            dateStyle.DataFormat = dataformat.GetFormat(dateFormatString);
            return dateStyle;
        }


        public static List<MiniProgromSetting> GetMiniProgromSettingFromFile()
        {
            try
            {
                string key = "MiniProgromSetting_cache_2154201";
                if (MemoryCacheManager.IsSet(key))
                {
                    return MemoryCacheManager.Get<List<MiniProgromSetting>>(key);
                }
                else
                {
                    List<MiniProgromSetting> setting = XmlHelper.XmlDeserializeFromFile<List<MiniProgromSetting>>(Const.RootWebPath + "/App_Data/MiniProgromSetting.xml", Encoding.UTF8);
                    MemoryCacheManager.Set(key, setting, 60 * 24);
                    return setting;
                }
            }
            catch (Exception e)
            {
                LogUtil.Write("读取小程序配置文件失败：" + e, LogType.Error);
                return new List<MiniProgromSetting>();
            }
        }

    }


}
