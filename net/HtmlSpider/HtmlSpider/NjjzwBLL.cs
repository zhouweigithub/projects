using HtmlSpider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace HtmlSpider
{
    public class NjjzwBLL
    {
        private Regex reg = new Regex("<td.*?>\\d{1,4}、(?<question>.*?)</td>(.|\\n)*?<td.*?><INPUT onClick=\"MM_popupMsg\\('答案：(?<answer>.*?)'\\)\" type=button value=答案></td>");
        private List<Njjzw> datas = new List<Njjzw>();
        private Encoding encoding = Encoding.GetEncoding("gb2312");

        public void GetData(string url)
        {
            try
            {
                (string html, string charset) = Common.GetRemoteHtml(url, Encoding.Default);
                if (string.IsNullOrWhiteSpace(html))
                    return;

                var collection = reg.Matches(html);

                foreach (Match item in collection)
                {
                    Njjzw data = new Njjzw()
                    {
                        Question = item.Groups["question"].Value,
                        Answer = item.Groups["answer"].Value,
                    };
                    datas.Add(data);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void SaveDatas()
        {
            using (FileStream fs = File.OpenWrite(AppDomain.CurrentDomain.BaseDirectory + "data.txt"))
            {
                foreach (Njjzw item in datas)
                {
                    string text = $"{item.Question}\t{item.Answer}\r\n";
                    byte[] bytes = encoding.GetBytes(text);
                    fs.Write(bytes, 0, bytes.Length);
                }
                fs.Close();
            }
        }
    }
}
