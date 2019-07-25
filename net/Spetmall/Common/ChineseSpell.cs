using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spetmall.Common
{
    /// <summary>
    /// 汉字拼音
    /// </summary>
    public class ChineseSpell
    {
        /// <summary>
        /// 获取汉字拼音首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetChineseSpell(string strText)
        {
            int len = strText.Length;
            string myStr = "";
            for (int i = 0; i < len; i++)
            {
                myStr += GetSpell(strText.Substring(i, 1));
            }
            return myStr;
        }

        /// <summary>
        /// 获取汉字拼音首字母
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string[] GetChineseSpell(string[] strText)
        {
            int len = strText.Length;
            string[] myStr = null;
            for (int i = 0; i < len; i++)
            {
                myStr[i] = GetSpell(strText[i]);
            }
            return myStr;
        }

        /// <summary>
        /// 获取汉字拼音首字母
        /// </summary>
        /// <param name="cnChar"></param>
        /// <returns></returns>
        private static string GetSpell(string cnChar)
        {
            byte[] arrCN = Encoding.Default.GetBytes(cnChar);
            if (arrCN.Length > 1)
            {
                int area = arrCN[0];
                int pos = arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "*";
            }
            else return cnChar;
        }
    }
}
