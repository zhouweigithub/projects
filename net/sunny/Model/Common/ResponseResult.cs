using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model
{
    /// <summary>
    /// ajax请求返回数据实体
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// 编码（默认值为0）
        /// </summary>
        public int code;
        /// <summary>
        /// 信息（默认值为ok）
        /// </summary>
        public string msg;
        /// <summary>
        /// 数据
        /// </summary>
        public object data;

        private ResponseResult()
        {
        }

        /// <summary>
        /// ajax请求返回数据实体
        /// </summary>
        /// <param name="_code">编码</param>
        /// <param name="_msg">信息</param>
        /// <param name="_data">数据</param>
        public ResponseResult(int _code = 0, string _msg = "ok", object _data = null)
        {
            code = _code;
            msg = _msg;
            data = _data;
        }
    }
}
