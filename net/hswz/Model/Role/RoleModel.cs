//***********************************************************************************
//文件名称：RoleModel.cs
//功能描述：用户角色实体类
//数据表： promotioncenter.role
//作者：周围
//日期：2017-09-18
//修改记录：
//***********************************************************************************
using Hswz.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hswz.Model
{
    /// <summary>
    /// 角色信息模型
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [TableField]
        public Int32 Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [TableField]
        public string RolesName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [TableField]
        public string Remark { get; set; }

        /// <summary>
        /// 角色具有的权限
        /// </summary>
        [TableField]
        public string Page { get; set; }
    }

    /// <summary>
    /// 角色信息列表
    /// </summary>
    public class RoleInfoListModel
    {
        /// <summary>
        /// 角色信息列表
        /// </summary>
        public List<RoleModel> RoleList { get; set; }
    }
}
