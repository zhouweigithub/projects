// ******************************************************
// 文件名（FileName）:               CommonReturnObject.ashx
// 功能描述（Description）:          通用返回对象
// 数据表（Tables）:                 Nothing
// 作者（Author）:                   Jordan Zuo
// 日期（Create Date）:              2015-01-13
// 修改记录（Revision History）:     
// ******************************************************

using System;

namespace Util
{
    /// <summary>
    /// 通用返回对象
    /// </summary>
    public class CommonReturnObject
    {
        /// <summary>
        /// 返回的状态值；0：成功；非0：失败（根据实际情况进行定义）
        /// </summary>
        public Int32 Code { get; set; }

        /// <summary>
        /// 返回的描述信息；
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public Object Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonReturnObject"/> class.
        /// </summary>
        public CommonReturnObject()
            : this(0, String.Empty, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonReturnObject"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        public CommonReturnObject(Int32 code, String message, Object data)
        {
            this.Code = code;
            this.Message = message;
            this.Data = data;
        }
    }
}