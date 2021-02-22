using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.Model
{

    /// <summary>
    /// 数据表信息
    /// </summary>
    public class TableSetting
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string KeyField { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderbyFields { get; set; }
        /// <summary>
        /// 是否缓存数据
        /// </summary>
        public bool IsAddIntoCache { get; set; }
    }
}
