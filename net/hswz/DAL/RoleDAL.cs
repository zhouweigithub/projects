//***********************************************************************************
//文件名称：RoleBLL.cs
//功能描述：用户角色操作类
//数据表： promotioncenter.role
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Moqikaka.Tmp.Model;
using System;
using System.Data;
using System.Linq;
using Moqikaka.Tmp.Common;
using MySql.Data.MySqlClient;

namespace Moqikaka.Tmp.DAL
{
    public class RoleDAL
    {

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns></returns>
        public static RoleInfoListModel GetListRoles()
        {
            var resultData = new RoleInfoListModel();

            var resultlst = DAL.DBData.GetInstance(DAL.DBTable.role).GetList<Model.RoleModel>();
            if (resultlst != null)
            {
                resultData.RoleList = resultlst.ToList();
            }

            return resultData;
        }

        /// <summary>
        /// 检测目标角色是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool IsExsitRole(Model.RoleModel model)
        {
            bool flag = true;
            try
            {
                using (DBHelper dbhelper = new DBHelper(WebConfigData.DataBaseType, WebConfigData.ConnString))
                {
                    MySqlParameter[] commandParameters = new MySqlParameter[] {
                        new MySqlParameter("@Id",model.Id),
                        new MySqlParameter("@RolesName",model.RolesName)
                    };
                    DataTable dt = dbhelper.ExecuteDataTableParams("select count(Id) from  role where Id!=@Id and RolesName=@RolesName", commandParameters);

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
                Util.Log.LogUtil.Write("检测角色是否存在时出错：" + ex.ToString(), Moqikaka.Util.Log.LogType.Error);
            }
            return flag;

        }
    }
}
