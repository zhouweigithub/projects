//***********************************************************************************
//文件名称：LoginUserBLL.cs
//功能描述：用户信息操作类
//数据表： promotioncenter.loginuser
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Moqikaka.Tmp.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Moqikaka.Tmp.DAL
{
    /// <summary>
    /// 用户信息BLL
    /// </summary>
    public class LoginUserDAL
    {

        /// <summary>
        /// 根据用户名和密码获取用户实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static MUser GetUser(string userName, string password)
        {
            MySqlParameter[] commandParameters = new MySqlParameter[] {
                new MySqlParameter("@username" ,userName),
                new MySqlParameter("@password", Util.Security.MD5Util.MD5(password).ToLower())
            };
            string where = "username=@username and password =@password";
            return DBData.GetInstance(DBTable.m_user).GetEntity<MUser>(where, commandParameters);
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public static ListLoginUserModel GetListUserInfo(string username)
        {
            var resultData = new ListLoginUserModel();

            string where = string.IsNullOrWhiteSpace(username) ? "1=1" : string.Format("username like '%{0}%' or name like '%{0}%'", username);
            var resultlst = DBData.GetInstance(DBTable.m_user).GetList<LoginUserModel>(where, null);
            if (resultlst != null)
            {
                resultData.LoginUserInfoList = resultlst.ToList();

                //获取角色信息列表
                //RoleInfoListModel LstRoleInfo = RoleDAL.GetListRoles();
                //foreach (var item in resultData.LoginUserInfoList)
                //{
                //    string[] lstRoleids = item.roleids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //    string strRoleNames = "";
                //    foreach (var kk in lstRoleids)
                //    {
                //        var roleInfo = LstRoleInfo.RoleList.FirstOrDefault(m => m.Id == Convert.ToInt32(kk));
                //        if (roleInfo != null)
                //        {
                //            strRoleNames += roleInfo.RolesName + ",";
                //        }
                //    }
                //    item.rolenames = strRoleNames.TrimEnd(',');

                //    //游戏权限
                //    string appids = string.Empty;
                //    if (!string.IsNullOrEmpty(item.appids))
                //    {
                //        appids = "'" + item.appids.Replace(",", "','") + "'";
                //    }
                //}
            }
            return resultData;
        }

        /// <summary>
        /// 根据用户名判断是否存在用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true:不存在；false：存在</returns>
        public static bool IsExsitUserInfo(MUser model)
        {
            try
            {
                MySqlParameter[] commandParameters = new MySqlParameter[] {
                    new MySqlParameter("@username",model.UserName)
                };
                int count = DBData.GetInstance(DBTable.m_user).GetCount("username=@username", commandParameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("IsExsitUserInfo出错：" + ex.ToString(), Util.Log.LogType.Error);
            }
            return false;

        }


        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool IsPasswordValid(string username, string password)
        {
            try
            {
                MySqlParameter[] commandParameters = new MySqlParameter[] {
                    new MySqlParameter("@username",username),
                    new MySqlParameter("@password",password)
                };
                int count = DBData.GetInstance(DBTable.m_user).GetCount("username=@username and password=@password", commandParameters);
                return count > 0;
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("IsPasswordValid出错：" + ex.ToString(), Util.Log.LogType.Error);
            }
            return false;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool UpdatePassword(string username, string password)
        {
            bool flag = true;
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@username",username),
                        new MySqlParameter("@password",password)
                    };
                    int count = dbhelper.ExecuteNonQueryParams("UPDATE loginuser SET password=@password WHERE username=@username", commandParameters);
                    flag = count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("UpdatePassword出错：" + ex.ToString(), Util.Log.LogType.Error);
            }
            return flag;
        }
    }
}
