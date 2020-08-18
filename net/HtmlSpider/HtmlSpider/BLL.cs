using System;
using System.Text;
using HtmlSpider.Model;

namespace HtmlSpider
{
    public class BLL
    {

        public static PageInfo GetPageInfo(String url)
        {
            (String html, String charset) = Common.GetRemoteHtml(url, Encoding.Default);

            if (String.IsNullOrEmpty(html))
                return null;

            html = html.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\t", String.Empty);

            PageInfo result = new PageInfo
            {
                Title = Common.GetTitle(html),
                H1 = Common.GetH1(html),
                KeyWords = Common.GetKeywords(html),
                Content = Common.GetContent(html),
            };

            return result;
        }


        private static String GetInnerHtml(String html)
        {
            return String.Empty;
        }
    }
}
