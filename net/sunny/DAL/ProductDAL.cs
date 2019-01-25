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
    /// 一般商品（非商品）操作类
    /// </summary>
    public class ProductDAL
    {

        /// <summary>
        /// 获取商品信息
        /// </summary>
        private static readonly string getProductListSql = @"
SELECT a.id product_id,a.name,a.main_img,(IFNULL(c.min_price,0)-IFNULL(d.dismoney,0))min_price,(IFNULL(c.max_price,0)-IFNULL(d.dismoney,0))max_price,IFNULL(d.dismoney,0)dismoney FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN(
	SELECT m.product_id,IFNULL(MIN(n.price),0)min_price,IFNULL(MAX(n.price),0)max_price FROM product_specification_detail m
	LEFT JOIN product_specification_detail_price n ON m.product_id=n.product_id AND m.plan_code=n.plan_code
	GROUP BY m.product_id
)c ON a.id=c.product_id
LEFT JOIN (
	SELECT product_id,SUM(n.money) dismoney FROM product_discount m 
	INNER JOIN discount n ON m.discount_id=n.id 
	WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() 
	GROUP BY m.product_id
)d ON a.id=d.product_id
WHERE b.type=1 {0}
 ";

        /// <summary>
        /// 随机获取商品信息
        /// </summary>
        private static readonly string getProductListRandomSql = getProductListSql + " ORDER BY RAND() LIMIT {1}";

        /// <summary>
        /// 单个商品信息
        /// </summary>
        private static readonly string getProductDetailSql = @"
SELECT a.id product_id,a.name,a.main_img,(IFNULL(c.min_price,0)-IFNULL(d.dismoney,0))min_price,(IFNULL(c.max_price,0)-IFNULL(d.dismoney,0))max_price,IFNULL(d.dismoney,0)dismoney,e.heading_urls,f.detail FROM product a
INNER JOIN category b ON a.category_id=b.id
LEFT JOIN(
	SELECT m.product_id,IFNULL(MIN(n.price),0)min_price,IFNULL(MAX(n.price),0)max_price FROM product_specification_detail m
	LEFT JOIN product_specification_detail_price n ON m.product_id=n.product_id AND m.plan_code=n.plan_code
	GROUP BY m.product_id
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
WHERE b.type=1 AND a.id={0}
 ";

        /// <summary>
        /// 获取商品的价格和折扣信息
        /// </summary>
        private static readonly string getProductPriceSql = @"
SELECT a.id product_id,a.name,(IFNULL(b.price,0)-IFNULL(c.dismoney,0))price,IFNULL(c.dismoney,0)discount_money,c.discount_id,discount_name FROM product a
LEFT JOIN (
	SELECT n.product_id,n.price FROM product_specification_detail_price n
	WHERE n.product_id={0} AND n.plan_code='{1}'
)b ON a.id=b.product_id
LEFT JOIN (
	SELECT m.product_id,n.money dismoney,n.id discount_id,n.name discount_name FROM product_discount m 
	INNER JOIN discount n ON m.discount_id=n.id 
	WHERE m.state=0 AND n.state=0 AND m.start_time<NOW() AND m.end_time>NOW() AND m.product_id={0}
)c ON a.id=c.product_id
WHERE a.id={0}
";

        /// <summary>
        /// 获取热闹商品
        /// </summary>
        private static readonly string getHotProductListSql = @"
SELECT a.product_id,b.name,b.main_img,(IFNULL(d.min_price,0)-IFNULL(e.dismoney,0))min_price,(IFNULL(d.max_price,0)-IFNULL(e.dismoney,0))max_price,IFNULL(e.dismoney,0)dismoney FROM hot_course a
INNER JOIN product b ON a.product_id=b.id
INNER JOIN category c ON b.category_id=c.id
LEFT JOIN (
	SELECT m.product_id,IFNULL(MIN(n.price),0)min_price,IFNULL(MAX(n.price),0)max_price FROM product_specification_detail m
	LEFT JOIN product_specification_detail_price n ON m.product_id=n.product_id AND m.plan_code=n.plan_code
	GROUP BY m.product_id
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
        /// 获取商品信息
        /// </summary>
        /// <param name="name">商品名字</param>
        /// <param name="categoryId">分类id</param>
        /// <returns></returns>
        public static List<ProductListJson> GetProductList(string name, int categoryId, int page, int pageSize)
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

                    DataTable dt = dbhelper.ExecuteDataTablePageParams(string.Format(getProductListSql, where), pageSize, page, commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetProductList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }

        /// <summary>
        /// 随机获取首页上的商品
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<ProductListJson> GetRandomProductList(int count)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getProductListRandomSql, string.Empty, count));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetRandomProductList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }

        /// <summary>
        /// 获取热闹商品
        /// </summary>
        /// <param name="type">类型0热门商品1精品商品</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public static List<ProductListJson> GetHotProductList(short type, int count)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getHotProductListSql, type, count));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductListJson>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetHotProductList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<ProductListJson>();
        }


        /// <summary>
        /// 获取单个商品的信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static ProductInfoJson GetProductInfo(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getProductDetailSql, productId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<ProductInfoJson>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetProductInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }

        /// <summary>
        /// 获取商品的价格和折扣信息
        /// </summary>
        /// <param name="productId">商品id</param>
        /// <param name="placnCode">规格编码</param>
        /// <returns></returns>
        public static CustProductPriceInfoJson GetProductPriceInfo(int productId, string placnCode)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getProductPriceSql, productId, placnCode));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustProductPriceInfoJson>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetProductPriceInfo 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }

    }
}
