using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 满就减编辑时发送到页面上的数据
    /// </summary>
    public class fullsend_edit : fullsend
    {
        public IList<salerule> rules { get; set; }
        public IList<saleproductObject> products { get; set; }
    }

    /// <summary>
    /// 满就减编辑回发信息
    /// </summary>
    public class fullsend_post : fullsend
    {
        /// <summary>
        /// 存储商品分类或商品，逗号隔开
        /// </summary>
        public string categoryOrProducts { get; set; }
        /// <summary>
        /// 存储折扣规则
        /// </summary>
        public string rules { get; set; }
    }
}
