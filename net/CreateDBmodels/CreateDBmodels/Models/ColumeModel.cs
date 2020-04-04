using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string TABLE_NAME { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string COLUMN_NAME { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DATA_TYPE { get; set; }
        /// <summary>
        /// 列备注
        /// </summary>
        public string COLUMN_COMMENT { get; set; }
    }
}
