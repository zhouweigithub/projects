using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class SpecificationDAL
    {

        /// <summary>
        /// 获取课程的最大上课人数
        /// </summary>
        private static readonly string getMaxPersonCountOfCourse = @"
SELECT b.value FROM product_specification_detail a
INNER JOIN specification_detail b ON a.specification_detail_id=b.id
INNER JOIN specification c ON b.specification_id=c.id
WHERE c.name='人数' AND a.plan_code='{0}'";



        /// <summary>
        /// 获取课程的最大上课人数
        /// </summary>
        /// <param name="planCode">规格分组代码</param>
        /// <returns></returns>
        public static int GetMaxPersonCountOfCourse(string planCode)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteScalarInt(string.Format(getMaxPersonCountOfCourse, planCode));
                    return count;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("GetMaxPersonCountOfCourse 出错：" + ex, Util.Log.LogType.Error);
            }

            return 0;
        }

    }
}
