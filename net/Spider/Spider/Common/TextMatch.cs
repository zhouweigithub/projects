using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider.Common
{
    public class TextMatch
    {
        /// <summary>
        /// 计算文章相似度（匹配相同语句）
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int GetSimilarity(string str1, string str2)
        {
            //将所有空白字符替换为逗号
            string tmp1 = Regex.Replace(str1, "\\s+", ",");
            string tmp2 = Regex.Replace(str2, "\\s+", ",");

            //按照标点符号集拆分语句
            string punctuations = " ,.?'\"!()<>{}[]-_+-*/\\　 ，。？‘“”！、：；《》（）【】－……——";
            char[] splitCharArry = punctuations.ToArray();
            //语句集
            var strArray1 = tmp1.Split(splitCharArry, StringSplitOptions.RemoveEmptyEntries).Distinct();
            var strArray2 = tmp2.Split(splitCharArry, StringSplitOptions.RemoveEmptyEntries).Distinct();
            //找出语句少的和多的集合
            var minArray = strArray1.Count() < strArray2.Count() ? strArray1 : strArray2;
            var maxArray = minArray == strArray1 ? strArray2 : strArray1;

            int sameCount = 0;      //相同语句的数量
            foreach (var item in minArray)
            {   //遍历小集合中的每句话，如果大集合中有这一句，则加1
                if (maxArray.Count(a => a == item) > 0)
                    sameCount++;
            }

            //最后用相同语句的数量除以大集合的数量，得到相似度
            return (int)((double)sameCount / maxArray.Count() * 100);
        }

    }
}
