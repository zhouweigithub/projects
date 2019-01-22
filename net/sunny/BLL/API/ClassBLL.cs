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

        /// <summary>
        /// 教练结束上课，更新上课的状态和上课的学生的状态
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static bool CoachCompleteClass(int classId)
        {
            Dictionary<string, object> fieldValueDic = new Dictionary<string, object>();
            fieldValueDic.Add("state", 1);
            //更新课程状态
            int count = DBData.GetInstance(DBTable.class_).UpdateByKey(fieldValueDic, classId);
            //更新学生上课状态
            int count2 = DBData.GetInstance(DBTable.class_student).Update(fieldValueDic, $"class_id='{classId}'");
            return count > 0;
        }

        /// <summary>
        /// 教练添加课后的评论
        /// </summary>
        /// <param name="classId">上课的id</param>
        /// <param name="commentString">评论文本</param>
        /// <param name="images">上课图片集</param>
        /// <param name="videos">上课视频集</param>
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
