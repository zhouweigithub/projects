using System;

namespace Hswz.DAL
{

    /// <summary>
    /// 数据库中所有的表名
    /// </summary>
    public class DBTable
    {
        /// <summary>
        /// 网站域名表
        /// </summary>
        public static readonly String url = "url";
        /// <summary>
        /// 网站域名赞与踩表
        /// </summary>
        public static readonly String url_attention = "url_attention";
        /// <summary>
        /// 网站点赞或踩的ip信息
        /// </summary>
        public static readonly String url_attention_ip = "url_attention_ip";
        /// <summary>
        /// 网站域名连接时长表
        /// </summary>
        public static readonly String url_connect_time = "url_connect_time";
        /// <summary>
        /// 用户点击信息
        /// </summary>
        public static readonly String url_click = "url_click";


        /// <summary>
        /// 资源表
        /// </summary>
        public static readonly String resource = "resource";
        /// <summary>
        /// 资源域名表
        /// </summary>
        public static readonly String domain = "domain";
        /// <summary>
        /// 已经搜索过的域名表
        /// </summary>
        public static readonly String tmp_searched_domains = "tmp_searched_domains";
        /// <summary>
        /// 资源详情链接
        /// </summary>
        public static readonly String resource_items = "resource_items";
        /// <summary>
        /// 已经获取到的各个站点的数据列表页的链接地址
        /// </summary>
        public static readonly String p_list_format = "p_list_format";
        /// <summary>
        /// 不再检测是否有列表页的站点
        /// </summary>
        public static readonly String p_list_forbid = "p_list_forbid";
        


        #region old

        /// <summary>
        /// 用户表
        /// </summary>
        public static readonly String m_user = "m_user";
        /// <summary>
        /// 文章表
        /// </summary>
        public static readonly String m_artical = "m_artical";
        /// <summary>
        /// 我的收藏
        /// </summary>
        public static readonly String m_favorite = "m_favorite";
        /// <summary>
        /// 我的浏览历史
        /// </summary>
        public static readonly String m_history = "m_history";
        /// <summary>
        /// 页面点击量
        /// </summary>
        public static readonly String m_page_click_log = "m_page_click_log";
        /// <summary>
        /// 文章点击量
        /// </summary>
        public static readonly String m_artical_click_log = "m_artical_click_log";
        /// <summary>
        /// 用户登录记录
        /// </summary>
        public static readonly String m_user_login_log = "m_user_login_log";
        /// <summary>
        /// 用户反馈
        /// </summary>
        public static readonly String m_feedback = "m_feedback";
        /// <summary>
        /// 用户分享
        /// </summary>
        public static readonly String m_share = "m_share";
        /// <summary>
        /// 设备号与OPENID对应关系
        /// </summary>
        public static readonly String m_device_openid = "m_device_openid";

        #endregion
    }
}
