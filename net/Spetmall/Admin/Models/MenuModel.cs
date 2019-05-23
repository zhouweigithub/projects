using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Spetmall.Admin.Models
{
    /// <summary>
    /// 系统菜单信息
    /// </summary>
    [XmlRoot]
    public class MenuModel
    {
        /// <summary>
        /// 一级标题
        /// </summary>
        [XmlElement("MenuGroup")]
        public List<MenuGroup> MenuGroupList { get; set; }
    }

    /// <summary>
    /// 一级标题
    /// </summary>
    public class MenuGroup
    {
        /// <summary>
        /// 一级标题编号
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }
        /// <summary>
        /// 一级标题
        /// </summary>
        [XmlAttribute]
        public string text { get; set; }
        /// <summary>
        /// 二级标题列表
        /// </summary>
        [XmlElement("MenuItem")]
        public List<MenuItem> MenuItemList { get; set; }

    }

    /// <summary>
    /// 二级标题
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// 二级标题编号
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }
        /// <summary>
        /// 二级标题名称
        /// </summary>
        [XmlAttribute]
        public string Text { get; set; }
        /// <summary>
        /// 二级标题Controller
        /// </summary>
        [XmlAttribute]
        public string Controller { get; set; }

        /// <summary>
        /// 二级标题View
        /// </summary>
        [XmlAttribute]
        public string Action { get; set; }

    }
}