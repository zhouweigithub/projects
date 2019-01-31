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
    /// 商品目录信息
    /// </summary>
    public class CategoryBLL
    {
        /// <summary>
        /// 获取所有上级目录信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<Category> GetSuperCategoryIds(int categoryId)
        {
            try
            {
                List<Category> parents = new List<Category>();
                IList<Category> categoryList = DBData.GetInstance(DBTable.category).GetList<Category>("state=0");
                GetParents(categoryId, parents, categoryList);
                return parents;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetSuperCategoryIds 获取所有上级目录信息时出错：" + e, Util.Log.LogType.Error);
                return new List<Category>();
            }
        }

        /// <summary>
        /// 递归获取自己及低级目录信息
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="result"></param>
        /// <param name="source"></param>
        private static void GetParents(int categoryId, List<Category> result, IList<Category> source)
        {
            if (source.Count == 0)
                return;

            Category tmp = source.FirstOrDefault(a => a.id == categoryId);
            if (tmp == null)
            {
                return;
            }
            else if (tmp.parent == 0)
            {
                result.Add(tmp);
            }
            else
            {
                result.Add(tmp);
                GetParents(tmp.parent, result, source);
            }

        }
    }
}
