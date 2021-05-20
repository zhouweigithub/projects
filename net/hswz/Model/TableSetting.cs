using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Hswz.Model
{

    [XmlRoot]
    public class TableList
    {
        [XmlElement("TableSetting")]
        public List<TableSetting> TableSettings { get; set; }
    }
    /// <summary>
    /// 数据表信息
    /// </summary>
    public class TableSetting
    {
        /// <summary>
        /// 表名
        /// </summary>
        [XmlAttribute]
        public String TableName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        [XmlAttribute]
        public String KeyField { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        [XmlAttribute]
        public String OrderbyFields { get; set; }
    }
}
