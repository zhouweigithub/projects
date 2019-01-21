using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.BLL.API
{
    public class ClassBLL
    {
        //下单前应该检查是否已经购买过本课程，同一微信号不能重复购买

        //public static bool Booking(int studentid, int productId, DateTime start_time)
        //{
        //    Course course = DBData.GetInstance(DBTable.course).GetEntity<Course>($"student_id='{studentid}' and product_id='{productId}'");
        //    Class data = new Class()
        //    {
        //        product_id = productId,
        //        coach_id = 0,
        //        venue_id = course.venue_id,
        //        hour = course.over_hour + 1,
        //        max_count = course.max_count,
        //        start_time = start_time,
        //        end_time = start_time.AddHours(1),
        //    };
        //    return ClassDAL.InsertClassData(data: data);
        //}

        public static bool CoachCompleteClass(int classId)
        {
            Dictionary<string, object> fieldValueDic = new Dictionary<string, object>();
            fieldValueDic.Add("state", 1);
            int count = DBData.GetInstance(DBTable.class_).UpdateByKey(fieldValueDic, classId);
            return count > 0;
        }

        /// <summary>
        /// 教练添加课后的评论
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="commentString"></param>
        /// <param name="images"></param>
        /// <param name="videos"></param>
        /// <returns></returns>
        public static bool CoachAddClassComment(int classId, string commentString, List<string> images, List<string> videos)
        {
            int count1 = DBData.GetInstance(DBTable.class_comment).Add(new ClassComment()
            {
                class_id = classId,
                comment = commentString,
            });

            foreach (string item in images)
            {
                DBData.GetInstance(DBTable.class_comment_url).Add(new ClassCommentUrl()
                {
                    class_id = classId,
                    url = item,
                    type = 0,
                });
            }
            foreach (string item in videos)
            {
                DBData.GetInstance(DBTable.class_comment_url).Add(new ClassCommentUrl()
                {
                    class_id = classId,
                    url = item,
                    type = 1,
                });
            }

            return count1 > 0;
        }

    }
}
