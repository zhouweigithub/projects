using Spetmall.Common;
using Spetmall.DAL;
using Spetmall.Model;
using Util.Log;
using Util.Web;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace Spetmall.BLL.Page
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

        public static string GetReturnJson(bool isOk, string errMsg)
        {
            var obj = new
            {
                status = isOk,
                code = 1,
                msg = "操作" + (isOk ? "成功" : "失败：" + errMsg),
                redirects = string.Empty,
            };

            string result = "<html><body><script>parent.yunmallIframe.Callback("
                + Util.Json.JsonUtil.Serialize(obj)
                + ");</script></body></html>";

            return result;
        }

        /// <summary>
        /// 登录结果
        /// </summary>
        /// <param name="isOk"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static string GetLoginReturnJson(bool isOk, string errMsg)
        {
            var obj = new
            {
                status = isOk,
                code = 1,
                msg = errMsg,
                redirects = string.Empty,
            };

            string result = "<html><body><script>parent.yunmallIframe.Callback("
                + Util.Json.JsonUtil.Serialize(obj)
                + ");</script></body></html>";

            return result;
        }

    }


}
