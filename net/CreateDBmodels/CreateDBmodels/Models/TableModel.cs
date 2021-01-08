using System;

namespace CreateDBmodels.Models
{
    /// <summary>
    /// 数据表信息
    /// </summary>
    public class TableModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public String TABLE_NAME { get; set; }
        /// <summary>
        /// 表的类型（BASE TABLE/VIEW）
        /// </summary>
        public String TABLE_TYPE { get; set; }
        /// <summary>
        /// 表的备注
        /// </summary>
        public String TABLE_COMMENT { get; set; }
    }
}
