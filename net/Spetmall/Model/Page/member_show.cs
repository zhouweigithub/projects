using Spetmall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Model.Page
{

    /// <summary>
    /// 页面展示的模型
    /// </summary>
    public class member_show : member
    {
        /// <summary>
        /// 总充值金额
        /// </summary>
        public decimal recharge { get; set; }
        /// <summary>
        /// 消费次数
        /// </summary>
        public int order_count { get; set; }
        /// <summary>
        /// 喜爱的宠物类型
        /// </summary>
        public string pets { get; set; }
        /// <summary>
        /// pets的展示HTML代码
        /// </summary>
        public string petsHtml
        {
            get
            {
                string html = string.Empty;
                if (!string.IsNullOrWhiteSpace(pets))
                {
                    var tmpArray = pets.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in tmpArray)
                    {
                        html += $"<span class=\"tags\">{item}</span>";
                    }
                }
                return html;
            }
        }

    }
}
