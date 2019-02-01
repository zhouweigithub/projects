using Sunny.DAL;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.BLL.API
{
    public class GeneralBLL
    {

        /// <summary>
        /// 根据学生用户名获取学生信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static Student GetStudentByUserName(string userName)
        {
            try
            {
                return DBData.GetInstance(DBTable.student).GetEntity<Student>($"username='{userName}'");
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"GetStudentByUserName 根据学生用户名获取学生信息失败：userName:{userName} \r\n {e}", Util.Log.LogType.Error);
                return null;
            }
        }

        /// <summary>
        /// 根据教练用户名获取教练信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static Coach GetCoachByUserName(string userName)
        {
            try
            {
                return DBData.GetInstance(DBTable.coach).GetEntity<Coach>($"username='{userName}'");
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"GetCoachByUserName 根据教练用户名获取教练信息失败：userName:{userName} \r\n {e}", Util.Log.LogType.Error);
                return null;
            }
        }
    }
}
