using Sunny.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;

namespace Sunny.BLL.API
{
    public class CampusBLL
    {

        /// <summary>
        /// 获取校区与场馆的关系
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, List<Venue>> GetCampusDic()
        {
            Dictionary<int, List<Venue>> result = new Dictionary<int, List<Venue>>();
            IList<Venue> datas = DBData.GetInstance(DBTable.venue).GetList<Venue>();
            var campusIds = datas.Select(a => a.campus_id).Distinct();
            foreach (int campid in campusIds)
            {
                var venues = datas.Where(a => a.campus_id == campid).ToList();
                result.Add(campid, venues);
            }

            return result;
        }
    }
}
