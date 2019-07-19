using HtmlSpider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HtmlSpider
{
    public class BLL
    {

        public static PageInfo GetPageInfo(string url)
        {
            (string html, string charset) = Common.GetRemoteHtml(url, Encoding.Default);

            if (string.IsNullOrEmpty(html))
                return null;

            PageInfo result = new PageInfo
            {
                Title = Common.GetTitle(html),
                H1 = Common.GetH1(html),
                KeyWords = Common.GetKeywords(html),
                Content = Common.GetContent(html),
            };

            return result;
        }


        private static string GetInnerHtml(string html)
        {
            return string.Empty;
        }
    }
}
