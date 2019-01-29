using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Custom
{
    public class CustInvationUserInfo
    {
        /// <summary>
        /// 学员id
        /// </summary>
        public int student_id { get; set; }
        /// <summary>
        /// 直接邀请人id
        /// </summary>
        public int from1 { get; set; }
        /// <summary>
        /// 间接邀请人id
        /// </summary>
        public int from2 { get; set; }
    }
}
