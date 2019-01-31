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
        /// 获取课程列表
        /// </summary>
        private static readonly string getCourseListSql = @"
SELECT a.id product_id,a.name,a.main_img,b.id category_id,b.name category_name,(IFNULL(c.min_price,0)-IFNULL(d.dismoney,0))min_price,(IFNULL(c.max_price,0)-IFNULL(d.dismoney,0))max_price,IFNULL(d.dismoney,0)dismoney FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN (
	SELECT product_id,MAX(price)max_price,MIN(price)min_price FROM course_price GROUP BY product_id
)c ON a.id=c.product_id
LEFT JOIN (
	SELECT product_id,SUM(n.money) dismoney FROM product_discount m 
    INNER JOIN discount n ON m.discount_id=n.id 
    WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() 
    GROUP BY m.product_id
)d ON a.id=d.product_id
WHERE a.state=0 AND b.state=0 AND b.type=0 {0}
 ";

        /// <summary>
        /// 随机获取课程信息
        /// </summary>
        private static readonly string getCourseListRandomSql = getCourseListSql + " ORDER BY RAND() LIMIT {1}";

        /// <summary>
        /// 获取单个课程信息
        /// </summary>
        private static readonly string getCourseDetailSql = @"
SELECT a.id product_id,a.name,a.main_img,b.id category_id,b.name category_name,(IFNULL(c.min_price,0)-IFNULL(d.dismoney,0))min_price,(IFNULL(c.max_price,0)-IFNULL(d.dismoney,0))max_price,IFNULL(d.dismoney,0)dismoney,GROUP_CONCAT(e.heading_urls)heading_urls,f.detail FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN (
	SELECT product_id,MAX(price)max_price,MIN(price)min_price FROM course_price GROUP BY product_id
)c ON a.id=c.product_id
LEFT JOIN (
	SELECT product_id,SUM(n.money) dismoney FROM product_discount m 
    INNER JOIN discount n ON m.discount_id=n.id 
    WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() 
    GROUP BY m.product_id
)d ON a.id=d.product_id
LEFT JOIN (
	SELECT product_id,GROUP_CONCAT(headimg_url)heading_urls FROM product_headimg GROUP BY product_id
)e ON a.id=e.product_id
LEFT JOIN product_detail f ON a.id=f.product_id
WHERE a.state=0 AND b.state=0 AND b.type=0 AND a.id='{0}'
 ";

        /// <summary>
        /// 获取课程的价格和折扣信息
        /// </summary>
        private static readonly string getCoursePriceSql = @"
SELECT a.id product_id,a.name,(IFNULL(b.price,0)-IFNULL(c.dismoney,0))price,IFNULL(c.dismoney,0)discount_money,c.discount_id,discount_name FROM product a
LEFT JOIN (
	SELECT product_id,price FROM course_price WHERE product_id={0} AND venue_id={1} AND type_id={2}
)b ON a.id=b.product_id
LEFT JOIN (
	SELECT m.product_id,n.money dismoney,n.id discount_id,n.name discount_name FROM product_discount m 
	INNER JOIN discount n ON m.discount_id=n.id 
	WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() AND m.product_id={0}
)c ON a.id=c.product_id
WHERE a.id={0}
";

        /// <summary>
        /// 获取热门课程
        /// </summary>
        private static readonly string getHotCourseListSql = @"
SELECT a.product_id,b.name,b.main_img,(IFNULL(d.min_price,0)-IFNULL(e.dismoney,0))min_price,(IFNULL(d.max_price,0)-IFNULL(e.dismoney,0))max_price,IFNULL(e.dismoney,0)dismoney FROM hot_course a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN category c ON b.category_id=c.id
LEFT JOIN (
	SELECT product_id,MAX(price)max_price,MIN(price)min_price FROM course_price GROUP BY product_id
)d ON a.product_id=d.product_id
LEFT JOIN (
	SELECT product_id,SUM(n.money) dismoney FROM product_discount m 
	INNER JOIN discount n ON m.discount_id=n.id 
	WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() 
	GROUP BY m.product_id
)e ON a.product_id=e.product_id
WHERE a.type={0} AND b.state=0 AND c.type=0
LIMIT {1}
";


        /// <summary>
        /// 获取课程信息
        /// </summary>
        /// <param name="name">课程名字</param>
        /// <param name="categoryId">分类id</param>
        /// <returns></returns>
        public static List<ProductListJson> GetCourseList(string name, int categoryId, int page = 0, int pageSize = 10)
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

                    DataTable dt = dbhelper.ExecuteDataTablePageParams(string.Format(getCourseListSql, where), pageSize, page, commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCourseList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }


        /// <summary>
        /// 随机获取商品
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static List<ProductListJson> GetRandomCourseList(int count)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCourseListRandomSql, string.Empty, count));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetRandomCourseList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }

        /// <summary>
        /// 获取热闹课程
        /// </summary>
        /// <param name="type">类型0热门课程1精品课程</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static List<ProductListJson> GetHotCourseList(short type, int count)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getHotCourseListSql, type, count));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetHotCourseList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }


        /// <summary>
        /// 获取单个课程的信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static CourseInfoJson GetCourseInfo(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCourseDetailSql, productId));

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

        /// <summary>
        /// 获取课程的价格和折扣信息
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <param name="venudId">场馆id</param>
        /// <param name="typeId">人数id</param>
        /// <returns></returns>
        public static CustProductPriceInfoJson GetCoursePriceInfo(int productId, int venudId, int typeId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCoursePriceSql, productId, venudId, typeId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustProductPriceInfoJson>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCoursePriceInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }

    }
}
