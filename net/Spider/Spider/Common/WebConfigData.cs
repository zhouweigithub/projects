//***********************************************************************************
//文件名称：WebConfigData.cs
//功能描述：web.config文件中配置的信息
//数据表： 
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Common
{
    /// <summary>
    /// web.config文件配置的参数
    /// </summary>
    public class WebConfigData
    {
        public static readonly string ConnString;
        public static readonly string LinkKeyWordsList;
        public static readonly string TitleKeyWordsList;
        public static readonly string ExceptKeyWordsList;
        public static readonly bool IsOnlyThisSite;
        public static readonly int ThreadCount;


        static WebConfigData()
        {
            try
            {
                ConnString = Config.GetConfigToString("ConnString");
                LinkKeyWordsList = Config.GetConfigToString("LinkKeyWordsList");
                TitleKeyWordsList = Config.GetConfigToString("TitleKeyWordsList");
                ExceptKeyWordsList = Config.GetConfigToString("ExceptKeyWordsList");
                IsOnlyThisSite = Config.GetConfigToBool("IsOnlyThisSite");
                ThreadCount = Config.GetConfigToInt("ThreadCount");
            }
            catch (Exception e)
            {
                LogUtil.Write("读取webconfig数据时出错\t" + e.Message, LogType.Error);
            }
        }
    }
}
