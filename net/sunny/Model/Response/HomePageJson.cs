using Sunny.Model.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Model.Response
{
    /// <summary>
    /// 首页数据源
    /// </summary>
    public class HomePageJson
    {
        public string tell { get; set; }
        public string tel_image { get; set; }
        public List<BannerJson> top_images { get; set; }
        public string introduce_image { get; set; }
        public List<ProductListJson> courses { get; set; }
    }

    /// <summary>
    /// 顶部图片
    /// </summary>
    public class BannerJson
    {
        public string image { get; set; }
    }


}
