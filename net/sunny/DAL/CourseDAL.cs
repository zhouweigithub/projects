using MySql.Data.MySqlClient;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    /// <summary>
    /// 课程信息操作类
    /// </summary>
    public class CourseDAL
    {

        /// <summary>
        /// 获取课程信息
        /// </summary>
        private static readonly string getCourseInfoListSql = @"
SELECT a.id courseid,a.name,a.main_img,MIN(d.price)minPrice,MAX(d.price)maxPrice,IFNULL(f.money,0) discount_money FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN product_specification_detail c ON a.id=c.product_id
LEFT JOIN product_specification_detail_price d ON c.product_id=d.product_id AND c.plan_code=d.plan_code
LEFT JOIN product_discount e ON a.id=e.product_id AND e.state=0
LEFT JOIN discount f ON e.discount_id=f.id AND f.state=0
WHERE b.type=0 {0}
GROUP BY a.id
 ";

        private static readonly string getCourseInfoSql = @"
SELECT a.id courseid,a.name,MIN(d.price)minPrice,MAX(d.price)maxPrice,IFNULL(f.money,0) discount_money,GROUP_CONCAT(g.headimg_url)heading_urls,h.detail FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN product_specification_detail c ON a.id=c.product_id
LEFT JOIN product_specification_detail_price d ON c.product_id=d.product_id AND c.plan_code=d.plan_code
LEFT JOIN product_discount e ON a.id=e.product_id AND e.state=0
LEFT JOIN discount f ON e.discount_id=f.id AND f.state=0
LEFT JOIN product_headimg g ON a.id=g.product_id
LEFT JOIN product_detail h ON a.id=h.product_id
WHERE a.id='{0}'
GROUP BY a.id
 ";

        /// <summary>
        /// 获取规格信息
        /// </summary>
        private static readonly string getSpecificationInfoSql = @"
SELECT q.product_id courseid,q.plan_code,s.id specificationid,s.name,r.id specificationdetailid,r.name FROM product p
LEFT JOIN product_specification_detail q ON p.id=q.product_id
LEFT JOIN specification_detail r ON q.specification_detail_id=r.id
LEFT JOIN specification s ON r.specification_id=s.id
";



        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <param name="name">课程名字</param>
        /// <param name="categoryId">分类id</param>
        /// <returns></returns>
        public static List<CourseListJson> GetCourseList(string name, int categoryId)
        {
            try
            {
                string where = string.Empty;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    name = name.Trim();
                    where += " and a.name like @name";
                }
                if (categoryId != 0)
                {
                    where += " and b.id = @categoryId";
                }

                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@name", $"%{name}%"),
                        new MySqlParameter("@categoryId", categoryId),
                    };

                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCourseInfoListSql, where), commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CourseListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCourseList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CourseListJson>();
        }

        /// <summary>
        /// 获取单个课程的信息
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static CourseInfoJson GetCourseInfo(int courseId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCourseInfoSql, courseId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CourseInfoJson>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCourseInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }

    }
}
