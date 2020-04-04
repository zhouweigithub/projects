using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{
    /// <summary>
    /// 限时折扣编辑时发送到页面上的数据
    /// </summary>
    public class discount_edit : discount
    {
        public IList<salerule> rules { get; set; }
        public IList<saleproductObject> products { get; set; }
    }

    /// <summary>
    /// 限时折扣编辑回发信息
    /// </summary>
    public class discount_post : discount
    {
        /// <summary>
        /// 存储商品分类或商品
        /// </summary>
        public string categoryOrProducts { get; set; }
        /// <summary>
        /// 存储折扣规则
        /// </summary>
        public string rules { get; set; }
    }
}
