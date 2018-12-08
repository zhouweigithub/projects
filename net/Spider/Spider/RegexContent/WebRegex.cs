using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.RegexContent
{
    public class WebRegex
    {
        public static RegexBase GetSiteRegex(string domain)
        {
            RegexBase result = null;
            switch (domain)
            {
                case "www.zuowen.com":
                    result = new zuowen();
                    break;
                case "www.gaokao.com":
                    result = new gaokao();
                    break;
                //case "gaokao.zxxk.com":
                //    result = new zxxk();
                //    break;
                case "www.diyifanwen.com":
                    result = new diyifanwen();
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
