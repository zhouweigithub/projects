using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spetmall.DAL;
using Spetmall.Model.Page;

namespace Spetmall.BLL.Page
{
    public class ActivityBLL
    {
        /// <summary>
        /// 获取限时折扣活动日期是否交叉的信息
        /// </summary>
        /// <param name="postInfo"></param>
        /// <returns></returns>
        public static object GetDisscountCrossInfo(discount_post postInfo)
        {
            object result = null;
            switch (postInfo.type)
            {
                case 0:
                    result = GetDiscountShopCrossMaxDate(postInfo.starttime.ToString("yyyy-MM-dd"), postInfo.endtime.ToString("yyyy-MM-dd"), postInfo.id);
                    break;
                case 1:
                case 2:
                    result = GetDiscountNotShopCrossMaxDate(postInfo.type, postInfo.categoryOrProducts, postInfo.starttime.ToString("yyyy-MM-dd"), postInfo.endtime.ToString("yyyy-MM-dd"), postInfo.id);
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 获取满就送活动日期是否交叉的信息
        /// </summary>
        /// <param name="postInfo"></param>
        /// <returns></returns>
        public static object GetFullsendCrossInfo(fullsend_post postInfo)
        {
            object result = null;
            switch (postInfo.type)
            {
                case 0:
                    result = GetFullsendShopCrossMaxDate(postInfo.starttime.ToString("yyyy-MM-dd"), postInfo.endtime.ToString("yyyy-MM-dd"), postInfo.id);
                    break;
                case 1:
                case 2:
                    result = GetFullsendNotShopCrossMaxDate(postInfo.type, postInfo.categoryOrProducts, postInfo.starttime.ToString("yyyy-MM-dd"), postInfo.endtime.ToString("yyyy-MM-dd"), postInfo.id);
                    break;
                default:
                    break;
            }
            return result;
        }


        /// <summary>
        /// 获取限时折扣店铺级活动时间交叉情况
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="currentActiveid">当前活动id</param>
        /// <returns></returns>
        private static object GetDiscountShopCrossMaxDate(string startdate, string enddate, int currentActiveid)
        {
            string endtime = discountDAL.GetIntersectDateShop(startdate, enddate, currentActiveid);
            if (!string.IsNullOrEmpty(endtime))
            {
                var data = new Dictionary<string, object> { { endtime, 0 } };
                return CreateReturnObject(data, false, true);
            }
            else
                return null;
        }

        /// <summary>
        /// 获取满就送店铺级活动时间交叉情况
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="currentActiveid">当前活动id</param>
        /// <returns></returns>
        private static object GetFullsendShopCrossMaxDate(string startdate, string enddate, int currentActiveid)
        {
            string endtime = fullsendDAL.GetIntersectDateShop(startdate, enddate, currentActiveid);
            if (!string.IsNullOrEmpty(endtime))
            {
                var data = new Dictionary<string, object> { { endtime, 0 } };
                return CreateReturnObject(data, false, false);
            }
            else
                return null;
        }

        /// <summary>
        /// 获取限时折扣非店铺级活动时间交叉情况
        /// </summary>
        /// <param name="type">1按分类 2按商品</param>
        /// <param name="productids">商品或分类id集</param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="currentActiveid">当前活动id</param>
        /// <returns></returns>
        private static object GetDiscountNotShopCrossMaxDate(short type, string productids, string startdate, string enddate, int currentActiveid)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<active_date> datas = discountDAL.GetIntersectDateNotShop(type, productids, startdate, enddate, currentActiveid);
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (type == 2)
                        result.Add(item.productid, new Dictionary<string, int> { { item.endtime.ToString("yyyy-MM-dd"), 0 } });
                    else if (type == 1)
                        result.Add(item.endtime.ToString("yyyy-MM-dd"), 0);
                }
            }
            return CreateReturnObject(result, type == 2, true);
        }

        /// <summary>
        /// 获取满就送非店铺级活动时间交叉情况
        /// </summary>
        /// <param name="type">1按分类 2按商品</param>
        /// <param name="productids"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="currentActiveid">当前活动id</param>
        /// <returns></returns>
        private static object GetFullsendNotShopCrossMaxDate(short type, string productids, string startdate, string enddate, int currentActiveid)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            List<active_date> datas = fullsendDAL.GetIntersectDateNotShop(type, productids, startdate, enddate, currentActiveid);
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (type == 2)
                        result.Add(item.productid, new Dictionary<string, int> { { item.endtime.ToString("yyyy-MM-dd"), 0 } });
                    else if (type == 1)
                        result.Add(item.endtime.ToString("yyyy-MM-dd"), 0);
                }
            }
            return CreateReturnObject(result, type == 2, false);
        }

        /// <summary>
        /// 创建结果对象，无交叉时返回空对象
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isProduct">是否为商品</param>
        /// <param name="isDiscount">是否为限时折扣</param>
        /// <returns></returns>
        private static object CreateReturnObject(Dictionary<string, object> data, bool isProduct, bool isDiscount)
        {
            int code = 0;
            bool status = true;
            string msg = string.Empty;
            string href = string.Empty;

            if (data.Count > 0)
            {   //有日期交叉
                status = false;
                if (isProduct)
                {
                    code = 1000;
                    msg = "某些规格运营时间范围需要调整";
                }
                else
                {
                    code = 1200;
                    msg = "运营时间范围需要调整,已存在";
                }
                return new
                {
                    code,
                    status,
                    data,
                    msg,
                    href,
                };
            }
            else
            {
                return null;
            }

        }
    }
}
