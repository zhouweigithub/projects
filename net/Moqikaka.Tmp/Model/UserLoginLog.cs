//***********************************************************************************
//文件名称：UserLoginLog.cs
//功能描述：sdkcenter.user_app_login_log表实体类
//数据表： sdkcenter.user_app_login_log
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moqikaka.Tmp.Model
{
    public class UserLoginLog
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string AppId { get; set; }
        public string AdSrc { get; set; }
        public string Udid { get; set; }
        public DateTime Crdate { get; set; }
        public DateTime Crtime { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LoginTime { get; set; }
    }
}
