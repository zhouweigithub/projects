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
        /// <returns></returns>
        public List<category> GetFloorDatas()
        {
            List<category> datas = new List<category>();
            List<category> list = GetList<category>($"pid=0").ToList();
            //按照序号和id排序
            list = list.OrderBy(a => a.index).ThenBy(b => b.id).ToList();

            GetFloorDatas(list, 0, datas);
            return datas;
        }

        /// <summary>
        /// 递归获取各层级的分类
        /// </summary>
        /// <param name="pids">父id集</param>
        /// <param name="floor">层级，顶层为0</param>
        /// <param name="result"></param>
        private void GetFloorDatas(List<category> parents, int floor, List<category> result)
        {
            try
            {
                foreach (category parent in parents)
                {
                    parent.floor = floor;
                    result.Add(parent);

                    List<category> list = GetList<category>($"pid={parent.id}").ToList();
                    //按照序号和id排序
                    list = list.OrderBy(a => a.index).ThenBy(b => b.id).ToList();

                    if (list.Count > 0)
                    {
                        GetFloorDatas(list, floor + 1, result);
                    }
                }
                //IList<category> list = GetList<category>($"pid in({pids})");
                //foreach (category item in list)
                //{
                //    item.floor = floor;
                //}

                //if (list.Count > 0)
                //{
                //    result.AddRange(list);
                //    string pids = string.Join(",", list.Select(a => a.id));
                //    GetFloorDatas(pids, ++floor, result);
                //}
                //else
                //{
                //    return;
                //}
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write($"GetDatas 根据父id获取分类数据出错\r\n" + e, Util.Log.LogType.Error);
            }
        }

    }
}
