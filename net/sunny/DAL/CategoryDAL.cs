using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    /// <summary>
    /// 商品分类操作类
    /// </summary>
    public class CategoryDAL
    {

        private static readonly string getCategoryByProductIdSql = "SELECT b.* FROM product a INNER JOIN category b ON a.category_id=b.id WHERE a.id='{0}'";


        public static Category GetCategoryByProductId(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTableParams(string.Format(getCategoryByProductIdSql, productId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<Category>().First();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCategoryByProductId 出错：" + ex, Util.Log.LogType.Error);
            }

            return null;
        }
    }
}
