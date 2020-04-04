//***********************************************************************************
//文件名称：LoginUserBLL.cs
//功能描述：用户信息操作类
//数据表： promotioncenter.loginuser
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Spetmall.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Spetmall.DAL
{
    /// <summary>
    /// 用户信息BLL
    /// </summary>
    public class LoginUserDAL : DAL.BaseQuery
    {
        private static readonly LoginUserDAL Instance = new LoginUserDAL();

        private LoginUserDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "loginuser";
            this.ItemName = "用户信息表";
            this.OrderbyFields = "ID ASC";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static LoginUserDAL GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 根据用户名和密码获取用户实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public Model.LoginUser GetUser(string userName, string password)
        {
            MySqlParameter[] commandParameters = new MySqlParameter[] {
                new MySqlParameter("@username" ,userName),
                new MySqlParameter("@password", Util.Security.MD5Util.MD5(password).ToLower())
            };
            return base.GetEntity<Model.LoginUser>("username=@username and password =@password ", commandParameters);
        }


        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public ListLoginUserModel GetListUserInfo(string username)
        {
            var resultData = new ListLoginUserModel();

            string where = string.IsNullOrWhiteSpace(username) ? "1=1" : string.Format("username like '%{0}%' or name like '%{0}%'", username);
            var resultlst = base.GetList<LoginUserModel>(where, null);
            if (resultlst != null)
            {
                resultData.LoginUserInfoList = resultlst.ToList();

                //获取角色信息列表
                RoleInfoListModel LstRoleInfo = RoleDAL.GetInstance().GetListRoles();
                foreach (var item in resultData.LoginUserInfoList)
                {
                    string[] lstRoleids = item.roleids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    string strRoleNames = "";
                    foreach (var kk in lstRoleids)
                    {
                        var roleInfo = LstRoleInfo.RoleList.FirstOrDefault(m => m.Id == Convert.ToInt32(kk));
                        if (roleInfo != null)
                        {
                            strRoleNames += roleInfo.RolesName + ",";
                        }
                    }
                    item.rolenames = strRoleNames.TrimEnd(',');

                    //游戏权限
                    string appids = string.Empty;
                    if (!string.IsNullOrEmpty(item.appids))
                    {
                        appids = "'" + item.appids.Replace(",", "','") + "'";
                    }
                }
            }
            return resultData;
        }

        /// <summary>
        /// 根据用户名判断是否存在用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true:不存在；false：存在</returns>
        public bool IsExsitUserInfo(LoginUserModel model)
        {
            bool flag = true;
            try
            {
                using (DBHelper dbhelper = new DBHelper())
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@id",model.id),
                        new MySqlParameter("@username",model.username)
                    };
                    DataTable dt = dbhelper.ExecuteDataTableParams("select count(Id) from  loginuser where id!=@id and username=@username", commandParameters);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            flag = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("IsExsitUserInfo出错：" + ex.ToString(), Util.Log.LogType.Error);
            }
            return flag;

        }



        ///// <summary>
        ///// 添加新appid后，需要单独刷新所有的游戏权限，不然新appid无法展示
        ///// </summary>
        ///// <returns>errmessage</returns>
        //public string RefreshGamePrivate()
        //{
        //    try
        //    {
        //        List<LoginUserModel> userList = base.GetList<LoginUserModel>().ToList();
        //        List<string> excludeFields = new List<string>() { "id", "password", "rolenames", "createtime", "appNames" };
        //        foreach (LoginUserModel user in userList)
        //        {
        //            if (!string.IsNullOrEmpty(user.appids))
        //            {
        //                string games = DAL.SdkcenterDatabaseDAL.Instance.GetGameByAppids(user.appids);
        //                string appids = DAL.SdkcenterDatabaseDAL.Instance.GetAppidsByGames(games);
        //                user.appids = appids;
        //                UpdateByKey<Model.LoginUserModel>(user, excludeFields, user.id);
        //            }
        //        }
        //        return string.Empty;
        //    }
        //    catch (Exception e)
        //    {
        //        WriteLog.Write(WriteLog.LogLevel.Error, "RefreshGamePrivate()更新所有游戏权限出错：\t" + e.ToString());
        //        return e.Message;
        //    }
        //}


        /// <summary>
        /// 验证密码是否正确
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool IsPasswordValid(string username, string password)
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
                    int count = dbhelper.ExecuteScalarIntParams("select count(Id) from  loginuser where username=@username and password=@password", commandParameters);
                    flag = count > 0;
                }
            }
            catch (Exception ex)
            {
                Util.Log.LogUtil.Write("IsPasswordValid出错：" + ex.ToString(), Util.Log.LogType.Error);
            }
            return flag;
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string username, string password)
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
