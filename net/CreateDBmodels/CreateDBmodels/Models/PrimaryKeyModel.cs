using System;

namespace CreateDBmodels.Models
{
    /// <summary>
    /// 数据表主键信息
    /// </summary>
    public class PrimaryKeyModel
    {

        /// <summary>
        /// 表名
        /// </summary>
        public String TABLE_NAME { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public String COLUMN_NAME { get; set; }

    }
}
