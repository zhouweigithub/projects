using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Response;
using Sunny.Common;

namespace Sunny.BLL.API
{

    public class CourseBLL
    {
        /// <summary>
        /// 获取课程详情，包含评论
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static CourseInfoJson GetCourseInfo(int productId)
        {
            //课程基本信息
            CourseInfoJson courseInfo = CourseDAL.GetCourseInfo(productId);

            //课程评论信息
            if (courseInfo != null)
            {
                List<ClassCommentJson> comments = ClassDAL.GetClassCommentList(0, productId, 10);
                courseInfo.commentslist = comments;
            }
            return courseInfo;
        }

        /// <summary>
        /// 获取首页数据源
        /// </summary>
        /// <returns></returns>
        public static HomePageJson GetHomePageDatas()
        {
            string where = "state=0 and type in(0,1,2)";
            IList<Banner> banners = DBData.GetInstance(DBTable.banner).GetList<Banner>(where);
            SiteInfo phoneInfo = DBData.GetInstance(DBTable.site_info).GetEntityByKey<SiteInfo>(Const.SitePhone);
            List<ProductListJson> courses = CourseDAL.GetCourseList(string.Empty, 0, 1, 10);
            var topImages = banners.Where(a => a.type == 0).Select(b => new BannerJson() { image = b.url }).ToList();
            var telImage = banners.Where(a => a.type == 1).Select(b => b.url).FirstOrDefault();
            var introduceImage = banners.Where(a => a.type == 2).Select(b => b.url).FirstOrDefault();
            HomePageJson result = new HomePageJson()
            {
                tel = phoneInfo != null ? phoneInfo.pvalue : string.Empty,
                top_images = topImages,
                tel_image = telImage ?? string.Empty,
                introduce_image = introduceImage ?? string.Empty,
                courses = courses,
            };
            return result;
        }

        /// <summary>
        /// 获取商城页数据源
        /// </summary>
        /// <returns></returns>
        public static MallPageJson GetMallPageDatas()
        {
            string where = "state=0 and type=4";
            IList<Banner> banners = DBData.GetInstance(DBTable.banner).GetList<Banner>(where);
            var topImages = banners.Select(b => new BannerJson() { image = b.url }).ToList();
            List<ProductListJson> hotCourses = CourseDAL.GetHotCourseList(0, 10);    //热门课程
            List<ProductListJson> NiceCourses = CourseDAL.GetHotCourseList(1, 10);   //精品课程
            MallPageJson result = new MallPageJson()
            {
                top_images = topImages,
                courses_jp = NiceCourses,
                courses_rm = hotCourses,
            };
            return result;
        }
    }
}
