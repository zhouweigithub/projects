using Sunny.Common;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
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
            InstanceList.Add("appointment", new BaseQuery("appointment", "id", "id", false));
            InstanceList.Add("class", new BaseQuery("class", "id", "id", false));
            InstanceList.Add("class_comment", new BaseQuery("class_comment", "class_id", "class_id", false));
            InstanceList.Add("class_student", new BaseQuery("class_student", "", "", false));
            InstanceList.Add("coach", new BaseQuery("coach", "id", "id", false));
            InstanceList.Add("coach_caption", new BaseQuery("coach_caption", "coach_id", "coach_id", false));
            InstanceList.Add("coachcaption_venue", new BaseQuery("coachcaption_venue", "coach_id", "coach_id", false));
            InstanceList.Add("coupon", new BaseQuery("coupon", "id", "id", false));
            InstanceList.Add("course", new BaseQuery("course", "id", "id", false));
            InstanceList.Add("pay_detail", new BaseQuery("pay_detail", "id", "id", false));
            InstanceList.Add("pay_detail_coupon", new BaseQuery("pay_detail_coupon", "", "", false));
            InstanceList.Add("settlement", new BaseQuery("settlement", "class_id", "class_id", false));
            InstanceList.Add("student", new BaseQuery("student", "id", "id", false));
            InstanceList.Add("student_coupon", new BaseQuery("student_coupon", "id", "id", false));
            InstanceList.Add("student_course", new BaseQuery("student_course", "id", "id", false));
            InstanceList.Add("venue", new BaseQuery("venue", "id", "id", false));
            InstanceList.Add("withdrawal", new BaseQuery("withdrawal", "id", "id", false));
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
                return null;
            }
        }

    }
}
