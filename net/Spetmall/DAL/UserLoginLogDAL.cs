//***********************************************************************************
//文件名称：UserLoginLogDAL.cs
//功能描述：游戏用户登录信息操作类
//数据表： sdkcenter.user_app_login_log
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using Spetmall.Common;

namespace Spetmall.DAL
{
    public class UserLoginLogDAL
    {
        private UserLoginLogDAL()
        {
        }

        private static readonly string updateOrderCrtime = "SELECT * FROM sdkcenter.user_app_login_log WHERE appid='{0}' {1} ORDER BY logintime DESC LIMIT 100";


        /// <summary>
        /// 查询前几十条用户登录数据，不用分页
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static List<Model.UserLoginLog> GetLoginDatas(string dbconfigId, string appid, string adsrc)
        {
            try
            {
                string adsrcWhere = string.Empty;
                if (!string.IsNullOrEmpty(adsrc))
                    adsrcWhere = string.Format(" and adsrc='{0}' ", adsrc);

                using (DBHelper dbHelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    DataTable dt = dbHelper.ExecuteDataTable(string.Format(updateOrderCrtime, appid, adsrcWhere));
                    if (dt != null && dt.Rows.Count > 0)
                        return dt.ToList<Model.UserLoginLog>();
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write(WriteLog.LogLevel.Error, "GetLoginDatas游戏用户登录信息出错\r\n" + ex.Message);
            }

            return new List<Model.UserLoginLog>();
        }



    }
}
