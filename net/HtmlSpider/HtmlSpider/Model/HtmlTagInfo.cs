using System;

namespace HtmlSpider.Model
{
    /// <summary>
    /// HTML标签对象
    /// </summary>
    public class HtmlTagInfo
    {
        /// <summary>
        /// 标签代码（可能为正则式），如<span>
        /// </summary>
        public String TagCode;
        /// <summary>
        /// 真实标签文本
        /// </summary>
        public String TagContent;
        /// <summary>
        /// 标签所在的位置
        /// </summary>
        public Int32 TagIndex;
        /// <summary>
        /// 1和-1 开始标签为1，结束标签为-1
        /// </summary>
        public Int32 Value;
    }
}
