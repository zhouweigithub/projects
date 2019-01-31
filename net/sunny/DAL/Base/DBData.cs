using Sunny.Common;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    /// <summary>
    /// 所有表实例的集合
    /// </summary>
    public class DBData
    {

        /// <summary>
        /// 所有表实例的集合
        /// </summary>
        private static Dictionary<string, BaseQuery> InstanceList = new Dictionary<string, BaseQuery>();


        private DBData()
        {
        }

        static DBData()
        {
            CreateInstanceList();
        }

        /// <summary>
        /// 创建所有表的实例
        /// </summary>
        private static void CreateInstanceList()
        {
            InstanceList.Add("class", new BaseQuery("class", "id", "id", false));
            InstanceList.Add("class_comment", new BaseQuery("class_comment", "class_id", "class_id", false));
            InstanceList.Add("class_student", new BaseQuery("class_student", "", "", false));
            InstanceList.Add("coach", new BaseQuery("coach", "id", "id", false));
            InstanceList.Add("coach_caption", new BaseQuery("coach_caption", "coach_id", "coach_id", false));
            InstanceList.Add("coachcaption_venue", new BaseQuery("coachcaption_venue", "coach_id", "coach_id", false));
            InstanceList.Add("coupon", new BaseQuery("coupon", "id", "id", false));
            InstanceList.Add("course", new BaseQuery("course", "id", "id", false));
            InstanceList.Add("settlement", new BaseQuery("settlement", "class_id", "class_id", false));
            InstanceList.Add("student", new BaseQuery("student", "id", "id", false));
            InstanceList.Add("student_coupon", new BaseQuery("student_coupon", "id", "id", false));
            InstanceList.Add("venue", new BaseQuery("venue", "id", "id", false));

            InstanceList.Add("campus", new BaseQuery("campus", "id", "id", false));
            InstanceList.Add("category", new BaseQuery("category", "id", "id", false));
            InstanceList.Add("class_comment_url", new BaseQuery("class_comment_url", "", "", false));
            InstanceList.Add("deliver", new BaseQuery("deliver", "id", "id", false));
            InstanceList.Add("discount", new BaseQuery("discount", "id", "id", false));
            InstanceList.Add("order", new BaseQuery("order", "order_id", "order_id", false));
            InstanceList.Add("order_coupon", new BaseQuery("order_coupon", "", "", false));
            InstanceList.Add("order_product", new BaseQuery("order_product", "", "", false));
            InstanceList.Add("order_product_specification_detail", new BaseQuery("order_product_specification_detail", "", "", false));
            InstanceList.Add("product", new BaseQuery("product", "id", "id", false));
            InstanceList.Add("product_detail", new BaseQuery("product_detail", "product_id", "product_id", false));
            InstanceList.Add("product_discount", new BaseQuery("product_discount", "", "", false));
            InstanceList.Add("product_headimg", new BaseQuery("product_headimg", "id", "id", false));
            InstanceList.Add("product_specification_detail", new BaseQuery("product_specification_detail", "", "", false));
            InstanceList.Add("product_specification_detail_price", new BaseQuery("product_specification_detail_price", "", "", false));
            InstanceList.Add("receiver_info", new BaseQuery("receiver_info", "id", "id desc", false));
            InstanceList.Add("specification", new BaseQuery("specification", "id", "id", false));
            InstanceList.Add("specification_detail", new BaseQuery("specification_detail", "id", "id", false));

            InstanceList.Add("hours", new BaseQuery("hours", "product_id", "product_id", false));
            InstanceList.Add("banner", new BaseQuery("banner", "id", "id", false));
            InstanceList.Add("booking_coach_queue", new BaseQuery("booking_coach_queue", "", "", false));
            InstanceList.Add("course_type", new BaseQuery("course_type", "id", "id", false));
            InstanceList.Add("booking_student", new BaseQuery("booking_student", "id", "id", false));
            InstanceList.Add("coupon_history", new BaseQuery("coupon_history", "id", "id", false));
            InstanceList.Add("cashback_history", new BaseQuery("cashback_history", "id", "id", false));
            InstanceList.Add("pay_record", new BaseQuery("pay_record", "id", "id", false));
            InstanceList.Add("site_info", new BaseQuery("site_info", "pkey", "pkey", false));
            InstanceList.Add("hot_course", new BaseQuery("hot_course", "product_id", "product_id", false));
            InstanceList.Add("coach_img", new BaseQuery("coach_img", "id", "id", false));
            InstanceList.Add("course_price", new BaseQuery("course_price", "", "", false));
            InstanceList.Add("invitation", new BaseQuery("invitation", "student_id", "student_id", false));
        }

        /// <summary>
        /// 获取目标表的实例
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static BaseQuery GetInstance(string tableName)
        {
            if (InstanceList.ContainsKey(tableName))
            {
                return InstanceList[tableName];
            }
            else
            {
                Util.Log.LogUtil.Write($"{tableName}表未创建数据库实例", Util.Log.LogType.Error);
                return null;
            }
        }

    }
}
