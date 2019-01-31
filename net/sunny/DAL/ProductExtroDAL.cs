using Sunny.Model;
using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class ProductExtroDAL
    {
        private static readonly string getVenueSql = @"
SELECT b.name venue_name,a.venue_id,a.price,b.campus_id,c.name campus_name FROM course_price a
INNER JOIN venue b ON a.venue_id=b.id
INNER JOIN campus c ON b.campus_id=c.id
WHERE a.product_id={0} AND b.state=0 AND c.state=0 
";

        private static readonly string getCourseTypeSql = @"
SELECT DISTINCT b.id,b.name FROM course_price a
INNER JOIN course_type b ON a.type_id=b.id
WHERE a.product_id={0} AND b.state=0
";

        /// <summary>
        /// 取课程的人数类型列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<CourseType> GetCourseTypeList(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getCourseTypeSql, productId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CourseType>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetCourseTypeList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CourseType>();
        }

        /// <summary>
        /// 取课程的场馆列表
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<CustVenue> GetVenueList(int productId)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    DataTable dt = dbhelper.ExecuteDataTable(string.Format(getVenueSql, productId));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.ToList<CustVenue>();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetVenueList 出错：" + ex, Util.Log.LogType.Error);
            }

            return new List<CustVenue>();
        }

    }
}
