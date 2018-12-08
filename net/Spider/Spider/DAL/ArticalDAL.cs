using MySql.Data.MySqlClient;
using Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DAL
{
    public class ArticalDAL : BaseDAL
    {
        private static readonly string conString = WebConfigData.ConnString;
        private static readonly string insertSql = "INSERT INTO my_test.t_artical ( title, content, domain, crtime, year, area) VALUES( @title,@content,@domain,@crtime,@year,@area ) ;";
        private static readonly string[] provinceList = new string[] { "全国", "北京", "天津", "上海", "重庆", "河北", "山西", "辽宁", "吉林", "黑龙江", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "海南", "四川", "贵州", "云南", "陕西", "甘肃", "青海", "台湾", "内蒙古", "广西", "西藏", "宁夏", "新疆", "香港", "澳门" };
        private static readonly string[] yearLest = new string[] { "1997", "1998", "1999", "2000", "2001", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020" };

        public static bool InsertData(string title, string content, string domain)
        {
            try
            {
                string province = string.Empty;
                string year = string.Empty;

                foreach (string item in provinceList)
                {
                    if (title.Contains(item))
                    {
                        province = item;
                        break;
                    }
                }

                foreach (string item in yearLest)
                {
                    if (title.Contains(item))
                    {
                        year = item;
                        break;
                    }
                }

                MySqlParameter[] paramList = new MySqlParameter[]
                {
                    new MySqlParameter("@title",title),
                    new MySqlParameter("@content", content),
                    new MySqlParameter("@domain", domain),
                    new MySqlParameter("@year", year),
                    new MySqlParameter("@area", province),
                    new MySqlParameter("@crtime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                };

                return ExecuteNonQuery(conString, insertSql, paramList) > 0;
            }
            catch (Exception e)
            {
                LogUtil.Write("写入文章失败：" + e, LogType.Error);
            }

            return false;
        }
    }
}
