using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.BLL.API
{

    public class CourseBLL
    {
        /// <summary>
        /// 获取课程详情
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public static CourseInfoJson GetCourseInfo(int courseId)
        {
            //课程基本信息
            CourseInfoJson courseInfo = CourseDAL.GetCourseInfo(courseId);
            //课程评论信息
            List<ClassCommentJson> comments = ClassDAL.GetClassCommentList(0, courseId);

            courseInfo.CommentsList = comments;
            return courseInfo;
        }
    }
}
