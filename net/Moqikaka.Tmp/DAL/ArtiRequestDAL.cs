using Moqikaka.Tmp.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.DAL
{
    public class ArtiRequestDAL
    {
        private static readonly string addPageClicskLogSql = @"INSERT IGNORE INTO m_page_click_log (crdate, page, deviceToken, clicks) VALUES(@crdate, @page, @deviceToken, 0) ;
UPDATE m_page_click_log SET clicks=clicks+1 WHERE crdate=@crdate AND page=@page AND deviceToken=@deviceToken;";

        private static readonly string addArticalClicksLogSql = @"INSERT IGNORE INTO m_artical_click_log (crdate, articalid, deviceToken, clicks) VALUES(@crdate, @articalid, @deviceToken, 0) ;
UPDATE m_artical_click_log SET clicks=clicks+1 WHERE crdate=@crdate AND articalid=@articalid AND deviceToken=@deviceToken;";

        private static readonly string addFeedbackSql = "INSERT INTO m_feedback (deviceToken, openid, content) VALUES(@deviceToken, @openid, @content);";

        /// <summary>
        /// 页面点击量+1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddPageClickLog(MPageClickLog model)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@crdate",DateTime.Today.ToString("yyyy-MM-dd")),
                        new MySqlParameter("@page",model.Page),
                        new MySqlParameter("@deviceToken",model.DeviceToken),
                    };
                    int count = dbhelper.ExecuteNonQueryParams(addPageClicskLogSql, commandParameters);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddPageClickLog 出错：" + ex, Util.Log.LogType.Error);
            }
            return false;
        }

        /// <summary>
        /// 文章点击量+1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddArticalClickLog(MArticalClickLog model)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@crdate",DateTime.Today.ToString("yyyy-MM-dd")),
                        new MySqlParameter("@articalid",model.Articalid),
                        new MySqlParameter("@deviceToken",model.DeviceToken),
                    };
                    int count = dbhelper.ExecuteNonQueryParams(addArticalClicksLogSql, commandParameters);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddArticalClickLog 出错：" + ex, Util.Log.LogType.Error);
            }
            return false;
        }

        /// <summary>
        /// 添加用户反馈
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddFeedback(MFeedback model)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@deviceToken",model.DeviceToken),
                        new MySqlParameter("@openid",model.Openid),
                        new MySqlParameter("@content",model.Content),
                    };
                    int count = dbhelper.ExecuteNonQueryParams(addFeedbackSql, commandParameters);
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddFeedback 出错：" + ex, Util.Log.LogType.Error);
            }
            return false;
        }
    }
}
