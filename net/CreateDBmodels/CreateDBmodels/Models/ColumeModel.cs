using System;

namespace CreateDBmodels.Models
{
    /// <summary>
    /// 数据表各字段信息
    /// </summary>
    public class ColumeModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public String TABLE_NAME { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public String COLUMN_NAME { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public String DATA_TYPE { get; set; }
        /// <summary>
        /// 列备注
        /// </summary>
        public String COLUMN_COMMENT { get; set; }
    }
}
