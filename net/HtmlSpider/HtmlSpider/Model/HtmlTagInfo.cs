using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSpider.Model
{
    /// <summary>
    /// HTML标签对象
    /// </summary>
    public class HtmlTagInfo
    {
        /// <summary>
        /// 标签代码，如<span>
        /// </summary>
        public string TagCode;
        /// <summary>
        /// 标签所在的位置
        /// </summary>
        public int TagIndex;
        /// <summary>
        /// 1和-1 开始标签为1，结束标签为-1
        /// </summary>
        public int Value;
    }
}
