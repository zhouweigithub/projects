using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.DAL
{

    /// <summary>
    /// 数据库中所有的表名
    /// </summary>
    public class DBTable
    {
        /// <summary>
        /// 用户表
        /// </summary>
        public static readonly string m_user = "m_user";
        /// <summary>
        /// 文章表
        /// </summary>
        public static readonly string m_artical = "m_artical";
        /// <summary>
        /// 我的收藏
        /// </summary>
        public static readonly string m_favorite = "m_favorite";
        /// <summary>
        /// 我的浏览历史
        /// </summary>
        public static readonly string m_history = "m_history";
        /// <summary>
        /// 页面点击量
        /// </summary>
        public static readonly string m_page_click_log = "m_page_click_log";
        /// <summary>
        /// 文章点击量
        /// </summary>
        public static readonly string m_artical_click_log = "m_artical_click_log";
        /// <summary>
        /// 用户登录记录
        /// </summary>
        public static readonly string m_user_login_log = "m_user_login_log";
        /// <summary>
        /// 用户反馈
        /// </summary>
        public static readonly string m_feedback = "m_feedback";
        /// <summary>
        /// 用户分享
        /// </summary>
        public static readonly string m_share = "m_share";

    }
}
