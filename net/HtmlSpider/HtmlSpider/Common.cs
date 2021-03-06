﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlSpider.Model;

namespace HtmlSpider
{
    public class Common
    {
        private static readonly Regex h1Reg = new Regex("<h1.*?>(?<h1>(.|\\n)*?)</h1>", RegexOptions.IgnoreCase);
        private static readonly Regex titleReg = new Regex("<title.*?>(?<title>(.|\\n)*?)</title>", RegexOptions.IgnoreCase);
        private static readonly Regex keyWordsReg = new Regex("<meta.*? name=\"keywords\".*? content=\"(?<keywords>.*?)\" */*?>", RegexOptions.IgnoreCase);
        private static readonly Regex charsetReg = new Regex("<meta.*?charset=\"*(?<charset>.*?)\".*?/*?>", RegexOptions.IgnoreCase);

        private static readonly List<String> tags3 = new List<String> { "<||>" };
        //文章内容存放区域
        private static readonly List<String> contentTagNamess = new List<String>() { "content", "detail", "article", "text" };

        private static readonly String attrFormatString = "<{0} [^>]*?(id|class)=[^>=]*?{1}.*?>";

        private static readonly Regex aReg = new Regex(@"<a .*?href=""(?<href>.*?)"".*?>.*?</a>", RegexOptions.IgnoreCase);


        /// <summary>
        /// 以当前计算机默认编码读取网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static (String html, String charset) GetRemoteHtml(String url, Encoding encoding)
        {
            Console.WriteLine("获取远程页面内容...");
            String html = String.Empty;
            String charSet = String.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "Get";
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), encoding);
                html = reader.ReadToEnd();
                charSet = myResponse.CharacterSet;

                reader.Close();
                myResponse.Close();

                String htmlCharset = GetCharset(html);
                if (!String.IsNullOrEmpty(htmlCharset))
                {
                    charSet = htmlCharset;
                }

                Encoding htmlEncoding = Encoding.GetEncoding(charSet);
                if (encoding != htmlEncoding && String.Compare(charSet, "ISO-8859-1", true) != 0)
                {   //如果远网页的实际编码不是default同时实际编码不是ISO-8859-1，则重新按照实际编码再获取一次源码
                    (html, charSet) = GetRemoteHtml(url, htmlEncoding);
                }

            }
            catch (Exception e)
            {
                String msg = $"获取网页内容失败。\r\nurl:{url}\r\n{e.Message}";
                Util.Log.LogUtil.Write(msg, Util.Log.LogType.Error);
                Console.WriteLine(msg);
            }

            return (html, charSet);
        }

        /// <summary>
        /// 获取网页内容中注明的字符集
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String GetCharset(String html)
        {
            Match match = charsetReg.Match(html);
            if (match.Success)
            {
                return match.Groups["charset"].Value.Trim();
            }
            return String.Empty;
        }

        public static String GetTitle(String html)
        {
            Console.WriteLine("读取标题...");
            return GetRegexValue(html, titleReg, "title");
        }

        public static String GetH1(String html)
        {
            Console.WriteLine("读取h1...");
            String h1 = GetRegexValue(html, h1Reg, "h1");
            if (h1.Contains("<"))
            {
                h1 = GetInnerText(h1);
            }

            return h1;
        }

        public static List<String> GetHrefs(String html)
        {
            Console.WriteLine("读取a...");
            List<String> hrefs = GetRegexValues(html, aReg, "href");

            return hrefs;
        }

        public static String GetKeywords(String html)
        {
            Console.WriteLine("读取关键字...");
            return GetRegexValue(html, keyWordsReg, "keywords");
        }

        public static String GetContent(String html)
        {
            Console.WriteLine("分析页面核心内容...");

            html = html.Replace("&nbsp;", " ").Replace("<br />", "\n").Replace("<br/>", "\n");

            String displayNoneRegString = "<{0} [^>]*?style=[^>]*?display:.*?none.*?>";

            List<String> tags1 = new List<String> { "<head||</head>", "<script||</script>", "<style||</style>", "<!--||-->", "<nav||</nav>" };

            List<String> singleTas = new List<String>() { "br", "hr", "img", "input", "link" };

            String[] tags2 = new String[] { "<a||</a>", "<h||</h>", "<span||</span>", "<p||</p>", "<strong||</strong>", "<u||</u>", "<b||</b>", "<big||</big>", "<del||</del>", "<em||</em>", "<ins||</ins>", "<small||</small>", "<sub||</sub>", "<sup||</sup>" };

            List<String> regStrings = new List<String>();

            //标签集
            List<String> tags = new List<String>() { "div", "ul", "table", "section", "span" };

            List<String> contentTags = new List<String>() { "div", "section" };


            //标签内的关键字集
            List<String> names = new List<String>() { "page", "nav", "head", "comment", "hot", "foot", "commend", "more", "grid", "list", "menu", "share" };

            foreach (String tag in tags)
            {
                foreach (String name in names)
                {
                    regStrings.Add(String.Format(attrFormatString, tag, name));
                }
            }

            List<String> displyNoneRegs = new List<String>();

            foreach (String tag in tags)
            {
                displyNoneRegs.Add(String.Format(displayNoneRegString, tag));
            }




            List<String> contentRegStrings = new List<String>();

            foreach (String tag in contentTags)
            {
                foreach (String name in contentTagNamess)
                {
                    contentRegStrings.Add(String.Format(attrFormatString, tag, name));
                }
            }

            //移除无用标签及内容，并在末尾添加换行符
            html = RemoveExtroChars(html, tags1);

            //删除一些无需单独结尾标记的标签内容
            html = RemoveSingleTag(html, singleTas);

            //删除空白内容标签
            html = RemoveEmptyTabs(html);

            //直接根据可能的标签取内容
            String content = GetContentByRegex(html, contentRegStrings);

            if (!String.IsNullOrEmpty(content))
            {
                html = content;
            }

            //根据正则式移除部分节点
            html = RemoveByRegex(html, regStrings);

            //删除display:none内容
            html = RemoveByRegex(html, displyNoneRegs);

            //移除这些标签，但保留之间的文本
            html = RemoveExtroCharsLeaveInnerTHML(html, tags2);

            //移除这些标签内的所有字符
            html = RemoveExtroChars(html, tags3);

            //过滤文本
            html = FilterContent(html);

            html = WebUtility.HtmlDecode(html);

            return html;
        }

        private static String GetRegexValue(String html, Regex reg, String groupName)
        {
            Match match = reg.Match(html);
            if (match.Success)
            {
                return match.Groups[groupName].Value.Trim();
            }
            return String.Empty;
        }

        private static List<String> GetRegexValues(String html, Regex reg, String groupName)
        {
            List<String> result = new List<String>();
            MatchCollection matches = reg.Matches(html);
            foreach (Match item in matches)
            {
                result.Add(item.Groups[groupName].Value.Trim());
            }
            return result;
        }

        /// <summary>
        /// 根据正则式移除部分节点
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static String RemoveByRegex(String html, List<String> tags)
        {
            foreach (var tag in tags)
            {
                //MatchCollection mc = Regex.Matches(html, tag, RegexOptions.IgnoreCase);
                //if (mc.Count > 20)
                //    break;

                Regex reg = new Regex(tag);
                Match match = Regex.Match(html, tag, RegexOptions.IgnoreCase);

                while (match.Success)
                {
                    HtmlTagInfo endTag = GetEndTag(html, match.Value, match.Index);

                    if (endTag != null)
                    {
                        String node = html.Substring(match.Index, endTag.TagIndex + endTag.TagContent.Length - match.Index);

                        //内容中不包含特定字符就删除
                        //if (!IsContainsReg(node, contentTagNamess))
                        if (true)
                        {
                            if (true)
                            {
                                html = html.Remove(match.Index, endTag.TagIndex - match.Index + endTag.TagContent.Length);
                                match = Regex.Match(html, tag, RegexOptions.IgnoreCase);
                            }
                            else
                            {
                                match = reg.Match(html, match.Index + match.Value.Length);
                            }
                        }
                    }
                    else
                    {
                        match = reg.Match(html, match.Index + match.Value.Length);
                    }
                }
            }

            return html;
        }

        /// <summary>
        /// 直接根据标签取内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static String GetContentByRegex(String html, List<String> tags)
        {
            List<String> sources = new List<String>();
            List<String> contents = new List<String>();

            foreach (var tag in tags)
            {
                Regex reg = new Regex(tag);
                MatchCollection mc = Regex.Matches(html, tag, RegexOptions.IgnoreCase);

                foreach (Match match in mc)
                {
                    HtmlTagInfo endTag = GetEndTag(html, match.Value, match.Index);

                    if (endTag != null)
                    {
                        String nodeHtml = html.Substring(match.Index, endTag.TagIndex + endTag.TagContent.Length - match.Index);
                        String innerHtml = GetInnerText(nodeHtml).Replace(" ", String.Empty).Trim();
                        if (!String.IsNullOrEmpty(innerHtml))
                        {
                            sources.Add(nodeHtml);
                            contents.Add(innerHtml);
                        }
                    }
                }

            }

            //取内容最多的那个
            if (contents.Count > 0)
            {
                String tmpContent = contents.OrderByDescending(a => a.Length).First();
                return sources[contents.IndexOf(tmpContent)];
            }
            else
            {
                return String.Empty;
            }
        }


        private static String GetInnerText(String html)
        {
            return RemoveExtroChars(html, tags3, false);
        }

        /// <summary>
        /// 是否包含指定正则式的内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private static Boolean IsContainsReg(String html, List<String> tags)
        {
            foreach (String tag in tags)
            {
                Match mmm = Regex.Match(html, String.Format(attrFormatString, "div", tag));
                if (Regex.IsMatch(html, String.Format(attrFormatString, "div", tag)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 移除无用标签及内容，并在末尾添加换行符
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <param name="isAddBreakLineChar">是否在移除的标签后面添加换行符</param>
        /// <returns></returns>
        public static String RemoveExtroChars(String html, List<String> tags, Boolean isAddBreakLineChar = true)
        {
            Int32 breakLineCharCount = 0;
            foreach (var tag in tags)
            {
                List<HtmlTagInfo> searchedTags = GetPositions(html, tag);
                HtmlTagInfo endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                while (endTag != null)
                {
                    Int32 index = searchedTags.IndexOf(endTag);
                    HtmlTagInfo preTag = index >= 1 ? searchedTags[index - 1] : null;
                    if (preTag == null)
                    {
                        html = html.Remove(endTag.TagIndex, endTag.TagContent.Length);
                        if (isAddBreakLineChar)
                        {
                            html = html.Insert(endTag.TagIndex, "\n");  //增加了一个换行符
                            breakLineCharCount = 1;
                        }
                        searchedTags.Where(a => a.TagIndex > endTag.TagIndex).ToList().ForEach(b => b.TagIndex -= endTag.TagContent.Length - breakLineCharCount);
                        searchedTags.Remove(endTag);
                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                    else if (preTag.Value == 1)
                    {   //前一个为开始标签
                        Int32 removeCount = endTag.TagIndex + endTag.TagContent.Length - preTag.TagIndex;
                        String nodeString = html.Substring(preTag.TagIndex, removeCount);
                        html = html.Remove(preTag.TagIndex, removeCount);
                        if (isAddBreakLineChar)
                        {
                            html = html.Insert(preTag.TagIndex, "\n");  //增加了一个换行符
                            breakLineCharCount = 1;
                        }

                        //更新节点之后的节点的index
                        searchedTags.Where(a => a.TagIndex > preTag.TagIndex).ToList().ForEach(b => b.TagIndex -= removeCount - breakLineCharCount);

                        //去除相关HTML代码后，移除相关节点数据
                        searchedTags.Remove(preTag);
                        searchedTags.Remove(endTag);

                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                    else
                    {
                        Util.Log.LogUtil.Write($"结束标签前面还是结束标签", Util.Log.LogType.Error);
                        searchedTags.Remove(endTag);
                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                }

            }

            return html;
        }

        /// <summary>
        /// 删除空白内容标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String RemoveEmptyTabs(String html)
        {
            MatchCollection mc = Regex.Matches(html, "<[^/>]+?> *?</.+?>", RegexOptions.IgnoreCase);

            foreach (Match match in mc)
            {
                html = html.Replace(match.Value, String.Empty);
            }

            return html;
        }


        /// <summary>
        /// 将部分标签及内容替换为纯内容，末尾不添加换行符
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static String RemoveExtroCharsLeaveInnerTHML(String html, String[] tags)
        {
            foreach (var item in tags)
            {
                List<HtmlTagInfo> searchedTags = GetPositions(html, item);
                HtmlTagInfo endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                while (endTag != null)
                {
                    Int32 index = searchedTags.IndexOf(endTag);
                    HtmlTagInfo preTag = index >= 1 ? searchedTags[index - 1] : null;
                    if (preTag == null)
                    {
                        html = html.Remove(endTag.TagIndex, endTag.TagContent.Length);
                        html = html.Insert(endTag.TagIndex, "\n");  //增加了一个换行符
                        searchedTags.Where(a => a.TagIndex > endTag.TagIndex).ToList().ForEach(b => b.TagIndex -= endTag.TagContent.Length - 1);

                        searchedTags.Remove(endTag);
                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                    else if (preTag.Value == 1)
                    {   //前一个为开始标签
                        Int32 removeCount = endTag.TagIndex + endTag.TagContent.Length - preTag.TagIndex;
                        String innerHTML = GetInnerHTML(html.Substring(preTag.TagIndex, removeCount));
                        html = html.Remove(preTag.TagIndex, removeCount);
                        html = html.Insert(preTag.TagIndex, innerHTML);     //将标签及内容替换为内容

                        //p标签末尾加上换行符
                        Int32 addChars = 0;
                        if (endTag.TagContent == "</p>")
                        {
                            String addCharString = "\n";
                            html = html.Insert(preTag.TagIndex + innerHTML.Length, addCharString);
                            addChars = addCharString.Length;
                        }

                        //更新节点之后的节点的index
                        searchedTags.Where(a => a.TagIndex > preTag.TagIndex).ToList().ForEach(b => b.TagIndex -= removeCount - innerHTML.Length - addChars);

                        //去除相关HTML代码后，移除相关节点数据
                        searchedTags.Remove(preTag);
                        searchedTags.Remove(endTag);

                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                    else
                    {
                        Util.Log.LogUtil.Write($"结束标签前面还是结束标签", Util.Log.LogType.Error);
                        searchedTags.Remove(endTag);
                        endTag = searchedTags.FirstOrDefault(a => a.Value == -1);
                    }
                }

            }

            return html.Trim();
        }

        /// <summary>
        /// 从HTML中过滤出需要的内容，从最长字符处入手
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static String FilterContentByMaxLength(String html)
        {
            String[] htmlArray = html.Split(new String[] { "\n" }, StringSplitOptions.None);
            for (Int32 i = 0; i < htmlArray.Length; i++)
            {
                htmlArray[i] = htmlArray[i].Trim();
            }
            var htmlList = htmlArray.ToList();
            htmlList.RemoveAll(a => String.IsNullOrEmpty(a));

            String maxContentItem = htmlList.OrderByDescending(a => a.Length).FirstOrDefault();
            Int32 maxContentIndex = Array.IndexOf(htmlArray, maxContentItem);

            Int32 validLineCount = 3;     //视为有效的间隔最大空白行数
            Int32 sepreteCount = 0;       //已间隔的行数
            StringBuilder sb = new StringBuilder();

            //往前取文章有效部分
            for (Int32 i = maxContentIndex; i >= 0; i--)
            {
                String item = htmlArray[i];
                if (String.IsNullOrWhiteSpace(item))
                {
                    sepreteCount++;
                    if (sepreteCount > validLineCount)
                    {   //间隔空白行超过三个则扔掉
                        break;
                    }
                }
                else
                {
                    if (sepreteCount <= validLineCount)
                    {   //间隔三个空白行之内则仍然视为文章内容
                        sb.Insert(0, item + "\n");
                        sepreteCount = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            sepreteCount = 0;
            //往后取文章有效部分
            for (Int32 i = maxContentIndex + 1; i < htmlArray.Length; i++)
            {
                String item = htmlArray[i];
                if (String.IsNullOrWhiteSpace(item))
                {
                    sepreteCount++;
                    if (sepreteCount > validLineCount)
                    {   //间隔空白行超过三个则扔掉
                        break;
                    }
                }
                else
                {
                    if (sepreteCount <= validLineCount)
                    {   //间隔三个空白行之内则仍然视为文章内容
                        sb.Append(item + "\n");
                        sepreteCount = 0;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return sb.ToString();
        }

        private static String FilterContent(String html)
        {
            String[] htmlArray = html.Split(new String[] { "\n" }, StringSplitOptions.None);
            for (Int32 i = 0; i < htmlArray.Length; i++)
            {
                htmlArray[i] = htmlArray[i].Trim();
            }

            Int32 validLineCount = 3;     //视为有效的间隔最大空白行数
            Int32 sepreteCount = 0;       //已间隔的行数
            List<String> lists = new List<String>();

            //往后取文章有效部分
            for (Int32 i = 0; i < htmlArray.Length; i++)
            {
                String item = htmlArray[i];
                if (String.IsNullOrWhiteSpace(item))
                {   //此行为空白行，则记录空白行数
                    sepreteCount++;
                }
                else
                {
                    if (sepreteCount <= validLineCount)
                    {   //间隔三个空白行之内则仍然视为文章内容
                        if (lists.Count == 0)
                        {
                            lists.Add(item + "\n");
                        }
                        else
                        {
                            lists[lists.Count - 1] = lists[lists.Count - 1] + item + "\n";
                        }
                    }
                    else
                    {
                        lists.Add(item + "\n");
                    }

                    //找到非空白行，则重新计数
                    sepreteCount = 0;
                }
            }

            //找到最长的那部分作为正文内容
            lists = lists.OrderByDescending(a => a.Length).ToList();
            return lists.Count == 0 ? String.Empty : lists[0];
        }


        /// <summary>
        /// 获取标签对存在的所有位置
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tagPair"></param>
        /// <returns></returns>
        private static List<HtmlTagInfo> GetPositions(String html, String tagPair)
        {
            String[] tags = tagPair.Split(new String[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            List<HtmlTagInfo> searchedTags = new List<HtmlTagInfo>();

            for (Int32 i = 0; i <= 1; i++)
            {
                String tagCode = tags[i];
                Regex reg = new Regex(tagCode);
                String tmpHtml = html;

                MatchCollection matchs = Regex.Matches(tmpHtml, tagCode, RegexOptions.IgnoreCase);

                foreach (Match item in matchs)
                {
                    HtmlTagInfo tag = new HtmlTagInfo()
                    {
                        TagCode = tagCode,
                        TagContent = item.Value,
                        TagIndex = item.Index,
                        Value = i == 0 ? 1 : -1
                    };
                    searchedTags.Add(tag);
                }
            }

            searchedTags = searchedTags.OrderBy(a => a.TagIndex).ToList();
            return searchedTags;
        }

        /// <summary>
        /// 将找到的标签信息存入链表
        /// </summary>
        /// <param name="searchedTags"></param>
        /// <returns></returns>
        private static MyLinkList<HtmlTagInfo> GetLinkLists(List<HtmlTagInfo> searchedTags)
        {
            MyLinkList<HtmlTagInfo> linkList = new MyLinkList<HtmlTagInfo>();
            if (searchedTags.Count == 0)
            {
                return linkList;
            }

            linkList.Add(searchedTags[0]);
            for (Int32 i = 1; i < searchedTags.Count; i++)
            {
                var pre = searchedTags[i - 1];
                var cur = searchedTags[i];

                //如果前一个为开始标签，当前为结束标签，则为一对，可以删除
                if (pre.Value == 1 && cur.Value == -1)
                {
                    if (linkList.Count > 1 && !IsAllTagsPaired(linkList))
                    {
                        linkList.Remove(pre);
                        continue;
                    }
                }
                linkList.Add(cur);
            }

            return linkList;
        }

        /// <summary>
        /// 检测前面所有的节点是否已配对完成
        /// </summary>
        /// <param name="linkList"></param>
        /// <returns></returns>
        private static Boolean IsAllTagsPaired(MyLinkList<HtmlTagInfo> linkList)
        {
            Int32 sum = 0;
            MyLinkListNode<HtmlTagInfo> curNode = linkList.FirstNode;
            while (curNode != null)
            {
                sum += curNode.Data.Value;
                curNode = curNode.Next;
            }

            return sum == 0;
        }

        /// <summary>
        /// 获取HTML的innnerHTML
        /// </summary>
        /// <param name="html">单个标签内容</param>
        /// <returns></returns>
        private static String GetInnerHTML(String html)
        {
            Int32 index1 = html.IndexOf("<");
            Int32 index2 = html.IndexOf(">", index1);
            Int32 startIndex = index2 + 1;

            Int32 index3 = html.LastIndexOf(">");
            Int32 index4 = html.LastIndexOf("<", index3);
            Int32 endIndex = index4;

            Char xxxx = html[startIndex];
            Char yyy = html[endIndex];

            return html.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 取结束标签位置
        /// </summary>
        /// <param name="html"></param>
        /// <param name="startTag"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private static HtmlTagInfo GetEndTag(String html, String startTag, Int32 startIndex)
        {
            //起始标签
            Regex regStart = new Regex("<[^/].*?>");

            //结束标签
            Regex regEnd = new Regex("( *?/>)|(</.+?>)");

            MatchCollection mcStart = regStart.Matches(html, startIndex);

            MatchCollection mcEnd = regEnd.Matches(html, startIndex);

            List<HtmlTagInfo> tagInfos = new List<HtmlTagInfo>();

            foreach (Match item in mcStart)
            {
                tagInfos.Add(new HtmlTagInfo()
                {
                    TagCode = regStart.ToString(),
                    TagContent = item.Value,
                    TagIndex = item.Index,
                    Value = 1
                });
            }

            foreach (Match item in mcEnd)
            {
                tagInfos.Add(new HtmlTagInfo()
                {
                    TagCode = regEnd.ToString(),
                    TagContent = item.Value,
                    TagIndex = item.Index,
                    Value = -1
                });
            }

            tagInfos = tagInfos.OrderBy(a => a.TagIndex).ToList();

            HtmlTagInfo tmpTagInfo = tagInfos.FirstOrDefault();

            Int32 startCount = 0;

            Int32 endCount = 0;

            String startTagName = startTag.Split(' ')[0].Replace("<", String.Empty);

            while (tmpTagInfo != null)
            {
                if (tmpTagInfo.Value == 1)
                {
                    startCount++;
                }
                else if (tmpTagInfo.Value == -1)
                {
                    //当开始标签与结束标签数量相等，又取到一个结束标签时，即为所找的结束标签
                    if (startCount == endCount + 1 && (tmpTagInfo.TagContent == " />" || tmpTagInfo.TagContent == $"</{startTagName}>"))
                    {
                        break;
                    }
                    else
                    {
                        endCount++;
                    }
                }

                //移除第一个
                tagInfos.Remove(tmpTagInfo);

                //重新取第一个
                tmpTagInfo = tagInfos.FirstOrDefault();
            }

            //if (tmpTagInfo != null)
            //{
            //    String nodeHtml = html.Substring(startIndex, tmpTagInfo.TagIndex + tmpTagInfo.TagContent.Length - startIndex);
            //    Console.WriteLine(nodeHtml);
            //}
            //else
            //{
            //    Console.WriteLine("未找到结束结点");
            //}

            return tmpTagInfo;
        }


        /// <summary>
        /// 删除一些无需单独结尾标记的标签内容
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private static String RemoveSingleTag(String html, List<String> tags)
        {
            foreach (String tag in tags)
            {
                Regex reg = new Regex($"<{tag}.*?/*?>");

                MatchCollection mc = reg.Matches(html);

                foreach (Match match in mc)
                {
                    html = html.Replace(match.Value, String.Empty);
                }
            }

            return html;
        }
    }
}
