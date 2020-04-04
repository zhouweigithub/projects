using Spetmall.Model;
using System;
using System.Data;
using System.Linq;
using Spetmall.Common;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Spetmall.DAL
{
    public class categoryDAL : DAL.BaseQuery
    {
        private static readonly categoryDAL Instance = new categoryDAL();

        private categoryDAL()
        {
            this.IsAddIntoCache = false;
            this.TableName = "category";
            this.ItemName = "商品分类";
            this.OrderbyFields = "id asc";
        }

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        /// <returns></returns>
        public static categoryDAL GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 获取分类数据（包含层级）
        /// </summary>
        /// <param name="isAll">是否包含隐藏了的分类</param>
        /// <returns></returns>
        public List<category> GetFloorDatas(bool isAll)
        {
            List<category> datas = new List<category>();
            string where = isAll ? string.Empty : "and state=1";
            List<category> list = GetList<category>($"pid=0 {where}").ToList();
            //按照序号和id排序
            list = list.OrderBy(a => a.index).ThenBy(b => b.id).ToList();

            GetFloorDatas(list, 0, isAll, datas);
            return datas;
        }

        /// <summary>
        /// 递归获取各层级的分类
        /// </summary>
        /// <param name="pids">父id集</param>
        /// <param name="floor">层级，顶层为0</param>
        /// <param name="result"></param>
        private void GetFloorDatas(List<category> parents, int floor, bool isAll, List<category> result)
        {
            try
            {
                string where = isAll ? string.Empty : "and state=1";
                foreach (category parent in parents)
                {
                    parent.floor = floor;
                    result.Add(parent);

                    List<category> list = GetList<category>($"pid={parent.id} {where}").ToList();
                    //按照序号和id排序
                    list = list.OrderBy(a => a.index).ThenBy(b => b.id).ToList();

                    if (list.Count > 0)
                    {
                        GetFloorDatas(list, floor + 1, isAll, result);
                    }
                }
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"GetDatas 根据父id获取分类数据出错\r\n" + e, Util.Log.LogType.Error);
            }
        }

        /// <summary>
        /// 获取当前分类的父级分类id集，包含本身，从近到远排列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parents"></param>
        public void GetParentIds(int id, List<int> parents)
        {
            parents.Add(id);

            category current = GetEntityByKey<category>(id);
            if (current != null)
            {
                if (current.pid == 0)
                    return;
                GetParentIds(current.pid, parents);
            }
        }
    }
}
