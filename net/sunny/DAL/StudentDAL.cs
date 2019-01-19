using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Sunny.Model;

namespace Sunny.DAL
{
    public class StudentDAL
    {
        private static readonly string insertStudentSql = @"INSERT INTO student (username,PASSWORD,NAME,sex,phone,birthday,headimg,cash,state) 
VALUES(@username,'',@name,@sex,@phone,@birthday,@headimg,0,0) ;";
        private static readonly string InsertIvitationSql = "INSERT INTO invitation (student_id,from_student_id,state) VALUES(@student_id,@from_student_id,0) ;";
        private static readonly string getStudentIdByPhoneSql = "SELECT id FROM student WHERE phone='{0}'";

        private static readonly string addCashSql = "UPDATE student SET cash=cash+{0} WHERE id={1}";

        /// <summary>
        /// 添加学员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool AddStudent(StudentRequest model)
        {
            try
            {
                MySqlParameter[] paras = new MySqlParameter[]{
                    new MySqlParameter("@username",model.username),
                    new MySqlParameter("@name",model.name),
                    new MySqlParameter("@sex",model.sex),
                    new MySqlParameter("@phone",model.phone),
                    new MySqlParameter("@birthday",model.birthday),
                    new MySqlParameter("@headimg",model.headimg),
                };
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(insertStudentSql, paras);
                    if (count > 0)
                    {
                        int newStudentId = dbhelper.ExecuteScalarInt(Common.Const.SELECT_LAST_INSERT_ID_SQL);
                        int invitationStudentId = dbhelper.ExecuteScalarInt(string.Format(getStudentIdByPhoneSql, model.Invitationcode));
                        paras = new MySqlParameter[]{
                            new MySqlParameter("@student_id",newStudentId),
                            new MySqlParameter("@from_student_id",invitationStudentId),
                        };
                        int count2 = dbhelper.ExecuteNonQueryParams(InsertIvitationSql, paras);
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddStudent 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 添加余额
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <param name="cash">添加的金额，可为负</param>
        /// <returns></returns>
        public static bool AddCash(int studentId, decimal cash)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(string.Format(addCashSql, cash, studentId));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("UpdateCash 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
