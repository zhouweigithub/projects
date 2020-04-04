using MySql.Data.MySqlClient;
using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.DAL
{
    public class CoachDAL
    {
        private static readonly string addCashSql = "UPDATE coach SET cash=cash+{0} WHERE id={1}";

        private static readonly string isCaptionPhoneExistSql = "SELECT COUNT(1) FROM coachcaption_venue a INNER JOIN coach b ON a.coach_id=b.id WHERE b.phone= '{0}' AND b.state= 0";

        private static readonly string addCoachSql = @"INSERT INTO coach (username,PASSWORD,NAME,sex,phone,TYPE,LEVEL,headimg,cash,state)
VALUES(@username,'',@name,@sex,@phone,@type,0,@headimg,0,0);";
        private static readonly string addCoachCaptionSql = "INSERT INTO coach_caption (coach_id, caption_id) VALUES('{0}','{1}');";



        /// <summary>
        /// 添加余额
        /// </summary>
        /// <param name="coachId">教练id</param>
        /// <param name="cash">添加的金额，可为负</param>
        /// <returns></returns>
        public static bool AddCash(int coachId, decimal cash)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(string.Format(addCashSql, cash, coachId));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddCash 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 检测教练队长是否存在
        /// </summary>
        /// <param name="phone">教练队长的手机号</param>
        /// <returns></returns>
        public static bool IsCaptionPhoneExist(string phone)
        {
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteScalarInt(string.Format(isCaptionPhoneExistSql, phone));
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("IsCaptionPhoneExist 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

        /// <summary>
        /// 添加教练
        /// </summary>
        /// <param name="model">教练信息</param>
        /// <param name="caption">教练队长信息</param>
        /// <returns></returns>
        public static bool AddCoach(CoachRequest model, Coach caption)
        {
            if (model == null || caption == null)
                return false;

            try
            {
                MySqlParameter[] paras = new MySqlParameter[]{
                    new MySqlParameter("@username",model.username),
                    new MySqlParameter("@name",model.name),
                    new MySqlParameter("@sex",model.sex),
                    new MySqlParameter("@phone",model.phone),
                    new MySqlParameter("@type",caption.type),
                    new MySqlParameter("@headimg",model.headimg),
                };
                using (DBHelper dbhelper = new DBHelper())
                {
                    int count = dbhelper.ExecuteNonQueryParams(addCoachSql, paras);
                    if (count > 0)
                    {   //插入教练和教练队长关系的数据
                        int newId = dbhelper.ExecuteScalarInt(Common.Const.SELECT_LAST_INSERT_ID_SQL);
                        int count2 = dbhelper.ExecuteNonQuery(string.Format(addCoachCaptionSql, newId, caption.id));
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("AddCoach 出错：" + ex, Util.Log.LogType.Error);
            }

            return false;
        }

    }
}
