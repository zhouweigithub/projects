using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spetmall.Model.Common;

namespace Spetmall.Model
{
    /// <summary>
    /// 会员喜欢的宠物类型(BASE TABLE)
    /// </summary>
    public class memberpet
    {

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        [TableField]
        public int memberid { get; set; }
        /// <summary>
        /// pet类型id
        /// </summary>
        [TableField]
        public int petid { get; set; }

    }
}
