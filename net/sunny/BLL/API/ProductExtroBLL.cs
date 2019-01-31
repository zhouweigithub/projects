using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Custom;
using Sunny.Model.Response;

namespace Sunny.BLL.API
{
    public class ProductExtroBLL
    {
        #region 课程规格 

        /// <summary>
        /// 获取课程的规格信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static CourseSpecificationJson GetCourseSpecification(int productId)
        {
            try
            {
                CourseSpecificationJson result = new CourseSpecificationJson()
                {
                    campus = GetCourseCampusList(productId),
                    campu_venues = GetCourseVenuesDic(productId),
                    types = GetCourseTypeList(productId),
                    prices = GetCoursePriceDic(productId),
                };

                return result;
            }
            catch (Exception e)
            {
                Util.Log.LogUtil.Write("GetCourseSpecification 出错：" + e, Util.Log.LogType.Error);
                return null;
            }
        }

        /// <summary>
        /// 课程列表人数类型
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<CourseType> GetCourseTypeList(int productId)
        {
            return ProductExtroDAL.GetCourseTypeList(productId);
        }

        /// <summary>
        /// 获取所有校区
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<Campus> GetCourseCampusList(int productId)
        {
            List<Campus> result = new List<Campus>();
            List<CustVenue> source = ProductExtroDAL.GetVenueList(productId);
            var campuIds = source.Select(a => a.campus_id).Distinct();
            foreach (var item in campuIds)
            {
                result.Add(new Campus()
                {
                    id = item,
                    name = source.First(a => a.campus_id == item).campus_name,
                });
            }

            return result;
        }

        /// <summary>
        /// 获取所有场馆
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<Venue> GetCourseVenuesList(int productId)
        {
            List<Venue> result = new List<Venue>();
            List<CustVenue> source = ProductExtroDAL.GetVenueList(productId);
            var venueIds = source.Select(a => a.venue_id).Distinct();
            foreach (var item in venueIds)
            {
                result.Add(new Venue()
                {
                    id = item,
                    name = source.First(a => a.venue_id == item).venue_name,
                });
            }

            return result;
        }

        /// <summary>
        /// 获取校区及下面的场馆
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Dictionary<int, List<Venue>> GetCourseVenuesDic(int productId)
        {
            Dictionary<int, List<Venue>> result = new Dictionary<int, List<Venue>>();
            List<CustVenue> source = ProductExtroDAL.GetVenueList(productId);
            var campuIds = source.Select(a => a.campus_id).Distinct();
            foreach (var campuId in campuIds)
            {
                var venues = source.Where(a => a.campus_id == campuId).Select(b => b.venue_id).Distinct();
                List<Venue> venueList = new List<Venue>();
                foreach (var venue in venues)
                {
                    venueList.Add(new Venue()
                    {
                        id = venue,
                        name = source.First(a => a.venue_id == venue).venue_name,
                    });
                }
                result.Add(campuId, venueList);
            }

            return result;
        }

        /// <summary>
        /// 取课程属性与价格的关系
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Dictionary<string, decimal> GetCoursePriceDic(int productId)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            IList<CoursePrice> source = DBData.GetInstance(DBTable.course_price).GetList<CoursePrice>($"product_id={productId}");
            foreach (CoursePrice item in source)
            {
                result.Add($"{item.venue_id }|{ item.type_id}", item.price);
            }

            return result;
        }

        #endregion

        #region 一般商品规格 



        #endregion

    }
}
