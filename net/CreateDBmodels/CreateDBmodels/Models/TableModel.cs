using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string TABLE_NAME { get; set; }
        /// <summary>
        /// 表的类型（BASE TABLE/VIEW）
        /// </summary>
        public string TABLE_TYPE { get; set; }
        /// <summary>
        /// 表的备注
        /// </summary>
        public string TABLE_COMMENT { get; set; }
    }
}
