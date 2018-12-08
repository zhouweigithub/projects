using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moqikaka.Tmp.Model
{

    /// <summary>
    /// 键值对（值相同则对象相同）
    /// </summary>
    public class DataValueModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// 键值对（值相同则对象相同）
        /// </summary>
        public DataValueModel()
        {

        }

        /// <summary>
        /// 键值对（值相同则对象相同）
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="value">值</param>
        public DataValueModel(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public override string ToString()
        {
            return string.Format("Text={0}, Value={1}", this.Text, this.Value);
        }

        public override bool Equals(object obj)
        {
            return ((DataValueModel)obj).Value == this.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

    }
}
