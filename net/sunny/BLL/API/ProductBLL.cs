using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunny.DAL;
using Sunny.Model;
using Sunny.Model.Response;

namespace Sunny.BLL.API
{

    public class ProductBLL
    {

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static ProductInfoJson GetProductInfo(int productId)
        {
            //课程基本信息
            ProductInfoJson courseInfo = ProductDAL.GetProductInfo(productId);
            return courseInfo;
        }

    }
}
