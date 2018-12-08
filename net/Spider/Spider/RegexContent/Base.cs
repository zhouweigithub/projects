using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.RegexContent
{
    public class RegexBase
    {
        public string TitleRegex;
        public string ContentRegex;

        protected RegexBase()
        {

        }
    }

    /// <summary>
    /// zuowen.com中的标题和内容的正则式
    /// </summary>
    public class zuowen : RegexBase
    {
        public zuowen()
        {
            TitleRegex = "<h1 class=\"h_title\">(?<title>.+?)</h1>";
            ContentRegex = "<div class=\"con_content\">(?<content>(.|\\n)+?)</div>";
        }
    }

    public class gaokao : RegexBase
    {
        public gaokao()
        {
            TitleRegex = "<h1 class=\"bm10\">.+?target=\"_blank\">(?<title>.+?)</a></h1>";
            ContentRegex = "<div class=\"main\">(?<content>(.|\n)+?)</div>";
        }
    }

    public class zxxk : RegexBase
    {
        public zxxk()
        {
            TitleRegex = "<h1>(?<title>.+?)</h1>";
            ContentRegex = "<div class=\"info_con\">(?<content>(.|\n)+?)<div class=\"showpageList\">";
        }
    }

    public class diyifanwen : RegexBase
    {
        public diyifanwen()
        {
            TitleRegex = "<h1>(?<title>.+?)</h1>";
            ContentRegex = "<p>(?<content>(.|\n)+)</p>";
        }
    }

}
