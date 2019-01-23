using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;

namespace Sunny.BLL.API
{
    /// <summary>
    /// 预约
    /// </summary>
    public class AppointmentBLL
    {
        private static readonly object lockObject = new object();

        /// <summary>
        /// 用户发出预约请求
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool AddAppointment(int courseId, DateTime startTime, DateTime endTime)
        {
            try
            {
                DBData.GetInstance(DBTable.booking_student).Add(new BookingStudent()
                {
                    course_id = courseId,
                    start_time = startTime,
                    end_time = endTime,
                    state = 0,
                });

                Course course = DBData.GetInstance(DBTable.course).GetEntityByKey<Course>(courseId);
                int preCoachId = ClassDAL.GetPreCoachOfCourse(course.product_id);   //上次上课的教练id

                if (preCoachId != 0)
                {
                    DBData.GetInstance(DBTable.booking_coach_queue).Add(new BookingCoachQueue()
                    {
                        coach_id = preCoachId,
                        course_id = courseId,
                        end_time = DateTime.Now.AddHours(1),
                    });
                }
                return true;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("AddAppointment 添加用户的预约请求时出错：" + e, Util.Log.LogType.Error);
            }
            return false;
        }



        /// <summary>
        /// 教练接收预约
        /// </summary>
        /// <param name="bookingId">预约id</param>
        /// <param name="coachId">教练id</param>
        /// <param name="startTime">教练设定的上课开始时间</param>
        /// <param name="endTime">教练设定的上课结束时间</param>
        /// <param name="msg">返回的错误信息</param>
        /// <returns></returns>
        public static bool ReceiveAppointment(int bookingId, int coachId, DateTime startTime, DateTime endTime, out string msg)
        {
            lock (lockObject)
            {
                try
                {
                    BookingStudent bookingStudent = DBData.GetInstance(DBTable.booking_student).GetEntityByKey<BookingStudent>(bookingId);
                    if (bookingStudent.state == 1)
                    {   //当预约请求状态为1，表示已被抢单
                        msg = "已有人接单";
                        return false;
                    }

                    Course course = DBData.GetInstance(DBTable.course).GetEntityByKey<Course>(bookingStudent.course_id);

                    bool result = ClassDAL.InsertClassData(course.student_id, new Class()
                    {
                        coach_id = coachId,
                        hour = course.over_hour + 1,
                        max_count = course.max_count,
                        product_id = course.product_id,
                        venue_id = course.venue_id,
                        start_time = startTime,
                        end_time = endTime,
                        state = 0,
                        rate = 0,
                    });

                    //更新该预约请求的状态
                    Dictionary<string, object> fieldValues = new Dictionary<string, object>();
                    fieldValues.Add("state", 1);
                    DBData.GetInstance(DBTable.booking_student).UpdateByKey(fieldValues, bookingStudent.Id);

                    msg = "success";

                    return result;
                }
                catch (Exception e)
                {
                    msg = "服务内部错误";
                    Util.Log.LogUtil.Write($"教练接收预约请求失败：bookingId {bookingId} coachId {coachId} startTime {startTime} endTime {endTime} \r\n {e} ", Util.Log.LogType.Error);
                    return false;
                }
            }

        }
    }
}
